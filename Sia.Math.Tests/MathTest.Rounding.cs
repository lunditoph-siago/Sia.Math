using Sia.Math;
using static Sia.Math.Tests.TestValue;

namespace Sia.Math.Tests;

public class TestMathRounding
{
    public static TheoryData<float, float> FloorFloatData => new()
    {
        { float.NegativeInfinity, float.NegativeInfinity },
        { -100.51f, -101f }, { -100.5f, -101f }, { -100.49f, -101f },
        { 0f, 0f },
        { 100.49f, 100f }, { 100.5f, 100f }, { 100.51f, 100f },
        { float.PositiveInfinity, float.PositiveInfinity },
        { SignedFloatQNaN(), SignedFloatQNaN() },
    };

    [Theory]
    [MemberData(nameof(FloorFloatData))]
    public void Floor_Float(float x, float expected) => Assert.Equal(expected, math.floor(x));

    [Theory]
    [MemberData(nameof(FloorFloatData))]
    public void Floor_Float2(float x, float expected)
    {
        var r = math.floor(new float2(x, x));
        Assert.Equal(expected, r.x);
        Assert.Equal(expected, r.y);
    }

    [Theory]
    [MemberData(nameof(FloorFloatData))]
    public void Floor_Float3(float x, float expected) =>
        Assert.Equal(expected, math.floor(new float3(x, x, x)).x);

    [Theory]
    [MemberData(nameof(FloorFloatData))]
    public void Floor_Float4(float x, float expected) =>
        Assert.Equal(expected, math.floor(new float4(x, x, x, x)).x);

    public static TheoryData<double, double> FloorDoubleData => new()
    {
        { double.NegativeInfinity, double.NegativeInfinity },
        { -100.51, -101.0 }, { -100.5, -101.0 }, { -100.49, -101.0 },
        { 0.0, 0.0 },
        { 100.49, 100.0 }, { 100.5, 100.0 }, { 100.51, 100.0 },
        { double.PositiveInfinity, double.PositiveInfinity },
        { SignedDoubleQNaN(), SignedDoubleQNaN() },
    };

    [Theory]
    [MemberData(nameof(FloorDoubleData))]
    public void Floor_Double(double x, double expected) => Assert.Equal(expected, math.floor(x));

    [Theory]
    [MemberData(nameof(FloorDoubleData))]
    public void Floor_Double2(double x, double expected) =>
        Assert.Equal(expected, math.floor(new double2(x, x)).x);

    [Theory]
    [MemberData(nameof(FloorDoubleData))]
    public void Floor_Double3(double x, double expected) =>
        Assert.Equal(expected, math.floor(new double3(x, x, x)).x);

    [Theory]
    [MemberData(nameof(FloorDoubleData))]
    public void Floor_Double4(double x, double expected) =>
        Assert.Equal(expected, math.floor(new double4(x, x, x, x)).x);

    public static TheoryData<float, float> CeilFloatData => new()
    {
        { float.NegativeInfinity, float.NegativeInfinity },
        { -100.51f, -100f }, { -100.5f, -100f }, { -100.49f, -100f },
        { 0f, 0f },
        { 100.49f, 101f }, { 100.5f, 101f }, { 100.51f, 101f },
        { float.PositiveInfinity, float.PositiveInfinity },
        { SignedFloatQNaN(), SignedFloatQNaN() },
    };

    [Theory]
    [MemberData(nameof(CeilFloatData))]
    public void Ceil_Float(float x, float expected) => Assert.Equal(expected, math.ceil(x));

    [Theory]
    [MemberData(nameof(CeilFloatData))]
    public void Ceil_Float2(float x, float expected) =>
        Assert.Equal(expected, math.ceil(new float2(x, x)).x);

    [Theory]
    [MemberData(nameof(CeilFloatData))]
    public void Ceil_Float3(float x, float expected) =>
        Assert.Equal(expected, math.ceil(new float3(x, x, x)).x);

    [Theory]
    [MemberData(nameof(CeilFloatData))]
    public void Ceil_Float4(float x, float expected) =>
        Assert.Equal(expected, math.ceil(new float4(x, x, x, x)).x);

    public static TheoryData<double, double> CeilDoubleData => new()
    {
        { double.NegativeInfinity, double.NegativeInfinity },
        { -100.51, -100.0 }, { -100.5, -100.0 }, { -100.49, -100.0 },
        { 0.0, 0.0 },
        { 100.49, 101.0 }, { 100.5, 101.0 }, { 100.51, 101.0 },
        { double.PositiveInfinity, double.PositiveInfinity },
        { SignedDoubleQNaN(), SignedDoubleQNaN() },
    };

    [Theory]
    [MemberData(nameof(CeilDoubleData))]
    public void Ceil_Double(double x, double expected) => Assert.Equal(expected, math.ceil(x));

    [Theory]
    [MemberData(nameof(CeilDoubleData))]
    public void Ceil_Double2(double x, double expected) =>
        Assert.Equal(expected, math.ceil(new double2(x, x)).x);

    [Theory]
    [MemberData(nameof(CeilDoubleData))]
    public void Ceil_Double3(double x, double expected) =>
        Assert.Equal(expected, math.ceil(new double3(x, x, x)).x);

    [Theory]
    [MemberData(nameof(CeilDoubleData))]
    public void Ceil_Double4(double x, double expected) =>
        Assert.Equal(expected, math.ceil(new double4(x, x, x, x)).x);

    public static TheoryData<float, float> RoundFloatData => new()
    {
        { float.NegativeInfinity, float.NegativeInfinity },
        { -100.51f, -101f }, { -100.5f, -100f }, { -100.49f, -100f },
        { 0f, 0f },
        { 100.49f, 100f }, { 100.5f, 100f }, { 100.51f, 101f }, { 101.5f, 102f },
        { float.PositiveInfinity, float.PositiveInfinity },
        { SignedFloatQNaN(), SignedFloatQNaN() },
    };

    [Theory]
    [MemberData(nameof(RoundFloatData))]
    public void Round_Float(float x, float expected) => Assert.Equal(expected, math.round(x));

    [Theory]
    [MemberData(nameof(RoundFloatData))]
    public void Round_Float2(float x, float expected) =>
        Assert.Equal(expected, math.round(new float2(x, x)).x);

    [Theory]
    [MemberData(nameof(RoundFloatData))]
    public void Round_Float3(float x, float expected) =>
        Assert.Equal(expected, math.round(new float3(x, x, x)).x);

    [Theory]
    [MemberData(nameof(RoundFloatData))]
    public void Round_Float4(float x, float expected) =>
        Assert.Equal(expected, math.round(new float4(x, x, x, x)).x);

    public static TheoryData<double, double> RoundDoubleData => new()
    {
        { double.NegativeInfinity, double.NegativeInfinity },
        { -100.51, -101.0 }, { -100.5, -100.0 }, { -100.49, -100.0 },
        { 0.0, 0.0 },
        { 100.49, 100.0 }, { 100.5, 100.0 }, { 100.51, 101.0 }, { 101.5, 102.0 },
        { double.PositiveInfinity, double.PositiveInfinity },
        { SignedDoubleQNaN(), SignedDoubleQNaN() },
    };

    [Theory]
    [MemberData(nameof(RoundDoubleData))]
    public void Round_Double(double x, double expected) => Assert.Equal(expected, math.round(x));

    [Theory]
    [MemberData(nameof(RoundDoubleData))]
    public void Round_Double2(double x, double expected) =>
        Assert.Equal(expected, math.round(new double2(x, x)).x);

    [Theory]
    [MemberData(nameof(RoundDoubleData))]
    public void Round_Double3(double x, double expected) =>
        Assert.Equal(expected, math.round(new double3(x, x, x)).x);

    [Theory]
    [MemberData(nameof(RoundDoubleData))]
    public void Round_Double4(double x, double expected) =>
        Assert.Equal(expected, math.round(new double4(x, x, x, x)).x);

    public static TheoryData<float, float> TruncFloatData => new()
    {
        { float.NegativeInfinity, float.NegativeInfinity },
        { -100.51f, -100f }, { -100.5f, -100f }, { -100.49f, -100f },
        { 0f, 0f },
        { 100.49f, 100f }, { 100.5f, 100f }, { 100.51f, 100f }, { 101.5f, 101f },
        { float.PositiveInfinity, float.PositiveInfinity },
        { SignedFloatQNaN(), SignedFloatQNaN() },
    };

    [Theory]
    [MemberData(nameof(TruncFloatData))]
    public void Trunc_Float(float x, float expected) => Assert.Equal(expected, math.trunc(x));

    [Theory]
    [MemberData(nameof(TruncFloatData))]
    public void Trunc_Float2(float x, float expected) =>
        Assert.Equal(expected, math.trunc(new float2(x, x)).x);

    [Theory]
    [MemberData(nameof(TruncFloatData))]
    public void Trunc_Float3(float x, float expected) =>
        Assert.Equal(expected, math.trunc(new float3(x, x, x)).x);

    [Theory]
    [MemberData(nameof(TruncFloatData))]
    public void Trunc_Float4(float x, float expected) =>
        Assert.Equal(expected, math.trunc(new float4(x, x, x, x)).x);

    public static TheoryData<double, double> TruncDoubleData => new()
    {
        { double.NegativeInfinity, double.NegativeInfinity },
        { -100.51, -100.0 }, { -100.5, -100.0 }, { -100.49, -100.0 },
        { 0.0, 0.0 },
        { 100.49, 100.0 }, { 100.5, 100.0 }, { 100.51, 100.0 }, { 101.5, 101.0 },
        { double.PositiveInfinity, double.PositiveInfinity },
        { SignedDoubleQNaN(), SignedDoubleQNaN() },
    };

    [Theory]
    [MemberData(nameof(TruncDoubleData))]
    public void Trunc_Double(double x, double expected) => Assert.Equal(expected, math.trunc(x));

    [Theory]
    [MemberData(nameof(TruncDoubleData))]
    public void Trunc_Double2(double x, double expected) =>
        Assert.Equal(expected, math.trunc(new double2(x, x)).x);

    [Theory]
    [MemberData(nameof(TruncDoubleData))]
    public void Trunc_Double3(double x, double expected) =>
        Assert.Equal(expected, math.trunc(new double3(x, x, x)).x);

    [Theory]
    [MemberData(nameof(TruncDoubleData))]
    public void Trunc_Double4(double x, double expected) =>
        Assert.Equal(expected, math.trunc(new double4(x, x, x, x)).x);

    public static TheoryData<float, float> FracFloatData => new()
    {
        { float.NegativeInfinity, float.NaN },
        { -1E+20f, 0f }, { -100.3f, 0.7f },
        { 0f, 0f },
        { 100.8f, 0.8f },
        { float.PositiveInfinity, float.NaN },
        { SignedFloatQNaN(), float.NaN },
    };

    [Theory]
    [MemberData(nameof(FracFloatData))]
    public void Frac_Float(float x, float expected)
    {
        var result = math.frac(x);
        if (float.IsNaN(expected)) Assert.True(float.IsNaN(result));
        else Assert.Equal(expected, result, 3);
    }

    [Theory]
    [MemberData(nameof(FracFloatData))]
    public void Frac_Float2(float x, float expected)
    {
        var result = math.frac(new float2(x, x)).x;
        if (float.IsNaN(expected)) Assert.True(float.IsNaN(result));
        else Assert.Equal(expected, result, 3);
    }

    public static TheoryData<double, double> FracDoubleData => new()
    {
        { double.NegativeInfinity, double.NaN },
        { -1E+20, 0.0 }, { -100.3, 0.7 },
        { 0.0, 0.0 },
        { 100.8, 0.8 },
        { double.PositiveInfinity, double.NaN },
        { SignedDoubleQNaN(), double.NaN },
    };

    [Theory]
    [MemberData(nameof(FracDoubleData))]
    public void Frac_Double(double x, double expected)
    {
        var result = math.frac(x);
        if (double.IsNaN(expected)) Assert.True(double.IsNaN(result));
        else Assert.Equal(expected, result, 5);
    }

    [Theory]
    [MemberData(nameof(FracDoubleData))]
    public void Frac_Double2(double x, double expected)
    {
        var result = math.frac(new double2(x, x)).x;
        if (double.IsNaN(expected)) Assert.True(double.IsNaN(result));
        else Assert.Equal(expected, result, 5);
    }
}
