using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace Sia.Math.CodeGenerators.Writer;

public class TransposeWriter(VectorType type) : IMathSourceWriter
{
    public HashSet<string> Imports { get; } = ["System.Runtime.CompilerServices"];

    public HashSet<string> Inherits { get; } = [];

    public Action<IndentedTextWriter> MathSourceWriter => source =>
    {
        if (type.Columns <= 1)
        {
            return;
        }

        var resultRows = type.Columns;
        var resultColumns = type.Rows;
        var resultType = type.BaseType.ToTypeName(resultRows, resultColumns);
        var resultColumnType = type.BaseType.ToTypeName(resultRows, 1);

        var columnExprs = Enumerable.Range(0, resultColumns)
            .Select(row =>
            {
                var components = Enumerable.Range(0, type.Columns)
                    .Select(col => $"v.{VectorType.MatrixFields[col]}.{VectorType.VectorFields[row]}");
                return $"new {resultColumnType}({string.Join(", ", components)})";
            })
            .ToList();

        source.WriteLine("/// <summary>Returns the transpose of a <see cref=\"{0}\" /> matrix.</summary>", type.TypeName);
        source.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
        source.WriteLine("public static {0} transpose({1} v)", resultType, type.TypeName);
        source.WriteLine("{");
        source.Indent++;
        source.WriteLine("return new {0}(", resultType);
        source.Indent++;
        for (var i = 0; i < columnExprs.Count; i++)
        {
            source.WriteLine(i < columnExprs.Count - 1 ? $"{columnExprs[i]}," : columnExprs[i]);
        }
        source.Indent--;
        source.WriteLine(");");
        source.Indent--;
        source.WriteLine("}");
    };
}
