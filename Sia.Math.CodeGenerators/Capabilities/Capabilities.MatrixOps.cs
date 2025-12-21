using System.Linq;
using System.Text;

namespace Sia.Math.CodeGenerators.Capabilities;

public static class MatrixOps
{
    public static CodeFragment Generate(TypeShape shape)
    {
        if (!shape.IsMatrix) return CodeFragment.Empty;

        var mathBody = new StringBuilder();
        EmitTranspose(shape, mathBody);
        EmitInverse(shape, mathBody);

        var result = mathBody.ToString().TrimEnd();
        return string.IsNullOrEmpty(result)
            ? CodeFragment.Empty
            : new CodeFragment { Usings = ["System.Runtime.CompilerServices"], MathBody = result };
    }

    private static void EmitTranspose(TypeShape shape, StringBuilder body)
    {
        var resultType = shape.BaseType.ToTypeName(shape.Columns, shape.Rows);
        var colType = shape.BaseType.ToTypeName(shape.Columns, 1);
        var exprs = Enumerable.Range(0, shape.Rows).Select(row =>
        {
            var comps = Enumerable.Range(0, shape.Columns)
                .Select(col => $"v.{TypeShape.MatrixFields[col]}.{TypeShape.VectorFields[row]}");
            return $"new {colType}({string.Join(", ", comps)})";
        }).ToList();

        body.AppendLine("        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]");
        body.AppendLine($"        public static {resultType} transpose({shape.TypeName} v)");
        body.AppendLine("        {");
        body.AppendLine($"            return new {resultType}(");
        for (var i = 0; i < exprs.Count; i++)
            body.AppendLine($"                {exprs[i]}{(i < exprs.Count - 1 ? "," : "")}");
        body.AppendLine("            );");
        body.AppendLine("        }");
    }

    private static void EmitInverse(TypeShape shape, StringBuilder body)
    {
        if (!shape.IsSquareMatrix || shape.Rows == 1) return;
        if (shape.BaseType is not BaseType.Float and not BaseType.Double) return;

        var tn = shape.BaseTypeName;
        var one = shape.BaseType.ToTypedLiteral(1);
        body.AppendLine();

        if (shape.Rows == 2)
        {
            body.AppendLine($"        public static {tn}2x2 inverse({tn}2x2 m)");
            body.AppendLine("        {");
            body.AppendLine("            var a = m.c0.x; var b = m.c0.y; var c = m.c1.x; var d = m.c1.y;");
            body.AppendLine("            var det = a * d - b * c;");
            body.AppendLine($"            return new {tn}2x2(d, -c, -b, a) * ({one} / det);");
            body.AppendLine("        }");
        }
        else if (shape.Rows == 3)
        {
            body.AppendLine($"        public static {tn}3x3 inverse({tn}3x3 m)");
            body.AppendLine("        {");
            body.AppendLine("            var r0 = m.c0; var r1 = m.c1; var r2 = m.c2;");
            body.AppendLine($"            var col0 = new {tn}3(r0.x, r1.x, r2.x);");
            body.AppendLine($"            var col1 = new {tn}3(r0.y, r1.y, r2.y);");
            body.AppendLine($"            var col2 = new {tn}3(r0.z, r1.z, r2.z);");
            body.AppendLine("            var row0 = cross(col1, col2);");
            body.AppendLine("            var row1 = cross(col2, col0);");
            body.AppendLine("            var row2 = cross(col0, col1);");
            body.AppendLine($"            var rcpDet = {one} / dot(col0, row0);");
            body.AppendLine($"            return new {tn}3x3(row0, row1, row2) * rcpDet;");
            body.AppendLine("        }");
        }
        else if (shape.Rows == 4)
        {
            body.AppendLine($"        public static {tn}4x4 inverse({tn}4x4 m)");
            body.AppendLine("        {");
            body.AppendLine("            var r0 = m.c0; var r1 = m.c1; var r2 = m.c2; var r3 = m.c3;");
            body.AppendLine("            var m00 = dot(r1.yzw, cross(r2.yzw, r3.yzw));");
            body.AppendLine("            var m10 = dot(r0.yzw, cross(r2.yzw, r3.yzw));");
            body.AppendLine("            var m20 = dot(r0.yzw, cross(r1.yzw, r3.yzw));");
            body.AppendLine("            var m30 = dot(r0.yzw, cross(r1.yzw, r2.yzw));");
            body.AppendLine("            var m01 = dot(r1.xzw, cross(r2.xzw, r3.xzw));");
            body.AppendLine("            var m11 = dot(r0.xzw, cross(r2.xzw, r3.xzw));");
            body.AppendLine("            var m21 = dot(r0.xzw, cross(r1.xzw, r3.xzw));");
            body.AppendLine("            var m31 = dot(r0.xzw, cross(r1.xzw, r2.xzw));");
            body.AppendLine("            var m02 = dot(r1.xyw, cross(r2.xyw, r3.xyw));");
            body.AppendLine("            var m12 = dot(r0.xyw, cross(r2.xyw, r3.xyw));");
            body.AppendLine("            var m22 = dot(r0.xyw, cross(r1.xyw, r3.xyw));");
            body.AppendLine("            var m32 = dot(r0.xyw, cross(r1.xyw, r2.xyw));");
            body.AppendLine("            var m03 = dot(r1.xyz, cross(r2.xyz, r3.xyz));");
            body.AppendLine("            var m13 = dot(r0.xyz, cross(r2.xyz, r3.xyz));");
            body.AppendLine("            var m23 = dot(r0.xyz, cross(r1.xyz, r3.xyz));");
            body.AppendLine("            var m33 = dot(r0.xyz, cross(r1.xyz, r2.xyz));");
            body.AppendLine("            var det = r0.x * m00 - r0.y * m01 + r0.z * m02 - r0.w * m03;");
            body.AppendLine($"            var rcpDet = {one} / det;");
            body.AppendLine($"            return new {tn}4x4(");
            body.AppendLine($"                new {tn}4(m00, -m10, m20, -m30) * rcpDet,");
            body.AppendLine($"                new {tn}4(-m01, m11, -m21, m31) * rcpDet,");
            body.AppendLine($"                new {tn}4(m02, -m12, m22, -m32) * rcpDet,");
            body.AppendLine($"                new {tn}4(-m03, m13, -m23, m33) * rcpDet");
            body.AppendLine("            );");
            body.AppendLine("        }");
        }
    }
}
