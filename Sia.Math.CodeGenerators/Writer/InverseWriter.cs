using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Sia.Math.CodeGenerators.Writer;

public class InverseWriter(VectorType type) : IMathSourceWriter
{
    public HashSet<string> Imports { get; } = ["System.Runtime.CompilerServices"];
    public HashSet<string> Inherits { get; } = [];

    public Action<IndentedTextWriter> MathSourceWriter => source =>
    {
        if (type.Rows != type.Columns || type.Rows == 1) return;
        if (type.BaseType is not BaseType.Float and not BaseType.Double) return;

        var one = type.BaseType.ToTypedLiteral(1);
        var typeName = type.BaseTypeName;

        if (type.Rows == 2)
        {
            source.WriteLine("/// <summary>Returns the {0}2x2 full inverse of a {0}2x2 matrix.</summary>", typeName);
            source.WriteLine("/// <param name=\"m\">Matrix to invert.</param>");
            source.WriteLine("/// <returns>The inverted matrix.</returns>");
            source.WriteLine("public static {0}2x2 inverse({0}2x2 m)", typeName);
            source.WriteLine("{");
            source.Indent++;
            {
                source.WriteLine("var a = m.c0.x;");
                source.WriteLine("var b = m.c0.y;");
                source.WriteLine("var c = m.c1.x;");
                source.WriteLine("var d = m.c1.y;");
                source.WriteLineNoTabs(string.Empty);
                source.WriteLine("var det = a * d - b * c;");
                source.WriteLineNoTabs(string.Empty);
                source.WriteLine("return new {0}2x2(d, -b, -c, a) * ({1} / det);", typeName, one);
            }
            source.Indent--;
            source.WriteLine("}");
        }
        else if (type.Rows == 3)
        {
            source.WriteLine("/// <summary>Returns the {0}3x3 full inverse of a {0}3x3 matrix.</summary>", typeName);
            source.WriteLine("/// <param name=\"m\">Matrix to invert.</param>");
            source.WriteLine("/// <returns>The inverted matrix.</returns>");
            source.WriteLine("public static {0}3x3 inverse({0}3x3 m)", typeName);
            source.WriteLine("{");
            source.Indent++;
            {
                source.WriteLine("var r0 = m.c0;");
                source.WriteLine("var r1 = m.c1;");
                source.WriteLine("var r2 = m.c2;");
                source.WriteLineNoTabs(string.Empty);
                source.WriteLine("// inverse(m)'s rows are cross products of m's columns (inverse(m)^T = inverse(m^T),");
                source.WriteLine("// and inverse(m^T)'s columns - the standard cross-product cofactor identity - are");
                source.WriteLine("// exactly m's rows crossed the same way, transposed once more back to rows here).");
                source.WriteLine("var col0 = new {0}3(r0.x, r1.x, r2.x);", typeName);
                source.WriteLine("var col1 = new {0}3(r0.y, r1.y, r2.y);", typeName);
                source.WriteLine("var col2 = new {0}3(r0.z, r1.z, r2.z);", typeName);
                source.WriteLineNoTabs(string.Empty);
                source.WriteLine("var row0 = cross(col1, col2);");
                source.WriteLine("var row1 = cross(col2, col0);");
                source.WriteLine("var row2 = cross(col0, col1);");
                source.WriteLineNoTabs(string.Empty);
                source.WriteLine("var rcpDet = {0} / dot(col0, row0);", one);
                source.WriteLine("return new {0}3x3(row0, row1, row2) * rcpDet;", typeName);
            }
            source.Indent--;
            source.WriteLine("}");
        }
        else if (type.Rows == 4)
        {
            source.WriteLine("/// <summary>Returns the {0}4x4 full inverse of a {0}4x4 matrix.</summary>", typeName);
            source.WriteLine("/// <param name=\"m\">Matrix to invert.</param>");
            source.WriteLine("/// <returns>The inverted matrix.</returns>");
            source.WriteLine("public static {0}4x4 inverse({0}4x4 m)", typeName);
            source.WriteLine("{");
            source.Indent++;
            {
                source.WriteLine("var r0 = m.c0;");
                source.WriteLine("var r1 = m.c1;");
                source.WriteLine("var r2 = m.c2;");
                source.WriteLine("var r3 = m.c3;");
                source.WriteLineNoTabs(string.Empty);
                source.WriteLine("// Cofactor expansion (Laplace) via 3x3 minors - robust for any invertible matrix");
                source.WriteLine("// (unlike a block/Schur-complement inverse, no sub-block needs to be invertible).");
                source.WriteLine("// det3(a,b,c) is the determinant of the matrix with rows a,b,c (scalar triple product).");
                source.WriteLine("");
                source.WriteLine("// minors excluding column 0 (drop the x component of each remaining row)");
                source.WriteLine("var m00 = dot(r1.yzw, cross(r2.yzw, r3.yzw));");
                source.WriteLine("var m10 = dot(r0.yzw, cross(r2.yzw, r3.yzw));");
                source.WriteLine("var m20 = dot(r0.yzw, cross(r1.yzw, r3.yzw));");
                source.WriteLine("var m30 = dot(r0.yzw, cross(r1.yzw, r2.yzw));");
                source.WriteLineNoTabs(string.Empty);
                source.WriteLine("// minors excluding column 1 (drop y)");
                source.WriteLine("var m01 = dot(r1.xzw, cross(r2.xzw, r3.xzw));");
                source.WriteLine("var m11 = dot(r0.xzw, cross(r2.xzw, r3.xzw));");
                source.WriteLine("var m21 = dot(r0.xzw, cross(r1.xzw, r3.xzw));");
                source.WriteLine("var m31 = dot(r0.xzw, cross(r1.xzw, r2.xzw));");
                source.WriteLineNoTabs(string.Empty);
                source.WriteLine("// minors excluding column 2 (drop z)");
                source.WriteLine("var m02 = dot(r1.xyw, cross(r2.xyw, r3.xyw));");
                source.WriteLine("var m12 = dot(r0.xyw, cross(r2.xyw, r3.xyw));");
                source.WriteLine("var m22 = dot(r0.xyw, cross(r1.xyw, r3.xyw));");
                source.WriteLine("var m32 = dot(r0.xyw, cross(r1.xyw, r2.xyw));");
                source.WriteLineNoTabs(string.Empty);
                source.WriteLine("// minors excluding column 3 (drop w)");
                source.WriteLine("var m03 = dot(r1.xyz, cross(r2.xyz, r3.xyz));");
                source.WriteLine("var m13 = dot(r0.xyz, cross(r2.xyz, r3.xyz));");
                source.WriteLine("var m23 = dot(r0.xyz, cross(r1.xyz, r3.xyz));");
                source.WriteLine("var m33 = dot(r0.xyz, cross(r1.xyz, r2.xyz));");
                source.WriteLineNoTabs(string.Empty);
                source.WriteLine("// expansion along row 0 using the column-0-excluded minors (each mX0 already excludes row X)");
                source.WriteLine("var det = r0.x * m00 - r0.y * m01 + r0.z * m02 - r0.w * m03;");
                source.WriteLine("var rcpDet = {0} / det;", one);
                source.WriteLineNoTabs(string.Empty);
                source.WriteLine("return new {0}4x4(", typeName);
                source.Indent++;
                source.WriteLine("new {0}4(m00, -m10, m20, -m30) * rcpDet,", typeName);
                source.WriteLine("new {0}4(-m01, m11, -m21, m31) * rcpDet,", typeName);
                source.WriteLine("new {0}4(m02, -m12, m22, -m32) * rcpDet,", typeName);
                source.WriteLine("new {0}4(-m03, m13, -m23, m33) * rcpDet", typeName);
                source.Indent--;
                source.WriteLine(");");
            }
            source.Indent--;
            source.WriteLine("}");
        }
    };
}