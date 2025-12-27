using System;
using System.Runtime.CompilerServices;

namespace Sia.Math.CodeGenerators;

public enum BaseType
{
    Bool,
    Int,
    UInt,
    Float,
    Double,
}

[Flags]
public enum Features
{
    Arithmetic = 1 << 0,
    Shifts = 1 << 1,
    BitwiseLogic = 1 << 2,
    BitwiseComplement = 1 << 3,
    UnaryNegation = 1 << 4,
    All = Arithmetic | Shifts | BitwiseLogic | BitwiseComplement | UnaryNegation
}

public readonly record struct TypeShape(BaseType BaseType, int Rows, int Columns, Features Operations)
{
    public bool IsScalar => Rows == 1 && Columns == 1;
    public bool IsVector => Columns == 1 && Rows > 1;
    public bool IsMatrix => Columns > 1;
    public bool IsSquareMatrix => Rows == Columns && IsMatrix;
    public bool IsSimdEligible => BaseType is not BaseType.Bool && IsVector && Rows >= 2 && Rows <= 4;

    public string BaseTypeName => BaseType.ToBaseTypeName();
    public string TypeName => BaseType.ToTypeName(Rows, Columns);
    public string FilePrefix => TypeName;

    public static readonly string[] Components = ["x", "y", "z", "w"];
    public static readonly string[] VectorFields = ["x", "y", "z", "w"];
    public static readonly string[] MatrixFields = ["c0", "c1", "c2", "c3"];
    public static readonly string[] ShuffleComponents =
    [
        "LeftX", "LeftY", "LeftZ", "LeftW",
        "RightX", "RightY", "RightZ", "RightW"
    ];
}

public static class BaseTypeHelper
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToBaseTypeName(this BaseType baseType) => baseType switch
    {
        BaseType.Bool => "bool",
        BaseType.Int => "int",
        BaseType.UInt => "uint",
        BaseType.Float => "float",
        BaseType.Double => "double",
        _ => throw new ArgumentOutOfRangeException(nameof(baseType), baseType, null)
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToTypeName(this BaseType baseType, int rows, int columns)
    {
        var name = baseType.ToBaseTypeName();
        if (rows == 1 && columns == 1) return name;
        if (rows == 1 && columns > 1) return name + columns;
        if (columns == 1) return name + rows;
        return $"{name}{rows}x{columns}";
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToTypeDescription(this BaseType baseType, int rows, int columns, int num, bool addPrefix = false)
    {
        var name = $"<see cref=\"{baseType.ToTypeName(rows, columns)}\" />";
        var numStr = num.FormatQuantifiers(baseType is BaseType.Int);
        var vectorPrefix = addPrefix
            ? rows == 1 ? " row" : columns == 1 ? " column" : string.Empty
            : string.Empty;

        return num > 1
            ? numStr + name + (rows == 1 && columns == 1 ? " values" :
                rows > 1 && columns > 1 ? " matrices" : $"{vectorPrefix} vectors")
            : numStr + name + (rows == 1 && columns == 1 ? " value" :
                rows > 1 && columns > 1 ? " matrix" : $"{vectorPrefix} vector");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToTypedLiteral(this BaseType baseType, int value) => baseType switch
    {
        BaseType.Int => $"{value}",
        BaseType.UInt => $"{value}u",
        BaseType.Float => $"{value}.0f",
        BaseType.Double => $"{value}.0",
        _ => string.Empty
    };
}

public static class NumberHelper
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string FormatOrdinals(this int number) => number switch
    {
        1 => "first",
        2 => "second",
        3 => "third",
        4 => "fourth",
        _ => string.Empty
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string FormatQuantifiers(this int number, bool isVowel) => number switch
    {
        1 => isVowel ? "an " : "a ",
        2 => "two ",
        3 => "three ",
        4 => "four ",
        _ => string.Empty
    };
}
