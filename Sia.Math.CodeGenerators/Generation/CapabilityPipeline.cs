namespace Sia.Math.CodeGenerators.Generation;

using System;
using System.Collections.Generic;

public sealed class CapabilityPipeline
{
    private readonly IReadOnlyList<CapabilityStage> _stages;

    private CapabilityPipeline(IReadOnlyList<CapabilityStage> stages)
    {
        _stages = stages;
    }

    public static CapabilityPipeline Create(
        params CapabilityStage[] stages)
        => new(stages);

    public CodeFragment Generate(TypeShape shape)
    {
        var fragment = CodeFragment.Empty;
        foreach (var stage in _stages) {
            fragment += stage.Generate(shape);
        }
        return fragment;
    }
}

public readonly record struct CapabilityStage(
    string Name,
    Func<TypeShape, CodeFragment> Generate)
{
    public static CapabilityStage From(
        string name,
        Func<TypeShape, CodeFragment> generate)
        => new(name, generate);
}
