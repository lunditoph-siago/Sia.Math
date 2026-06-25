namespace Sia.Math.CodeGenerators.Simd;

using System;
using System.Collections.Generic;

public enum SimdBackend
{
    X86,
    Arm,
    Portable,
    Scalar,
}

public readonly record struct SimdBranch(
    SimdBackend Backend,
    string? Guard,
    string[] Statements);

public sealed class SimdDispatch
{
    private readonly SimdTypeProfile _profile;
    private readonly string[] _portable;
    private readonly string[] _scalar;
    private SimdBranch? _x86;
    private SimdBranch? _arm;

    public SimdDispatch(
        SimdTypeProfile profile,
        string[] portableStatements,
        string[] scalarStatements)
    {
        if (!profile.IsSimd) {
            throw new ArgumentException(
                "SIMD dispatch requires a SIMD-capable profile.",
                nameof(profile));
        }

        _profile = profile;
        _portable = portableStatements;
        _scalar = scalarStatements;
    }

    public SimdDispatch WithX86(
        string guard,
        params string[] statements)
    {
        _x86 = new SimdBranch(
            SimdBackend.X86,
            guard,
            statements);
        return this;
    }

    public SimdDispatch WithArm(
        string guard,
        params string[] statements)
    {
        _arm = new SimdBranch(
            SimdBackend.Arm,
            guard,
            statements);
        return this;
    }

    public IReadOnlyList<SimdBranch> Build()
    {
        var branches = new List<SimdBranch>(4);

        if (_x86 is { } x86) {
            branches.Add(x86);
        }

        if (_arm is { } arm) {
            branches.Add(arm);
        }

        branches.Add(new SimdBranch(
            SimdBackend.Portable,
            _profile.PortableGuard,
            _portable));
        branches.Add(new SimdBranch(
            SimdBackend.Scalar,
            null,
            _scalar));
        return branches;
    }

    public void Generate(SourceBuilder source)
    {
        foreach (var branch in Build()) {
            if (branch.Guard is null) {
                source.Lines(branch.Statements);
                continue;
            }

            source.Block(
                $"if ({branch.Guard})",
                body => body.Lines(branch.Statements));
        }
    }

    public static string[] Return(string expression)
        => [$"return {expression};"];
}
