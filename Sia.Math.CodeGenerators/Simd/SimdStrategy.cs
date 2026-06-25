namespace Sia.Math.CodeGenerators.Simd;

public static class SimdStrategy
{
    public static SimdTypeProfile Profile(BaseType baseType, int rows) =>
        SimdTypeProfile.Create(baseType, rows);

    public static int NativeLaneCount(BaseType baseType, int rows) =>
        Profile(baseType, rows).LaneCount;

    public static bool IsExactFit(BaseType baseType, int rows) =>
        Profile(baseType, rows).IsExactFit;

    public static string NativeVectorTypeName(BaseType baseType, int rows) =>
        Profile(baseType, rows).VectorType;

    public static string NativeVectorClassName(BaseType baseType, int rows) =>
        Profile(baseType, rows).VectorClass;

    public static string CreateBroadcast(
        BaseType baseType,
        int rows,
        string scalarExpression) =>
        Profile(baseType, rows).Broadcast(scalarExpression);

    public static bool SupportsSimdOp(string op, BaseType baseType, int rows)
    {
        var profile = Profile(baseType, rows);
        return profile.IsSimd && profile.SupportsOperator(op);
    }
}
