namespace Sia.Math.CodeGenerators;

using Microsoft.CodeAnalysis;

using Sia.Math.CodeGenerators.Generation;

[Generator(LanguageNames.CSharp)]
public sealed class MathSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static production => {
            foreach (var source in TypeSourceEmitter.GenerateAll()) {
                production.AddSource(source.HintName, source.Source);
            }

            var math = MathSourceEmitter.Generate();
            production.AddSource(math.HintName, math.Source);

            var matrix = MatrixMultiplicationEmitter.Generate();
            production.AddSource(matrix.HintName, matrix.Source);
        });
    }
}
