using System.Linq;
using System.Text;

namespace Sia.Math.CodeGenerators.Capabilities;

public static class ToStringGen
{
    public static CodeFragment Generate(TypeShape shape)
    {
        var template = BuildTemplate(shape);
        var hasFormatter = shape.BaseType != BaseType.Bool;
        var body = new StringBuilder();

        body.AppendLine($"        /// <summary>Returns the string representation of the <see cref=\"{shape.TypeName}\" />.</summary>");
        body.AppendLine("        public readonly override string ToString()");
        body.AppendLine("        {");
        body.AppendLine(hasFormatter
            ? "            return ToString(\"G\", System.Globalization.CultureInfo.CurrentCulture);"
            : $"            return $\"{shape.TypeName}({template})\";");
        body.AppendLine("        }");

        if (hasFormatter)
        {
            body.AppendLine();
            body.AppendLine("        public readonly string ToString([System.Diagnostics.CodeAnalysis.StringSyntax(System.Diagnostics.CodeAnalysis.StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? formatProvider)");
            body.AppendLine("        {");
            body.AppendLine($"            return $\"{shape.TypeName}({template})\";");
            body.AppendLine("        }");
        }

        return new CodeFragment
        {
            Usings = hasFormatter ? ["System.Diagnostics.CodeAnalysis"] : [],
            Inherits = hasFormatter ? ["IFormattable"] : [],
            TypeBody = body.ToString().TrimEnd()
        };
    }

    private static string BuildTemplate(TypeShape shape)
    {
        var cells =
            from row in Enumerable.Range(0, shape.Rows)
            from col in Enumerable.Range(0, shape.Columns)
            select FormatCell(shape, row, col);
        return string.Join(", ", cells);
    }

    private static string FormatCell(TypeShape shape, int row, int col)
    {
        var field = shape.IsMatrix
            ? $"{TypeShape.MatrixFields[col]}.{TypeShape.VectorFields[row]}"
            : TypeShape.VectorFields[row];
        var expr = shape.BaseType == BaseType.Bool ? field : $"{field}.ToString(format, formatProvider)";
        var suffix = shape.BaseType == BaseType.Float ? "f" : "";
        return $"{{{expr}}}{suffix}";
    }
}
