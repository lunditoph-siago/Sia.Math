namespace Sia.Math.CodeGenerators.Simd;

public sealed class Vector128FloatStrategy : ISimdStrategy
{
    public static Vector128FloatStrategy Instance { get; } = new();

    public bool SupportsOp(string op, BaseType baseType) => op switch
    {
        "%" => false,
        _ => true
    };
}

public sealed class Vector128IntStrategy : ISimdStrategy
{
    public static Vector128IntStrategy Instance { get; } = new();

    public bool SupportsOp(string op, BaseType baseType) => op switch
    {
        "/" or "%" => false,
        _ => true
    };
}

public sealed class Vector128UIntStrategy : ISimdStrategy
{
    public static Vector128UIntStrategy Instance { get; } = new();

    public bool SupportsOp(string op, BaseType baseType) => op switch
    {
        "/" or "%" => false,
        _ => true
    };
}
