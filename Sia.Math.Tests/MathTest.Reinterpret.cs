using Sia.Math;
using static Sia.Math.Tests.TestValue;

namespace Sia.Math.Tests;

public class TestMathReinterpret
{
    [Theory]
    [InlineData(0u, 0)]
    [InlineData(0x12345678u, 0x12345678)]
    [InlineData(0x7FFFFFFFu, 0x7FFFFFFF)]
    [InlineData(0x80000000u, -2147483648)]
    [InlineData(0x87654321u, -2023406815)]
    [InlineData(0xFFFFFFFFu, -1)]
    public void AsInt_FromUInt(uint x, int expected) => Assert.Equal(expected, math.asint(x));

    [Fact]
    public void AsInt_FromUInt2()
    {
        var r = math.asint(new uint2(0u, 0x12345678u));
        Assert.Equal(0, r.x);
        Assert.Equal(0x12345678, r.y);
    }

    [Fact]
    public void AsInt_FromUInt3()
    {
        var r = math.asint(new uint3(0u, 0x7FFFFFFFu, 0x80000000u));
        Assert.Equal(0, r.x);
        Assert.Equal(0x7FFFFFFF, r.y);
        Assert.Equal(-2147483648, r.z);
    }

    [Fact]
    public void AsInt_FromUInt4()
    {
        var r = math.asint(new uint4(0x87654321u, 0xFFFFFFFFu, 0u, 0u));
        Assert.Equal(-2023406815, r.x);
        Assert.Equal(-1, r.y);
    }

    [Theory]
    [InlineData(0.0f, 0)]
    [InlineData(1.0f, 0x3F800000)]
    [InlineData(1234.56f, 0x449A51EC)]
    public void AsInt_FromFloat(float x, int expected) => Assert.Equal(expected, math.asint(x));

    [Fact]
    public void AsInt_FromFloat2()
    {
        var r = math.asint(new float2(0.0f, 1.0f));
        Assert.Equal(0, r.x);
        Assert.Equal(0x3F800000, r.y);
    }

    [Fact]
    public void AsInt_FromFloat3()
    {
        var r = math.asint(new float3(1234.56f, float.PositiveInfinity, -1.0f));
        Assert.Equal(0x449A51EC, r.x);
        Assert.Equal(0x7F800000, r.y);
        Assert.Equal(unchecked((int)0xBF800000), r.z);
    }

    [Fact]
    public void AsInt_FromFloat4()
    {
        var r = math.asint(new float4(0.0f, 1.0f, 1234.56f, float.PositiveInfinity));
        Assert.Equal(0, r.x);
        Assert.Equal(0x3F800000, r.y);
        Assert.Equal(0x449A51EC, r.z);
        Assert.Equal(0x7F800000, r.w);
    }

    [Fact]
    public void AsInt_FromPositiveInfinity() => Assert.Equal(0x7F800000, math.asint(float.PositiveInfinity));

    [Fact]
    public void AsInt_FromNegativeInfinity() => Assert.Equal(unchecked((int)0xFF800000), math.asint(float.NegativeInfinity));

    [Fact]
    public void AsInt_FromSignedFloatQNaN() => Assert.Equal(unchecked((int)0xFFC00000), math.asint(SignedFloatQNaN()));

    [Fact]
    public void AsInt_FromUnsignedFloatQNaN() => Assert.Equal(unchecked((int)0x7FC00000), math.asint(UnsignedFloatQNaN()));

    [Theory]
    [InlineData(0, 0u)]
    [InlineData(0x12345678, 0x12345678u)]
    [InlineData(0x7FFFFFFF, 0x7FFFFFFFu)]
    [InlineData(-2147483648, 0x80000000u)]
    [InlineData(-2023406815, 0x87654321u)]
    [InlineData(-1, 0xFFFFFFFFu)]
    public void AsUInt_FromInt(int x, uint expected) => Assert.Equal(expected, math.asuint(x));

    [Fact]
    public void AsUInt_FromInt2()
    {
        var r = math.asuint(new int2(0, 0x12345678));
        Assert.Equal(0u, r.x);
        Assert.Equal(0x12345678u, r.y);
    }

    [Fact]
    public void AsUInt_FromInt3()
    {
        var r = math.asuint(new int3(0x7FFFFFFF, -2147483648, -1));
        Assert.Equal(0x7FFFFFFFu, r.x);
        Assert.Equal(0x80000000u, r.y);
        Assert.Equal(0xFFFFFFFFu, r.z);
    }

    [Fact]
    public void AsUInt_FromInt4()
    {
        var r = math.asuint(new int4(-2023406815, -1, 0, 0));
        Assert.Equal(0x87654321u, r.x);
        Assert.Equal(0xFFFFFFFFu, r.y);
    }

    [Theory]
    [InlineData(0.0f, 0u)]
    [InlineData(1.0f, 0x3F800000u)]
    public void AsUInt_FromFloat(float x, uint expected) => Assert.Equal(expected, math.asuint(x));

    [Fact]
    public void AsUInt_FromFloat2()
    {
        var r = math.asuint(new float2(1234.56f, float.PositiveInfinity));
        Assert.Equal(0x449A51ECu, r.x);
        Assert.Equal(0x7F800000u, r.y);
    }

    [Fact]
    public void AsUInt_FromFloat3()
    {
        var r = math.asuint(new float3(0.0f, 1.0f, 1234.56f));
        Assert.Equal(0u, r.x);
        Assert.Equal(0x3F800000u, r.y);
        Assert.Equal(0x449A51ECu, r.z);
    }

    [Fact]
    public void AsUInt_FromFloat4()
    {
        var r = math.asuint(new float4(0u, 1.0f, 1234.56f, float.PositiveInfinity));
        Assert.Equal(0u, r.x);
        Assert.Equal(0x3F800000u, r.y);
        Assert.Equal(0x449A51ECu, r.z);
        Assert.Equal(0x7F800000u, r.w);
    }

    [Fact]
    public void AsUInt_FromPositiveInfinity() => Assert.Equal(0x7F800000u, math.asuint(float.PositiveInfinity));

    [Fact]
    public void AsUInt_FromSignedFloatQNaN() => Assert.Equal(0xFFC00000u, math.asuint(SignedFloatQNaN()));

    [Theory]
    [InlineData(0, 0.0f)]
    [InlineData(0x3F800000, 1.0f)]
    [InlineData(unchecked((int)0xBF800000), -1.0f)]
    public void AsFloat_FromInt(int x, float expected) => Assert.Equal(expected, math.asfloat(x));

    [Fact]
    public void AsFloat_FromInt2()
    {
        var r = math.asfloat(new int2(0, 0x3F800000));
        Assert.Equal(0.0f, r.x);
        Assert.Equal(1.0f, r.y);
    }

    [Fact]
    public void AsFloat_FromInt3()
    {
        var r = math.asfloat(new int3(0, 0x3F800000, 0x449A51EC));
        Assert.Equal(0.0f, r.x);
        Assert.Equal(1.0f, r.y);
        Assert.Equal(1234.56f, r.z);
    }

    [Fact]
    public void AsFloat_FromInt4()
    {
        var r = math.asfloat(new int4(0, 0x3F800000, 0x449A51EC, 0x7F800000));
        Assert.Equal(0.0f, r.x);
        Assert.Equal(1.0f, r.y);
        Assert.Equal(1234.56f, r.z);
        Assert.Equal(float.PositiveInfinity, r.w);
    }

    [Theory]
    [InlineData(0u, 0.0f)]
    [InlineData(0x3F800000u, 1.0f)]
    [InlineData(0xBF800000u, -1.0f)]
    public void AsFloat_FromUInt(uint x, float expected) => Assert.Equal(expected, math.asfloat(x));

    [Fact]
    public void AsFloat_FromUInt2()
    {
        var r = math.asfloat(new uint2(0u, 0x3F800000u));
        Assert.Equal(0.0f, r.x);
        Assert.Equal(1.0f, r.y);
    }

    [Fact]
    public void AsFloat_FromUInt3()
    {
        var r = math.asfloat(new uint3(0u, 0x3F800000u, 0x449A51ECu));
        Assert.Equal(0.0f, r.x);
        Assert.Equal(1.0f, r.y);
        Assert.Equal(1234.56f, r.z);
    }

    [Fact]
    public void AsFloat_FromUInt4()
    {
        var r = math.asfloat(new uint4(0u, 0x3F800000u, 0x80000000u, 0xBF800000u));
        Assert.Equal(0.0f, r.x);
        Assert.Equal(1.0f, r.y);
        Assert.Equal(-0.0f, r.z);
        Assert.Equal(-1.0f, r.w);
    }

    [Theory]
    [InlineData(0uL, 0L)]
    [InlineData(0x123456789ABCDEF0uL, 0x123456789ABCDEF0L)]
    public void AsLong_FromULong(ulong x, long expected) => Assert.Equal(expected, math.aslong(x));

    [Theory]
    [InlineData(0.0, 0L)]
    [InlineData(1.0, 0x3FF0000000000000L)]
    public void AsLong_FromDouble(double x, long expected) => Assert.Equal(expected, math.aslong(x));

    [Fact]
    public void AsLong_FromPositiveInfinity() => Assert.Equal(0x7FF0000000000000L, math.aslong(double.PositiveInfinity));

    [Fact]
    public void AsLong_FromSignedDoubleQNaN() => Assert.Equal(unchecked((long)0xFFF8000000000000UL), math.aslong(SignedDoubleQNaN()));

    [Theory]
    [InlineData(0, 0uL)]
    [InlineData(0x123456789ABCDEF0L, 0x123456789ABCDEF0uL)]
    public void AsULong_FromLong(long x, ulong expected) => Assert.Equal(expected, math.asulong(x));

    [Theory]
    [InlineData(0.0, 0uL)]
    [InlineData(1.0, 0x3FF0000000000000uL)]
    public void AsULong_FromDouble(double x, ulong expected) => Assert.Equal(expected, math.asulong(x));

    [Theory]
    [InlineData(0uL, 0.0)]
    public void AsDouble_FromULong(ulong x, double expected) => Assert.Equal(expected, math.asdouble(x));

    [Theory]
    [InlineData(0L, 0.0)]
    [InlineData(0x3FF0000000000000L, 1.0)]
    public void AsDouble_FromLong(long x, double expected) => Assert.Equal(expected, math.asdouble(x));
}
