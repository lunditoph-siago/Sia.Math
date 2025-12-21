namespace Sia.Math.CodeGenerators.Simd;

public sealed class Vector256Strategy : ISimdStrategy
{
    public static Vector256Strategy Instance { get; } = new();

    public bool SupportsOp(string op, BaseType baseType) => op switch
    {
        "%" => false,
        _ => true
    };
}
