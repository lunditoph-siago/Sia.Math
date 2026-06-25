namespace Sia.Math.CodeGenerators.Capabilities;

using System.Linq;
using System.Text;

public static class Fields
{
    public static CodeFragment Generate(TypeShape shape)
    {
        if (shape.IsMatrix) {
            return GenerateMatrix(shape);
        }

        return shape.BaseType == BaseType.Bool
            ? GenerateBoolVector(shape)
            : GenerateSimdVector(shape);
    }

    private static CodeFragment GenerateBoolVector(TypeShape shape)
    {
        var body = new StringBuilder();
        foreach (var i in Enumerable.Range(0, shape.Rows)) {
            body.AppendLine($"        [MarshalAs(UnmanagedType.U1)]");
            body.AppendLine($"        public {shape.BaseTypeName} {TypeShape.VectorFields[i]};");
        }

        return new CodeFragment {
            Usings = ["System.Runtime.InteropServices"],
            TypeBody = body.ToString().TrimEnd()
        };
    }

    private static CodeFragment GenerateSimdVector(TypeShape shape)
    {
        var vectorTypeName = Simd.SimdStrategy.NativeVectorTypeName(shape.BaseType, shape.Rows);
        var body = new StringBuilder();
        body.AppendLine($"        public {vectorTypeName} data;");

        foreach (var i in Enumerable.Range(0, shape.Rows)) {
            body.AppendLine();
            body.AppendLine($"        public {shape.BaseTypeName} {TypeShape.VectorFields[i]}");
            body.AppendLine("        {");
            body.AppendLine("            [MethodImpl(MethodImplOptions.AggressiveInlining)]");
            body.AppendLine($"            readonly get => data.GetElement({i});");
            body.AppendLine("            [MethodImpl(MethodImplOptions.AggressiveInlining)]");
            body.AppendLine($"            set => data = data.WithElement({i}, value);");
            body.AppendLine("        }");
        }

        body.AppendLine();
        body.AppendLine("        [MethodImpl(MethodImplOptions.AggressiveInlining)]");
        body.AppendLine($"        public {shape.TypeName}({vectorTypeName} v) => data = v;");

        return new CodeFragment {
            Usings = ["System.Runtime.CompilerServices", "System.Runtime.Intrinsics"],
            TypeBody = body.ToString().TrimEnd()
        };
    }

    private static CodeFragment GenerateMatrix(TypeShape shape)
    {
        var columnType = shape.BaseType.ToTypeName(shape.Rows, 1);
        var body = new StringBuilder();
        for (var i = 0; i < shape.Columns; i++) {
            body.AppendLine($"        public {columnType} {TypeShape.MatrixFields[i]};");
        }

        return new CodeFragment { TypeBody = body.ToString().TrimEnd() };
    }
}
