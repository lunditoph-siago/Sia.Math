using Microsoft.CodeAnalysis;
using Sia.Math.CodeGenerators.Writer;

namespace Sia.Math.CodeGenerators;

public partial class MathSourceGenerator
{
    private static void AddMatrixCode(ref GeneratorExecutionContext context)
    {
        var source = Generator.CreateFileSource(out var builder);
        var mathWriter = new MathOnlyCompositeWriter();
        GenerateMulImplementations(mathWriter, BaseType.Int);
        GenerateMulImplementations(mathWriter, BaseType.UInt);
        GenerateMulImplementations(mathWriter, BaseType.Float);
        GenerateMulImplementations(mathWriter, BaseType.Double);

        source.WriteLine("using System;");

        foreach (var import in mathWriter.Imports)
            source.WriteLine("using {0};", import);

        source.WriteLine();
        source.WriteLine("#pragma warning disable 0660, 0661, 8981");
        source.WriteLine();

        using (Generator.GenerateInNamespace(source))
        {
            using (Generator.GenerateInMath(source))
            {
                mathWriter.MathSourceWriter.Invoke(source);
            }
        }

        context.AddSource("matrix.g.cs", builder.ToString());
    }

    private static void GenerateMulImplementations(MathOnlyCompositeWriter writer, BaseType type)
    {

        for (var n = 1; n <= 4; n++)
        {
            for (var m = 1; m <= 4; m++)
            {
                for (var k = 1; k <= 4; k++)
                {
                    if (n > 1 && m == 1)
                        continue;
                    if (m == 1 && k > 1)
                        continue;

                    var lhsIsMatrix = n > 1 && m > 1;
                    var rhsIsMatrix = m > 1 && k > 1;
                    var lhsSquare = n > 1 && n == m;
                    var rhsSquare = m > 1 && m == k;

                    if (lhsIsMatrix && rhsIsMatrix && lhsSquare != rhsSquare)
                        continue;

                    writer.Add(new MulWriter(type, (n, m), (m, k), GenerateMulType.Mul, false));
                }
            }
        }
    }
}
