namespace Sia.Math.CodeGenerators.Functions;

using System;

using Sia.Math.CodeGenerators.Simd;

public static class FunctionEmitter
{
    public const string AggressiveInlining =
        "[MethodImpl(MethodImplOptions.AggressiveInlining)]";

    public static string[] GenerateExpression(
        string declaration,
        string expression) =>
    [
        AggressiveInlining,
        $"{declaration} => {expression};",
    ];

    public static string[] GenerateBlock(
        string declaration,
        params string[] statements)
    {
        var source = new SourceBuilder();
        source.Line(AggressiveInlining);
        source.Block(declaration, body => body.Lines(statements));
        return SplitLines(source.ToString());
    }

    public static string[] GenerateSimd(
        string declaration,
        TypeShape shape,
        string portableExpression,
        string scalarExpression,
        Action<SimdDispatch>? configure = null)
        => GenerateSimd(
            declaration,
            shape,
            SimdDispatch.Return(portableExpression),
            SimdDispatch.Return(scalarExpression),
            configure);

    public static string[] GenerateSimd(
        string declaration,
        TypeShape shape,
        string[] portableStatements,
        string[] scalarStatements,
        Action<SimdDispatch>? configure = null)
    {
        var profile = SimdTypeProfile.Create(shape.BaseType, shape.Rows);
        if (!profile.IsSimd) {
            return GenerateBlock(declaration, scalarStatements);
        }

        var dispatch = new SimdDispatch(
            profile,
            portableStatements,
            scalarStatements);
        configure?.Invoke(dispatch);

        var source = new SourceBuilder();
        source.Line(AggressiveInlining);
        source.Block(declaration, dispatch.Generate);
        return SplitLines(source.ToString());
    }

    private static string[] SplitLines(string source)
        => source.TrimEnd('\r', '\n')
            .Replace("\r\n", "\n")
            .Split('\n');
}
