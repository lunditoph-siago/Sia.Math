namespace Sia.Math.CodeGenerators;

public readonly record struct CodeFragment
{
    public string[] Usings { get; init; }
    public string[] Inherits { get; init; }
    public string? TypeBody { get; init; }
    public string? MathBody { get; init; }

    public static CodeFragment Empty => new()
    {
        Usings = [],
        Inherits = [],
        TypeBody = null,
        MathBody = null
    };

    public bool IsEmpty => TypeBody is null && MathBody is null;

    public static CodeFragment operator +(CodeFragment a, CodeFragment b)
    {
        if (a.IsEmpty) return b;
        if (b.IsEmpty) return a;

        var aU = a.Usings ?? [];
        var bU = b.Usings ?? [];
        var aI = a.Inherits ?? [];
        var bI = b.Inherits ?? [];

        return new CodeFragment
        {
            Usings = [..aU, ..bU],
            Inherits = [..aI, ..bI],
            TypeBody = JoinBodies(a.TypeBody, b.TypeBody),
            MathBody = JoinBodies(a.MathBody, b.MathBody),
        };
    }

    private static string? JoinBodies(string? a, string? b) => (a, b) switch
    {
        (null, null) => null,
        (null, _) => b,
        (_, null) => a,
        _ => a + "\n\n" + b
    };
}
