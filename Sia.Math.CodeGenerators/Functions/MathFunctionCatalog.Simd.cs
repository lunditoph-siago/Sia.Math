namespace Sia.Math.CodeGenerators.Functions;

using System.Linq;

public static partial class MathFunctionCatalog
{
    private static MathFunction SimdUnary(
        string name,
        string portableMethod)
    {
        return Fn(
            name,
            [BaseType.Float, BaseType.Double],
            (2, 4),
            signature => {
                var typeName = signature.Shape.TypeName;
                var vectorClass = Simd.SimdStrategy.NativeVectorClassName(
                    signature.Type,
                    signature.Dimension);
                return FunctionEmitter.GenerateSimd(
                    $"public static {typeName} {name}({typeName} x)",
                    signature.Shape,
                    $"new {typeName}({vectorClass}.{portableMethod}(x.data))",
                    $"new {typeName}({PerComp(name, signature.Dimension)})");
            });
    }

    private static MathFunction SimdTrig(
        string name,
        string portableMethod)
    {
        return Fn(
            name,
            [BaseType.Float, BaseType.Double],
            (1, 4),
            signature => {
                var typeName = signature.Shape.TypeName;
                if (signature.Dimension == 1) {
                    var scalarClass = signature.Type == BaseType.Float
                        ? "System.MathF"
                        : "System.Math";
                    return
                    [
                        Attr,
                        $"public static {typeName} {name}({typeName} x) => {scalarClass}.{portableMethod}(x);",
                    ];
                }

                var vectorClass = Simd.SimdStrategy.NativeVectorClassName(
                    signature.Type,
                    signature.Dimension);
                return FunctionEmitter.GenerateSimd(
                    $"public static {typeName} {name}({typeName} x)",
                    signature.Shape,
                    $"new {typeName}({vectorClass}.{portableMethod}(x.data))",
                    $"new {typeName}({PerComp(name, signature.Dimension)})");
            });
    }

    private static MathFunction SimdLerp()
    {
        return Fn(
            "lerp",
            [BaseType.Float, BaseType.Double],
            (1, 4),
            signature => {
                var typeName = signature.Shape.TypeName;
                if (signature.Dimension == 1) {
                    return
                    [
                        Attr,
                        $"public static {typeName} lerp({typeName} a, {typeName} b, {typeName} s) => a + s * (b - a);",
                    ];
                }

                var scalarType = signature.Type.ToBaseTypeName();
                var vectorClass = Simd.SimdStrategy.NativeVectorClassName(
                    signature.Type,
                    signature.Dimension);
                var vectorScalar = string.Join(
                    ", ",
                    Range(signature.Dimension).Select(index =>
                        $"lerp(a.{TypeShape.Components[index]}, b.{TypeShape.Components[index]}, s.{TypeShape.Components[index]})"));
                var uniformScalar = string.Join(
                    ", ",
                    Range(signature.Dimension).Select(index =>
                        $"lerp(a.{TypeShape.Components[index]}, b.{TypeShape.Components[index]}, s)"));

                var vectorFactor = FunctionEmitter.GenerateSimd(
                    $"public static {typeName} lerp({typeName} a, {typeName} b, {typeName} s)",
                    signature.Shape,
                    $"new {typeName}({vectorClass}.FusedMultiplyAdd(s.data, b.data - a.data, a.data))",
                    $"new {typeName}({vectorScalar})",
                    dispatch => ConfigureLerpFusedMultiplyAdd(
                        dispatch,
                        signature,
                        typeName,
                        "s.data"));
                var uniformFactor = FunctionEmitter.GenerateSimd(
                    $"public static {typeName} lerp({typeName} a, {typeName} b, {scalarType} s)",
                    signature.Shape,
                    $"new {typeName}({vectorClass}.FusedMultiplyAdd({vectorClass}.Create(s), b.data - a.data, a.data))",
                    $"new {typeName}({uniformScalar})",
                    dispatch => ConfigureLerpFusedMultiplyAdd(
                        dispatch,
                        signature,
                        typeName,
                        $"{vectorClass}.Create(s)"));

                return [.. vectorFactor, .. uniformFactor];
            });
    }

    private static void ConfigureLerpFusedMultiplyAdd(
        Simd.SimdDispatch dispatch,
        MathSignature signature,
        string typeName,
        string factor)
    {
        dispatch.WithX86(
            "global::System.Runtime.Intrinsics.X86.Fma.IsSupported",
            $"return new {typeName}(global::System.Runtime.Intrinsics.X86.Fma.MultiplyAdd({factor}, b.data - a.data, a.data));");

        var profile = Simd.SimdTypeProfile.Create(
            signature.Type,
            signature.Dimension);
        if (profile.Width != Simd.SimdVectorWidth.Vector128) {
            return;
        }

        var armClass = signature.Type == BaseType.Float
            ? "global::System.Runtime.Intrinsics.Arm.AdvSimd"
            : "global::System.Runtime.Intrinsics.Arm.AdvSimd.Arm64";
        dispatch.WithArm(
            $"{armClass}.IsSupported",
            $"return new {typeName}({armClass}.FusedMultiplyAdd(a.data, {factor}, b.data - a.data));");
    }

    private static void ConfigureFusedMultiplyAdd(
        Simd.SimdDispatch dispatch,
        MathSignature signature,
        string typeName)
    {
        if (signature.Type is not (BaseType.Float or BaseType.Double)) {
            return;
        }

        dispatch.WithX86(
            "global::System.Runtime.Intrinsics.X86.Fma.IsSupported",
            $"return new {typeName}(global::System.Runtime.Intrinsics.X86.Fma.MultiplyAdd(a.data, b.data, c.data));");

        var profile = Simd.SimdTypeProfile.Create(
            signature.Type,
            signature.Dimension);
        if (profile.Width != Simd.SimdVectorWidth.Vector128) {
            return;
        }

        var armClass = signature.Type == BaseType.Float
            ? "global::System.Runtime.Intrinsics.Arm.AdvSimd"
            : "global::System.Runtime.Intrinsics.Arm.AdvSimd.Arm64";
        dispatch.WithArm(
            $"{armClass}.IsSupported",
            $"return new {typeName}({armClass}.FusedMultiplyAdd(c.data, a.data, b.data));");
    }

    private static string FillUnusedLanes(
        MathSignature signature,
        string expression,
        string fillValue)
    {
        var laneCount = Simd.SimdStrategy.NativeLaneCount(
            signature.Type,
            signature.Dimension);
        for (var index = signature.Dimension; index < laneCount; index++) {
            expression += $".WithElement({index}, {fillValue})";
        }

        return expression;
    }
}
