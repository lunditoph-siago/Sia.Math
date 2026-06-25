namespace Sia.Math.CodeGenerators.Generation;

using System.Collections.Generic;

using Sia.Math.CodeGenerators.Capabilities;

public static class TypeSourceEmitter
{
    private static readonly CapabilityPipeline CorePipeline =
        CapabilityPipeline.Create(
            CapabilityStage.From("fields", Fields.Generate),
            CapabilityStage.From("constants", Constants.Generate),
            CapabilityStage.From("constructors", Constructors.Generate),
            CapabilityStage.From("conversions", Conversions.Generate),
            CapabilityStage.From("operators", Operators.Generate),
            CapabilityStage.From("equality", Equality.Generate),
            CapabilityStage.From("formatting", ToStringGen.Generate),
            CapabilityStage.From("debug-proxy", DebugProxy.Generate));

    private static readonly CapabilityPipeline MathPipeline =
        CapabilityPipeline.Create(
            CapabilityStage.From("hashing", Hashing.Generate),
            CapabilityStage.From("shuffle", Shuffle.Generate),
            CapabilityStage.From("matrix-operations", MatrixOps.Generate));

    public static IEnumerable<GeneratedSource> GenerateAll()
    {
        foreach (var shape in TypeShapeCatalog.All) {
            var core = Generate(
                shape,
                string.Empty,
                CorePipeline.Generate(shape),
                isPrimary: true);
            if (core is { } coreSource) {
                yield return coreSource;
            }

            var swizzles = Generate(
                shape,
                "Swizzles",
                Capabilities.Swizzles.Generate(shape),
                isPrimary: false);
            if (swizzles is { } swizzleSource) {
                yield return swizzleSource;
            }

            var math = Generate(
                shape,
                "Math",
                MathPipeline.Generate(shape),
                isPrimary: false);
            if (math is { } mathSource) {
                yield return mathSource;
            }
        }
    }

    private static GeneratedSource? Generate(
        TypeShape shape,
        string suffix,
        CodeFragment fragment,
        bool isPrimary)
    {
        if (fragment.IsEmpty) {
            return null;
        }

        var hintName = string.IsNullOrEmpty(suffix)
            ? $"{shape.FilePrefix}.g.cs"
            : $"{shape.FilePrefix}.{suffix}.g.cs";
        var source = ScopeWriter.GenerateTypePartial(
            shape,
            fragment,
            isPrimary);
        return new GeneratedSource(hintName, source);
    }
}
