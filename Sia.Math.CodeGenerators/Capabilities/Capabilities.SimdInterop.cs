using System.Linq;

namespace Sia.Math.CodeGenerators.Capabilities;

public static class SimdInterop
{
    private static readonly string[] s_Imports = ["System.Runtime.CompilerServices", "System.Runtime.Intrinsics"];

    public static CodeFragment Generate(TypeShape shape)
    {
        if (!shape.IsSimdEligible)
            return CodeFragment.Empty;

        var vectorName = Simd.SimdStrategy.NativeVectorTypeName(shape.BaseType, shape.Rows);
        var laneCount = Simd.SimdStrategy.NativeLaneCount(shape.BaseType, shape.Rows);
        var isExact = Simd.SimdStrategy.IsExactFit(shape.BaseType, shape.Rows)
                   || Simd.SimdStrategy.IsPaddedFit(shape.BaseType, shape.Rows);

        var body = new System.Text.StringBuilder();

        body.AppendLine("        [MethodImpl(MethodImplOptions.AggressiveInlining)]");
        if (isExact)
        {
            body.AppendLine($"        internal readonly {vectorName} AsSimd() => Unsafe.As<{shape.TypeName}, {vectorName}>(ref Unsafe.AsRef(in this));");
        }
        else
        {
            var zero = shape.BaseType.ToTypedLiteral(0);
            var className = Simd.SimdStrategy.NativeVectorClassName(shape.BaseType, shape.Rows);
            var args = string.Join(", ", TypeShape.VectorFields.Take(shape.Rows)
                .Concat(Enumerable.Repeat(zero, laneCount - shape.Rows)));
            body.AppendLine($"        internal readonly {vectorName} AsSimd() => {className}.Create({args});");
        }

        body.AppendLine();

        body.AppendLine("        [MethodImpl(MethodImplOptions.AggressiveInlining)]");
        if (isExact)
        {
            body.AppendLine($"        internal readonly {vectorName} AsSimdUnsafe() => AsSimd();");
        }
        else
        {
            body.AppendLine($"        internal readonly {vectorName} AsSimdUnsafe() => Unsafe.ReadUnaligned<{vectorName}>(ref Unsafe.As<{shape.TypeName}, byte>(ref Unsafe.AsRef(in this)));");
        }

        body.AppendLine();

        body.AppendLine("        [MethodImpl(MethodImplOptions.AggressiveInlining)]");
        if (isExact)
        {
            body.AppendLine($"        internal {shape.TypeName}({vectorName} v) => this = Unsafe.As<{vectorName}, {shape.TypeName}>(ref v);");
        }
        else
        {
            var assigns = string.Join(" ", TypeShape.VectorFields.Take(shape.Rows)
                .Select((f, i) => $"{f} = v.GetElement({i});"));
            body.AppendLine($"        internal {shape.TypeName}({vectorName} v) {{ {assigns} }}");
        }

        return new CodeFragment
        {
            Usings = s_Imports,
            TypeBody = body.ToString().TrimEnd()
        };
    }
}
