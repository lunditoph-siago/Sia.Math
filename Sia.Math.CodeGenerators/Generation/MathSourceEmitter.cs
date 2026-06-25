namespace Sia.Math.CodeGenerators.Generation;

using Sia.Math.CodeGenerators.Functions;

public static class MathSourceEmitter
{
    public static GeneratedSource Generate()
    {
        var body = new SourceBuilder(indent: 1);
        MathFunctionCatalog.GenerateAll(body);

        var source = ScopeWriter.GenerateStandaloneFile(
            [
                "System",
                "System.Runtime.CompilerServices",
                "System.Runtime.Intrinsics",
            ],
            body.ToString());
        return new GeneratedSource("math.g.cs", source);
    }
}
