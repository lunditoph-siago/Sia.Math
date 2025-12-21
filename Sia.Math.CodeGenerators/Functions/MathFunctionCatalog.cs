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
                    return [Attr, $"public static {name} min({name} x, {name} y) => isnan(y) ? x : (isnan(x) ? y : (x < y ? x : y));"];
                return [Attr, $"public static {name} min({name} x, {name} y) => x < y ? x : y;"];
            }
            if (sig.Type is BaseType.Int or BaseType.UInt && sig.Shape.IsSimdEligible)
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [Attr, $"public static {name} min({name} x, {name} y) => new {name}({vc}.Min(x.AsSimd(), y.AsSimd()));"];
            }
            return [Attr, $"public static {name} min({name} x, {name} y) => new({PerCompBin("min", sig.Dimension)});"];
        }),

        Fn("max", [BaseType.Int, BaseType.UInt, BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Dimension == 1)
            {
                if (sig.Type is BaseType.Float or BaseType.Double)
                    return [Attr, $"public static {name} max({name} x, {name} y) => isnan(y) ? x : (isnan(x) ? y : (x > y ? x : y));"];
                return [Attr, $"public static {name} max({name} x, {name} y) => x > y ? x : y;"];
            }
            if (sig.Type is BaseType.Int or BaseType.UInt && sig.Shape.IsSimdEligible)
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [Attr, $"public static {name} max({name} x, {name} y) => new {name}({vc}.Max(x.AsSimd(), y.AsSimd()));"];
            }
            return [Attr, $"public static {name} max({name} x, {name} y) => new({PerCompBin("max", sig.Dimension)});"];
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
            if (sig.Type is BaseType.Int or BaseType.Float && sig.Shape.IsSimdEligible)
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [Attr, $"public static {name} abs({name} x) => new {name}({vc}.Abs(x.AsSimd()));"];
            }
            return sig.Type switch
            {
                BaseType.Int => [Attr, $"public static {name} abs({name} x) => new {name}({string.Join(", ", Range(sig.Dimension).Select(i => $"abs(x.{TypeShape.Components[i]})"))});"],
                BaseType.Float => [Attr, $"public static {name} abs({name} x) => new({string.Join(", ", Range(sig.Dimension).Select(i => $"asfloat(asuint(x.{TypeShape.Components[i]}) & 0x7FFFFFFF)"))});"],
                BaseType.Double => [Attr, $"public static {name} abs({name} x) => new({string.Join(", ", Range(sig.Dimension).Select(i => $"asdouble(asulong(x.{TypeShape.Components[i]}) & 0x7FFFFFFFFFFFFFFF)"))});"],
                _ => throw new ArgumentOutOfRangeException()
            };
        }),

        Trig("tan"), Trig("sinh"), Trig("cosh"), Trig("tanh"),
        Trig("asin"), Trig("acos"), Trig("atan"),
        Trig("pow"),

        SimdTrig("sin", "Sin"), SimdTrig("cos", "Cos"), SimdTrig("exp", "Exp"), SimdTrig("log", "Log"),

        Fn("sqrt", [BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Dimension == 1)
                return [Attr, sig.Type == BaseType.Float
                    ? $"public static {name} sqrt({name} x) => (float)System.Math.Sqrt(x);"
                    : $"public static {name} sqrt({name} x) => System.Math.Sqrt(x);"];
            if (sig.Shape.IsSimdEligible)
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [Attr, $"public static {name} sqrt({name} x) => new {name}({vc}.Sqrt(x.AsSimd()));"];
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
                return [Attr, $"public static {name} rsqrt({name} x) => new {name}({vt}.One / {vc}.Sqrt(x.AsSimd()));"];
            }
            return [Attr, $"public static {name} rsqrt({name} x) => 1.0f / sqrt(x);"];
        }),

        Fn("rcp", [BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var one = sig.Type.ToTypedLiteral(1);
            return [Attr, $"public static {name} rcp({name} x) => {one} / x;"];
        }),

        Fn("lerp", [BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Dimension >= 2 && sig.Type == BaseType.Float && sig.Shape.IsSimdEligible)
                return [Attr, $"public static {name} lerp({name} a, {name} b, {name} s) => new {name}(Vector128.Lerp(a.AsSimd(), b.AsSimd(), s.AsSimd()));"];
            if (sig.Dimension >= 2 && sig.Shape.IsSimdEligible)
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [Attr, $"public static {name} lerp({name} a, {name} b, {name} s) => new {name}({vc}.FusedMultiplyAdd(s.AsSimd(), b.AsSimd() - a.AsSimd(), a.AsSimd()));"];
            }
            return [Attr, $"public static {name} lerp({name} a, {name} b, {name} s) => a + s * (b - a);"];
        }),

        Fn("unlerp", [BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            return [Attr, $"public static {name} unlerp({name} a, {name} b, {name} x) => (x - a) / (b - a);"];
        }),

        Fn("clamp", [BaseType.Int, BaseType.UInt, BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Dimension >= 2 && sig.Type == BaseType.Float && sig.Shape.IsSimdEligible)
                return [Attr, $"public static {name} clamp({name} v, {name} a, {name} b) => new {name}(Vector128.Clamp(v.AsSimd(), a.AsSimd(), b.AsSimd()));"];
            if (sig.Dimension >= 2 && sig.Type is BaseType.Int or BaseType.UInt && sig.Shape.IsSimdEligible)
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [Attr, $"public static {name} clamp({name} v, {name} a, {name} b) => new {name}({vc}.Max(a.AsSimd(), {vc}.Min(b.AsSimd(), v.AsSimd())));"];
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

        Fn("dot", [BaseType.Int, BaseType.UInt, BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var baseName = sig.Type.ToBaseTypeName();
            if (sig.Dimension == 1)
                return [Attr, $"public static {name} dot({name} x, {name} y) => x * y;"];
            if (sig.Shape.IsSimdEligible)
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [Attr, $"public static {baseName} dot({name} x, {name} y) => {vc}.Dot(x.AsSimd(), y.AsSimd());"];
            }
            return [Attr, $"public static {baseName} dot({name} x, {name} y) => {string.Join(" + ", Range(sig.Dimension).Select(i => $"x.{TypeShape.Components[i]} * y.{TypeShape.Components[i]}"))};"];
        }),

        Fn("lengthsq", [BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var baseName = sig.Type.ToBaseTypeName();
            if (sig.Dimension == 1)
                return [Attr, $"public static {baseName} lengthsq({name} x) => x * x;"];
            return [Attr, $"public static {baseName} lengthsq({name} x) => dot(x, x);"];
        }),

        Fn("length", [BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var baseName = sig.Type.ToBaseTypeName();
            if (sig.Dimension == 1)
                return [Attr, $"public static {baseName} length({name} x) => abs(x);"];
            return [Attr, $"public static {baseName} length({name} x) => sqrt(dot(x, x));"];
        }),

        Fn("normalize", [BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            return [Attr, $"public static {name} normalize({name} x) => rsqrt(dot(x, x)) * x;"];
        }),

        Fn("normalizesafe", [BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var minNormal = sig.Type == BaseType.Double ? "DBL_MIN_NORMAL" : "FLT_MIN_NORMAL";
            return [
                Attr,
                $"public static {name} normalizesafe({name} x, {name} defaultvalue = default)",
                "{",
                "    var len = dot(x, x);",
                $"    return select(defaultvalue, x * rsqrt(len), len > {minNormal});",
                "}"
            ];
        }),

        Fn("any", [BaseType.Bool, BaseType.Int, BaseType.UInt, BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Type != BaseType.Bool && sig.Shape.IsSimdEligible && Simd.SimdStrategy.IsExactFit(sig.Type, sig.Dimension))
            {
                var zero = sig.Type.ToTypedLiteral(0);
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [Attr, $"public static bool any({name} x) => !{vc}.All(x.AsSimd(), {zero});"];
            }
            var cond = string.Join(" || ", Range(sig.Dimension).Select(i =>
                sig.Type == BaseType.Bool ? $"x.{TypeShape.Components[i]}" :
                sig.Type.ToBaseTypeName() == "float" ? $"x.{TypeShape.Components[i]} != 0.0f" :
                $"x.{TypeShape.Components[i]} != 0.0"));
            return [Attr, $"public static bool any({name} x) => {cond};"];
        }),

        Fn("all", [BaseType.Bool, BaseType.Int, BaseType.UInt, BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Type != BaseType.Bool && sig.Shape.IsSimdEligible && Simd.SimdStrategy.IsExactFit(sig.Type, sig.Dimension))
            {
                var zero = sig.Type.ToTypedLiteral(0);
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [Attr, $"public static bool all({name} x) => {vc}.None(x.AsSimd(), {zero});"];
            }
            var cond = string.Join(" && ", Range(sig.Dimension).Select(i =>
                sig.Type == BaseType.Bool ? $"x.{TypeShape.Components[i]}" :
                sig.Type.ToBaseTypeName() == "float" ? $"x.{TypeShape.Components[i]} != 0.0f" :
                $"x.{TypeShape.Components[i]} != 0.0"));
            return [Attr, $"public static bool all({name} x) => {cond};"];
        }),

        Fn("select", [BaseType.Int, BaseType.UInt, BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var test = BaseType.Bool.ToTypeName(1, sig.Dimension);
            var name = sig.Shape.TypeName;
            if (sig.Dimension == 1)
                return [Attr, $"public static {name} select({name} a, {name} b, {test} test) => test ? b : a;"];
            var comps = string.Join(", ", Range(sig.Dimension).Select(i => $"test.{TypeShape.Components[i]} ? b.{TypeShape.Components[i]} : a.{TypeShape.Components[i]}"));
            return [Attr, $"public static {name} select({name} a, {name} b, {test} test) => new({comps});"];
        }),

        Fn("sincos", [BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Type == BaseType.Float && sig.Dimension >= 2 && sig.Shape.IsSimdEligible)
            {
                return [
                    Attr,
                    $"public static void sincos({name} v, out {name} s, out {name} c)",
                    "{",
                    "    var (sin, cos) = Vector128.SinCos(v.AsSimd());",
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
            if (sig.Dimension >= 2 && sig.Type == BaseType.Float && sig.Shape.IsSimdEligible)
                return [Attr, $"public static {name} radians({name} x) => new(Vector128.DegreesToRadians(x.AsSimd()));"];
            var constant = sig.Type == BaseType.Double ? "TORADIANS_DBL" : "TORADIANS";
            return [Attr, $"public static {name} radians({name} x) => x * {constant};"];
        }),

        Fn("degrees", [BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Dimension >= 2 && sig.Type == BaseType.Float && sig.Shape.IsSimdEligible)
                return [Attr, $"public static {name} degrees({name} x) => new(Vector128.RadiansToDegrees(x.AsSimd()));"];
            var constant = sig.Type == BaseType.Double ? "TODEGREES_DBL" : "TODEGREES";
            return [Attr, $"public static {name} degrees({name} x) => x * {constant};"];
        }),

        Fn("csum", [BaseType.Int, BaseType.UInt, BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Shape.IsSimdEligible && Simd.SimdStrategy.IsExactFit(sig.Type, sig.Dimension))
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [Attr, $"public static {sig.Type.ToBaseTypeName()} csum({name} x) => {vc}.Sum(x.AsSimd());"];
            }
            return [Attr, $"public static {sig.Type.ToBaseTypeName()} csum({name} x) => {string.Join(" + ", Range(sig.Dimension).Select(i => $"x.{TypeShape.Components[i]}"))};"];
        }),

        Fn("round", [BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Type == BaseType.Float && sig.Shape.IsSimdEligible)
                return [Attr, $"public static {name} round({name} x) => new {name}(Vector128.Round(x.AsSimd()));"];
            return [Attr, $"public static {name} round({name} x) => new {name}({PerComp("round", sig.Dimension)});"];
        }),

        Fn("trunc", [BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Shape.IsSimdEligible)
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [Attr, $"public static {name} trunc({name} x) => new {name}({vc}.Truncate(x.AsSimd()));"];
            }
            return [Attr, $"public static {name} trunc({name} x) => new {name}({PerComp("trunc", sig.Dimension)});"];
        }),

        Fn("atan2", [BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var scalarCall = sig.Type == BaseType.Float
                ? "(float)global::System.Math.Atan2"
                : "global::System.Math.Atan2";
            return [Attr, $"public static {name} atan2({name} y, {name} x) => new {name}({string.Join(", ", Range(sig.Dimension).Select(i => $"{scalarCall}(y.{TypeShape.Components[i]}, x.{TypeShape.Components[i]})"))});"];
        }),

        Cw("floor"), Cw("ceil"), Cw("frac"), Cw("sign"),
        Cw("smoothstep", 3),
        Cw("ceilpow2", 1, [BaseType.Int, BaseType.UInt]),
        Cw("floorlog2", 1, [BaseType.Int, BaseType.UInt], retPrefix: "int"),
        Cw("ceillog2", 1, [BaseType.Int, BaseType.UInt], retPrefix: "int"),
        Cw("countbits", 1, [BaseType.Int, BaseType.UInt], retPrefix: "int"),
        Cw("lzcnt", 1, [BaseType.Int, BaseType.UInt], retPrefix: "int"),
        Cw("tzcnt", 1, [BaseType.Int, BaseType.UInt], retPrefix: "int"),
        Cw("reversebits", 1, [BaseType.Int, BaseType.UInt]),

        Fn("step", [BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            var zero = sig.Type.ToTypedLiteral(0);
            var one = sig.Type.ToTypedLiteral(1);
            return [Attr, $"public static {name} step({name} y, {name} x) => select({zero}, {one}, x >= y);"];
        }),

        Fn("mad", [BaseType.Int, BaseType.UInt, BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            if (sig.Dimension >= 2 && sig.Type is BaseType.Float or BaseType.Double && sig.Shape.IsSimdEligible)
            {
                var vc = Simd.SimdStrategy.NativeVectorClassName(sig.Type, sig.Dimension);
                return [Attr, $"public static {name} mad({name} a, {name} b, {name} c) => new {name}({vc}.FusedMultiplyAdd(a.AsSimd(), b.AsSimd(), c.AsSimd()));"];
            }
            return [Attr, $"public static {name} mad({name} a, {name} b, {name} c) => a * b + c;"];
        }),

        Fn("fmod", [BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            return [Attr, $"public static {name} fmod({name} x, {name} y) => x % y;"];
        }),

        Fn("remap", [BaseType.Float, BaseType.Double], (2, 4), sig =>
        {
            var name = sig.Shape.TypeName;
            return [Attr, $"public static {name} remap({name} a, {name} b, {name} c, {name} d, {name} x) => lerp(c, d, unlerp(a, b, x));"];
        }),

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
            "sin" => "Sin", "cos" => "Cos", "tan" => "Tan",
            "sinh" => "Sinh", "cosh" => "Cosh", "tanh" => "Tanh",
            "asin" => "Asin", "acos" => "Acos", "atan" => "Atan",
            "exp" => "Exp", "log" => "Log", "pow" => "Pow",
            _ => char.ToUpper(name[0]) + name.Substring(1)
        };

        return Fn(name, [BaseType.Float, BaseType.Double], (1, 4), sig =>
        {
            var typeName = sig.Shape.TypeName;
            if (sig.Dimension == 1)
            {
                var cast = sig.Type == BaseType.Float ? $"(float)System.Math.{mathName}" : $"System.Math.{mathName}";
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
                var cast = sig.Type == BaseType.Float ? $"(float)System.Math.{simdMethod}" : $"System.Math.{simdMethod}";
                return [Attr, $"public static {typeName} {name}({typeName} x) => {cast}(x);"];
            }
            if (sig.Type == BaseType.Float && sig.Shape.IsSimdEligible)
                return [Attr, $"public static {typeName} {name}({typeName} x) => new {typeName}(Vector128.{simdMethod}(x.AsSimd()));"];
            return [Attr, $"public static {typeName} {name}({typeName} x) => new({PerComp(name, sig.Dimension)});"];
        });
    }

    private static MathFunction Cw(string name, int argCount = 1, BaseType[]? types = null, string retPrefix = "")
    {
        types ??= [BaseType.Float, BaseType.Double];
        return Fn(name, types, (2, 4), sig =>
        {
            var typeName = sig.Shape.TypeName;
            var retType = string.IsNullOrEmpty(retPrefix) ? typeName : $"{retPrefix}{sig.Dimension}";
            if (argCount == 1)
                return [Attr, $"public static {retType} {name}({typeName} x) => new {retType}({PerComp(name, sig.Dimension)});"];
            else if (argCount == 2)
                return [Attr, $"public static {typeName} {name}({typeName} y, {typeName} x) => new {typeName}({string.Join(", ", Range(sig.Dimension).Select(i => $"{name}(y.{TypeShape.Components[i]}, x.{TypeShape.Components[i]})"))});"];
            else
                return [Attr, $"public static {typeName} {name}({typeName} a, {typeName} b, {typeName} x) => new {typeName}({string.Join(", ", Range(sig.Dimension).Select(i => $"{name}(a.{TypeShape.Components[i]}, b.{TypeShape.Components[i]}, x.{TypeShape.Components[i]})"))});"];
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
