namespace Sia.Math.CodeGenerators.Functions;

using System.Linq;

public static partial class MathFunctionCatalog
{
    private static MathFunction[] GeometryFunctions() =>
    [
        Fn("dot", [BaseType.Int, BaseType.UInt, BaseType.Float, BaseType.Double], (1, 4), signature => {
            var typeName = signature.Shape.TypeName;
            var scalarType = signature.Type.ToBaseTypeName();
            if (signature.Dimension == 1) {
                return FunctionEmitter.GenerateExpression(
                    $"public static {typeName} dot({typeName} x, {typeName} y)",
                    "x * y");
            }

            var vectorClass = Simd.SimdStrategy.NativeVectorClassName(
                signature.Type,
                signature.Dimension);
            var zero = signature.Type.ToTypedLiteral(0);
            var left = FillUnusedLanes(signature, "x.data", zero);
            var right = FillUnusedLanes(signature, "y.data", zero);
            var scalar = string.Join(
                " + ",
                Range(signature.Dimension).Select(index =>
                    $"x.{TypeShape.Components[index]} * y.{TypeShape.Components[index]}"));

            return FunctionEmitter.GenerateSimd(
                $"public static {scalarType} dot({typeName} x, {typeName} y)",
                signature.Shape,
                $"{vectorClass}.Dot({left}, {right})",
                scalar,
                dispatch => ConfigureDot(
                    dispatch,
                    signature,
                    left,
                    right));
        }),

        Fn("length", [BaseType.Float, BaseType.Double], (1, 4), signature => {
            var typeName = signature.Shape.TypeName;
            var scalarType = signature.Type.ToBaseTypeName();
            var expression = signature.Dimension == 1
                ? "abs(x)"
                : "sqrt(dot(x, x))";
            return FunctionEmitter.GenerateExpression(
                $"public static {scalarType} length({typeName} x)",
                expression);
        }),

        Fn("lengthsq", [BaseType.Float, BaseType.Double], (1, 4), signature => {
            var typeName = signature.Shape.TypeName;
            var scalarType = signature.Type.ToBaseTypeName();
            var expression = signature.Dimension == 1
                ? "x * x"
                : "dot(x, x)";
            return FunctionEmitter.GenerateExpression(
                $"public static {scalarType} lengthsq({typeName} x)",
                expression);
        }),

        Fn("distance", [BaseType.Float, BaseType.Double], (1, 4), signature => {
            var typeName = signature.Shape.TypeName;
            var scalarType = signature.Type.ToBaseTypeName();
            var expression = signature.Dimension == 1
                ? "abs(y - x)"
                : "length(y - x)";
            return FunctionEmitter.GenerateExpression(
                $"public static {scalarType} distance({typeName} x, {typeName} y)",
                expression);
        }),

        Fn("distancesq", [BaseType.Float, BaseType.Double], (1, 4), signature => {
            var typeName = signature.Shape.TypeName;
            var scalarType = signature.Type.ToBaseTypeName();
            var expression = signature.Dimension == 1
                ? "(y - x) * (y - x)"
                : "lengthsq(y - x)";
            return FunctionEmitter.GenerateExpression(
                $"public static {scalarType} distancesq({typeName} x, {typeName} y)",
                expression);
        }),

        Fn("normalize", [BaseType.Float, BaseType.Double], (2, 4), signature => {
            var typeName = signature.Shape.TypeName;
            var vectorClass = Simd.SimdStrategy.NativeVectorClassName(
                signature.Type,
                signature.Dimension);
            var lengthSquared = ComponentSumOfSquares(signature);
            var scalar = string.Join(
                ", ",
                Range(signature.Dimension).Select(index =>
                    $"x.{TypeShape.Components[index]} / length"));

            return FunctionEmitter.GenerateSimd(
                $"public static {typeName} normalize({typeName} x)",
                signature.Shape,
                Simd.SimdDispatch.Return(
                    $"new {typeName}(x.data / {vectorClass}.Sqrt({vectorClass}.Create(dot(x, x))))"),
                [
                    $"var length = sqrt({lengthSquared});",
                    $"return new {typeName}({scalar});",
                ],
                dispatch => ConfigureNormalize(dispatch, signature, typeName, vectorClass));
        }),

        Fn("normalizesafe", [BaseType.Float, BaseType.Double], (2, 4), signature => {
            var typeName = signature.Shape.TypeName;
            var minimumNormal = signature.Type == BaseType.Double
                ? "DBL_MIN_NORMAL"
                : "FLT_MIN_NORMAL";
            var vectorClass = Simd.SimdStrategy.NativeVectorClassName(
                signature.Type,
                signature.Dimension);
            var scalar = string.Join(
                ", ",
                Range(signature.Dimension).Select(index =>
                    $"x.{TypeShape.Components[index]} * inverseLength"));

            var vectorType = Simd.SimdStrategy.NativeVectorTypeName(
                signature.Type,
                signature.Dimension);

            return FunctionEmitter.GenerateSimd(
                $"public static {typeName} normalizesafe({typeName} x, {typeName} defaultvalue = default)",
                signature.Shape,
                [
                    "var len = dot(x, x);",
                    $"var mask = {vectorClass}.GreaterThan({vectorClass}.Create(len), {vectorClass}.Create({minimumNormal}));",
                    $"return new {typeName}({vectorClass}.ConditionalSelect(mask, x.data * {vectorClass}.Create(rsqrt(len)), defaultvalue.data));",
                ],
                [
                    $"var scalarLengthSquared = {ComponentSumOfSquares(signature)};",
                    $"if (!(scalarLengthSquared > {minimumNormal})) return defaultvalue;",
                    "var inverseLength = rsqrt(scalarLengthSquared);",
                    $"return new {typeName}({scalar});",
                ],
                dispatch => ConfigureNormalizeSafe(
                    dispatch,
                    signature,
                    typeName,
                    vectorClass,
                    vectorType,
                    minimumNormal));
        }),

        Fn("reflect", [BaseType.Float, BaseType.Double], (2, 4), signature => {
            var typeName = signature.Shape.TypeName;
            var two = signature.Type.ToTypedLiteral(2);
            return FunctionEmitter.GenerateExpression(
                $"public static {typeName} reflect({typeName} i, {typeName} n)",
                $"i - {two} * n * dot(i, n)");
        }),

        Fn("refract", [BaseType.Float, BaseType.Double], (2, 4), signature => {
            var typeName = signature.Shape.TypeName;
            var scalarType = signature.Type.ToBaseTypeName();
            var one = signature.Type.ToTypedLiteral(1);
            var zero = signature.Type.ToTypedLiteral(0);
            return FunctionEmitter.GenerateBlock(
                $"public static {typeName} refract({typeName} i, {typeName} n, {scalarType} eta)",
                "var ni = dot(n, i);",
                $"var k = {one} - eta * eta * ({one} - ni * ni);",
                $"return select({zero}, eta * i - (eta * ni + sqrt(k)) * n, k >= 0);");
        }),

        Fn("project", [BaseType.Float, BaseType.Double], (2, 4), signature => {
            var typeName = signature.Shape.TypeName;
            return FunctionEmitter.GenerateExpression(
                $"public static {typeName} project({typeName} a, {typeName} b)",
                "(dot(a, b) / dot(b, b)) * b");
        }),

        Fn("projectsafe", [BaseType.Float, BaseType.Double], (2, 4), signature => {
            var typeName = signature.Shape.TypeName;
            return FunctionEmitter.GenerateBlock(
                $"public static {typeName} projectsafe({typeName} a, {typeName} b, {typeName} defaultValue = default)",
                "var projection = project(a, b);",
                "return select(defaultValue, projection, all(isfinite(projection)));");
        }),

        Fn("faceforward", [BaseType.Float, BaseType.Double], (2, 4), signature => {
            var typeName = signature.Shape.TypeName;
            var zero = signature.Type.ToTypedLiteral(0);
            return FunctionEmitter.GenerateExpression(
                $"public static {typeName} faceforward({typeName} n, {typeName} i, {typeName} ng)",
                $"select(n, -n, dot(ng, i) >= {zero})");
        }),
    ];

    private static string ComponentSumOfSquares(MathSignature signature) =>
        string.Join(
            " + ",
            Range(signature.Dimension).Select(index =>
                $"x.{TypeShape.Components[index]} * x.{TypeShape.Components[index]}"));

    private static void ConfigureDot(
        Simd.SimdDispatch dispatch,
        MathSignature signature,
        string left,
        string right)
    {
        if (signature.Type == BaseType.Float) {
            var mask = signature.Dimension switch {
                2 => "0x31",
                3 => "0x71",
                _ => "0xF1",
            };
            dispatch.WithX86(
                "global::System.Runtime.Intrinsics.X86.Sse41.IsSupported",
                $"return global::System.Runtime.Intrinsics.X86.Sse41.DotProduct({left}, {right}, {mask}).ToScalar();");
            dispatch.WithArm(
                "global::System.Runtime.Intrinsics.Arm.AdvSimd.Arm64.IsSupported",
                $"var product = global::System.Runtime.Intrinsics.Arm.AdvSimd.Multiply({left}, {right});",
                "var pair = global::System.Runtime.Intrinsics.Arm.AdvSimd.Arm64.AddPairwise(product, product);",
                "return global::System.Runtime.Intrinsics.Arm.AdvSimd.Arm64.AddPairwise(pair, pair).ToScalar();");
            return;
        }

        if (signature is not { Type: BaseType.Double, Dimension: 2 }) {
            return;
        }

        dispatch.WithX86(
            "global::System.Runtime.Intrinsics.X86.Sse3.IsSupported",
            "var product = global::System.Runtime.Intrinsics.X86.Sse2.Multiply(x.data, y.data);",
            "return global::System.Runtime.Intrinsics.X86.Sse3.HorizontalAdd(product, product).ToScalar();");
        dispatch.WithArm(
            "global::System.Runtime.Intrinsics.Arm.AdvSimd.Arm64.IsSupported",
            "var product = global::System.Runtime.Intrinsics.Arm.AdvSimd.Arm64.Multiply(x.data, y.data);",
            "return global::System.Runtime.Intrinsics.Arm.AdvSimd.Arm64.AddPairwise(product, product).ToScalar();");
    }

    private static bool TryGetBroadcastLengthSquared(
        MathSignature signature,
        out string x86Guard,
        out string[] x86Statements,
        out string armGuard,
        out string[] armStatements)
    {
        var zero = signature.Type.ToTypedLiteral(0);
        var data = FillUnusedLanes(signature, "x.data", zero);

        if (signature.Type == BaseType.Float) {
            var mask = signature.Dimension switch {
                2 => "0x3F",
                3 => "0x7F",
                _ => "0xFF",
            };
            x86Guard = "global::System.Runtime.Intrinsics.X86.Sse41.IsSupported";
            x86Statements =
            [
                $"var lenSq = global::System.Runtime.Intrinsics.X86.Sse41.DotProduct({data}, {data}, {mask});",
            ];
            armGuard = "global::System.Runtime.Intrinsics.Arm.AdvSimd.Arm64.IsSupported";
            armStatements =
            [
                $"var product = global::System.Runtime.Intrinsics.Arm.AdvSimd.Multiply({data}, {data});",
                "var pair = global::System.Runtime.Intrinsics.Arm.AdvSimd.Arm64.AddPairwise(product, product);",
                "var lenSq = global::System.Runtime.Intrinsics.Arm.AdvSimd.Arm64.AddPairwise(pair, pair);",
            ];
            return true;
        }

        if (signature is not { Type: BaseType.Double, Dimension: 2 }) {
            x86Guard = armGuard = string.Empty;
            x86Statements = armStatements = [];
            return false;
        }

        x86Guard = "global::System.Runtime.Intrinsics.X86.Sse3.IsSupported";
        x86Statements =
        [
            "var product = global::System.Runtime.Intrinsics.X86.Sse2.Multiply(x.data, x.data);",
            "var lenSq = global::System.Runtime.Intrinsics.X86.Sse3.HorizontalAdd(product, product);",
        ];
        armGuard = "global::System.Runtime.Intrinsics.Arm.AdvSimd.Arm64.IsSupported";
        armStatements =
        [
            "var product = global::System.Runtime.Intrinsics.Arm.AdvSimd.Arm64.Multiply(x.data, x.data);",
            "var lenSq = global::System.Runtime.Intrinsics.Arm.AdvSimd.Arm64.AddPairwise(product, product);",
        ];
        return true;
    }

    private static void ConfigureNormalize(
        Simd.SimdDispatch dispatch,
        MathSignature signature,
        string typeName,
        string vectorClass)
    {
        if (!TryGetBroadcastLengthSquared(signature, out var x86Guard, out var x86Statements, out var armGuard, out var armStatements)) {
            return;
        }

        string[] Tail() => [$"return new {typeName}(x.data / {vectorClass}.Sqrt(lenSq));"];
        dispatch.WithX86(x86Guard, [.. x86Statements, .. Tail()]);
        dispatch.WithArm(armGuard, [.. armStatements, .. Tail()]);
    }

    private static void ConfigureNormalizeSafe(
        Simd.SimdDispatch dispatch,
        MathSignature signature,
        string typeName,
        string vectorClass,
        string vectorType,
        string minimumNormal)
    {
        if (!TryGetBroadcastLengthSquared(signature, out var x86Guard, out var x86Statements, out var armGuard, out var armStatements)) {
            return;
        }

        string[] Tail() =>
        [
            $"var mask = {vectorClass}.GreaterThan(lenSq, {vectorClass}.Create({minimumNormal}));",
            $"var invLen = {vectorType}.One / {vectorClass}.Sqrt(lenSq);",
            $"return new {typeName}({vectorClass}.ConditionalSelect(mask, x.data * invLen, defaultvalue.data));",
        ];
        dispatch.WithX86(x86Guard, [.. x86Statements, .. Tail()]);
        dispatch.WithArm(armGuard, [.. armStatements, .. Tail()]);
    }
}
