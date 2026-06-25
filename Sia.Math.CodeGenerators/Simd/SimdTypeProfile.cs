namespace Sia.Math.CodeGenerators.Simd;

using System;

public enum SimdVectorWidth
{
    Scalar,
    Vector128,
    Vector256,
}

public readonly record struct SimdTypeProfile(
    BaseType BaseType,
    int Dimension,
    SimdVectorWidth Width,
    int LaneCount,
    string VectorClass,
    string VectorType,
    string PortableGuard)
{
    public bool IsSimd => Width != SimdVectorWidth.Scalar;
    public bool IsExactFit => IsSimd && Dimension == LaneCount;

    public static SimdTypeProfile Create(BaseType baseType, int dimension)
    {
        if (!IsSupportedVector(baseType, dimension)) {
            return Scalar(baseType, dimension);
        }

        if (baseType == BaseType.Double && dimension > 2) {
            return new SimdTypeProfile(
                baseType,
                dimension,
                SimdVectorWidth.Vector256,
                4,
                "Vector256",
                "Vector256<double>",
                "global::System.Runtime.Intrinsics.Vector256.IsHardwareAccelerated");
        }

        var scalar = baseType.ToBaseTypeName();

        return new SimdTypeProfile(
            baseType,
            dimension,
            SimdVectorWidth.Vector128,
            baseType == BaseType.Double ? 2 : 4,
            "Vector128",
            $"Vector128<{scalar}>",
            "global::System.Runtime.Intrinsics.Vector128.IsHardwareAccelerated");
    }

    public bool SupportsOperator(string op) => BaseType switch {
        BaseType.Float or BaseType.Double => op != "%",
        BaseType.Int or BaseType.UInt => op is not "/" and not "%",
        _ => false,
    };

    public string Broadcast(string scalarExpression) =>
        $"{VectorClass}.Create({scalarExpression})";

    private static bool IsSupportedVector(BaseType baseType, int dimension) =>
        dimension is >= 2 and <= 4
        && baseType is BaseType.Float or BaseType.Double or BaseType.Int or BaseType.UInt;

    private static SimdTypeProfile Scalar(BaseType baseType, int dimension) =>
        new(
            baseType,
            dimension,
            SimdVectorWidth.Scalar,
            dimension,
            string.Empty,
            string.Empty,
            "false");
}
