using System.Linq;
using System.Text;

namespace Sia.Math.CodeGenerators.Capabilities;

public static class Swizzles
{
    public static CodeFragment Generate(TypeShape shape)
    {
        if (shape.IsMatrix) return CodeFragment.Empty;

        var body = new StringBuilder();
        var count = shape.Rows;

        for (var x = 0; x < count; x++)
        for (var y = 0; y < count; y++)
        for (var z = 0; z < count; z++)
        for (var w = 0; w < count; w++)
            EmitOne(shape, [x, y, z, w], body);

        for (var x = 0; x < count; x++)
        for (var y = 0; y < count; y++)
        for (var z = 0; z < count; z++)
            EmitOne(shape, [x, y, z], body);

        for (var x = 0; x < count; x++)
        for (var y = 0; y < count; y++)
            EmitOne(shape, [x, y], body);

        return new CodeFragment
        {
            Usings = ["System.Runtime.CompilerServices", "System.Runtime.Intrinsics"],
            TypeBody = body.ToString().TrimEnd()
        };
    }

    private static void EmitOne(TypeShape shape, int[] sw, StringBuilder body)
    {
        var bits = 0;
        var allowSetter = true;
        foreach (var s in sw)
        {
            if ((bits & (1 << s)) != 0) allowSetter = false;
            bits |= 1 << s;
        }

        var names = sw.Select(s => TypeShape.Components[s]).ToArray();
        var fieldType = shape.BaseType.ToTypeName(sw.Length, 1);

        body.AppendLine($"        public {fieldType} {string.Concat(names)}");
        body.AppendLine("        {");
        body.AppendLine("            [MethodImpl(MethodImplOptions.AggressiveInlining)]");
        body.AppendLine($"            get => {BuildGetter(shape, sw, fieldType, names)};");
        if (allowSetter)
        {
            body.AppendLine("            [MethodImpl(MethodImplOptions.AggressiveInlining)]");
            body.AppendLine("            set {");
            for (var i = 0; i < sw.Length; i++)
                body.AppendLine($"                {TypeShape.Components[sw[i]]} = {(sw.Length > 1 ? $"value.{TypeShape.Components[i]}" : "value")};");
            body.AppendLine("            }");
        }
        body.AppendLine("        }");
    }

    private static string BuildGetter(TypeShape shape, int[] sw, string fieldType, string[] names)
    {
        var bt = shape.BaseType;
        if (bt == BaseType.Bool)
            return $"new {fieldType}({string.Join(", ", names)})";

        var comp = sw.Length;
        var srcLanes = Simd.SimdStrategy.NativeLaneCount(bt, shape.Rows);
        var dstLanes = Simd.SimdStrategy.NativeLaneCount(bt, comp);
        var width = System.Math.Max(srcLanes, dstLanes);

        var shuffleClass = bt == BaseType.Double
            ? (width == 2 ? "Vector128" : "Vector256")
            : "Vector128";

        var suffix = bt switch
        {
            BaseType.Double => "L",
            BaseType.UInt => "u",
            _ => ""
        };

        var indices = string.Join(", ", Enumerable.Range(0, width)
            .Select(i => $"{(i < comp ? sw[i] : width)}{suffix}"));

        var source = srcLanes < width
            ? "Vector256.Create(data, Vector128<double>.Zero)"
            : "data";

        var shuffled = $"{shuffleClass}.Shuffle({source}, {shuffleClass}.Create({indices}))";
        var result = dstLanes < width ? $"Vector256.GetLower({shuffled})" : shuffled;

        return $"new {fieldType}({result})";
    }
}
