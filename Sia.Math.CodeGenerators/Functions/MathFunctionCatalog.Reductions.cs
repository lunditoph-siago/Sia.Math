namespace Sia.Math.CodeGenerators.Functions;

using System.Linq;

public static partial class MathFunctionCatalog
{
    private static MathFunction[] ReductionFunctions() =>
    [
        Fn(
            "any",
            [
                BaseType.Bool,
                BaseType.Int,
                BaseType.UInt,
                BaseType.Float,
                BaseType.Double,
            ],
            (2, 4),
            signature => {
                var typeName = signature.Shape.TypeName;
                var scalar = string.Join(
                    " || ",
                    Range(signature.Dimension).Select(index =>
                        signature.Type == BaseType.Bool
                            ? $"x.{TypeShape.Components[index]}"
                            : $"x.{TypeShape.Components[index]} != {signature.Type.ToTypedLiteral(0)}"));
                if (signature.Type == BaseType.Bool) {
                    return FunctionEmitter.GenerateExpression(
                        $"public static bool any({typeName} x)",
                        scalar);
                }

                var zero = signature.Type.ToTypedLiteral(0);
                var vectorClass = Simd.SimdStrategy.NativeVectorClassName(
                    signature.Type,
                    signature.Dimension);
                var data = FillUnusedLanes(
                    signature,
                    "x.data",
                    zero);
                return FunctionEmitter.GenerateSimd(
                    $"public static bool any({typeName} x)",
                    signature.Shape,
                    $"!{vectorClass}.All({data}, {zero})",
                    scalar);
            }),

        Fn(
            "all",
            [
                BaseType.Bool,
                BaseType.Int,
                BaseType.UInt,
                BaseType.Float,
                BaseType.Double,
            ],
            (2, 4),
            signature => {
                var typeName = signature.Shape.TypeName;
                var scalar = string.Join(
                    " && ",
                    Range(signature.Dimension).Select(index =>
                        signature.Type == BaseType.Bool
                            ? $"x.{TypeShape.Components[index]}"
                            : $"x.{TypeShape.Components[index]} != {signature.Type.ToTypedLiteral(0)}"));
                if (signature.Type == BaseType.Bool) {
                    return FunctionEmitter.GenerateExpression(
                        $"public static bool all({typeName} x)",
                        scalar);
                }

                var zero = signature.Type.ToTypedLiteral(0);
                var one = signature.Type.ToTypedLiteral(1);
                var vectorClass = Simd.SimdStrategy.NativeVectorClassName(
                    signature.Type,
                    signature.Dimension);
                var data = FillUnusedLanes(
                    signature,
                    "x.data",
                    one);
                return FunctionEmitter.GenerateSimd(
                    $"public static bool all({typeName} x)",
                    signature.Shape,
                    $"{vectorClass}.None({data}, {zero})",
                    scalar);
            }),

        Fn(
            "select",
            [
                BaseType.Int,
                BaseType.UInt,
                BaseType.Float,
                BaseType.Double,
            ],
            (1, 4),
            signature => {
                var maskType = BaseType.Bool.ToTypeName(
                    1,
                    signature.Dimension);
                var typeName = signature.Shape.TypeName;
                if (signature.Dimension == 1) {
                    return FunctionEmitter.GenerateExpression(
                        $"public static {typeName} select({typeName} a, {typeName} b, {maskType} test)",
                        "test ? b : a");
                }

                var components = string.Join(
                    ", ",
                    Range(signature.Dimension).Select(index =>
                        $"test.{TypeShape.Components[index]} ? b.{TypeShape.Components[index]} : a.{TypeShape.Components[index]}"));
                return
                [
                    .. FunctionEmitter.GenerateExpression(
                        $"public static {typeName} select({typeName} a, {typeName} b, {maskType} test)",
                        $"new {typeName}({components})"),
                    .. FunctionEmitter.GenerateExpression(
                        $"public static {typeName} select({typeName} a, {typeName} b, bool test)",
                        "test ? b : a"),
                ];
            }),

        Fn(
            "csum",
            [
                BaseType.Int,
                BaseType.UInt,
                BaseType.Float,
                BaseType.Double,
            ],
            (2, 4),
            signature => {
                var typeName = signature.Shape.TypeName;
                var vectorClass = Simd.SimdStrategy.NativeVectorClassName(
                    signature.Type,
                    signature.Dimension);
                var zero = signature.Type.ToTypedLiteral(0);
                var data = FillUnusedLanes(
                    signature,
                    "x.data",
                    zero);
                var scalar = string.Join(
                    " + ",
                    Range(signature.Dimension).Select(index =>
                        $"x.{TypeShape.Components[index]}"));
                return FunctionEmitter.GenerateSimd(
                    $"public static {signature.Type.ToBaseTypeName()} csum({typeName} x)",
                    signature.Shape,
                    $"{vectorClass}.Sum({data})",
                    scalar);
            }),

        Fn(
            "cmin",
            [
                BaseType.Int,
                BaseType.UInt,
                BaseType.Float,
                BaseType.Double,
            ],
            (2, 4),
            signature => {
                var typeName = signature.Shape.TypeName;
                var scalarType = signature.Type.ToBaseTypeName();
                var expression = Range(signature.Dimension - 1).Aggregate(
                    "x.x",
                    (current, index) =>
                        $"min({current}, x.{TypeShape.Components[index + 1]})");
                return FunctionEmitter.GenerateExpression(
                    $"public static {scalarType} cmin({typeName} x)",
                    expression);
            }),

        Fn(
            "cmax",
            [
                BaseType.Int,
                BaseType.UInt,
                BaseType.Float,
                BaseType.Double,
            ],
            (2, 4),
            signature => {
                var typeName = signature.Shape.TypeName;
                var scalarType = signature.Type.ToBaseTypeName();
                var expression = Range(signature.Dimension - 1).Aggregate(
                    "x.x",
                    (current, index) =>
                        $"max({current}, x.{TypeShape.Components[index + 1]})");
                return FunctionEmitter.GenerateExpression(
                    $"public static {scalarType} cmax({typeName} x)",
                    expression);
            }),
    ];
}
