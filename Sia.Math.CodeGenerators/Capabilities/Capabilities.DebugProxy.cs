namespace Sia.Math.CodeGenerators.Capabilities;

public static class DebugProxy
{
    public static CodeFragment Generate(TypeShape shape)
    {
        if (shape.IsMatrix) {
            return CodeFragment.Empty;
        }

        var body = new System.Text.StringBuilder();
        body.AppendLine("        internal sealed class DebuggerProxy");
        body.AppendLine("        {");
        for (var i = 0; i < shape.Rows; i++) {
            body.AppendLine($"            public {shape.BaseTypeName} {TypeShape.VectorFields[i]};");
        }

        body.AppendLine();
        body.AppendLine($"            public DebuggerProxy({shape.TypeName} v)");
        body.AppendLine("            {");
        for (var i = 0; i < shape.Rows; i++) {
            body.AppendLine($"                {TypeShape.VectorFields[i]} = v.{TypeShape.VectorFields[i]};");
        }

        body.AppendLine("            }");
        body.AppendLine("        }");

        return new CodeFragment { Usings = ["System.Diagnostics"], TypeBody = body.ToString().TrimEnd() };
    }
}
