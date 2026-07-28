namespace Sia.Math.CodeGenerators.Generation;

using System.Collections.Generic;
using System.Linq;

public static class MatrixMultiplicationEmitter
{
    private static readonly BaseType[] BaseTypes =
    [
        BaseType.Int,
        BaseType.UInt,
        BaseType.Float,
        BaseType.Double,
    ];

    public static GeneratedSource Generate()
    {
        var body = new SourceBuilder(indent: 1);

        foreach (var signature in GetSignatures()) {
            GenerateMethod(signature, body);
        }

        var source = ScopeWriter.GenerateStandaloneFile(
            ["System", "System.Runtime.CompilerServices", "System.Runtime.Intrinsics"],
            body.ToString());
        return new GeneratedSource("matrix.g.cs", source);
    }

    private static IEnumerable<MatrixMultiplicationSignature> GetSignatures()
    {
        foreach (var type in BaseTypes) {
            for (var rows = 1; rows <= 4; rows++) {
                for (var shared = 1; shared <= 4; shared++) {
                    for (var columns = 1; columns <= 4; columns++) {
                        if (rows > 1 && shared == 1) {
                            continue;
                        }

                        if (shared == 1 && columns > 1) {
                            continue;
                        }

                        var lhsIsMatrix = rows > 1 && shared > 1;
                        var rhsIsMatrix = shared > 1 && columns > 1;
                        var lhsSquare = rows > 1 && rows == shared;
                        var rhsSquare = shared > 1 && shared == columns;

                        if (lhsIsMatrix && rhsIsMatrix && lhsSquare != rhsSquare) {
                            continue;
                        }

                        yield return new MatrixMultiplicationSignature(
                            type,
                            rows,
                            shared,
                            columns,
                            lhsIsMatrix,
                            rhsIsMatrix);
                    }
                }
            }
        }
    }

    private static void GenerateMethod(
        MatrixMultiplicationSignature signature,
        SourceBuilder source)
    {
        var declaration =
            $"public static {signature.ResultType} mul({signature.LeftParameter} a, {signature.RightParameter} b)";

        source.Line("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
        source.Block(declaration, body => {
            body.Line($"return {GenerateProduct(signature)};");
        });
        source.Line();
    }

    private static string GenerateProduct(MatrixMultiplicationSignature signature)
    {
        var terms = Enumerable.Range(0, signature.Columns)
            .Select(column => GenerateColumn(signature, column));
        var expression = string.Join(", ", terms);

        return signature.Columns > 1 || signature.Rows != 1
            ? $"new {signature.ResultType}({expression})"
            : expression;
    }

    private static string GenerateColumn(
        MatrixMultiplicationSignature signature,
        int column)
    {
        if (signature.Rows > 1 && signature.Shared > 1
            && signature.Type is BaseType.Float or BaseType.Double) {
            return GenerateFusedColumn(signature, column);
        }

        var terms = Enumerable.Range(0, signature.Shared)
            .Select(row => GenerateTerm(signature, row, column));
        return string.Join(" + ", terms);
    }

    private static string GenerateFusedColumn(
        MatrixMultiplicationSignature signature,
        int column)
    {
        var columnType = signature.Type.ToTypeName(signature.Rows, 1);
        var vectorClass = Simd.SimdStrategy.NativeVectorClassName(signature.Type, signature.Rows);

        var expression = $"{GenerateLeft(signature, 0)}.data * {vectorClass}.Create({GenerateRight(signature, 0, column)})";
        for (var row = 1; row < signature.Shared; row++) {
            expression = $"{vectorClass}.FusedMultiplyAdd({GenerateLeft(signature, row)}.data, {vectorClass}.Create({GenerateRight(signature, row, column)}), {expression})";
        }

        return $"new {columnType}({expression})";
    }

    private static string GenerateLeft(
        MatrixMultiplicationSignature signature,
        int row) =>
        (signature.Rows, signature.Shared) switch {
            (1, 1) => "a",
            (1, _) or (_, 1) => $"a.{TypeShape.Components[row]}",
            _ => $"a.{TypeShape.MatrixFields[row]}",
        };

    private static string GenerateRight(
        MatrixMultiplicationSignature signature,
        int row,
        int column) =>
        (signature.Shared, signature.Columns) switch {
            (1, 1) => "b",
            (1, _) or (_, 1) => $"b.{TypeShape.Components[row]}",
            _ => $"b.{TypeShape.MatrixFields[column]}.{TypeShape.Components[row]}",
        };

    private static string GenerateTerm(
        MatrixMultiplicationSignature signature,
        int row,
        int column) =>
        $"{GenerateLeft(signature, row)} * {GenerateRight(signature, row, column)}";

    private readonly record struct MatrixMultiplicationSignature(
        BaseType Type,
        int Rows,
        int Shared,
        int Columns,
        bool LeftIsMatrix,
        bool RightIsMatrix)
    {
        public string ResultType => Type.ToTypeName(Rows, Columns);
        public string LeftType => Type.ToTypeName(Rows, Shared);
        public string RightType => Type.ToTypeName(Shared, Columns);
        public string LeftParameter => LeftIsMatrix ? $"in {LeftType}" : LeftType;
        public string RightParameter => RightIsMatrix ? $"in {RightType}" : RightType;
    }
}
