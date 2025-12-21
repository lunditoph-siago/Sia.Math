namespace Sia.Math.CodeGenerators.Simd;

public static class SimdStrategy
{
    public static bool IsSimdEligible(BaseType baseType) => baseType switch
    {
        BaseType.Float or BaseType.Int or BaseType.UInt or BaseType.Double => true,
        _ => false
    };

    public static bool IsSimdEligibleVector(BaseType baseType, int rows, int columns)
    {
        if (columns != 1) return false;
        if (rows < 2 || rows > 4) return false;
        return IsSimdEligible(baseType);
    }

    public static int NativeLaneCount(BaseType baseType, int rows)
    {
        if (baseType == BaseType.Double)
            return rows > 2 ? 4 : 2;
        return 4;
    }

    public static bool IsExactFit(BaseType baseType, int rows)
    {
        return rows == NativeLaneCount(baseType, rows);
    }

    public static bool IsPaddedFit(BaseType baseType, int rows)
    {
        return rows == 3;
    }

    public static bool UsesVector256(BaseType baseType, int rows)
    {
        return baseType == BaseType.Double && rows > 2;
    }

    public static string NativeVectorTypeName(BaseType baseType, int rows)
    {
        if (UsesVector256(baseType, rows))
            return "Vector256<double>";
        var type = baseType.ToBaseTypeName();
        return $"Vector128<{type}>";
    }

    public static string NativeVectorClassName(BaseType baseType, int rows)
    {
        return UsesVector256(baseType, rows) ? "Vector256" : "Vector128";
    }

    public static ISimdStrategy For(BaseType baseType, int rows)
    {
        if (!IsSimdEligibleVector(baseType, rows, 1))
            return ScalarStrategy.Instance;
        if (baseType == BaseType.Double && rows > 2)
            return Vector256Strategy.Instance;
        return baseType switch
        {
            BaseType.Float => Vector128FloatStrategy.Instance,
            BaseType.Int => Vector128IntStrategy.Instance,
            BaseType.UInt => Vector128UIntStrategy.Instance,
            _ => ScalarStrategy.Instance
        };
    }

    public static string CreateBroadcast(BaseType baseType, int rows, string scalarExpr)
    {
        var className = NativeVectorClassName(baseType, rows);
        return $"{className}.Create({scalarExpr})";
    }

    public static bool SupportsSimdOp(string op, BaseType baseType, int rows)
    {
        var strategy = For(baseType, rows);
        return strategy is not ScalarStrategy && strategy.SupportsOp(op, baseType);
    }
}
