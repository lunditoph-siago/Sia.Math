using System.Linq;
using System.Text;

namespace Sia.Math.CodeGenerators.Capabilities;

public static class Operators
{
    public static CodeFragment Generate(TypeShape shape)
    {
        var typeBody = new StringBuilder();

        EmitIndexer(shape, typeBody);

        var ops = shape.Operations;

        if ((ops & Features.Arithmetic) != 0)
        {
            EmitBinaryOp(shape, "*", typeBody);
            EmitBinaryOp(shape, "+", typeBody);
            EmitBinaryOp(shape, "-", typeBody);
            EmitBinaryOp(shape, "/", typeBody);
            EmitBinaryOp(shape, "%", typeBody);
            EmitUnaryOp(shape, "++", typeBody);
            EmitUnaryOp(shape, "--", typeBody);
            EmitBinaryOp(shape, "<", typeBody);
            EmitBinaryOp(shape, "<=", typeBody);
            EmitBinaryOp(shape, ">", typeBody);
            EmitBinaryOp(shape, ">=", typeBody);
        }

        if ((ops & Features.UnaryNegation) != 0)
        {
            EmitUnaryOp(shape, "-", typeBody);
            EmitUnaryOp(shape, "+", typeBody);
        }

        if ((ops & Features.Shifts) != 0)
        {
            EmitShiftOp(shape, "<<", typeBody);
            EmitShiftOp(shape, ">>", typeBody);
        }

        EmitBinaryOp(shape, "==", typeBody);
        EmitBinaryOp(shape, "!=", typeBody);

        if ((ops & Features.BitwiseComplement) != 0)
            EmitUnaryOp(shape, "~", typeBody);

        if (shape.BaseType == BaseType.Bool)
            EmitUnaryOp(shape, "!", typeBody);

        if ((ops & Features.BitwiseLogic) != 0)
        {
            EmitBinaryOp(shape, "&", typeBody);
            EmitBinaryOp(shape, "|", typeBody);
            EmitBinaryOp(shape, "^", typeBody);
        }

        return new CodeFragment
        {
            Usings = ["System.Numerics", "System.Runtime.CompilerServices", "System.Runtime.Intrinsics"],
            TypeBody = typeBody.ToString().TrimEnd()
        };
    }

    private static void EmitIndexer(TypeShape shape, StringBuilder body)
    {
        var count = shape.IsMatrix ? shape.Columns : shape.Rows;
        var returnType = shape.IsMatrix ? shape.BaseType.ToTypeName(shape.Rows, 1) : shape.BaseType.ToBaseTypeName();
        var isByRef = shape.IsMatrix;
        var refPrefix = isByRef ? "ref " : "";

        body.AppendLine($"        public unsafe {refPrefix}{returnType} this[int index]");
        body.AppendLine("        {");
        body.AppendLine("            get");
        body.AppendLine("            {");
        body.AppendLine("#if DEBUG");
        body.AppendLine($"                if ((uint)index >= {count}) throw new System.ArgumentException(\"index must be between [0...{count - 1}]\");");
        body.AppendLine("#endif");
        body.AppendLine($"                fixed ({shape.TypeName}* array = &this) {{ return {refPrefix}(({returnType}*)array)[index]; }}");
        body.AppendLine("            }");
        if (!isByRef)
        {
            body.AppendLine("            set");
            body.AppendLine("            {");
            body.AppendLine("#if DEBUG");
            body.AppendLine($"                if ((uint)index >= {count}) throw new System.ArgumentException(\"index must be between [0...{count - 1}]\");");
            body.AppendLine("#endif");
            body.AppendLine($"                fixed ({shape.TypeName}* self = &this) {{ (({returnType}*)self)[index] = value; }}");
            body.AppendLine("            }");
        }
        body.AppendLine("        }");
    }

    private static void EmitBinaryOp(TypeShape shape, string op, StringBuilder body)
    {
        var isCompare = op is "==" or "!=" or "<" or "<=" or ">" or ">=";
        var resultType = isCompare ? BaseType.Bool : shape.BaseType;
        var resultName = resultType.ToTypeName(shape.Rows, shape.Columns);
        var typeName = shape.TypeName;
        var paramType = shape.IsMatrix ? $"in {typeName}" : typeName;
        var fields = shape.IsMatrix ? TypeShape.MatrixFields : TypeShape.VectorFields;
        var count = shape.IsMatrix ? shape.Columns : shape.Rows;
        var scalar = shape.BaseType.ToBaseTypeName();

        body.AppendLine();
        body.AppendLine("        [MethodImpl(MethodImplOptions.AggressiveInlining)]");

        if (isCompare)
        {
            body.AppendLine($"        public static {resultName} operator {op}({paramType} lhs, {paramType} rhs) => new {resultName}({string.Join(", ", Enumerable.Range(0, count).Select(i => $"lhs.{fields[i]} {op} rhs.{fields[i]}"))});");
            body.AppendLine($"        public static {resultName} operator {op}({scalar} lhs, {paramType} rhs) => new {resultName}({string.Join(", ", Enumerable.Range(0, count).Select(i => $"lhs {op} rhs.{fields[i]}"))});");
            body.AppendLine($"        public static {resultName} operator {op}({paramType} lhs, {scalar} rhs) => new {resultName}({string.Join(", ", Enumerable.Range(0, count).Select(i => $"lhs.{fields[i]} {op} rhs"))});");
        }
        else if (!isCompare && !shape.IsMatrix && Simd.SimdStrategy.SupportsSimdOp(op, shape.BaseType, shape.Rows))
        {
            body.AppendLine($"        public static {resultName} operator {op}({typeName} lhs, {typeName} rhs) => new {resultName}(lhs.data {op} rhs.data);");
            body.AppendLine($"        public static {resultName} operator {op}({scalar} lhs, {typeName} rhs) => new {resultName}({Simd.SimdStrategy.CreateBroadcast(shape.BaseType, shape.Rows, "lhs")} {op} rhs.data);");
            body.AppendLine($"        public static {resultName} operator {op}({typeName} lhs, {scalar} rhs) => new {resultName}(lhs.data {op} {Simd.SimdStrategy.CreateBroadcast(shape.BaseType, shape.Rows, "rhs")});");
        }
        else
        {
            body.AppendLine($"        public static {resultName} operator {op}({paramType} lhs, {paramType} rhs) => new {resultName}({string.Join(", ", Enumerable.Range(0, count).Select(i => $"lhs.{fields[i]} {op} rhs.{fields[i]}"))});");
            body.AppendLine($"        public static {resultName} operator {op}({scalar} lhs, {paramType} rhs) => new {resultName}({string.Join(", ", Enumerable.Range(0, count).Select(i => $"lhs {op} rhs.{fields[i]}"))});");
            body.AppendLine($"        public static {resultName} operator {op}({paramType} lhs, {scalar} rhs) => new {resultName}({string.Join(", ", Enumerable.Range(0, count).Select(i => $"lhs.{fields[i]} {op} rhs"))});");
        }
    }

    private static void EmitUnaryOp(TypeShape shape, string op, StringBuilder body)
    {
        var typeName = shape.TypeName;
        var paramType = shape.IsMatrix ? $"in {typeName}" : typeName;
        body.AppendLine();
        body.AppendLine("        [MethodImpl(MethodImplOptions.AggressiveInlining)]");

        if ((op is "-" or "~") && shape.IsSimdEligible && !shape.IsMatrix)
        {
            body.AppendLine($"        public static {typeName} operator {op}({typeName} val) => new {typeName}({op}val.data);");
        }
        else if (op is "++" or "--")
        {
            var incOp = op == "++" ? "+" : "-";
            var fields = shape.IsMatrix ? TypeShape.MatrixFields : TypeShape.VectorFields;
            var count = shape.IsMatrix ? shape.Columns : shape.Rows;
            body.AppendLine($"        public static {typeName} operator {op}({paramType} val) => new {typeName}({string.Join(", ", Enumerable.Range(0, count).Select(i => $"val.{fields[i]} {incOp} 1"))});");
        }
        else
        {
            var fields = shape.IsMatrix ? TypeShape.MatrixFields : TypeShape.VectorFields;
            var count = shape.IsMatrix ? shape.Columns : shape.Rows;
            var comps = string.Join(", ", Enumerable.Range(0, count).Select(i =>
                op == "-" && shape is { BaseType: BaseType.UInt, Columns: 1 }
                    ? $"(uint){op}val.{fields[i]}"
                    : $"{op}val.{fields[i]}"));
            body.AppendLine($"        public static {typeName} operator {op}({paramType} val) => new {typeName}({comps});");
        }
    }

    private static void EmitShiftOp(TypeShape shape, string op, StringBuilder body)
    {
        var typeName = shape.TypeName;
        var paramType = shape.IsMatrix ? $"in {typeName}" : typeName;
        body.AppendLine();
        body.AppendLine("        [MethodImpl(MethodImplOptions.AggressiveInlining)]");

        if (shape.IsSimdEligible && !shape.IsMatrix)
        {
            body.AppendLine($"        public static {typeName} operator {op}({typeName} x, int n) => new {typeName}(x.data {op} n);");
        }
        else
        {
            var fields = shape.IsMatrix ? TypeShape.MatrixFields : TypeShape.VectorFields;
            var count = shape.IsMatrix ? shape.Columns : shape.Rows;
            var comps = string.Join(", ", Enumerable.Range(0, count).Select(i => shape.Rows == 1 ? $"x {op} n" : $"x.{fields[i]} {op} n"));
            body.AppendLine($"        public static {typeName} operator {op}({paramType} x, int n) => new {typeName}({comps});");
        }
    }
}
