namespace Sia.Math.CodeGenerators.Capabilities;

using System.Linq;

public static class Equality
{
    public static CodeFragment Generate(TypeShape shape)
    {
        var fields = shape.IsMatrix ? TypeShape.MatrixFields : TypeShape.VectorFields;
        var count = shape.IsMatrix ? shape.Columns : shape.Rows;
        var cat = shape.IsMatrix ? ("matrix", "matrices") : ("vector", "vectors");

        var conditions = string.Join(" && ", Enumerable.Range(0, count)
            .Select(i => shape.IsMatrix
                ? $"{fields[i]}.Equals(other.{fields[i]})"
                : $"{fields[i]} == other.{fields[i]}"));

        var body = new System.Text.StringBuilder();
        body.AppendLine($"        /// <summary>Returns true if this and another <see cref=\"{shape.TypeName}\" /> {cat.Item1} are equal.</summary>");
        body.AppendLine("        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]");
        body.AppendLine($"        public readonly bool Equals({shape.TypeName} other) => {conditions};");
        body.AppendLine();
        body.AppendLine("        public override readonly bool Equals([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] object? obj)");
        body.AppendLine("        {");
        body.AppendLine($"            return (obj is {shape.TypeName} other) && Equals(other);");
        body.AppendLine("        }");

        return new CodeFragment {
            Usings = ["System.Diagnostics.CodeAnalysis", "System.Runtime.CompilerServices"],
            Inherits = [$"System.IEquatable<{shape.TypeName}>"],
            TypeBody = body.ToString().TrimEnd()
        };
    }
}
