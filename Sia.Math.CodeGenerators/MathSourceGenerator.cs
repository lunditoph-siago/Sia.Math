using System;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Sia.Math.CodeGenerators.Capabilities;
using Sia.Math.CodeGenerators.Functions;

namespace Sia.Math.CodeGenerators;

[Generator]
public sealed class MathSourceGenerator : ISourceGenerator
{
    private static readonly Func<TypeShape, CodeFragment> s_CorePipeline =
        Pipeline.Compose(Fields.Generate, SimdInterop.Generate, Constants.Generate,
            Constructors.Generate, Conversions.Generate, Operators.Generate,
            Equality.Generate, ToStringGen.Generate, DebugProxy.Generate);

    private static readonly Func<TypeShape, CodeFragment> s_ExtPipeline =
        Pipeline.Compose(Hashing.Generate, Shuffle.Generate, MatrixOps.Generate);

    private static readonly TypeShape[] s_Shapes = CreateShapes();

    private static TypeShape[] CreateShapes()
    {
        var shapes = new List<TypeShape>();
        for (var r = 2; r <= 4; r++)
        for (var c = 1; c <= 4; c++)
        {
            shapes.Add(new TypeShape(BaseType.Bool, r, c, Features.BitwiseLogic));
            shapes.Add(new TypeShape(BaseType.Int, r, c, Features.All));
            shapes.Add(new TypeShape(BaseType.UInt, r, c, Features.All));
            shapes.Add(new TypeShape(BaseType.Float, r, c, Features.Arithmetic | Features.UnaryNegation));
            shapes.Add(new TypeShape(BaseType.Double, r, c, Features.Arithmetic | Features.UnaryNegation));
        }
        return shapes.ToArray();
    }

    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var results = new ConcurrentBag<(string HintName, string Source)>();

        Parallel.ForEach(s_Shapes, shape =>
        {
            EmitTypeFile(results, shape, "", s_CorePipeline(shape), isPrimary: true);
            EmitTypeFile(results, shape, "Swizzles", Swizzles.Generate(shape), isPrimary: false);
            EmitTypeFile(results, shape, "Math", s_ExtPipeline(shape), isPrimary: false);
        });

        foreach (var (hintName, source) in results)
            context.AddSource(hintName, source);

        AddMathCode(context);
        AddMatrixCode(context);
    }

    private static void EmitTypeFile(ConcurrentBag<(string, string)> results,
        TypeShape shape, string suffix, CodeFragment fragment, bool isPrimary)
    {
        if (fragment.IsEmpty) return;

        var fileName = string.IsNullOrEmpty(suffix)
            ? $"{shape.FilePrefix}.g.cs"
            : $"{shape.FilePrefix}.{suffix}.g.cs";

        var source = RenderTypePartial(shape, fragment, isPrimary);
        results.Add((fileName, source));
    }

    private static string RenderTypePartial(TypeShape shape, CodeFragment fragment, bool isPrimary)
        => ScopeWriter.RenderTypePartial(shape, fragment, isPrimary);

    private static void AddMathCode(GeneratorExecutionContext context)
    {
        var innerSb = new System.Text.StringBuilder();
        var innerWriter = new System.CodeDom.Compiler.IndentedTextWriter(
            new System.IO.StringWriter(innerSb, System.Globalization.CultureInfo.InvariantCulture), "    ");
        innerWriter.Indent = 1;
        MathFunctionCatalog.EmitAll(innerWriter);
        innerWriter.Flush();

        var source = ScopeWriter.RenderStandaloneFile(
            ["System", "System.Runtime.CompilerServices", "System.Runtime.Intrinsics"],
            innerSb.ToString());
        context.AddSource("math.g.cs", source);
    }

    private static void AddMatrixCode(GeneratorExecutionContext context)
    {
        var innerSb = new System.Text.StringBuilder();
        var innerWriter = new System.CodeDom.Compiler.IndentedTextWriter(
            new System.IO.StringWriter(innerSb, System.Globalization.CultureInfo.InvariantCulture), "    ");
        innerWriter.Indent = 1;
        GenerateMatrixMul(innerWriter);
        innerWriter.Flush();

        var source = ScopeWriter.RenderStandaloneFile(
            ["System", "System.Runtime.CompilerServices"],
            innerSb.ToString());
        context.AddSource("matrix.g.cs", source);
    }

    private static void GenerateMatrixMul(IndentedTextWriter w)
    {
        foreach (var type in new[] { BaseType.Int, BaseType.UInt, BaseType.Float, BaseType.Double })
        {
            for (var n = 1; n <= 4; n++)
            for (var m = 1; m <= 4; m++)
            for (var k = 1; k <= 4; k++)
            {
                if (n > 1 && m == 1) continue;
                if (m == 1 && k > 1) continue;

                var lhsIsMatrix = n > 1 && m > 1;
                var rhsIsMatrix = m > 1 && k > 1;
                var lhsSquare = n > 1 && n == m;
                var rhsSquare = m > 1 && m == k;

                if (lhsIsMatrix && rhsIsMatrix && lhsSquare != rhsSquare)
                    continue;

                var resultType = type.ToTypeName(n, k);
                var lhsType = type.ToTypeName(n, m);
                var rhsType = type.ToTypeName(m, k);

                var rowMajor = rhsSquare && (n == 1 || lhsSquare);

                w.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
                w.WriteLine($"public static {resultType} mul({lhsType} a, {rhsType} b)");
                w.WriteLine("{");

                if (lhsSquare && k == 1 && m > 1)
                {
                    var dotTerms = string.Join(", ", Enumerable.Range(0, n)
                        .Select(i => $"dot(a.{TypeShape.MatrixFields[i]}, b)"));
                    w.WriteLine($"    return new {resultType}({dotTerms});");
                }
                else
                {
                    var terms = Enumerable.Range(0, k).Select(col =>
                    {
                        var colTerms = Enumerable.Range(0, m).Select(row =>
                        {
                            string lhsElem, rhsElem;

                            if (rowMajor)
                            {
                                lhsElem = n == 1
                                    ? $"a.{TypeShape.Components[row]}"
                                    : $"a.{TypeShape.MatrixFields[col]}.{TypeShape.Components[row]}";
                                rhsElem = n == 1
                                    ? $"b.{TypeShape.MatrixFields[row]}.{TypeShape.Components[col]}"
                                    : $"b.{TypeShape.MatrixFields[row]}";
                            }
                            else
                            {
                                lhsElem = (n, m) switch
                                {
                                    (1, 1) => "a",
                                    (1, _) or (_, 1) => $"a.{TypeShape.Components[row]}",
                                    _ => $"a.{TypeShape.MatrixFields[row]}"
                                };
                                rhsElem = (m, k) switch
                                {
                                    (1, 1) => "b",
                                    (1, _) or (_, 1) => $"b.{TypeShape.Components[row]}",
                                    _ => $"b.{TypeShape.MatrixFields[col]}.{TypeShape.Components[row]}"
                                };
                            }

                            return col < k ? $"{lhsElem} * {rhsElem}" : lhsElem;
                        });
                        return string.Join(" + ", colTerms);
                    });

                    var body = string.Join(", ", terms);
                    if (k > 1 || n != 1)
                        body = $"new {resultType}({body})";
                    w.WriteLine($"    return {body};");
                }

                w.WriteLine("}");
                w.WriteLineNoTabs(string.Empty);
            }
        }
    }
}
