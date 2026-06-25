namespace Sia.Math.CodeGenerators.Functions;

public static partial class MathFunctionCatalog
{
    private static MathFunction[] BitFunctions() =>
    [
        Fn("countbits", [BaseType.Int, BaseType.UInt], (2, 4), signature => {
            var resultType = BaseType.Int.ToTypeName(
                signature.Dimension,
                1);
            return
            [
                Attr,
                $"public static {resultType} countbits({signature.Shape.TypeName} x) => new({PerComp("countbits", signature.Dimension)});",
            ];
        }),

        Fn("lzcnt", [BaseType.Int, BaseType.UInt], (2, 4), signature => {
            var resultType = BaseType.Int.ToTypeName(
                signature.Dimension,
                1);
            return
            [
                Attr,
                $"public static {resultType} lzcnt({signature.Shape.TypeName} x) => new({PerComp("lzcnt", signature.Dimension)});",
            ];
        }),

        Fn("tzcnt", [BaseType.Int, BaseType.UInt], (2, 4), signature => {
            var resultType = BaseType.Int.ToTypeName(
                signature.Dimension,
                1);
            return
            [
                Attr,
                $"public static {resultType} tzcnt({signature.Shape.TypeName} x) => new({PerComp("tzcnt", signature.Dimension)});",
            ];
        }),

        Fn("floorlog2", [BaseType.Int, BaseType.UInt], (2, 4), signature => {
            var typeName = signature.Shape.TypeName;
            var resultType = BaseType.Int.ToTypeName(
                signature.Dimension,
                1);
            var unsignedType = BaseType.UInt.ToTypeName(
                signature.Dimension,
                1);
            var value = signature.Type == BaseType.Int
                ? $"(({unsignedType})x)"
                : "x";
            return
            [
                Attr,
                $"public static {resultType} floorlog2({typeName} x) => 31 - lzcnt({value});",
            ];
        }),

        Fn("ceillog2", [BaseType.Int, BaseType.UInt], (2, 4), signature => {
            var typeName = signature.Shape.TypeName;
            var resultType = BaseType.Int.ToTypeName(
                signature.Dimension,
                1);
            var unsignedType = BaseType.UInt.ToTypeName(
                signature.Dimension,
                1);
            var value = signature.Type == BaseType.Int
                ? $"(({unsignedType})x)"
                : "x";
            return
            [
                Attr,
                $"public static {resultType} ceillog2({typeName} x) => 32 - lzcnt({value} - 1u);",
            ];
        }),

        Fn("ceilpow2", [BaseType.Int, BaseType.UInt], (2, 4), signature => {
            var typeName = signature.Shape.TypeName;
            var one = signature.Type.ToTypedLiteral(1);
            return FunctionEmitter.GenerateBlock(
                $"public static {typeName} ceilpow2({typeName} x)",
                $"x -= {one};",
                "x |= x >> 1;",
                "x |= x >> 2;",
                "x |= x >> 4;",
                "x |= x >> 8;",
                "x |= x >> 16;",
                $"return x + {one};");
        }),

        Fn("reversebits", [BaseType.Int, BaseType.UInt], (2, 4), signature => {
            var typeName = signature.Shape.TypeName;
            var unsignedType = BaseType.UInt.ToTypeName(
                signature.Dimension,
                1);
            var value = signature.Type == BaseType.Int
                ? $"(({unsignedType})x)"
                : "x";
            return FunctionEmitter.GenerateBlock(
                $"public static {typeName} reversebits({typeName} x)",
                $"var v = {value};",
                "v = ((v >> 1) & 0x55555555u) | ((v & 0x55555555u) << 1);",
                "v = ((v >> 2) & 0x33333333u) | ((v & 0x33333333u) << 2);",
                "v = ((v >> 4) & 0x0F0F0F0Fu) | ((v & 0x0F0F0F0Fu) << 4);",
                "v = ((v >> 8) & 0x00FF00FFu) | ((v & 0x00FF00FFu) << 8);",
                "v = (v >> 16) | (v << 16);",
                $"return ({typeName})v;");
        }),

        Fn("rol", [BaseType.Int, BaseType.UInt], (1, 4), signature => {
            var typeName = signature.Shape.TypeName;
            var unsignedType = BaseType.UInt.ToTypeName(
                signature.Dimension,
                1);
            var value = signature.Type == BaseType.Int
                ? $"(({unsignedType})x)"
                : "x";
            return FunctionEmitter.GenerateExpression(
                $"public static {typeName} rol({typeName} x, int n)",
                $"({typeName})(({value} << n) | ({value} >> (32 - n)))");
        }),

        Fn("ror", [BaseType.Int, BaseType.UInt], (1, 4), signature => {
            var typeName = signature.Shape.TypeName;
            var unsignedType = BaseType.UInt.ToTypeName(
                signature.Dimension,
                1);
            var value = signature.Type == BaseType.Int
                ? $"(({unsignedType})x)"
                : "x";
            return FunctionEmitter.GenerateExpression(
                $"public static {typeName} ror({typeName} x, int n)",
                $"({typeName})(({value} >> n) | ({value} << (32 - n)))");
        }),
    ];
}
