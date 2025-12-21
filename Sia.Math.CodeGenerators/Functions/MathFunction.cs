using System;

namespace Sia.Math.CodeGenerators.Functions;

public readonly record struct MathSignature(
    string Name,
    BaseType Type,
    int Dimension,
    TypeShape Shape
);

public sealed class MathFunction
{
    public string Name { get; init; } = string.Empty;
    public BaseType[] Types { get; init; } = [];
    public (int Min, int Max) DimRange { get; init; } = (1, 4);
    public Func<MathSignature, string[]> Generator { get; init; } = _ => [];

    public MathFunction() { }
}
