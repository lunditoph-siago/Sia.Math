using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Sia.Math.Benchmarks.CodeGenerators;

[Generator]
public class BenchRegistryGenerator : IIncrementalGenerator
{
    private const int Iterations = 2000;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var benchClasses = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: (node, _) => node is ClassDeclarationSyntax c
                    && c.BaseList != null
                    && c.BaseList.Types.Any(t => t.Type.ToString() == "BenchBase"),
                transform: (ctx, _) => GetBenchInfo(ctx))
            .Where(info => info != null)
            .Select((info, _) => info!.Value)
            .Collect();

        context.RegisterSourceOutput(benchClasses, GenerateRegistry);
    }

    private static (string ClassName, string Group, string Op)? GetBenchInfo(
        GeneratorSyntaxContext ctx)
    {
        var classDecl = (ClassDeclarationSyntax)ctx.Node;
        var symbol = ctx.SemanticModel.GetDeclaredSymbol(classDecl);
        if (symbol?.BaseType?.Name != "BenchBase")
            return null;

        var className = classDecl.Identifier.Text;

        var sysMethod = classDecl.Members
            .OfType<MethodDeclarationSyntax>()
            .FirstOrDefault(m => m.Identifier.Text.StartsWith("Sys_"));

        if (sysMethod == null)
            return null;

        string? op = null;
        foreach (var attrList in sysMethod.AttributeLists)
        foreach (var attr in attrList.Attributes)
        {
            if (attr.Name.ToString() is not ("BenchmarkCategory" or "BenchmarkCategoryAttribute"))
                continue;
            if (attr.ArgumentList?.Arguments.Count > 0)
            {
                var arg = attr.ArgumentList.Arguments[0].ToString().Trim('"');
                if (arg is "HighPriority" or "Normal" or "LowPriority")
                    continue;
                op = arg;
                break;
            }
        }

        if (op == null) return null;

        var prefix = className.Substring(0, className.Length - op.Length);
        var group = prefix.ToLowerInvariant();

        return (className, group, op);
    }

    private static void GenerateRegistry(
        SourceProductionContext ctx,
        ImmutableArray<(string ClassName, string Group, string Op)> items)
    {
        var sb = new StringBuilder();
        sb.AppendLine("#if BROWSER");
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine();
        sb.AppendLine("namespace Sia.Math.Benchmarks;");
        sb.AppendLine();
        sb.AppendLine("public static class BenchRegistry");
        sb.AppendLine("{");
        sb.AppendLine("    public static IReadOnlyList<BenchDef> GetAll()");
        sb.AppendLine("    {");
        sb.AppendLine("        return new BenchDef[]");
        sb.AppendLine("        {");

        foreach (var (className, group, op) in
            items.OrderBy(i => i.Group).ThenBy(i => i.Op))
        {
            sb.AppendLine($"            // {group}.{op}");
            sb.AppendLine($"            new(\"{group}\", \"{op}\", {Iterations},");
            sb.AppendLine($"                Sys: () => {{ var b = new {className}(); b.Setup(); b.Sys_{op}(); }},");
            sb.AppendLine($"                Sia: () => {{ var b = new {className}(); b.Setup(); b.Sia_{op}(); }}),");
        }

        sb.AppendLine("        };");
        sb.AppendLine("    }");
        sb.AppendLine("}");
        sb.AppendLine("#endif");

        ctx.AddSource("BenchRegistry.Browser.g.cs", sb.ToString());
    }
}
