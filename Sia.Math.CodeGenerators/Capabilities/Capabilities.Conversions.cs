using System.Collections.Generic;
using System.Text;

namespace Sia.Math.CodeGenerators.Capabilities;

public static class Conversions
{
    private static readonly string[] s_Imports = ["System.Runtime.CompilerServices"];

    public static CodeFragment Generate(TypeShape shape)
    {
        var typeBody = new StringBuilder();
        var mathBody = new StringBuilder();

        EmitConversion(shape, shape.BaseType, false, true, typeBody, mathBody);

        foreach (var srcType in GetSourceTypes(shape.BaseType))
        {
            var isExplicit = NeedsExplicit(shape.BaseType, srcType);
            EmitConversion(shape, srcType, isExplicit, true, typeBody, mathBody);
            if (!shape.IsScalar)
                EmitConversion(shape, srcType, isExplicit, false, typeBody, mathBody);
        }

        return new CodeFragment
        {
            Usings = s_Imports,
            TypeBody = typeBody.ToString().TrimEnd(),
            MathBody = mathBody.ToString().TrimEnd()
        };
    }

    private static BaseType[] GetSourceTypes(BaseType target) => target switch
    {
        BaseType.Int => [BaseType.Bool, BaseType.UInt, BaseType.Float, BaseType.Double],
        BaseType.UInt => [BaseType.Bool, BaseType.Int, BaseType.Float, BaseType.Double],
        BaseType.Float => [BaseType.Bool, BaseType.Int, BaseType.UInt, BaseType.Double],
        BaseType.Double => [BaseType.Bool, BaseType.Int, BaseType.UInt, BaseType.Float],
        _ => []
    };

    private static bool NeedsExplicit(BaseType target, BaseType source) => (target, source) switch
    {
        (BaseType.Float, BaseType.Int or BaseType.UInt) => false,
        (BaseType.Double, BaseType.Int or BaseType.UInt or BaseType.Float) => false,
        _ => true
    };

    private static void EmitConversion(TypeShape shape, BaseType srcType, bool isExplicit, bool isScalar,
        StringBuilder typeBody, StringBuilder mathBody)
    {
        var srcName = isScalar ? srcType.ToBaseTypeName() : srcType.ToTypeName(shape.Rows, shape.Columns);
        var typeCategory = shape.IsMatrix ? "matrix" : "vector";
        var fieldCount = shape.IsMatrix ? shape.Columns : shape.Rows;
        var fields = shape.IsMatrix ? TypeShape.MatrixFields : TypeShape.VectorFields;
        var fieldType = shape.IsMatrix ? shape.BaseType.ToTypeName(shape.Rows, 1) : shape.BaseType.ToBaseTypeName();

        if (isScalar)
        {
            typeBody.AppendLine(srcType != shape.BaseType
                ? $"        /// <summary>Constructs a <see cref=\"{shape.TypeName}\" /> {typeCategory} from a single <see cref=\"{srcName}\" /> value by converting it to <see cref=\"{shape.BaseTypeName}\" /> and assigning it to every component.</summary>"
                : $"        /// <summary>Constructs a <see cref=\"{shape.TypeName}\" /> {typeCategory} from a single <see cref=\"{srcName}\" /> value by assigning it to every component.</summary>");
        }
        else
        {
            if (srcType != shape.BaseType)
                typeBody.AppendLine($"        /// <summary>Constructs a <see cref=\"{shape.TypeName}\" /> {typeCategory} from a <see cref=\"{srcName}\" /> {typeCategory} by component-wise conversion.</summary>");
        }
        typeBody.AppendLine($"        /// <param name=\"v\">The <see cref=\"{srcName}\" /> to convert.</param>");
        typeBody.AppendLine("        [MethodImpl(MethodImplOptions.AggressiveInlining)]");
        typeBody.AppendLine($"        public {shape.TypeName}({srcName} v)");
        typeBody.AppendLine("        {");
        var args = new List<string>();
        for (var i = 0; i < fieldCount; i++)
        {
            var rhs = isScalar ? "v" : $"v.{fields[i]}";
            if (srcType != shape.BaseType)
            {
                if (srcType == BaseType.Bool)
                {
                    rhs = shape.IsMatrix
                        ? $"math.select(new {fieldType}({shape.BaseType.ToTypedLiteral(0)}), new {fieldType}({shape.BaseType.ToTypedLiteral(1)}), {rhs})"
                        : $"{rhs} ? {shape.BaseType.ToTypedLiteral(1)} : {shape.BaseType.ToTypedLiteral(0)}";
                }
                else if (!shape.IsMatrix || isExplicit)
                {
                    rhs = $"({fieldType}){rhs}";
                }
            }
            args.Add(rhs);
        }

        if (shape.IsMatrix || shape.BaseType == BaseType.Bool)
        {
            for (var i = 0; i < fieldCount; i++)
                typeBody.AppendLine($"            this.{fields[i]} = {args[i]};");
        }
        else
        {
            var laneCount = Simd.SimdStrategy.NativeLaneCount(shape.BaseType, shape.Rows);
            var zero = shape.BaseType.ToTypedLiteral(0);
            for (var i = fieldCount; i < laneCount; i++)
                args.Add(zero);
            var vectorClassName = Simd.SimdStrategy.NativeVectorClassName(shape.BaseType, shape.Rows);
            typeBody.AppendLine($"            data = {vectorClassName}.Create({string.Join(", ", args)});");
        }
        typeBody.AppendLine("        }");

        typeBody.AppendLine("        [MethodImpl(MethodImplOptions.AggressiveInlining)]");
        typeBody.AppendLine($"        public static {(isExplicit ? "explicit" : "implicit")} operator {shape.TypeName}({srcName} v) => new {shape.TypeName}(v);");

        mathBody.AppendLine("        [MethodImpl(MethodImplOptions.AggressiveInlining)]");
        mathBody.AppendLine($"        public static {shape.TypeName} {shape.TypeName}({srcName} v) => new {shape.TypeName}(v);");
    }
}
