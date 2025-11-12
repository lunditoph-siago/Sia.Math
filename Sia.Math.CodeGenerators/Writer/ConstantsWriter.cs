using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace Sia.Math.CodeGenerators.Writer;

public class ConstantsWriter(VectorType type) : ITypeSourceWriter
{
    public HashSet<string> Imports { get; } = [];

    public HashSet<string> Inherits { get; } = [];

    public Action<IndentedTextWriter> TypeSourceWriter => source =>
    {
        if (type.Columns > 1)
        {
            source.WriteLine("/// <summary>A <see cref=\"{0}\" /> matrix with all cells set to zero.</summary>", type.TypeName);
            source.WriteLine("public static readonly {0} zero = default;", type.TypeName);

            if (type.Rows == type.Columns && type.BaseType is BaseType.Float or BaseType.Double)
            {
                var columnType = type.BaseType.ToTypeName(type.Rows, 1);
                var identityArgs = string.Join(", ", Enumerable.Range(0, type.Columns)
                    .Select(col => $"new {columnType}({string.Join(", ", Enumerable.Range(0, type.Rows)
                        .Select(row => row == col ? type.BaseType.ToTypedLiteral(1) : type.BaseType.ToTypedLiteral(0)))})"));
                source.WriteLine("/// <summary>The <see cref=\"{0}\" /> identity matrix.</summary>", type.TypeName);
                source.WriteLine("public static readonly {0} identity = new {0}({1});", type.TypeName, identityArgs);
            }
        }
        else
        {
            source.WriteLine("/// <summary>A <see cref=\"{0}\" /> vector with all components set to zero.</summary>", type.TypeName);
            source.WriteLine("public static readonly {0} zero = default;", type.TypeName);

            if (type.BaseType is BaseType.Float or BaseType.Double or BaseType.Int or BaseType.UInt)
            {
                var oneArgs = string.Join(", ", Enumerable.Repeat(type.BaseType.ToTypedLiteral(1), type.Rows));
                source.WriteLine("/// <summary>A <see cref=\"{0}\" /> vector with all components set to one.</summary>", type.TypeName);
                source.WriteLine("public static readonly {0} one = new {0}({1});", type.TypeName, oneArgs);
            }
        }
    };
}
