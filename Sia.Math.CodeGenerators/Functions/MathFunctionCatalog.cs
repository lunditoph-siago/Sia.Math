using System;
using System.CodeDom.Compiler;
using System.Linq;

namespace Sia.Math.CodeGenerators.Functions;

public static class MathFunctionCatalog
{
    private const string Attr = "[MethodImpl(MethodImplOptions.AggressiveInlining)]";

    public static MathFunction[] All { get; } = CreateCatalog();

    private static MathFunction[] CreateCatalog() => new MathFunction[]
    {
        Fn("min", [BaseType.Int, BaseType.UInt, BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Dimension == 1)
            {
                if (sig.Type is BaseType.Float or BaseType.Double)
                    return [Attr, $"public static {name} min({name} x, {name} y) => {name}.MinNumber(x, y);"];
                return [Attr, $"public static {name} min({name} x, {name} y) => x < y ? x : y;"];
            }
            if (sig.Shape.IsSimdEligible)
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                if (sig.Type is BaseType.Int or BaseType.UInt)
                    return [Attr, $"public static {name} min({name} x, {name} y) => new {name}({vc}.Min(x.data, y.data));"];
                return [
                    Attr,
                    $"public static {name} min({name} x, {name} y)",
                    "{",
                    "    var xv = x.data;",
                    "    var yv = y.data;",
                    $"    var mask = {vc}.BitwiseOr({vc}.LessThan(xv, yv), {vc}.IsNaN(yv));",
                    $"    return new {name}({vc}.ConditionalSelect(mask, xv, yv));",
                    "}"
                ];
            }
            return [Attr, $"public static {name} min({name} x, {name} y) => new({PerCompBin("min", sig.Dimension)});"];
        }),

        Fn("max", [BaseType.Int, BaseType.UInt, BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Dimension == 1)
            {
                if (sig.Type is BaseType.Float or BaseType.Double)
                    return [Attr, $"public static {name} max({name} x, {name} y) => {name}.MaxNumber(x, y);"];
                return [Attr, $"public static {name} max({name} x, {name} y) => x > y ? x : y;"];
            }
            if (sig.Shape.IsSimdEligible)
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                if (sig.Type is BaseType.Int or BaseType.UInt)
                    return [Attr, $"public static {name} max({name} x, {name} y) => new {name}({vc}.Max(x.data, y.data));"];
                return [
                    Attr,
                    $"public static {name} max({name} x, {name} y)",
                    "{",
                    "    var xv = x.data;",
                    "    var yv = y.data;",
                    $"    var mask = {vc}.BitwiseOr({vc}.GreaterThan(xv, yv), {vc}.IsNaN(yv));",
                    $"    return new {name}({vc}.ConditionalSelect(mask, xv, yv));",
                    "}"
                ];
            }
            return [Attr, $"public static {name} max({name} x, {name} y) => new({PerCompBin("max", sig.Dimension)});"];
        }),

        Fn("clamp", [BaseType.Int, BaseType.UInt, BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Dimension >= 2 && sig.Type is BaseType.Int or BaseType.UInt && sig.Shape.IsSimdEligible)
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [Attr, $"public static {name} clamp({name} v, {name} a, {name} b) => new {name}({vc}.Max(a.data, {vc}.Min(b.data, v.data)));"];
            }
            if (sig.Dimension >= 2 && sig.Type is BaseType.Float or BaseType.Double && sig.Shape.IsSimdEligible)
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [
                    Attr,
                    $"public static {name} clamp({name} v, {name} a, {name} b)",
                    "{",
                    "    var av = a.data;",
                    "    var bv = b.data;",
                    "    var vv = v.data;",
                    $"    var mask = {vc}.BitwiseOr({vc}.LessThan(bv, vv), {vc}.IsNaN(vv));",
                    $"    var t = {vc}.ConditionalSelect(mask, bv, vv);",
                    $"    return new {name}({vc}.ConditionalSelect({vc}.GreaterThan(av, t), av, t));",
                    "}"
                ];
            }
            return [Attr, $"public static {name} clamp({name} v, {name} a, {name} b) => max(a, min(b, v));"];
        }),

        Fn("saturate", [BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var zero = sig.Type.ToTypedLiteral(0);
            var one = sig.Type.ToTypedLiteral(1);
            if (sig.Dimension == 1)
                return [Attr, $"public static {name} saturate({name} x) => clamp(x, {zero}, {one});"];
            return [Attr, $"public static {name} saturate({name} x) => clamp(x, new({zero}), new({one}));"];
        }),

        Fn("abs", [BaseType.Int, BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Dimension == 1)
            {
                return sig.Type switch
                {
                    BaseType.Int => [Attr, $"public static {name} abs({name} x) => max(-x, x);"],
                    BaseType.Float => [Attr, $"public static {name} abs({name} x) => asfloat(asuint(x) & 0x7FFFFFFF);"],
                    BaseType.Double => [Attr, $"public static {name} abs({name} x) => asdouble(asulong(x) & 0x7FFFFFFFFFFFFFFF);"],
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            if (sig.Type is BaseType.Int or BaseType.Float or BaseType.Double && sig.Shape.IsSimdEligible)
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [Attr, $"public static {name} abs({name} x) => new {name}({vc}.Abs(x.data));"];
            }
            return sig.Type switch
            {
                BaseType.Int => [Attr, $"public static {name} abs({name} x) => new {name}({string.Join(", ", Range(sig.Dimension).Select(i => $"abs(x.{TypeShape.Components[i]})"))});"],
                BaseType.Float => [Attr, $"public static {name} abs({name} x) => new({string.Join(", ", Range(sig.Dimension).Select(i => $"asfloat(asuint(x.{TypeShape.Components[i]}) & 0x7FFFFFFF)"))});"],
                BaseType.Double => [Attr, $"public static {name} abs({name} x) => new({string.Join(", ", Range(sig.Dimension).Select(i => $"asdouble(asulong(x.{TypeShape.Components[i]}) & 0x7FFFFFFFFFFFFFFF)"))});"],
                _ => throw new ArgumentOutOfRangeException()
            };
        }),

        Fn("sign", [BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Shape.IsSimdEligible)
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                var vt = Simd.SimdStrategy.NativeVectorTypeName(sig.Type, sig.Dimension);
                return [
                    Attr,
                    $"public static {name} sign({name} x)",
                    "{",
                    $"    var pos = {vc}.ConditionalSelect({vc}.GreaterThan(x.data, {vt}.Zero), {vt}.One, {vt}.Zero);",
                    $"    var neg = {vc}.ConditionalSelect({vc}.LessThan(x.data, {vt}.Zero), {vt}.One, {vt}.Zero);",
                    $"    return new {name}(pos - neg);",
                    "}"
                ];
            }
            return [Attr, $"public static {name} sign({name} x) => new {name}({PerComp("sign", sig.Dimension)});"];
        }),

        Fn("rcp", [BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var one = sig.Type.ToTypedLiteral(1);
            return [Attr, $"public static {name} rcp({name} x) => {one} / x;"];
        }),

        Fn("mad", [BaseType.Int, BaseType.UInt, BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Dimension >= 2 && sig.Type is BaseType.Float or BaseType.Double && sig.Shape.IsSimdEligible)
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [Attr, $"public static {name} mad({name} a, {name} b, {name} c) => new {name}({vc}.FusedMultiplyAdd(a.data, b.data, c.data));"];
            }
            return [Attr, $"public static {name} mad({name} a, {name} b, {name} c) => a * b + c;"];
        }),

        Fn("fmod", [BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            return [Attr, $"public static {name} fmod({name} x, {name} y) => x % y;"];
        }),

        Fn("modf", [BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            return [Attr, $"public static {name} modf({name} x, out {name} i) {{ i = trunc(x); return x - i; }}"];
        }),

        Fn("floor", [BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Shape.IsSimdEligible)
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [Attr, $"public static {name} floor({name} x) => new {name}({vc}.Floor(x.data));"];
            }
            return [Attr, $"public static {name} floor({name} x) => new {name}({PerComp("floor", sig.Dimension)});"];
        }),

        Fn("ceil", [BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Shape.IsSimdEligible)
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [Attr, $"public static {name} ceil({name} x) => new {name}({vc}.Ceiling(x.data));"];
            }
            return [Attr, $"public static {name} ceil({name} x) => new {name}({PerComp("ceil", sig.Dimension)});"];
        }),

        Fn("round", [BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Shape.IsSimdEligible)
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [Attr, $"public static {name} round({name} x) => new {name}({vc}.Round(x.data));"];
            }
            return [Attr, $"public static {name} round({name} x) => new {name}({PerComp("round", sig.Dimension)});"];
        }),

        Fn("trunc", [BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Shape.IsSimdEligible)
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [Attr, $"public static {name} trunc({name} x) => new {name}({vc}.Truncate(x.data));"];
            }
            return [Attr, $"public static {name} trunc({name} x) => new {name}({PerComp("trunc", sig.Dimension)});"];
        }),

        Fn("frac", [BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Shape.IsSimdEligible)
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [Attr, $"public static {name} frac({name} x) => new {name}(x.data - {vc}.Floor(x.data));"];
            }
            return [Attr, $"public static {name} frac({name} x) => new {name}({PerComp("frac", sig.Dimension)});"];
        }),

        Fn("lerp", [BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var scalar = sig.Type.ToBaseTypeName();
            if (sig.Dimension >= 2 && sig.Type == BaseType.Float && sig.Shape.IsSimdEligible)
                return [
                    Attr, $"public static {name} lerp({name} a, {name} b, {name} s) => new {name}(Vector128.Lerp(a.data, b.data, s.data));",
                    Attr, $"public static {name} lerp({name} a, {name} b, {scalar} s) => new {name}(Vector128.Lerp(a.data, b.data, Vector128.Create(s)));"
                ];
            if (sig.Dimension >= 2 && sig.Shape.IsSimdEligible)
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [
                    Attr, $"public static {name} lerp({name} a, {name} b, {name} s) => new {name}({vc}.FusedMultiplyAdd(s.data, b.data - a.data, a.data));",
                    Attr, $"public static {name} lerp({name} a, {name} b, {scalar} s) => new {name}({vc}.FusedMultiplyAdd({vc}.Create(s), b.data - a.data, a.data));"
                ];
            }
            return [Attr, $"public static {name} lerp({name} a, {name} b, {name} s) => a + s * (b - a);"];
        }),

        Fn("unlerp", [BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            return [Attr, $"public static {name} unlerp({name} a, {name} b, {name} x) => (x - a) / (b - a);"];
        }),

        Fn("remap", [BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            return [Attr, $"public static {name} remap({name} a, {name} b, {name} c, {name} d, {name} x) => lerp(c, d, unlerp(a, b, x));"];
        }),

        Fn("step", [BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var zero = sig.Type.ToTypedLiteral(0);
            var one = sig.Type.ToTypedLiteral(1);
            if (sig.Shape.IsSimdEligible)
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [Attr, $"public static {name} step({name} y, {name} x) => new {name}({vc}.ConditionalSelect({vc}.GreaterThanOrEqual(x.data, y.data), {vc}.Create({one}), {vc}.Create({zero})));"];
            }
            return [Attr, $"public static {name} step({name} y, {name} x) => select({zero}, {one}, x >= y);"];
        }),

        Fn("smoothstep", [BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
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

        Fn("tan", [BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Dimension == 1)
            {
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

        Fn("atan2", [BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var scalarCall = sig.Type == BaseType.Float
                ? "global::System.MathF.Atan2"
                : "global::System.Math.Atan2";
            return [Attr, $"public static {name} atan2({name} y, {name} x) => new {name}({string.Join(", ", Range(sig.Dimension).Select(i => $"{scalarCall}(y.{TypeShape.Components[i]}, x.{TypeShape.Components[i]})"))});"];
        }),

        Fn("sinh", [BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Dimension == 1)
            {
                var cast = sig.Type == BaseType.Float ? "System.MathF.Sinh" : "System.Math.Sinh";
                return [Attr, $"public static {name} sinh({name} x) => {cast}(x);"];
            }
            var two = sig.Type.ToTypedLiteral(2);
            return [Attr, $"public static {name} sinh({name} x) => (exp(x) - exp(-x)) / {two};"];
        }),

        Fn("cosh", [BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Dimension == 1)
            {
                var cast = sig.Type == BaseType.Float ? "System.MathF.Cosh" : "System.Math.Cosh";
                return [Attr, $"public static {name} cosh({name} x) => {cast}(x);"];
            }
            var two = sig.Type.ToTypedLiteral(2);
            return [Attr, $"public static {name} cosh({name} x) => (exp(x) + exp(-x)) / {two};"];
        }),

        Fn("tanh", [BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Dimension == 1)
            {
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

        Fn("sincos", [BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Dimension >= 2 && sig.Shape.IsSimdEligible)
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [
                    Attr,
                    $"public static void sincos({name} v, out {name} s, out {name} c)",
                    "{",
                    $"    var (sin, cos) = {vc}.SinCos(v.data);",
                    $"    s = new {name}(sin);",
                    $"    c = new {name}(cos);",
                    "}"
                ];
            }
            return [Attr, $"public static void sincos({name} v, out {name} s, out {name} c) {{ s = sin(v); c = cos(v); }}"];
        }),

        Fn("radians", [BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Dimension >= 2 && sig.Shape.IsSimdEligible)
            {
                if (sig.Type == BaseType.Float)
                    return [Attr, $"public static {name} radians({name} x) => new {name}(Vector128.DegreesToRadians(x.data));"];
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [Attr, $"public static {name} radians({name} x) => new {name}(x.data * {vc}.Create(TORADIANS_DBL));"];
            }
            var constant = sig.Type == BaseType.Double ? "TORADIANS_DBL" : "TORADIANS";
            return [Attr, $"public static {name} radians({name} x) => x * {constant};"];
        }),

        Fn("degrees", [BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Dimension >= 2 && sig.Shape.IsSimdEligible)
            {
                if (sig.Type == BaseType.Float)
                    return [Attr, $"public static {name} degrees({name} x) => new {name}(Vector128.RadiansToDegrees(x.data));"];
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [Attr, $"public static {name} degrees({name} x) => new {name}(x.data * {vc}.Create(TODEGREES_DBL));"];
            }
            var constant = sig.Type == BaseType.Double ? "TODEGREES_DBL" : "TODEGREES";
            return [Attr, $"public static {name} degrees({name} x) => x * {constant};"];
        }),

        SimdTrig("exp", "Exp"),

        Fn("exp2", [BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var ln2 = sig.Type == BaseType.Double ? "LN2_DBL" : "LN2";
            return [Attr, $"public static {name} exp2({name} x) => exp(x * {ln2});"];
        }),

        Fn("exp10", [BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var ln10 = sig.Type == BaseType.Double ? "LN10_DBL" : "LN10";
            return [Attr, $"public static {name} exp10({name} x) => exp(x * {ln10});"];
        }),

        SimdTrig("log", "Log"),

        Fn("log2", [BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var log2e = sig.Type == BaseType.Double ? "LOG2E_DBL" : "LOG2E";
            return [Attr, $"public static {name} log2({name} x) => log(x) * {log2e};"];
        }),

        Fn("log10", [BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var log10e = sig.Type == BaseType.Double ? "LOG10E_DBL" : "LOG10E";
            return [Attr, $"public static {name} log10({name} x) => log(x) * {log10e};"];
        }),

        Trig("pow"),

        Fn("sqrt", [BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Dimension == 1)
                return [Attr, sig.Type == BaseType.Float
                    ? $"public static {name} sqrt({name} x) => System.MathF.Sqrt(x);"
                    : $"public static {name} sqrt({name} x) => System.Math.Sqrt(x);"];
            if (sig.Shape.IsSimdEligible)
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [Attr, $"public static {name} sqrt({name} x) => new {name}({vc}.Sqrt(x.data));"];
            }
            return [Attr, $"public static {name} sqrt({name} x) => new({PerComp("sqrt", sig.Dimension)});"];
        }),

        Fn("rsqrt", [BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Shape.IsSimdEligible)
            {
                var vt = Simd.SimdStrategy.NativeVectorTypeName(sig.Type, sig.Dimension);
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [Attr, $"public static {name} rsqrt({name} x) => new {name}({vt}.One / {vc}.Sqrt(x.data));"];
            }
            return [Attr, $"public static {name} rsqrt({name} x) => 1.0f / sqrt(x);"];
        }),

        Fn("dot", [BaseType.Int, BaseType.UInt, BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var baseName = sig.Type.ToBaseTypeName();
            if (sig.Dimension == 1)
                return [Attr, $"public static {name} dot({name} x, {name} y) => x * y;"];
            if (sig.Shape.IsSimdEligible)
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [Attr, $"public static {baseName} dot({name} x, {name} y) => {vc}.Dot(x.data, y.data);"];
            }
            return [Attr, $"public static {baseName} dot({name} x, {name} y) => {string.Join(" + ", Range(sig.Dimension).Select(i => $"x.{TypeShape.Components[i]} * y.{TypeShape.Components[i]}"))};"];
        }),

        Fn("length", [BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var baseName = sig.Type.ToBaseTypeName();
            if (sig.Dimension == 1)
                return [Attr, $"public static {baseName} length({name} x) => abs(x);"];
            return [Attr, $"public static {baseName} length({name} x) => sqrt(dot(x, x));"];
        }),

        Fn("lengthsq", [BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var baseName = sig.Type.ToBaseTypeName();
            if (sig.Dimension == 1)
                return [Attr, $"public static {baseName} lengthsq({name} x) => x * x;"];
            return [Attr, $"public static {baseName} lengthsq({name} x) => dot(x, x);"];
        }),

        Fn("distance", [BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var baseName = sig.Type.ToBaseTypeName();
            if (sig.Dimension == 1)
                return [Attr, $"public static {baseName} distance({name} x, {name} y) => abs(y - x);"];
            return [Attr, $"public static {baseName} distance({name} x, {name} y) => length(y - x);"];
        }),

        Fn("distancesq", [BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var baseName = sig.Type.ToBaseTypeName();
            if (sig.Dimension == 1)
                return [Attr, $"public static {baseName} distancesq({name} x, {name} y) => (y - x) * (y - x);"];
            return [Attr, $"public static {baseName} distancesq({name} x, {name} y) => lengthsq(y - x);"];
        }),

        Fn("normalize", [BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Shape.IsSimdEligible)
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [Attr, $"public static {name} normalize({name} x) => new {name}(x.data / {vc}.Sqrt({vc}.Create(dot(x, x))));"];
            }
            return [Attr, $"public static {name} normalize({name} x) => rsqrt(dot(x, x)) * x;"];
        }),

        Fn("normalizesafe", [BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var minNormal = sig.Type == BaseType.Double ? "DBL_MIN_NORMAL" : "FLT_MIN_NORMAL";
            if (sig.Shape.IsSimdEligible)
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [
                    Attr,
                    $"public static {name} normalizesafe({name} x, {name} defaultvalue = default)",
                    "{",
                    "    var len = dot(x, x);",
                    $"    var mask = {vc}.GreaterThan({vc}.Create(len), {vc}.Create({minNormal}));",
                    $"    return new {name}({vc}.ConditionalSelect(mask, x.data * {vc}.Create(rsqrt(len)), defaultvalue.data));",
                    "}"
                ];
            }
            return [
                Attr,
                $"public static {name} normalizesafe({name} x, {name} defaultvalue = default)",
                "{",
                "    var len = dot(x, x);",
                $"    return select(defaultvalue, x * rsqrt(len), len > {minNormal});",
                "}"
            ];
        }),

        Fn("reflect", [BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var two = sig.Type.ToTypedLiteral(2);
            return [Attr, $"public static {name} reflect({name} i, {name} n) => i - {two} * n * dot(i, n);"];
        }),

        Fn("refract", [BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var baseName = sig.Type.ToBaseTypeName();
            var one = sig.Type.ToTypedLiteral(1);
            var zero = sig.Type.ToTypedLiteral(0);
            return [
                Attr,
                $"public static {name} refract({name} i, {name} n, {baseName} eta)",
                "{",
                "    var ni = dot(n, i);",
                $"    var k = {one} - eta * eta * ({one} - ni * ni);",
                $"    return select({zero}, eta * i - (eta * ni + sqrt(k)) * n, k >= 0);",
                "}"
            ];
        }),

        Fn("project", [BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            return [Attr, $"public static {name} project({name} a, {name} b) => (dot(a, b) / dot(b, b)) * b;"];
        }),

        Fn("projectsafe", [BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            return [
                Attr,
                $"public static {name} projectsafe({name} a, {name} b, {name} defaultValue = default)",
                "{",
                "    var proj = project(a, b);",
                "    return select(defaultValue, proj, all(isfinite(proj)));",
                "}"
            ];
        }),

        Fn("faceforward", [BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var zero = sig.Type.ToTypedLiteral(0);
            return [Attr, $"public static {name} faceforward({name} n, {name} i, {name} ng) => select(n, -n, dot(ng, i) >= {zero});"];
        }),

        Fn("any", [BaseType.Bool, BaseType.Int, BaseType.UInt, BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Type != BaseType.Bool && sig.Shape.IsSimdEligible)
            {
                var zero = sig.Type.ToTypedLiteral(0);
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [Attr, $"public static bool any({name} x) => !{vc}.All(x.data, {zero});"];
            }
            var cond = string.Join(" || ", Range(sig.Dimension).Select(i =>
                sig.Type == BaseType.Bool ? $"x.{TypeShape.Components[i]}" :
                $"x.{TypeShape.Components[i]} != {sig.Type.ToTypedLiteral(0)}"));
            return [Attr, $"public static bool any({name} x) => {cond};"];
        }),

        Fn("all", [BaseType.Bool, BaseType.Int, BaseType.UInt, BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Type != BaseType.Bool && sig.Shape.IsSimdEligible)
            {
                var zero = sig.Type.ToTypedLiteral(0);
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                if (Simd.SimdStrategy.IsExactFit(sig.Type, sig.Dimension))
                    return [Attr, $"public static bool all({name} x) => {vc}.None(x.data, {zero});"];

                var one = sig.Type.ToTypedLiteral(1);
                var laneCount = Simd.SimdStrategy.NativeLaneCount(sig.Type, sig.Dimension);
                var patch = string.Concat(Range(laneCount - sig.Dimension)
                    .Select(i => $".WithElement({sig.Dimension + i}, {one})"));
                return [Attr, $"public static bool all({name} x) => {vc}.None(x.data{patch}, {zero});"];
            }
            var cond = string.Join(" && ", Range(sig.Dimension).Select(i =>
                sig.Type == BaseType.Bool ? $"x.{TypeShape.Components[i]}" :
                $"x.{TypeShape.Components[i]} != {sig.Type.ToTypedLiteral(0)}"));
            return [Attr, $"public static bool all({name} x) => {cond};"];
        }),

        Fn("select", [BaseType.Int, BaseType.UInt, BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var maskType = BaseType.Bool.ToTypeName(1, sig.Dimension);
            var name = sig.Shape.TypeName;
            if (sig.Dimension == 1)
                return [Attr, $"public static {name} select({name} a, {name} b, {maskType} test) => test ? b : a;"];
            var comps = string.Join(", ", Range(sig.Dimension).Select(i => $"test.{TypeShape.Components[i]} ? b.{TypeShape.Components[i]} : a.{TypeShape.Components[i]}"));
            return [
                Attr, $"public static {name} select({name} a, {name} b, {maskType} test) => new({comps});",
                Attr, $"public static {name} select({name} a, {name} b, bool test) => test ? b : a;"
            ];
        }),

        Fn("csum", [BaseType.Int, BaseType.UInt, BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Shape.IsSimdEligible)
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [Attr, $"public static {sig.Type.ToBaseTypeName()} csum({name} x) => {vc}.Sum(x.data);"];
            }
            return [Attr, $"public static {sig.Type.ToBaseTypeName()} csum({name} x) => {string.Join(" + ", Range(sig.Dimension).Select(i => $"x.{TypeShape.Components[i]}"))};"];
        }),

        Fn("cmin", [BaseType.Int, BaseType.UInt, BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var baseName = sig.Type.ToBaseTypeName();
            var chain = Range(sig.Dimension - 1).Aggregate("x.x", (acc, i) => $"min({acc}, x.{TypeShape.Components[i + 1]})");
            return [Attr, $"public static {baseName} cmin({name} x) => {chain};"];
        }),

        Fn("cmax", [BaseType.Int, BaseType.UInt, BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var baseName = sig.Type.ToBaseTypeName();
            var chain = Range(sig.Dimension - 1).Aggregate("x.x", (acc, i) => $"max({acc}, x.{TypeShape.Components[i + 1]})");
            return [Attr, $"public static {baseName} cmax({name} x) => {chain};"];
        }),

        Fn("countbits", [BaseType.Int, BaseType.UInt], (2, 4), sig =>
        {
            var retType = BaseType.Int.ToTypeName(sig.Dimension, 1);
            return [Attr, $"public static {retType} countbits({sig.Shape.TypeName} x) => new({PerComp("countbits", sig.Dimension)});"];
        }),

        Fn("lzcnt", [BaseType.Int, BaseType.UInt], (2, 4), sig =>
        {
            var retType = BaseType.Int.ToTypeName(sig.Dimension, 1);
            return [Attr, $"public static {retType} lzcnt({sig.Shape.TypeName} x) => new({PerComp("lzcnt", sig.Dimension)});"];
        }),

        Fn("tzcnt", [BaseType.Int, BaseType.UInt], (2, 4), sig =>
        {
            var retType = BaseType.Int.ToTypeName(sig.Dimension, 1);
            return [Attr, $"public static {retType} tzcnt({sig.Shape.TypeName} x) => new({PerComp("tzcnt", sig.Dimension)});"];
        }),

        Fn("floorlog2", [BaseType.Int, BaseType.UInt], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var retType = BaseType.Int.ToTypeName(sig.Dimension, 1);
            var uName = BaseType.UInt.ToTypeName(sig.Dimension, 1);
            var xExpr = sig.Type == BaseType.Int ? $"(({uName})x)" : "x";
            return [Attr, $"public static {retType} floorlog2({name} x) => 31 - lzcnt({xExpr});"];
        }),

        Fn("ceillog2", [BaseType.Int, BaseType.UInt], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var retType = BaseType.Int.ToTypeName(sig.Dimension, 1);
            var uName = BaseType.UInt.ToTypeName(sig.Dimension, 1);
            var xExpr = sig.Type == BaseType.Int ? $"(({uName})x)" : "x";
            return [Attr, $"public static {retType} ceillog2({name} x) => 32 - lzcnt({xExpr} - 1u);"];
        }),

        Fn("ceilpow2", [BaseType.Int, BaseType.UInt], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var one = sig.Type.ToTypedLiteral(1);
            return [
                Attr,
                $"public static {name} ceilpow2({name} x)",
                "{",
                $"    x -= {one};",
                "    x |= x >> 1;",
                "    x |= x >> 2;",
                "    x |= x >> 4;",
                "    x |= x >> 8;",
                "    x |= x >> 16;",
                $"    return x + {one};",
                "}"
            ];
        }),

        Fn("reversebits", [BaseType.Int, BaseType.UInt], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var uName = BaseType.UInt.ToTypeName(sig.Dimension, 1);
            var xExpr = sig.Type == BaseType.Int ? $"(({uName})x)" : "x";
            return [
                Attr,
                $"public static {name} reversebits({name} x)",
                "{",
                $"    var v = {xExpr};",
                "    v = ((v >> 1) & 0x55555555u) | ((v & 0x55555555u) << 1);",
                "    v = ((v >> 2) & 0x33333333u) | ((v & 0x33333333u) << 2);",
                "    v = ((v >> 4) & 0x0F0F0F0Fu) | ((v & 0x0F0F0F0Fu) << 4);",
                "    v = ((v >> 8) & 0x00FF00FFu) | ((v & 0x00FF00FFu) << 8);",
                "    v = (v >> 16) | (v << 16);",
                $"    return ({name})v;",
                "}"
            ];
        }),

        Fn("rol", [BaseType.Int, BaseType.UInt], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var uName = BaseType.UInt.ToTypeName(sig.Dimension, 1);
            var xExpr = sig.Type == BaseType.Int ? $"(({uName})x)" : "x";
            return [Attr, $"public static {name} rol({name} x, int n) => ({name})(({xExpr} << n) | ({xExpr} >> (32 - n)));"];
        }),

        Fn("ror", [BaseType.Int, BaseType.UInt], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var uName = BaseType.UInt.ToTypeName(sig.Dimension, 1);
            var xExpr = sig.Type == BaseType.Int ? $"(({uName})x)" : "x";
            return [Attr, $"public static {name} ror({name} x, int n) => ({name})(({xExpr} >> n) | ({xExpr} << (32 - n)));"];
        }),
    };

    private static MathFunction Fn(string name, BaseType[] types, (int, int) dims, Func<MathSignature, string[]> gen) =>
        new() { Name = name, Types = types, DimRange = dims, Generator = gen };

    private static int[] Range(int n) => Enumerable.Range(0, n).ToArray();

    private static string PerComp(string func, int dim) =>
        string.Join(", ", Range(dim).Select(i => $"{func}(x.{TypeShape.Components[i]})"));

    private static string PerCompBin(string func, int dim) =>
        string.Join(", ", Range(dim).Select(i => $"{func}(x.{TypeShape.Components[i]}, y.{TypeShape.Components[i]})"));

    private static MathFunction Trig(string name)
    {
        var mathName = name switch
        {
            "asin" => "Asin", "acos" => "Acos", "atan" => "Atan",
            "pow" => "Pow",
            _ => char.ToUpper(name[0]) + name.Substring(1)
        };

        return Fn(name, [BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var typeName = sig.Shape.TypeName;
            if (sig.Dimension == 1)
            {
                var cast = sig.Type == BaseType.Float ? $"System.MathF.{mathName}" : $"System.Math.{mathName}";
                if (name == "pow")
                    return [Attr, $"public static {typeName} {name}({typeName} x, {typeName} y) => {cast}(x, y);"];
                return [Attr, $"public static {typeName} {name}({typeName} x) => {cast}(x);"];
            }
            if (name == "pow")
                return [Attr, $"public static {typeName} pow({typeName} x, {typeName} y) => new({PerCompBin("pow", sig.Dimension)});"];
            return [Attr, $"public static {typeName} {name}({typeName} x) => new({PerComp(name, sig.Dimension)});"];
        });
    }

    private static MathFunction SimdTrig(string name, string simdMethod)
    {
        return Fn(name, [BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var typeName = sig.Shape.TypeName;
            if (sig.Dimension == 1)
            {
                var cast = sig.Type == BaseType.Float ? $"System.MathF.{simdMethod}" : $"System.Math.{simdMethod}";
                return [Attr, $"public static {typeName} {name}({typeName} x) => {cast}(x);"];
            }
            if (sig.Shape.IsSimdEligible)
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [Attr, $"public static {typeName} {name}({typeName} x) => new {typeName}({vc}.{simdMethod}(x.data));"];
            }
            return [Attr, $"public static {typeName} {name}({typeName} x) => new({PerComp(name, sig.Dimension)});"];
        });
    }

    public static void EmitAll(IndentedTextWriter writer)
    {
        foreach (var fn in All)
        {
            writer.WriteLine("#region {0}", fn.Name);
            writer.WriteLineNoTabs(string.Empty);

            foreach (var type in fn.Types)
            {
                for (var dim = fn.DimRange.Min; dim <= fn.DimRange.Max; dim++)
                {
                    var shape = new TypeShape(type, dim, 1, Features.All);
                    var sig = new MathSignature(fn.Name, type, dim, shape);
                    foreach (var line in fn.Generator(sig))
                        writer.WriteLine(line);
                    writer.WriteLineNoTabs(string.Empty);
                }
            }

            writer.WriteLine("#endregion");
            writer.WriteLineNoTabs(string.Empty);
        }
    }
}
