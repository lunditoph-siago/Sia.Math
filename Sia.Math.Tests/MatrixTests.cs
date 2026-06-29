using Sia.Math;

namespace Sia.Math.Tests;

public class MatrixTests
{
    private static readonly TestRng Rng = new(271828);

    private static float Det2(float2x2 m) => m[0][0] * m[1][1] - m[0][1] * m[1][0];

    private static float Det3(float3x3 m) =>
        m[0][0] * (m[1][1] * m[2][2] - m[1][2] * m[2][1]) -
        m[0][1] * (m[1][0] * m[2][2] - m[1][2] * m[2][0]) +
        m[0][2] * (m[1][0] * m[2][1] - m[1][1] * m[2][0]);

    private static float Det3Raw(
        float a, float b, float c,
        float d, float e, float f,
        float g, float h, float i) =>
        a * (e * i - f * h) - b * (d * i - f * g) + c * (d * h - e * g);

    private static float Det4(float4x4 m)
    {
        var sign = 1f;
        var det = 0f;
        for (var col = 0; col < 4; col++)
        {
            var rows = new float[3][];
            var r = 0;
            for (var row = 1; row < 4; row++)
            {
                var vals = new float[3];
                var c = 0;
                for (var k = 0; k < 4; k++)
                {
                    if (k == col) continue;
                    vals[c++] = m[row][k];
                }
                rows[r++] = vals;
            }
            det += sign * m[0][col] * Det3Raw(
                rows[0][0], rows[0][1], rows[0][2],
                rows[1][0], rows[1][1], rows[1][2],
                rows[2][0], rows[2][1], rows[2][2]);
            sign = -sign;
        }
        return det;
    }

    private static float2x2 RandomInvertible2x2()
    {
        float2x2 m;
        do { m = new float2x2(Rng.Float(-2f, 2f), Rng.Float(-2f, 2f), Rng.Float(-2f, 2f), Rng.Float(-2f, 2f)); }
        while (System.Math.Abs(Det2(m)) < 0.1f);
        return m;
    }

    private static float3x3 RandomInvertible3x3()
    {
        float3x3 m;
        do
        {
            m = new float3x3(
                Rng.Float(-2f, 2f), Rng.Float(-2f, 2f), Rng.Float(-2f, 2f),
                Rng.Float(-2f, 2f), Rng.Float(-2f, 2f), Rng.Float(-2f, 2f),
                Rng.Float(-2f, 2f), Rng.Float(-2f, 2f), Rng.Float(-2f, 2f));
        }
        while (System.Math.Abs(Det3(m)) < 0.1f);
        return m;
    }

    private static float4x4 RandomInvertible4x4()
    {
        float4x4 m;
        do
        {
            m = new float4x4(
                Rng.Float(-2f, 2f), Rng.Float(-2f, 2f), Rng.Float(-2f, 2f), Rng.Float(-2f, 2f),
                Rng.Float(-2f, 2f), Rng.Float(-2f, 2f), Rng.Float(-2f, 2f), Rng.Float(-2f, 2f),
                Rng.Float(-2f, 2f), Rng.Float(-2f, 2f), Rng.Float(-2f, 2f), Rng.Float(-2f, 2f),
                Rng.Float(-2f, 2f), Rng.Float(-2f, 2f), Rng.Float(-2f, 2f), Rng.Float(-2f, 2f));
        }
        while (System.Math.Abs(Det4(m)) < 0.1f);
        return m;
    }

    private static void AssertApproxIdentity2(float2x2 m)
    {
        Assert.True(System.Math.Abs(m.c0.x - 1f) < 1e-2f, m.ToString());
        Assert.True(System.Math.Abs(m.c1.y - 1f) < 1e-2f, m.ToString());
        Assert.True(System.Math.Abs(m.c0.y) < 1e-2f, m.ToString());
        Assert.True(System.Math.Abs(m.c1.x) < 1e-2f, m.ToString());
    }

    private static void AssertApproxIdentity3(float3x3 m)
    {
        for (var i = 0; i < 3; i++)
        for (var j = 0; j < 3; j++)
        {
            var expected = i == j ? 1f : 0f;
            Assert.True(System.Math.Abs(m[i][j] - expected) < 1e-2f, $"[{i},{j}] = {m[i][j]}, expected {expected}");
        }
    }

    private static void AssertApproxIdentity4(float4x4 m)
    {
        for (var i = 0; i < 4; i++)
        for (var j = 0; j < 4; j++)
        {
            var expected = i == j ? 1f : 0f;
            Assert.True(System.Math.Abs(m[i][j] - expected) < 1e-2f, $"[{i},{j}] = {m[i][j]}, expected {expected}");
        }
    }

    [Fact]
    public void Inverse2x2_TimesOriginal_IsIdentity()
    {
        for (var i = 0; i < 64; i++)
        {
            var m = RandomInvertible2x2();
            AssertApproxIdentity2(math.mul(math.inverse(m), m));
            AssertApproxIdentity2(math.mul(m, math.inverse(m)));
        }
    }

    [Fact]
    public void Inverse3x3_TimesOriginal_IsIdentity()
    {
        for (var i = 0; i < 64; i++)
        {
            var m = RandomInvertible3x3();
            AssertApproxIdentity3(math.mul(math.inverse(m), m));
            AssertApproxIdentity3(math.mul(m, math.inverse(m)));
        }
    }

    [Fact]
    public void Inverse4x4_TimesOriginal_IsIdentity()
    {
        for (var i = 0; i < 64; i++)
        {
            var m = RandomInvertible4x4();
            AssertApproxIdentity4(math.mul(math.inverse(m), m));
            AssertApproxIdentity4(math.mul(m, math.inverse(m)));
        }
    }

    [Fact]
    public void Mul_MatchesNaiveRowColumnFormula_4x4()
    {
        for (var i = 0; i < 32; i++)
        {
            var a = RandomInvertible4x4();
            var b = RandomInvertible4x4();
            var result = math.mul(a, b);

            for (var row = 0; row < 4; row++)
            for (var col = 0; col < 4; col++)
            {
                var expected = 0f;
                for (var k = 0; k < 4; k++)
                    expected += a[k][row] * b[col][k];

                Assert.True(System.Math.Abs(result[col][row] - expected) < 1e-2f,
                    $"[{row},{col}]: expected {expected}, got {result[col][row]}");
            }
        }
    }

    [Fact]
    public void Mul_VectorTimesMatrix_MatchesRowVectorConvention()
    {
        for (var i = 0; i < 32; i++)
        {
            var v = new float4(Rng.Float(-2f, 2f), Rng.Float(-2f, 2f), Rng.Float(-2f, 2f), Rng.Float(-2f, 2f));
            var m = RandomInvertible4x4();
            var result = math.mul(v, m);

            for (var col = 0; col < 4; col++)
            {
                var expected = 0f;
                for (var k = 0; k < 4; k++)
                    expected += v[k] * m[col][k];

                Assert.True(System.Math.Abs(result[col] - expected) < 1e-2f,
                    $"[{col}]: expected {expected}, got {result[col]}");
            }
        }
    }

    [Fact]
    public void Mul_MatrixTimesVector_IsDotProductOfRows()
    {
        for (var i = 0; i < 32; i++)
        {
            var m = RandomInvertible4x4();
            var v = new float4(Rng.Float(-2f, 2f), Rng.Float(-2f, 2f), Rng.Float(-2f, 2f), Rng.Float(-2f, 2f));
            var result = math.mul(m, v);

            for (var row = 0; row < 4; row++)
            {
                var expected = 0f;
                for (var k = 0; k < 4; k++)
                    expected += m[k][row] * v[k];

                Assert.True(System.Math.Abs(result[row] - expected) < 1e-2f,
                    $"[{row}]: expected {expected}, got {result[row]}");
            }
        }
    }

    [Fact]
    public void Transpose_Twice_IsOriginal()
    {
        for (var i = 0; i < 32; i++)
        {
            var m = RandomInvertible4x4();
            var t = math.transpose(math.transpose(m));

            for (var row = 0; row < 4; row++)
            for (var col = 0; col < 4; col++)
                Assert.Equal(m[row][col], t[row][col], 4);
        }
    }

    [Fact]
    public void Transpose_SwapsRowsAndColumns()
    {
        for (var i = 0; i < 32; i++)
        {
            var m = RandomInvertible4x4();
            var t = math.transpose(m);

            for (var row = 0; row < 4; row++)
            for (var col = 0; col < 4; col++)
                Assert.Equal(m[row][col], t[col][row], 4);
        }
    }
}
