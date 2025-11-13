namespace Sia.Math.CodeGenerators;

public static class SimdSupport
{
    public static bool IsEligibleElement(BaseType baseType) =>
        baseType is BaseType.Float or BaseType.Int or BaseType.UInt or BaseType.Double;

    public static bool IsEligibleVector(BaseType baseType, int rows, int columns) =>
        columns == 1 && rows is >= 2 and <= 4 && IsEligibleElement(baseType);

    public static int NativeLaneCount(BaseType baseType, int rows) =>
        baseType == BaseType.Double ? (rows <= 2 ? 2 : 4) : 4;

    private static bool UsesVector256(BaseType baseType, int rows) =>
        baseType == BaseType.Double && rows > 2;

    public static string NativeVectorTypeName(BaseType baseType, int rows) =>
        UsesVector256(baseType, rows) ? "Vector256<double>" : $"Vector128<{baseType.ToBaseTypeName()}>";

    public static string NativeVectorClassName(BaseType baseType, int rows) =>
        UsesVector256(baseType, rows) ? "Vector256" : "Vector128";

    public static bool IsExactFit(BaseType baseType, int rows) =>
        rows == NativeLaneCount(baseType, rows);

    public static bool IsPaddedFit(BaseType baseType, int rows) =>
        rows == 3 && IsEligibleElement(baseType);

    public static string CreateBroadcast(BaseType baseType, int rows, string scalarExpr) =>
        $"{NativeVectorClassName(baseType, rows)}.Create({scalarExpr})";

    public static bool SupportsSimdOp(string op, BaseType baseType, int rows)
    {
        if (op is not ("+" or "-" or "*" or "/" or "&" or "|" or "^")) return false;

        if (op == "/" && baseType is BaseType.Int or BaseType.UInt && !(IsExactFit(baseType, rows) || IsPaddedFit(baseType, rows)))
            return false;

        return true;
    }
}
