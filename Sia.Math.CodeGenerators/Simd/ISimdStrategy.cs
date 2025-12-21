namespace Sia.Math.CodeGenerators.Simd;

public interface ISimdStrategy
{
    bool SupportsOp(string op, BaseType baseType);
}
