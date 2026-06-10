using Sia.Math;
using static Sia.Math.Tests.TestValue;
using static Sia.Math.Tests.TestAssert;

namespace Sia.Math.Tests;

public class TestMathVector
{
    public static TheoryData<float, float, float, float> NormalizeFloat2Data => new()
    {
        { 3.1f, -5.3f, 0.504883f, -0.863188f },
        { 1.0f, 0.0f, 1.0f, 0.0f },
        { 0.0f, 1.0f, 0.0f, 1.0f },
    };

    [Theory]
    [MemberData(nameof(NormalizeFloat2Data))]
    public void Normalize_Float2(float x, float y, float ex, float ey)
    {
        var result = math.normalize(new float2(x, y));
        Approx(ex, result.x);
        Approx(ey, result.y);
    }

    [Fact]
    public void Normalize_Float2_Zero_ReturnsNaN()
    {
        var result = math.normalize(new float2(0.0f, 0.0f));
        Assert.True(math.all(math.isnan(result)));
    }

    public static TheoryData<float, float, float, float, float, float> NormalizeFloat3Data => new()
    {
        { 3.1f, -5.3f, 2.6f, 0.464916f, -0.794861f, 0.389932f },
        { 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f },
    };

    [Theory]
    [MemberData(nameof(NormalizeFloat3Data))]
    public void Normalize_Float3(float x, float y, float z, float ex, float ey, float ez)
    {
        var result = math.normalize(new float3(x, y, z));
        Approx(ex, result.x);
        Approx(ey, result.y);
        Approx(ez, result.z);
    }

    [Fact]
    public void Normalize_Float3_Zero_ReturnsNaN()
    {
        var result = math.normalize(new float3(0.0f, 0.0f, 0.0f));
        Assert.True(math.all(math.isnan(result)));
    }

    [Fact]
    public void Normalize_Float4()
    {
        var result = math.normalize(new float4(3.1f, -5.3f, 2.6f, 11.4f));
        Approx(0.234727f, result.x);
        Approx(-0.401308f, result.y);
        Approx(0.196868f, result.z);
        Approx(0.863191f, result.w);
    }

    [Fact]
    public void Normalize_Float4_Zero_ReturnsNaN()
    {
        var result = math.normalize(new float4(0.0f, 0.0f, 0.0f, 0.0f));
        Assert.True(math.all(math.isnan(result)));
    }

    [Theory]
    [MemberData(nameof(NormalizeFloat2Data))]
    public void Normalize_Double2(float xf, float yf, float exf, float eyf)
    {
        double x = xf, y = yf, ex = exf, ey = eyf;
        var result = math.normalize(new double2(x, y));
        Approx(ex, result.x, 1e-4);
        Approx(ey, result.y, 1e-4);
    }

    [Fact]
    public void Normalize_Double2_Zero_ReturnsNaN()
    {
        var result = math.normalize(new double2(0.0, 0.0));
        Assert.True(math.all(math.isnan(result)));
    }

    [Fact]
    public void Normalize_Double3()
    {
        var result = math.normalize(new double3(3.1, -5.3, 2.6));
        Approx(0.464916, result.x, 1e-4);
        Approx(-0.794861, result.y, 1e-4);
        Approx(0.389932, result.z, 1e-4);
    }

    [Fact]
    public void Normalize_Double4()
    {
        var result = math.normalize(new double4(3.1, -5.3, 2.6, 11.4));
        Approx(0.234727, result.x, 1e-4);
        Approx(-0.401308, result.y, 1e-4);
        Approx(0.196868, result.z, 1e-4);
        Approx(0.863191, result.w, 1e-4);
    }

    [Fact]
    public void NormalizeSafe_Float2()
    {
        var result = math.normalizesafe(new float2(3.1f, -5.3f));
        Approx(0.504883f, result.x);
        Approx(-0.863188f, result.y);
    }

    [Fact]
    public void NormalizeSafe_Float2_Zero_ReturnsZero()
    {
        var result = math.normalizesafe(new float2(0.0f, 0.0f));
        Assert.Equal(new float2(0.0f, 0.0f), result);
    }

    [Fact]
    public void NormalizeSafe_Float2_Zero_ReturnsDefault()
    {
        var result = math.normalizesafe(new float2(0.0f, 0.0f), new float2(1.0f, 2.0f));
        Assert.Equal(new float2(1.0f, 2.0f), result);
    }

    [Fact]
    public void NormalizeSafe_Float2_Tiny_ReturnsNormalized()
    {
        var result = math.normalizesafe(new float2(1e-18f, 2e-18f));
        Approx(0.447214f, result.x);
        Approx(0.894427f, result.y);
    }

    [Fact]
    public void NormalizeSafe_Float3_Zero_ReturnsDefault()
    {
        var zero = new float3(0f, 0f, 0f);
        var @default = new float3(1f, 0f, 0f);
        Assert.Equal(@default, math.normalizesafe(zero, @default));
    }

    [Fact]
    public void NormalizeSafe_Float3_NonZero_ReturnsNormalized()
    {
        var v = new float3(3f, 4f, 0f);
        var result = math.normalizesafe(v);
        Approx(1f, math.length(result));
    }

    [Fact]
    public void NormalizeSafe_Float3()
    {
        var result = math.normalizesafe(new float3(3.1f, -5.3f, 2.6f));
        Approx(0.464916f, result.x);
        Approx(-0.794861f, result.y);
        Approx(0.389932f, result.z);
    }

    [Fact]
    public void NormalizeSafe_Float4()
    {
        var result = math.normalizesafe(new float4(3.1f, -5.3f, 2.6f, 11.4f));
        Approx(0.234727f, result.x);
        Approx(-0.401308f, result.y);
        Approx(0.196868f, result.z);
        Approx(0.863191f, result.w);
    }

    [Fact]
    public void NormalizeSafe_Double2()
    {
        var result = math.normalizesafe(new double2(3.1, -5.3));
        Approx(0.504883, result.x, 1e-4);
        Approx(-0.863188, result.y, 1e-4);
    }

    [Fact]
    public void NormalizeSafe_Double2_Zero_ReturnsDefault()
    {
        var result = math.normalizesafe(new double2(0.0, 0.0), new double2(1.0, 2.0));
        Assert.Equal(new double2(1.0, 2.0), result);
    }

    [Fact]
    public void NormalizeSafe_Double3()
    {
        var result = math.normalizesafe(new double3(3.1, -5.3, 2.6));
        Approx(0.464916, result.x, 1e-4);
        Approx(-0.794861, result.y, 1e-4);
    }

    [Fact]
    public void NormalizeSafe_Double4()
    {
        var result = math.normalizesafe(new double4(3.1, -5.3, 2.6, 11.4));
        Approx(0.234727, result.x, 1e-4);
        Approx(-0.401308, result.y, 1e-4);
    }

    [Fact]
    public void NormalizeSafe_Double4_Zero_ReturnsDefault()
    {
        var result = math.normalizesafe(new double4(0.0, 0.0, 0.0, 0.0), new double4(1.0, 2.0, 3.0, 4.0));
        Assert.Equal(new double4(1.0, 2.0, 3.0, 4.0), result);
    }

    [Theory]
    [InlineData(3.0f, 3.0f)]
    [InlineData(-4.0f, 4.0f)]
    [InlineData(0.0f, 0.0f)]
    public void Length_Float(float x, float expected) => Assert.Equal(expected, math.length(x));

    [Theory]
    [InlineData(3.0f, 4.0f, 5.0f)]
    [InlineData(1.0f, 0.0f, 1.0f)]
    [InlineData(0.0f, 0.0f, 0.0f)]
    public void Length_Float2(float x, float y, float expected) =>
        Assert.Equal(expected, math.length(new float2(x, y)), 3);

    [Fact]
    public void Length_Float3()
    {
        var v = new float3(0.0f, 0.0f, 0.0f);
        Assert.Equal(0.0f, math.length(v));
    }

    [Fact]
    public void Length_Float4()
    {
        var v = new float4(1.0f, 2.0f, 3.0f, 4.0f);
        Approx(5.477226f, math.length(v));
    }

    [Theory]
    [InlineData(3.0, 3.0)]
    [InlineData(-4.0, 4.0)]
    public void Length_Double(double x, double expected) => Assert.Equal(expected, math.length(x));

    [Theory]
    [InlineData(3.0, 4.0, 5.0)]
    public void Length_Double2(double x, double y, double expected) =>
        Assert.Equal(expected, math.length(new double2(x, y)), 3);

    [Fact]
    public void Length_Double3()
    {
        var v = new double3(1.0, 2.0, 3.0);
        Approx(3.7416573867739413, math.length(v));
    }

    [Fact]
    public void Length_Double4()
    {
        var v = new double4(1.0, 2.0, 3.0, 4.0);
        Approx(5.477225575051661, math.length(v));
    }

    [Theory]
    [InlineData(3.0f, 9.0f)]
    [InlineData(-3.0f, 9.0f)]
    [InlineData(0.0f, 0.0f)]
    public void LengthSq_Float(float x, float expected) => Assert.Equal(expected, math.lengthsq(x));

    [Theory]
    [InlineData(3.0f, 4.0f, 25.0f)]
    [InlineData(0.0f, 0.0f, 0.0f)]
    public void LengthSq_Float2(float x, float y, float expected) =>
        Assert.Equal(expected, math.lengthsq(new float2(x, y)), 3);

    [Theory]
    [InlineData(3.0, 4.0, 25.0)]
    public void LengthSq_Double2(double x, double y, double expected) =>
        Assert.Equal(expected, math.lengthsq(new double2(x, y)), 3);

    [Theory]
    [InlineData(1, 2, 3, 4, 5, 6, 32)]
    [InlineData(0, 0, 0, 0, 0, 0, 0)]
    public void Dot_Int3(int ax, int ay, int az, int bx, int by, int bz, int expected) =>
        Assert.Equal(expected, math.dot(new int3(ax, ay, az), new int3(bx, by, bz)));

    [Theory]
    [InlineData(1.0f, 2.0f, 3.0f, 4.0f, 11.0f)]
    public void Dot_Float2(float ax, float ay, float bx, float by, float expected) =>
        Assert.Equal(expected, math.dot(new float2(ax, ay), new float2(bx, by)), 3);

    [Fact]
    public void Dot_Float3() =>
        Assert.Equal(32.0f, math.dot(new float3(1f, 2f, 3f), new float3(4f, 5f, 6f)), 3);

    [Fact]
    public void Dot_Float4() =>
        Assert.Equal(30.0f, math.dot(new float4(1f, 2f, 3f, 4f), new float4(1f, 2f, 3f, 4f)), 3);

    [Fact]
    public void Dot_Double4() =>
        Assert.Equal(70.0, math.dot(new double4(1, 2, 3, 4), new double4(5, 6, 7, 8)), 3);

    [Theory]
    [InlineData(1f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 1f)]
    [InlineData(2f, 0f, 0f, 0f, 3f, 0f, 0f, 0f, 6f)]
    public void Cross_Float3(
        float ax, float ay, float az, float bx, float by, float bz, float ex, float ey, float ez)
    {
        var cross = math.cross(new float3(ax, ay, az), new float3(bx, by, bz));
        Assert.Equal(ex, cross.x, 3);
        Assert.Equal(ey, cross.y, 3);
        Assert.Equal(ez, cross.z, 3);
    }

    [Fact]
    public void Cross_Double3()
    {
        var cross = math.cross(new double3(1.0, 0.0, 0.0), new double3(0.0, 1.0, 0.0));
        Assert.Equal(0.0, cross.x, 3);
        Assert.Equal(0.0, cross.y, 3);
        Assert.Equal(1.0, cross.z, 3);
    }

    [Theory]
    [InlineData(1, 2, 3, 6)]
    [InlineData(-1, 1, 0, 0)]
    [InlineData(0, 0, 0, 0)]
    [InlineData(100, 200, 300, 600)]
    public void CSum_Int3(int x, int y, int z, int expected) =>
        Assert.Equal(expected, math.csum(new int3(x, y, z)));

    [Theory]
    [InlineData(1.0f, 2.0f, 3.0f, 4.0f, 10.0f)]
    public void CSum_Float4(float x, float y, float z, float w, float expected) =>
        Assert.Equal(expected, math.csum(new float4(x, y, z, w)), 3);

    [Theory]
    [InlineData(1.0, 2.0, 3.0)]
    [InlineData(-1.0, 1.0, 0.0)]
    public void CSum_Double2(double x, double y, double expected) =>
        Assert.Equal(expected, math.csum(new double2(x, y)));

    [Fact]
    public void CSum_Int2() => Assert.Equal(7, math.csum(new int2(3, 4)));

    [Fact]
    public void CSum_Int4() => Assert.Equal(10, math.csum(new int4(1, 2, 3, 4)));

    [Fact]
    public void CSum_Float2() => Assert.Equal(5.0f, math.csum(new float2(2.0f, 3.0f)), 3);

    [Fact]
    public void CSum_Float3() => Assert.Equal(6.0f, math.csum(new float3(1.0f, 2.0f, 3.0f)), 3);

    [Fact]
    public void CSum_Double3() => Assert.Equal(6.0, math.csum(new double3(1.0, 2.0, 3.0)), 3);

    [Fact]
    public void CSum_Double4() => Assert.Equal(10.0, math.csum(new double4(1.0, 2.0, 3.0, 4.0)), 3);

    [Fact]
    public void CSum_UInt3() => Assert.Equal(6u, math.csum(new uint3(1u, 2u, 3u)));

    [Fact]
    public void CSum_UInt4() => Assert.Equal(10u, math.csum(new uint4(1u, 2u, 3u, 4u)));
}
