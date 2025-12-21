using System.Linq;

namespace Sia.Math.CodeGenerators.Capabilities;

public static class Fields
{
    public static CodeFragment Generate(TypeShape shape)
    {
        if (shape.IsMatrix)
            return GenerateMatrix(shape);
        return GenerateVector(shape);
    }

    private static CodeFragment GenerateVector(TypeShape shape)
    {
        var useMarshal = shape.BaseType == BaseType.Bool;
        var imports = useMarshal ? new[] { "System.Runtime.InteropServices" } : System.Array.Empty<string>();

        var body = new System.Text.StringBuilder();
        foreach (var i in Enumerable.Range(0, shape.Rows))
        {
            if (useMarshal)
                body.AppendLine($"        [MarshalAs(UnmanagedType.U1)]");
            body.AppendLine($"        public {shape.BaseTypeName} {TypeShape.VectorFields[i]};");
        }

        if (shape.NeedsPadding)
            body.AppendLine($"        internal {shape.BaseTypeName} __pad;");

        return new CodeFragment
        {
            Usings = imports,
            TypeBody = body.ToString().TrimEnd()
        };
    }

    private static CodeFragment GenerateMatrix(TypeShape shape)
    {
        var columnType = shape.BaseType.ToTypeName(shape.Rows, 1);
        var body = new System.Text.StringBuilder();
        for (var i = 0; i < shape.Columns; i++)
            body.AppendLine($"        public {columnType} {TypeShape.MatrixFields[i]};");

        return new CodeFragment { TypeBody = body.ToString().TrimEnd() };
    }
}
