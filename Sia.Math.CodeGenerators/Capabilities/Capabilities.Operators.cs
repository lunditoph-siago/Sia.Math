namespace Sia.Math.CodeGenerators.Capabilities;

using System;
using System.Linq;

using Sia.Math.CodeGenerators.Functions;

public static class Operators
{
    public static CodeFragment Generate(TypeShape shape)
    {
        var body = new SourceBuilder(indent: 2);

        GenerateIndexer(shape, body);

        var operations = shape.Operations;
        if ((operations & Features.Arithmetic) != 0) {
            GenerateBinary(shape, "*", body);
            GenerateBinary(shape, "+", body);
            GenerateBinary(shape, "-", body);
            GenerateBinary(shape, "/", body);
            GenerateBinary(shape, "%", body);
            GenerateUnary(shape, "++", body);
            GenerateUnary(shape, "--", body);
            GenerateBinary(shape, "<", body);
            GenerateBinary(shape, "<=", body);
            GenerateBinary(shape, ">", body);
            GenerateBinary(shape, ">=", body);
        }

        if ((operations & Features.UnaryNegation) != 0) {
            GenerateUnary(shape, "-", body);
            GenerateUnary(shape, "+", body);
        }

        if ((operations & Features.Shifts) != 0) {
            GenerateShift(shape, "<<", body);
            GenerateShift(shape, ">>", body);
        }

        GenerateBinary(shape, "==", body);
        GenerateBinary(shape, "!=", body);

        if ((operations & Features.BitwiseComplement) != 0) {
            GenerateUnary(shape, "~", body);
        }

        if (shape.BaseType == BaseType.Bool) {
            GenerateUnary(shape, "!", body);
        }

        if ((operations & Features.BitwiseLogic) != 0) {
            GenerateBinary(shape, "&", body);
            GenerateBinary(shape, "|", body);
            GenerateBinary(shape, "^", body);
        }

        return new CodeFragment {
            Usings =
            [
                "System.Numerics",
                "System.Runtime.CompilerServices",
                "System.Runtime.Intrinsics",
            ],
            TypeBody = body.ToString().TrimEnd(),
        };
    }

    private static void GenerateIndexer(TypeShape shape, SourceBuilder source)
    {
        var count = shape.IsMatrix ? shape.Columns : shape.Rows;
        var returnType = shape.IsMatrix
            ? shape.BaseType.ToTypeName(shape.Rows, 1)
            : shape.BaseType.ToBaseTypeName();
        var byRef = shape.IsMatrix ? "ref " : string.Empty;

        source.Block($"public unsafe {byRef}{returnType} this[int index]", property => {
            property.Block("get", getter => {
                getter.Line("#if DEBUG");
                getter.Line(
                    $"if ((uint)index >= {count}) throw new System.ArgumentException(\"index must be between [0...{count - 1}]\");");
                getter.Line("#endif");
                getter.Line(
                    $"fixed ({shape.TypeName}* array = &this) {{ return {byRef}(({returnType}*)array)[index]; }}");
            });

            if (shape.IsMatrix) {
                return;
            }

            property.Block("set", setter => {
                setter.Line("#if DEBUG");
                setter.Line(
                    $"if ((uint)index >= {count}) throw new System.ArgumentException(\"index must be between [0...{count - 1}]\");");
                setter.Line("#endif");
                setter.Line(
                    $"fixed ({shape.TypeName}* self = &this) {{ (({returnType}*)self)[index] = value; }}");
            });
        });
    }

    private static void GenerateBinary(
        TypeShape shape,
        string operation,
        SourceBuilder source)
    {
        var isComparison =
            operation is "==" or "!=" or "<" or "<=" or ">" or ">=";
        var resultType = isComparison ? BaseType.Bool : shape.BaseType;
        var resultName = resultType.ToTypeName(shape.Rows, shape.Columns);
        var typeName = shape.TypeName;
        var parameterType = shape.IsMatrix ? $"in {typeName}" : typeName;
        var scalar = shape.BaseType.ToBaseTypeName();

        if (!isComparison
            && shape.IsVector
            && Simd.SimdStrategy.SupportsSimdOp(
                operation,
                shape.BaseType,
                shape.Rows)) {
            GenerateSimdBinaryOverloads(
                shape,
                operation,
                resultName,
                typeName,
                scalar,
                source);
            return;
        }

        var fields = shape.IsMatrix
            ? TypeShape.MatrixFields
            : TypeShape.VectorFields;
        var count = shape.IsMatrix ? shape.Columns : shape.Rows;

        GenerateExpression(
            source,
            $"public static {resultName} operator {operation}({parameterType} lhs, {parameterType} rhs)",
            Construct(
                resultName,
                count,
                index => $"lhs.{fields[index]} {operation} rhs.{fields[index]}"));
        GenerateExpression(
            source,
            $"public static {resultName} operator {operation}({scalar} lhs, {parameterType} rhs)",
            Construct(
                resultName,
                count,
                index => $"lhs {operation} rhs.{fields[index]}"));
        GenerateExpression(
            source,
            $"public static {resultName} operator {operation}({parameterType} lhs, {scalar} rhs)",
            Construct(
                resultName,
                count,
                index => $"lhs.{fields[index]} {operation} rhs"));
    }

    private static void GenerateSimdBinaryOverloads(
        TypeShape shape,
        string operation,
        string resultName,
        string typeName,
        string scalar,
        SourceBuilder source)
    {
        var broadcastLeft = Simd.SimdStrategy.CreateBroadcast(
            shape.BaseType,
            shape.Rows,
            "lhs");
        var broadcastRight = Simd.SimdStrategy.CreateBroadcast(
            shape.BaseType,
            shape.Rows,
            "rhs");

        GenerateMethod(
            source,
            FunctionEmitter.GenerateSimd(
                $"public static {resultName} operator {operation}({typeName} lhs, {typeName} rhs)",
                shape,
                $"new {resultName}(lhs.data {operation} rhs.data)",
                Construct(
                    resultName,
                    shape.Rows,
                    index =>
                        $"lhs.{TypeShape.Components[index]} {operation} rhs.{TypeShape.Components[index]}")));
        GenerateMethod(
            source,
            FunctionEmitter.GenerateSimd(
                $"public static {resultName} operator {operation}({scalar} lhs, {typeName} rhs)",
                shape,
                $"new {resultName}({broadcastLeft} {operation} rhs.data)",
                Construct(
                    resultName,
                    shape.Rows,
                    index =>
                        $"lhs {operation} rhs.{TypeShape.Components[index]}")));
        GenerateMethod(
            source,
            FunctionEmitter.GenerateSimd(
                $"public static {resultName} operator {operation}({typeName} lhs, {scalar} rhs)",
                shape,
                $"new {resultName}(lhs.data {operation} {broadcastRight})",
                Construct(
                    resultName,
                    shape.Rows,
                    index =>
                        $"lhs.{TypeShape.Components[index]} {operation} rhs")));
    }

    private static void GenerateUnary(
        TypeShape shape,
        string operation,
        SourceBuilder source)
    {
        var typeName = shape.TypeName;
        var parameterType = shape.IsMatrix ? $"in {typeName}" : typeName;
        var fields = shape.IsMatrix
            ? TypeShape.MatrixFields
            : TypeShape.VectorFields;
        var count = shape.IsMatrix ? shape.Columns : shape.Rows;

        if ((operation is "-" or "~" or "++" or "--")
            && shape.IsVector
            && shape.IsSimdEligible) {
            var scalar = Construct(
                typeName,
                shape.Rows,
                index => operation switch {
                    "++" => $"val.{TypeShape.Components[index]} + 1",
                    "--" => $"val.{TypeShape.Components[index]} - 1",
                    "-" when shape.BaseType == BaseType.UInt =>
                        $"(uint)-val.{TypeShape.Components[index]}",
                    _ => $"{operation}val.{TypeShape.Components[index]}",
                });
            var portable = operation switch {
                "++" => $"val.data + {Simd.SimdStrategy.CreateBroadcast(shape.BaseType, shape.Rows, shape.BaseType.ToTypedLiteral(1))}",
                "--" => $"val.data - {Simd.SimdStrategy.CreateBroadcast(shape.BaseType, shape.Rows, shape.BaseType.ToTypedLiteral(1))}",
                _ => $"{operation}val.data",
            };
            GenerateMethod(
                source,
                FunctionEmitter.GenerateSimd(
                    $"public static {typeName} operator {operation}({typeName} val)",
                    shape,
                    $"new {typeName}({portable})",
                    scalar));
            return;
        }

        var expression = operation switch {
            "++" => Construct(
                typeName,
                count,
                index => $"val.{fields[index]} + 1"),
            "--" => Construct(
                typeName,
                count,
                index => $"val.{fields[index]} - 1"),
            _ => Construct(
                typeName,
                count,
                index =>
                    operation == "-" && shape is {
                        BaseType: BaseType.UInt,
                        Columns: 1,
                    }
                        ? $"(uint)-val.{fields[index]}"
                        : $"{operation}val.{fields[index]}"),
        };

        GenerateExpression(
            source,
            $"public static {typeName} operator {operation}({parameterType} val)",
            expression);
    }

    private static void GenerateShift(
        TypeShape shape,
        string operation,
        SourceBuilder source)
    {
        var typeName = shape.TypeName;
        var parameterType = shape.IsMatrix ? $"in {typeName}" : typeName;
        var fields = shape.IsMatrix
            ? TypeShape.MatrixFields
            : TypeShape.VectorFields;
        var count = shape.IsMatrix ? shape.Columns : shape.Rows;

        if (shape.IsVector && shape.IsSimdEligible) {
            GenerateMethod(
                source,
                FunctionEmitter.GenerateSimd(
                    $"public static {typeName} operator {operation}({typeName} x, int n)",
                    shape,
                    $"new {typeName}(x.data {operation} n)",
                    Construct(
                        typeName,
                        shape.Rows,
                        index =>
                            $"x.{TypeShape.Components[index]} {operation} n")));
            return;
        }

        GenerateExpression(
            source,
            $"public static {typeName} operator {operation}({parameterType} x, int n)",
            Construct(
                typeName,
                count,
                index =>
                    shape.Rows == 1
                        ? $"x {operation} n"
                        : $"x.{fields[index]} {operation} n"));
    }

    private static string Construct(
        string typeName,
        int count,
        Func<int, string> component) =>
        $"new {typeName}({string.Join(", ", Enumerable.Range(0, count).Select(component))})";

    private static void GenerateExpression(
        SourceBuilder source,
        string declaration,
        string expression) =>
        GenerateMethod(source, FunctionEmitter.GenerateExpression(declaration, expression));

    private static void GenerateMethod(SourceBuilder source, string[] lines)
    {
        source.Line();
        source.Lines(lines);
    }
}
