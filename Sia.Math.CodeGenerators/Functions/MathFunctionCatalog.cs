namespace Sia.Math.CodeGenerators.Functions;

using System;
using System.Linq;

public static partial class MathFunctionCatalog
{
    private const string Attr = "[MethodImpl(MethodImplOptions.AggressiveInlining)]";

    public static MathFunction[] All { get; } = CreateCatalog();

    private static MathFunction[] CreateCatalog() =>
    [
        Fn("min", [BaseType.Int, BaseType.UInt, BaseType.Float, BaseType.Double], (1, 4), sig => {
            var name = sig.Shape.TypeName;
            if (sig.Dimension == 1) {
                if (sig.Type is BaseType.Float or BaseType.Double) {
                    return [Attr, $"public static {name} min({name} x, {name} y) => {name}.MinNumber(x, y);"];
                }

                return [Attr, $"public static {name} min({name} x, {name} y) => x < y ? x : y;"];
            }

            var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
            var portable = sig.Type is BaseType.Int or BaseType.UInt
                ? Simd.SimdDispatch.Return($"new {name}({vc}.Min(x.data, y.data))")
                :
                [
                    "var xv = x.data;",
                    "var yv = y.data;",
                    $"var mask = {vc}.BitwiseOr({vc}.LessThan(xv, yv), {vc}.IsNaN(yv));",
                    $"return new {name}({vc}.ConditionalSelect(mask, xv, yv));",
                ];
            return FunctionEmitter.GenerateSimd(
                $"public static {name} min({name} x, {name} y)",
                sig.Shape,
                portable,
                Simd.SimdDispatch.Return($"new {name}({PerCompBin("min", sig.Dimension)})"));
        }),

        Fn("max", [BaseType.Int, BaseType.UInt, BaseType.Float, BaseType.Double], (1, 4), sig => {
            var name = sig.Shape.TypeName;
            if (sig.Dimension == 1) {
                if (sig.Type is BaseType.Float or BaseType.Double) {
                    return [Attr, $"public static {name} max({name} x, {name} y) => {name}.MaxNumber(x, y);"];
                }

                return [Attr, $"public static {name} max({name} x, {name} y) => x > y ? x : y;"];
            }

            var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
            var portable = sig.Type is BaseType.Int or BaseType.UInt
                ? Simd.SimdDispatch.Return($"new {name}({vc}.Max(x.data, y.data))")
                :
                [
                    "var xv = x.data;",
                    "var yv = y.data;",
                    $"var mask = {vc}.BitwiseOr({vc}.GreaterThan(xv, yv), {vc}.IsNaN(yv));",
                    $"return new {name}({vc}.ConditionalSelect(mask, xv, yv));",
                ];
            return FunctionEmitter.GenerateSimd(
                $"public static {name} max({name} x, {name} y)",
                sig.Shape,
                portable,
                Simd.SimdDispatch.Return($"new {name}({PerCompBin("max", sig.Dimension)})"));
        }),

        Fn("clamp", [BaseType.Int, BaseType.UInt, BaseType.Float, BaseType.Double], (1, 4), sig => {
            var name = sig.Shape.TypeName;
            if (sig.Dimension == 1) {
                return [Attr, $"public static {name} clamp({name} v, {name} a, {name} b) => max(a, min(b, v));"];
            }

            var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
            var portable = sig.Type is BaseType.Int or BaseType.UInt
                ? Simd.SimdDispatch.Return($"new {name}({vc}.Max(a.data, {vc}.Min(b.data, v.data)))")
                :
                [
                    "var av = a.data;",
                    "var bv = b.data;",
                    "var vv = v.data;",
                    $"var mask = {vc}.BitwiseOr({vc}.LessThan(bv, vv), {vc}.IsNaN(vv));",
                    $"var t = {vc}.ConditionalSelect(mask, bv, vv);",
                    $"return new {name}({vc}.ConditionalSelect({vc}.GreaterThan(av, t), av, t));",
                ];
            var scalar = string.Join(", ", Range(sig.Dimension).Select(index =>
                $"clamp(v.{TypeShape.Components[index]}, a.{TypeShape.Components[index]}, b.{TypeShape.Components[index]})"));
            return FunctionEmitter.GenerateSimd(
                $"public static {name} clamp({name} v, {name} a, {name} b)",
                sig.Shape,
                portable,
                Simd.SimdDispatch.Return($"new {name}({scalar})"));
        }),

        Fn("saturate", [BaseType.Float, BaseType.Double], (1, 4), sig => {
            var name = sig.Shape.TypeName;
            var zero = sig.Type.ToTypedLiteral(0);
            var one = sig.Type.ToTypedLiteral(1);
            if (sig.Dimension == 1) {
                return [Attr, $"public static {name} saturate({name} x) => clamp(x, {zero}, {one});"];
            }

            return [Attr, $"public static {name} saturate({name} x) => clamp(x, new({zero}), new({one}));"];
        }),

        Fn("abs", [BaseType.Int, BaseType.Float, BaseType.Double], (1, 4), sig => {
            var name = sig.Shape.TypeName;
            if (sig.Dimension == 1) {
                return sig.Type switch {
                    BaseType.Int => [Attr, $"public static {name} abs({name} x) => max(-x, x);"],
                    BaseType.Float => [Attr, $"public static {name} abs({name} x) => asfloat(asuint(x) & 0x7FFFFFFF);"],
                    BaseType.Double => [Attr, $"public static {name} abs({name} x) => asdouble(asulong(x) & 0x7FFFFFFFFFFFFFFF);"],
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
            var scalar = sig.Type switch {
                BaseType.Int => string.Join(", ", Range(sig.Dimension).Select(index =>
                    $"abs(x.{TypeShape.Components[index]})")),
                BaseType.Float => string.Join(", ", Range(sig.Dimension).Select(index =>
                    $"asfloat(asuint(x.{TypeShape.Components[index]}) & 0x7FFFFFFF)")),
                BaseType.Double => string.Join(", ", Range(sig.Dimension).Select(index =>
                    $"asdouble(asulong(x.{TypeShape.Components[index]}) & 0x7FFFFFFFFFFFFFFF)")),
                _ => throw new ArgumentOutOfRangeException()
            };
            return FunctionEmitter.GenerateSimd(
                $"public static {name} abs({name} x)",
                sig.Shape,
                $"new {name}({vc}.Abs(x.data))",
                $"new {name}({scalar})");
        }),

        Fn("sign", [BaseType.Float, BaseType.Double], (2, 4), sig => {
            var name = sig.Shape.TypeName;
            var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
            var vt = Simd.SimdStrategy.NativeVectorTypeName(sig.Type, sig.Dimension);
            return FunctionEmitter.GenerateSimd(
                $"public static {name} sign({name} x)",
                sig.Shape,
                [
                    $"var pos = {vc}.ConditionalSelect({vc}.GreaterThan(x.data, {vt}.Zero), {vt}.One, {vt}.Zero);",
                    $"var neg = {vc}.ConditionalSelect({vc}.LessThan(x.data, {vt}.Zero), {vt}.One, {vt}.Zero);",
                    $"return new {name}(pos - neg);",
                ],
                Simd.SimdDispatch.Return($"new {name}({PerComp("sign", sig.Dimension)})"));
        }),

        Fn("rcp", [BaseType.Float, BaseType.Double], (1, 4), sig => {
            var name = sig.Shape.TypeName;
            var one = sig.Type.ToTypedLiteral(1);
            return [Attr, $"public static {name} rcp({name} x) => {one} / x;"];
        }),

        Fn("mad", [BaseType.Int, BaseType.UInt, BaseType.Float, BaseType.Double], (1, 4), sig => {
            var name = sig.Shape.TypeName;
            if (sig.Dimension == 1) {
                return [Attr, $"public static {name} mad({name} a, {name} b, {name} c) => a * b + c;"];
            }

            var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
            var portable = sig.Type is BaseType.Float or BaseType.Double
                ? $"new {name}({vc}.FusedMultiplyAdd(a.data, b.data, c.data))"
                : $"new {name}(a.data * b.data + c.data)";
            var scalar = string.Join(", ", Range(sig.Dimension).Select(index =>
                $"a.{TypeShape.Components[index]} * b.{TypeShape.Components[index]} + c.{TypeShape.Components[index]}"));
            return FunctionEmitter.GenerateSimd(
                $"public static {name} mad({name} a, {name} b, {name} c)",
                sig.Shape,
                portable,
                $"new {name}({scalar})",
                dispatch => ConfigureFusedMultiplyAdd(dispatch, sig, name));
        }),

        Fn("fmod", [BaseType.Float, BaseType.Double], (2, 4), sig => {
            var name = sig.Shape.TypeName;
            return [Attr, $"public static {name} fmod({name} x, {name} y) => x % y;"];
        }),

        Fn("modf", [BaseType.Float, BaseType.Double], (1, 4), sig => {
            var name = sig.Shape.TypeName;
            return [Attr, $"public static {name} modf({name} x, out {name} i) {{ i = trunc(x); return x - i; }}"];
        }),

        SimdUnary("floor", "Floor"),
        SimdUnary("ceil", "Ceiling"),
        SimdUnary("round", "Round"),
        SimdUnary("trunc", "Truncate"),

        Fn("frac", [BaseType.Float, BaseType.Double], (2, 4), sig => {
            var name = sig.Shape.TypeName;
            var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
            return FunctionEmitter.GenerateSimd(
                $"public static {name} frac({name} x)",
                sig.Shape,
                $"new {name}(x.data - {vc}.Floor(x.data))",
                $"new {name}({PerComp("frac", sig.Dimension)})");
        }),

        SimdLerp(),

        Fn("unlerp", [BaseType.Float, BaseType.Double], (1, 4), sig => {
            var name = sig.Shape.TypeName;
            return [Attr, $"public static {name} unlerp({name} a, {name} b, {name} x) => (x - a) / (b - a);"];
        }),

        Fn("remap", [BaseType.Float, BaseType.Double], (2, 4), sig => {
            var name = sig.Shape.TypeName;
            return [Attr, $"public static {name} remap({name} a, {name} b, {name} c, {name} d, {name} x) => lerp(c, d, unlerp(a, b, x));"];
        }),

        Fn("step", [BaseType.Float, BaseType.Double], (2, 4), sig => {
            var name = sig.Shape.TypeName;
            var zero = sig.Type.ToTypedLiteral(0);
            var one = sig.Type.ToTypedLiteral(1);
            var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
            var scalar = string.Join(", ", Range(sig.Dimension).Select(index =>
                $"x.{TypeShape.Components[index]} >= y.{TypeShape.Components[index]} ? {one} : {zero}"));
            return FunctionEmitter.GenerateSimd(
                $"public static {name} step({name} y, {name} x)",
                sig.Shape,
                $"new {name}({vc}.ConditionalSelect({vc}.GreaterThanOrEqual(x.data, y.data), {vc}.Create({one}), {vc}.Create({zero})))",
                $"new {name}({scalar})");
        }),

        Fn("smoothstep", [BaseType.Float, BaseType.Double], (2, 4), sig => {
            var name = sig.Shape.TypeName;
            var two = sig.Type.ToTypedLiteral(2);
            var three = sig.Type.ToTypedLiteral(3);
            return [
                Attr,
                $"public static {name} smoothstep({name} a, {name} b, {name} x)",
                "{",
                "    var t = saturate((x - a) / (b - a));",
                $"    return t * t * ({three} - {two} * t);",
                "}"
            ];
        }),

        Fn("tan", [BaseType.Float, BaseType.Double], (1, 4), sig => {
            var name = sig.Shape.TypeName;
            if (sig.Dimension == 1) {
                var cast = sig.Type == BaseType.Float ? "System.MathF.Tan" : "System.Math.Tan";
                return [Attr, $"public static {name} tan({name} x) => {cast}(x);"];
            }
            return [
                Attr,
                $"public static {name} tan({name} x)",
                "{",
                "    sincos(x, out var s, out var c);",
                "    return s / c;",
                "}"
            ];
        }),

        SimdTrig("sin", "Sin"), SimdTrig("cos", "Cos"),

        Trig("asin"), Trig("acos"), Trig("atan"),

        Fn("atan2", [BaseType.Float, BaseType.Double], (2, 4), sig => {
            var name = sig.Shape.TypeName;
            var scalarCall = sig.Type == BaseType.Float
                ? "global::System.MathF.Atan2"
                : "global::System.Math.Atan2";
            return [Attr, $"public static {name} atan2({name} y, {name} x) => new {name}({string.Join(", ", Range(sig.Dimension).Select(i => $"{scalarCall}(y.{TypeShape.Components[i]}, x.{TypeShape.Components[i]})"))});"];
        }),

        Fn("sinh", [BaseType.Float, BaseType.Double], (1, 4), sig => {
            var name = sig.Shape.TypeName;
            if (sig.Dimension == 1) {
                var cast = sig.Type == BaseType.Float ? "System.MathF.Sinh" : "System.Math.Sinh";
                return [Attr, $"public static {name} sinh({name} x) => {cast}(x);"];
            }
            var two = sig.Type.ToTypedLiteral(2);
            return [Attr, $"public static {name} sinh({name} x) => (exp(x) - exp(-x)) / {two};"];
        }),

        Fn("cosh", [BaseType.Float, BaseType.Double], (1, 4), sig => {
            var name = sig.Shape.TypeName;
            if (sig.Dimension == 1) {
                var cast = sig.Type == BaseType.Float ? "System.MathF.Cosh" : "System.Math.Cosh";
                return [Attr, $"public static {name} cosh({name} x) => {cast}(x);"];
            }
            var two = sig.Type.ToTypedLiteral(2);
            return [Attr, $"public static {name} cosh({name} x) => (exp(x) + exp(-x)) / {two};"];
        }),

        Fn("tanh", [BaseType.Float, BaseType.Double], (1, 4), sig => {
            var name = sig.Shape.TypeName;
            if (sig.Dimension == 1) {
                var cast = sig.Type == BaseType.Float ? "System.MathF.Tanh" : "System.Math.Tanh";
                return [Attr, $"public static {name} tanh({name} x) => {cast}(x);"];
            }
            var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
            var one = sig.Type.ToTypedLiteral(1);
            var two = sig.Type.ToTypedLiteral(2);
            return [
                Attr,
                $"public static {name} tanh({name} x)",
                "{",
                $"    var e = exp(abs(x) * {two});",
                $"    var t = {one} - {two} / (e + {one});",
                $"    return new {name}({vc}.CopySign(t.data, x.data));",
                "}"
            ];
        }),

        Fn("sincos", [BaseType.Float, BaseType.Double], (1, 4), sig => {
            var name = sig.Shape.TypeName;
            if (sig.Dimension == 1) {
                return [Attr, $"public static void sincos({name} v, out {name} s, out {name} c) {{ s = sin(v); c = cos(v); }}"];
            }

            var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
            var scalarSin = string.Join(", ", Range(sig.Dimension).Select(index =>
                $"sin(v.{TypeShape.Components[index]})"));
            var scalarCos = string.Join(", ", Range(sig.Dimension).Select(index =>
                $"cos(v.{TypeShape.Components[index]})"));
            return FunctionEmitter.GenerateSimd(
                $"public static void sincos({name} v, out {name} s, out {name} c)",
                sig.Shape,
                [
                    $"var (sin, cos) = {vc}.SinCos(v.data);",
                    $"s = new {name}(sin);",
                    $"c = new {name}(cos);",
                    "return;",
                ],
                [
                    $"s = new {name}({scalarSin});",
                    $"c = new {name}({scalarCos});",
                ]);
        }),

        Fn("radians", [BaseType.Float, BaseType.Double], (1, 4), sig => {
            var name = sig.Shape.TypeName;
            var constant = sig.Type == BaseType.Double ? "TORADIANS_DBL" : "TORADIANS";
            if (sig.Dimension == 1) {
                return [Attr, $"public static {name} radians({name} x) => x * {constant};"];
            }

            var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
            var scalar = string.Join(", ", Range(sig.Dimension).Select(index =>
                $"x.{TypeShape.Components[index]} * {constant}"));
            return FunctionEmitter.GenerateSimd(
                $"public static {name} radians({name} x)",
                sig.Shape,
                $"new {name}({vc}.DegreesToRadians(x.data))",
                $"new {name}({scalar})");
        }),

        Fn("degrees", [BaseType.Float, BaseType.Double], (1, 4), sig => {
            var name = sig.Shape.TypeName;
            var constant = sig.Type == BaseType.Double ? "TODEGREES_DBL" : "TODEGREES";
            if (sig.Dimension == 1) {
                return [Attr, $"public static {name} degrees({name} x) => x * {constant};"];
            }

            var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
            var scalar = string.Join(", ", Range(sig.Dimension).Select(index =>
                $"x.{TypeShape.Components[index]} * {constant}"));
            return FunctionEmitter.GenerateSimd(
                $"public static {name} degrees({name} x)",
                sig.Shape,
                $"new {name}({vc}.RadiansToDegrees(x.data))",
                $"new {name}({scalar})");
        }),

        SimdTrig("exp", "Exp"),

        Fn("exp2", [BaseType.Float, BaseType.Double], (2, 4), sig => {
            var name = sig.Shape.TypeName;
            var ln2 = sig.Type == BaseType.Double ? "LN2_DBL" : "LN2";
            return [Attr, $"public static {name} exp2({name} x) => exp(x * {ln2});"];
        }),

        Fn("exp10", [BaseType.Float, BaseType.Double], (2, 4), sig => {
            var name = sig.Shape.TypeName;
            var ln10 = sig.Type == BaseType.Double ? "LN10_DBL" : "LN10";
            return [Attr, $"public static {name} exp10({name} x) => exp(x * {ln10});"];
        }),

        SimdTrig("log", "Log"),

        Fn("log2", [BaseType.Float, BaseType.Double], (2, 4), sig => {
            var name = sig.Shape.TypeName;
            var log2e = sig.Type == BaseType.Double ? "LOG2E_DBL" : "LOG2E";
            return [Attr, $"public static {name} log2({name} x) => log(x) * {log2e};"];
        }),

        Fn("log10", [BaseType.Float, BaseType.Double], (2, 4), sig => {
            var name = sig.Shape.TypeName;
            var log10e = sig.Type == BaseType.Double ? "LOG10E_DBL" : "LOG10E";
            return [Attr, $"public static {name} log10({name} x) => log(x) * {log10e};"];
        }),

        Trig("pow"),

        Fn("sqrt", [BaseType.Float, BaseType.Double], (1, 4), sig => {
            var name = sig.Shape.TypeName;
            if (sig.Dimension == 1) {
                return [Attr, sig.Type == BaseType.Float
                    ? $"public static {name} sqrt({name} x) => System.MathF.Sqrt(x);"
                    : $"public static {name} sqrt({name} x) => System.Math.Sqrt(x);"];
            }

            var vectorClass = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
            return FunctionEmitter.GenerateSimd(
                $"public static {name} sqrt({name} x)",
                sig.Shape,
                $"new {name}({vectorClass}.Sqrt(x.data))",
                $"new {name}({PerComp("sqrt", sig.Dimension)})");
        }),

        Fn("rsqrt", [BaseType.Float, BaseType.Double], (1, 4), sig => {
            var name = sig.Shape.TypeName;
            if (sig.Shape.IsSimdEligible) {
                var vt = Simd.SimdStrategy.NativeVectorTypeName(sig.Type, sig.Dimension);
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return FunctionEmitter.GenerateSimd(
                    $"public static {name} rsqrt({name} x)",
                    sig.Shape,
                    $"new {name}({vt}.One / {vc}.Sqrt(x.data))",
                    $"new {name}({PerComp("rsqrt", sig.Dimension)})");
            }
            return [Attr, $"public static {name} rsqrt({name} x) => 1.0f / sqrt(x);"];
        }),

        .. GeometryFunctions(),

        .. ReductionFunctions(),

        .. BitFunctions(),
    ];

    private static MathFunction Fn(string name, BaseType[] types, (int, int) dims, Func<MathSignature, string[]> gen) =>
        new() {
            Name = name, Types = types, DimRange = dims, Generator = gen
        };

    private static int[] Range(int n) => Enumerable.Range(0, n).ToArray();

    private static string PerComp(string func, int dim) =>
        string.Join(", ", Range(dim).Select(i => $"{func}(x.{TypeShape.Components[i]})"));

    private static string PerCompBin(string func, int dim) =>
        string.Join(", ", Range(dim).Select(i => $"{func}(x.{TypeShape.Components[i]}, y.{TypeShape.Components[i]})"));

    private static MathFunction Trig(string name)
    {
        var mathName = name switch {
            "asin" => "Asin",
            "acos" => "Acos",
            "atan" => "Atan",
            "pow" => "Pow",
            _ => char.ToUpper(name[0]) + name.Substring(1)
        };

        return Fn(name, [BaseType.Float, BaseType.Double], (1, 4), sig => {
            var typeName = sig.Shape.TypeName;
            if (sig.Dimension == 1) {
                var cast = sig.Type == BaseType.Float ? $"System.MathF.{mathName}" : $"System.Math.{mathName}";
                if (name == "pow") {
                    return [Attr, $"public static {typeName} {name}({typeName} x, {typeName} y) => {cast}(x, y);"];
                }

                return [Attr, $"public static {typeName} {name}({typeName} x) => {cast}(x);"];
            }
            if (name == "pow") {
                return [Attr, $"public static {typeName} pow({typeName} x, {typeName} y) => new({PerCompBin("pow", sig.Dimension)});"];
            }

            return [Attr, $"public static {typeName} {name}({typeName} x) => new({PerComp(name, sig.Dimension)});"];
        });
    }

    public static void GenerateAll(SourceBuilder source)
    {
        foreach (var function in All) {
            foreach (var type in function.Types) {
                for (var dimension = function.DimRange.Min;
                    dimension <= function.DimRange.Max;
                    dimension++) {
                    var shape = new TypeShape(type, dimension, 1, Features.All);
                    var signature = new MathSignature(function.Name, type, dimension, shape);
                    source.Lines(function.Generator(signature));
                    source.Line();
                }
            }
        }
    }
}
