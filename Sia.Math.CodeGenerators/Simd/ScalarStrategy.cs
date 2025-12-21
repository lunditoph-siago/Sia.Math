namespace Sia.Math.CodeGenerators.Simd;

public sealed class ScalarStrategy : ISimdStrategy
{
    public static ScalarStrategy Instance { get; } = new();

    public bool SupportsOp(string op, BaseType baseType) => false;
}
