namespace Sia.Math.CodeGenerators.Capabilities;

using System.Linq;
using System.Text;

public static class Constants
{
    public static CodeFragment Generate(TypeShape shape)
    {
        var body = new StringBuilder();

        if (shape.IsMatrix) {
            body.AppendLine($"        /// <summary>A <see cref=\"{shape.TypeName}\" /> matrix with all cells set to zero.</summary>");
            body.AppendLine($"        public static readonly {shape.TypeName} zero = default;");

            if (shape.IsSquareMatrix && shape.BaseType is BaseType.Float or BaseType.Double) {
                body.AppendLine();
                var columnType = shape.BaseType.ToTypeName(shape.Rows, 1);
                var identityArgs = string.Join(", ", Enumerable.Range(0, shape.Columns)
                    .Select(col => $"new {columnType}({string.Join(", ", Enumerable.Range(0, shape.Rows)
                        .Select(row => row == col ? shape.BaseType.ToTypedLiteral(1) : shape.BaseType.ToTypedLiteral(0)))})"));
                body.AppendLine($"        /// <summary>The <see cref=\"{shape.TypeName}\" /> identity matrix.</summary>");
                body.AppendLine($"        public static readonly {shape.TypeName} identity = new {shape.TypeName}({identityArgs});");
            }
        }
        else {
            body.AppendLine($"        /// <summary>A <see cref=\"{shape.TypeName}\" /> vector with all components set to zero.</summary>");
            body.AppendLine($"        public static readonly {shape.TypeName} zero = default;");

            if (shape.BaseType is BaseType.Float or BaseType.Double or BaseType.Int or BaseType.UInt) {
                body.AppendLine();
                var oneArgs = string.Join(", ", Enumerable.Repeat(shape.BaseType.ToTypedLiteral(1), shape.Rows));
                body.AppendLine($"        /// <summary>A <see cref=\"{shape.TypeName}\" /> vector with all components set to one.</summary>");
                body.AppendLine($"        public static readonly {shape.TypeName} one = new {shape.TypeName}({oneArgs});");
            }
        }

        return new CodeFragment { TypeBody = body.ToString().TrimEnd() };
    }
}
