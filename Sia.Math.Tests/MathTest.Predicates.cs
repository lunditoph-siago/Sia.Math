using Sia.Math;
using static Sia.Math.Tests.TestValue;

namespace Sia.Math.Tests;

public class TestMathPredicates
{
    public static TheoryData<float, bool> FloatSpecialData => new()
    {
        { SignedFloatQNaN(), false },
        { float.NegativeInfinity, false },
        { float.MinValue, true },
        { -1.0f, true },
        { -0.0f, true },
        { 0.0f, true },
        { 1.0f, true },
        { float.MaxValue, true },
        { float.PositiveInfinity, false },
        { UnsignedFloatQNaN(), false },
    };

    public static TheoryData<float, bool> FloatNanData => new()
    {
        { SignedFloatQNaN(), true },
        { float.NegativeInfinity, false },
        { float.MinValue, false },
        { -1.0f, false },
        { 0.0f, false },
        { 1.0f, false },
        { float.MaxValue, false },
        { float.PositiveInfinity, false },
        { UnsignedFloatQNaN(), true },
    };

    public static TheoryData<double, bool> DoubleSpecialData => new()
    {
        { SignedDoubleQNaN(), false },
        { double.NegativeInfinity, false },
        { double.MinValue, true },
        { -1.0, true },
        { -0.0, true },
        { 0.0, true },
        { 1.0, true },
        { double.MaxValue, true },
        { double.PositiveInfinity, false },
        { UnsignedDoubleQNaN(), false },
    };

    public static TheoryData<double, bool> DoubleNanData => new()
    {
        { SignedDoubleQNaN(), true },
        { double.NegativeInfinity, false },
        { double.MinValue, false },
        { -1.0, false },
        { 0.0, false },
        { 1.0, false },
        { double.MaxValue, false },
        { double.PositiveInfinity, false },
        { UnsignedDoubleQNaN(), true },
    };

    [Theory]
    [MemberData(nameof(FloatSpecialData))]
    public void IsFinite_Float(float x, bool expected) => Assert.Equal(expected, math.isfinite(x));

    [Theory]
    [MemberData(nameof(FloatSpecialData))]
    public void IsFinite_Float2(float x, bool expected)
    {
        var r = math.isfinite(new float2(x, x));
        Assert.Equal(expected, r.x);
        Assert.Equal(expected, r.y);
    }

    [Theory]
    [MemberData(nameof(FloatSpecialData))]
    public void IsFinite_Float3(float x, bool expected)
    {
        var r = math.isfinite(new float3(x, x, x));
        Assert.Equal(expected, r.x);
        Assert.Equal(expected, r.y);
        Assert.Equal(expected, r.z);
    }

    [Theory]
    [MemberData(nameof(FloatSpecialData))]
    public void IsFinite_Float4(float x, bool expected)
    {
        var r = math.isfinite(new float4(x, x, x, x));
        Assert.Equal(expected, r.x);
        Assert.Equal(expected, r.w);
    }

    [Theory]
    [MemberData(nameof(DoubleSpecialData))]
    public void IsFinite_Double(double x, bool expected) => Assert.Equal(expected, math.isfinite(x));

    [Theory]
    [MemberData(nameof(DoubleSpecialData))]
    public void IsFinite_Double2(double x, bool expected) =>
        Assert.Equal(expected, math.isfinite(new double2(x, x)).x);

    [Theory]
    [MemberData(nameof(DoubleSpecialData))]
    public void IsFinite_Double3(double x, bool expected) =>
        Assert.Equal(expected, math.isfinite(new double3(x, x, x)).x);

    [Theory]
    [MemberData(nameof(DoubleSpecialData))]
    public void IsFinite_Double4(double x, bool expected) =>
        Assert.Equal(expected, math.isfinite(new double4(x, x, x, x)).x);

    [Theory]
    [InlineData(float.NegativeInfinity, true)]
    [InlineData(float.PositiveInfinity, true)]
    [InlineData(-1.0f, false)]
    [InlineData(0.0f, false)]
    [InlineData(1.0f, false)]
    [InlineData(float.NaN, false)]
    public void IsInf_Float(float x, bool expected) => Assert.Equal(expected, math.isinf(x));

    [Theory]
    [InlineData(float.NegativeInfinity, true)]
    [InlineData(float.PositiveInfinity, true)]
    [InlineData(0.0f, false)]
    public void IsInf_Float2(float x, bool expected) =>
        Assert.Equal(expected, math.isinf(new float2(x, x)).x);

    [Theory]
    [InlineData(float.NegativeInfinity, true)]
    [InlineData(float.PositiveInfinity, true)]
    [InlineData(0.0f, false)]
    public void IsInf_Float3(float x, bool expected) =>
        Assert.Equal(expected, math.isinf(new float3(x, x, x)).x);

    [Theory]
    [InlineData(float.NegativeInfinity, true)]
    [InlineData(float.PositiveInfinity, true)]
    [InlineData(0.0f, false)]
    public void IsInf_Float4(float x, bool expected) =>
        Assert.Equal(expected, math.isinf(new float4(x, x, x, x)).x);

    [Theory]
    [InlineData(double.NegativeInfinity, true)]
    [InlineData(double.PositiveInfinity, true)]
    [InlineData(-1.0, false)]
    [InlineData(0.0, false)]
    [InlineData(1.0, false)]
    [InlineData(double.NaN, false)]
    public void IsInf_Double(double x, bool expected) => Assert.Equal(expected, math.isinf(x));

    [Theory]
    [InlineData(double.NegativeInfinity, true)]
    [InlineData(double.PositiveInfinity, true)]
    [InlineData(0.0, false)]
    public void IsInf_Double2(double x, bool expected) =>
        Assert.Equal(expected, math.isinf(new double2(x, x)).x);

    [Theory]
    [InlineData(double.NegativeInfinity, true)]
    [InlineData(double.PositiveInfinity, true)]
    [InlineData(0.0, false)]
    public void IsInf_Double3(double x, bool expected) =>
        Assert.Equal(expected, math.isinf(new double3(x, x, x)).x);

    [Theory]
    [InlineData(double.NegativeInfinity, true)]
    [InlineData(double.PositiveInfinity, true)]
    [InlineData(0.0, false)]
    public void IsInf_Double4(double x, bool expected) =>
        Assert.Equal(expected, math.isinf(new double4(x, x, x, x)).x);

    [Theory]
    [MemberData(nameof(FloatNanData))]
    public void IsNan_Float(float x, bool expected) => Assert.Equal(expected, math.isnan(x));

    [Theory]
    [MemberData(nameof(FloatNanData))]
    public void IsNan_Float2(float x, bool expected) =>
        Assert.Equal(expected, math.isnan(new float2(x, x)).x);

    [Theory]
    [MemberData(nameof(FloatNanData))]
    public void IsNan_Float3(float x, bool expected) =>
        Assert.Equal(expected, math.isnan(new float3(x, x, x)).x);

    [Theory]
    [MemberData(nameof(FloatNanData))]
    public void IsNan_Float4(float x, bool expected) =>
        Assert.Equal(expected, math.isnan(new float4(x, x, x, x)).x);

    [Theory]
    [MemberData(nameof(DoubleNanData))]
    public void IsNan_Double(double x, bool expected) => Assert.Equal(expected, math.isnan(x));

    [Theory]
    [MemberData(nameof(DoubleNanData))]
    public void IsNan_Double2(double x, bool expected) =>
        Assert.Equal(expected, math.isnan(new double2(x, x)).x);

    [Theory]
    [MemberData(nameof(DoubleNanData))]
    public void IsNan_Double3(double x, bool expected) =>
        Assert.Equal(expected, math.isnan(new double3(x, x, x)).x);

    [Theory]
    [MemberData(nameof(DoubleNanData))]
    public void IsNan_Double4(double x, bool expected) =>
        Assert.Equal(expected, math.isnan(new double4(x, x, x, x)).x);

    public static TheoryData<int, bool> IsPow2IntData => new()
    {
        { -3, false }, { -2, false }, { -1, false }, { 0, false },
        { 1, true }, { 2, true }, { 3, false }, { 4, true },
        { (1 << 15) - 1, false }, { 1 << 15, true }, { (1 << 15) + 1, false },
        { (1 << 21) - 1, false }, { 1 << 21, true }, { 268431360, false },
    };

    [Theory]
    [MemberData(nameof(IsPow2IntData))]
    public void IsPow2_Int(int x, bool expected) => Assert.Equal(expected, math.ispow2(x));

    [Theory]
    [MemberData(nameof(IsPow2IntData))]
    public void IsPow2_Int2(int x, bool expected) =>
        Assert.Equal(expected, math.ispow2(new int2(x, x)).x);

    [Theory]
    [MemberData(nameof(IsPow2IntData))]
    public void IsPow2_Int3(int x, bool expected) =>
        Assert.Equal(expected, math.ispow2(new int3(x, x, x)).x);

    [Theory]
    [MemberData(nameof(IsPow2IntData))]
    public void IsPow2_Int4(int x, bool expected) =>
        Assert.Equal(expected, math.ispow2(new int4(x, x, x, x)).x);

    public static TheoryData<uint, bool> IsPow2UIntData => new()
    {
        { 0u, false }, { 1u, true }, { 2u, true }, { 3u, false }, { 4u, true },
        { (1u << 15) - 1, false }, { 1u << 15, true }, { (1u << 15) + 1, false },
        { (1u << 21) - 1, false }, { 1u << 21, true }, { 268431360u, false },
    };

    [Theory]
    [MemberData(nameof(IsPow2UIntData))]
    public void IsPow2_UInt(uint x, bool expected) => Assert.Equal(expected, math.ispow2(x));

    [Theory]
    [MemberData(nameof(IsPow2UIntData))]
    public void IsPow2_UInt2(uint x, bool expected) =>
        Assert.Equal(expected, math.ispow2(new uint2(x, x)).x);

    [Theory]
    [MemberData(nameof(IsPow2UIntData))]
    public void IsPow2_UInt3(uint x, bool expected) =>
        Assert.Equal(expected, math.ispow2(new uint3(x, x, x)).x);

    [Theory]
    [MemberData(nameof(IsPow2UIntData))]
    public void IsPow2_UInt4(uint x, bool expected) =>
        Assert.Equal(expected, math.ispow2(new uint4(x, x, x, x)).x);
}
