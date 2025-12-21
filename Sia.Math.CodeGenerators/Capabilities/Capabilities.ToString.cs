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

        if (hasFormatter)
            body.AppendLine("            return ToString(\"G\", System.Globalization.CultureInfo.CurrentCulture);");
        else
        {
            body.AppendLine("            var separator = System.Globalization.NumberFormatInfo.GetInstance(System.Globalization.CultureInfo.CurrentCulture).NumberGroupSeparator;");
            body.AppendLine($"            return $\"{shape.TypeName}({template})\";");
        }
        body.AppendLine("        }");

        if (hasFormatter)
        {
            body.AppendLine();
            body.AppendLine("        public readonly string ToString([System.Diagnostics.CodeAnalysis.StringSyntax(System.Diagnostics.CodeAnalysis.StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? formatProvider)");
            body.AppendLine("        {");
            body.AppendLine("            var separator = System.Globalization.NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;");
            body.AppendLine($"            return $\"{shape.TypeName}({template})\";");
            body.AppendLine("        }");
        }

        return new CodeFragment
        {
            Usings = hasFormatter
                ? ["System.Diagnostics.CodeAnalysis", "System.Globalization"]
                : ["System.Globalization"],
            Inherits = hasFormatter ? ["IFormattable"] : [],
            TypeBody = body.ToString().TrimEnd()
        };
    }

    private static string BuildTemplate(TypeShape shape)
    {
        var sb = new StringBuilder();
        for (var row = 0; row < shape.Rows; row++)
        {
            for (var col = 0; col < shape.Columns; col++)
            {
                var idx = row * shape.Columns + col;
                if (idx > 0) sb.Append("{separator} ");
                sb.Append('{');
                sb.Append(shape.IsMatrix
                    ? $"{TypeShape.MatrixFields[col]}.{TypeShape.VectorFields[row]}"
                    : TypeShape.VectorFields[row]);
                if (shape.BaseType != BaseType.Bool) sb.Append(".ToString(format, formatProvider)");
                sb.Append('}');
                if (shape.BaseType == BaseType.Float) sb.Append('f');
            }
        }
        return sb.ToString();
    }
}
