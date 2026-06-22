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
        EmitDeterminant(shape, mathBody);
        EmitFastInverse(shape, mathBody);

        var result = mathBody.ToString().TrimEnd();
        return string.IsNullOrEmpty(result)
            ? CodeFragment.Empty
            : new CodeFragment { Usings = ["System.Runtime.CompilerServices", "System.Numerics"], MathBody = result };
    }

    private static void EmitTranspose(TypeShape shape, StringBuilder body)
    {
        var resultType = shape.BaseType.ToTypeName(shape.Columns, shape.Rows);
        var colType = shape.BaseType.ToTypeName(shape.Columns, 1);

        body.AppendLine("        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]");
        body.AppendLine($"        public static {resultType} transpose(in {shape.TypeName} v)");
        body.AppendLine("        {");

        if (shape.BaseType == BaseType.Float && shape.IsSquareMatrix && shape.Rows == 4)
        {
            body.AppendLine($"            ref var m = ref global::System.Runtime.CompilerServices.Unsafe.As<{shape.TypeName}, global::System.Numerics.Matrix4x4>(ref global::System.Runtime.CompilerServices.Unsafe.AsRef(in v));");
            body.AppendLine( "            var t = global::System.Numerics.Matrix4x4.Transpose(m);");
            body.AppendLine($"            return global::System.Runtime.CompilerServices.Unsafe.As<global::System.Numerics.Matrix4x4, {shape.TypeName}>(ref t);");
            body.AppendLine("        }");
        }
        else if (shape.BaseType == BaseType.Float && shape.IsSquareMatrix && shape.Rows == 3)
        {
            EmitShuffleOrTranspose(shape, resultType, colType, body, "            ");
            body.AppendLine("        }");
        }
        else
        {
            EmitScalarGatherTranspose(shape, resultType, colType, body, "            ");
            body.AppendLine("        }");
        }
    }

    private static void EmitScalarGatherTranspose(TypeShape shape, string resultType, string colType, StringBuilder body, string indent)
    {
        var exprs = Enumerable.Range(0, shape.Rows).Select(row =>
        {
            var comps = Enumerable.Range(0, shape.Columns)
                .Select(col => $"v.{TypeShape.MatrixFields[col]}.{TypeShape.VectorFields[row]}");
            return $"new {colType}({string.Join(", ", comps)})";
        }).ToList();

        body.AppendLine($"{indent}return new {resultType}(");
        for (var i = 0; i < exprs.Count; i++)
            body.AppendLine($"{indent}    {exprs[i]}{(i < exprs.Count - 1 ? "," : "")}");
        body.AppendLine($"{indent});");
    }

    private static void EmitShuffleOrTranspose(TypeShape shape, string resultType, string colType, StringBuilder body, string indent)
    {
        const string Vector128 = "global::System.Runtime.Intrinsics.Vector128";
        var rows = shape.Rows;
        var fields = TypeShape.MatrixFields;

        string ShuffleOr(string sourceA, string maskA, string sourceB, string maskB) =>
            $"{Vector128}.Shuffle({sourceA}, {maskA}) | {Vector128}.Shuffle({sourceB}, {maskB})";

        for (var i = 0; i < rows; i++)
            body.AppendLine($"{indent}    var row{i} = v.{fields[i]}.data;");
        body.AppendLine($"{indent}    var row3 = {Vector128}<float>.Zero;");

        body.AppendLine($"{indent}    const int zero = 4;");
        body.AppendLine($"{indent}    var mask01a = {Vector128}.Create(0, 1, zero, zero);");
        body.AppendLine($"{indent}    var mask01b = {Vector128}.Create(zero, zero, 0, 1);");
        body.AppendLine($"{indent}    var mask23a = {Vector128}.Create(2, 3, zero, zero);");
        body.AppendLine($"{indent}    var mask23b = {Vector128}.Create(zero, zero, 2, 3);");
        body.AppendLine($"{indent}    var mask02a = {Vector128}.Create(0, 2, zero, zero);");
        body.AppendLine($"{indent}    var mask02b = {Vector128}.Create(zero, zero, 0, 2);");
        body.AppendLine($"{indent}    var mask13a = {Vector128}.Create(1, 3, zero, zero);");
        body.AppendLine($"{indent}    var mask13b = {Vector128}.Create(zero, zero, 1, 3);");
        body.AppendLine();

        body.AppendLine($"{indent}    var tmp0 = {ShuffleOr("row0", "mask01a", "row1", "mask01b")};");
        body.AppendLine($"{indent}    var tmp2 = {ShuffleOr("row0", "mask23a", "row1", "mask23b")};");
        body.AppendLine($"{indent}    var tmp1 = {ShuffleOr("row2", "mask01a", "row3", "mask01b")};");
        body.AppendLine($"{indent}    var tmp3 = {ShuffleOr("row2", "mask23a", "row3", "mask23b")};");
        body.AppendLine();

        body.AppendLine($"{indent}    return new {resultType}(");
        body.AppendLine($"{indent}        new {colType}({ShuffleOr("tmp0", "mask02a", "tmp1", "mask02b")}),");
        body.AppendLine($"{indent}        new {colType}({ShuffleOr("tmp0", "mask13a", "tmp1", "mask13b")}),");
        body.AppendLine($"{indent}        new {colType}({ShuffleOr("tmp2", "mask02a", "tmp3", "mask02b")})");
        body.AppendLine($"{indent}    );");
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
            body.AppendLine($"        public static {tn}2x2 inverse(in {tn}2x2 m)");
            body.AppendLine("        {");
            body.AppendLine("            var a = m.c0.x; var b = m.c0.y; var c = m.c1.x; var d = m.c1.y;");
            body.AppendLine("            var det = a * d - b * c;");
            body.AppendLine($"            return new {tn}2x2(d, -c, -b, a) * ({one} / det);");
            body.AppendLine("        }");
        }
        else if (shape.Rows == 3)
        {
            body.AppendLine($"        public static {tn}3x3 inverse(in {tn}3x3 m)");
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
            body.AppendLine($"        public static {tn}4x4 inverse(in {tn}4x4 m)");
            body.AppendLine("        {");
            body.AppendLine("            var r0 = m.c0; var r1 = m.c1; var r2 = m.c2; var r3 = m.c3;");
            body.AppendLine("            var r0x = r0.x; var r0y = r0.y; var r0z = r0.z; var r0w = r0.w;");
            body.AppendLine("            var r1x = r1.x; var r1y = r1.y; var r1z = r1.z; var r1w = r1.w;");
            body.AppendLine("            var r2x = r2.x; var r2y = r2.y; var r2z = r2.z; var r2w = r2.w;");
            body.AppendLine("            var r3x = r3.x; var r3y = r3.y; var r3z = r3.z; var r3w = r3.w;");
            body.AppendLine();
            body.AppendLine("            var xy23 = r2x * r3y - r2y * r3x; var xz23 = r2x * r3z - r2z * r3x; var xw23 = r2x * r3w - r2w * r3x;");
            body.AppendLine("            var yz23 = r2y * r3z - r2z * r3y; var yw23 = r2y * r3w - r2w * r3y; var zw23 = r2z * r3w - r2w * r3z;");
            body.AppendLine("            var xy13 = r1x * r3y - r1y * r3x; var xz13 = r1x * r3z - r1z * r3x; var xw13 = r1x * r3w - r1w * r3x;");
            body.AppendLine("            var yz13 = r1y * r3z - r1z * r3y; var yw13 = r1y * r3w - r1w * r3y; var zw13 = r1z * r3w - r1w * r3z;");
            body.AppendLine("            var xy12 = r1x * r2y - r1y * r2x; var xz12 = r1x * r2z - r1z * r2x; var xw12 = r1x * r2w - r1w * r2x;");
            body.AppendLine("            var yz12 = r1y * r2z - r1z * r2y; var yw12 = r1y * r2w - r1w * r2y; var zw12 = r1z * r2w - r1w * r2z;");
            body.AppendLine();
            body.AppendLine("            var m00 = r1y * zw23 - r1z * yw23 + r1w * yz23;");
            body.AppendLine("            var m10 = r0y * zw23 - r0z * yw23 + r0w * yz23;");
            body.AppendLine("            var m20 = r0y * zw13 - r0z * yw13 + r0w * yz13;");
            body.AppendLine("            var m30 = r0y * zw12 - r0z * yw12 + r0w * yz12;");
            body.AppendLine("            var m01 = r1x * zw23 - r1z * xw23 + r1w * xz23;");
            body.AppendLine("            var m11 = r0x * zw23 - r0z * xw23 + r0w * xz23;");
            body.AppendLine("            var m21 = r0x * zw13 - r0z * xw13 + r0w * xz13;");
            body.AppendLine("            var m31 = r0x * zw12 - r0z * xw12 + r0w * xz12;");
            body.AppendLine("            var m02 = r1x * yw23 - r1y * xw23 + r1w * xy23;");
            body.AppendLine("            var m12 = r0x * yw23 - r0y * xw23 + r0w * xy23;");
            body.AppendLine("            var m22 = r0x * yw13 - r0y * xw13 + r0w * xy13;");
            body.AppendLine("            var m32 = r0x * yw12 - r0y * xw12 + r0w * xy12;");
            body.AppendLine("            var m03 = r1x * yz23 - r1y * xz23 + r1z * xy23;");
            body.AppendLine("            var m13 = r0x * yz23 - r0y * xz23 + r0z * xy23;");
            body.AppendLine("            var m23 = r0x * yz13 - r0y * xz13 + r0z * xy13;");
            body.AppendLine("            var m33 = r0x * yz12 - r0y * xz12 + r0z * xy12;");
            body.AppendLine();
            body.AppendLine("            var det = r0x * m00 - r0y * m01 + r0z * m02 - r0w * m03;");
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

    private static void EmitDeterminant(TypeShape shape, StringBuilder body)
    {
        if (!shape.IsSquareMatrix || shape.Rows == 1) return;
        if (shape.BaseType is not BaseType.Int and not BaseType.Float and not BaseType.Double) return;

        var tn = shape.TypeName;
        var bn = shape.BaseTypeName;
        body.AppendLine();

        if (shape.Rows == 2)
        {
            body.AppendLine($"        public static {bn} determinant(in {tn} m)");
            body.AppendLine("        {");
            body.AppendLine("            var a = m.c0.x; var b = m.c1.x; var c = m.c0.y; var d = m.c1.y;");
            body.AppendLine("            return a * d - b * c;");
            body.AppendLine("        }");
        }
        else if (shape.Rows == 3)
        {
            body.AppendLine($"        public static {bn} determinant(in {tn} m)");
            body.AppendLine("        {");
            body.AppendLine("            var c0 = m.c0; var c1 = m.c1; var c2 = m.c2;");
            body.AppendLine("            var m00 = c1.y * c2.z - c1.z * c2.y;");
            body.AppendLine("            var m01 = c0.y * c2.z - c0.z * c2.y;");
            body.AppendLine("            var m02 = c0.y * c1.z - c0.z * c1.y;");
            body.AppendLine("            return c0.x * m00 - c1.x * m01 + c2.x * m02;");
            body.AppendLine("        }");
        }
        else if (shape.Rows == 4)
        {
            body.AppendLine($"        public static {bn} determinant(in {tn} m)");
            body.AppendLine("        {");
            body.AppendLine("            var c0 = m.c0; var c1 = m.c1; var c2 = m.c2; var c3 = m.c3;");
            body.AppendLine("            var m00 = c1.y * (c2.z * c3.w - c2.w * c3.z) - c2.y * (c1.z * c3.w - c1.w * c3.z) + c3.y * (c1.z * c2.w - c1.w * c2.z);");
            body.AppendLine("            var m01 = c0.y * (c2.z * c3.w - c2.w * c3.z) - c2.y * (c0.z * c3.w - c0.w * c3.z) + c3.y * (c0.z * c2.w - c0.w * c2.z);");
            body.AppendLine("            var m02 = c0.y * (c1.z * c3.w - c1.w * c3.z) - c1.y * (c0.z * c3.w - c0.w * c3.z) + c3.y * (c0.z * c1.w - c0.w * c1.z);");
            body.AppendLine("            var m03 = c0.y * (c1.z * c2.w - c1.w * c2.z) - c1.y * (c0.z * c2.w - c0.w * c2.z) + c2.y * (c0.z * c1.w - c0.w * c1.z);");
            body.AppendLine("            return c0.x * m00 - c1.x * m01 + c2.x * m02 - c3.x * m03;");
            body.AppendLine("        }");
        }
    }

    private static void EmitFastInverse(TypeShape shape, StringBuilder body)
    {
        if (shape.Columns != 4 || shape.Rows is not (3 or 4)) return;
        if (shape.BaseType is not BaseType.Float and not BaseType.Double) return;

        var tn = shape.TypeName;
        var colType = shape.BaseType.ToTypeName(shape.Rows, 1);
        var zero = shape.BaseType.ToTypedLiteral(0);
        var one = shape.BaseType.ToTypedLiteral(1);
        body.AppendLine();

        body.AppendLine($"        public static {tn} fastinverse(in {tn} m)");
        body.AppendLine("        {");
        body.AppendLine("            var c0 = m.c0; var c1 = m.c1; var c2 = m.c2; var pos = m.c3;");
        if (shape.Rows == 3)
        {
            body.AppendLine($"            var r0 = new {colType}(c0.x, c1.x, c2.x);");
            body.AppendLine($"            var r1 = new {colType}(c0.y, c1.y, c2.y);");
            body.AppendLine($"            var r2 = new {colType}(c0.z, c1.z, c2.z);");
            body.AppendLine("            pos = -(r0 * pos.x + r1 * pos.y + r2 * pos.z);");
        }
        else
        {
            body.AppendLine($"            var r0 = new {colType}(c0.x, c1.x, c2.x, {zero});");
            body.AppendLine($"            var r1 = new {colType}(c0.y, c1.y, c2.y, {zero});");
            body.AppendLine($"            var r2 = new {colType}(c0.z, c1.z, c2.z, {zero});");
            body.AppendLine("            pos = -(r0 * pos.x + r1 * pos.y + r2 * pos.z);");
            body.AppendLine($"            pos.w = {one};");
        }
        body.AppendLine($"            return new {tn}(r0, r1, r2, pos);");
        body.AppendLine("        }");
    }
}
