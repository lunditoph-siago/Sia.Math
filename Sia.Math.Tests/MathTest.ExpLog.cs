using Sia.Math;
using static Sia.Math.Tests.TestValue;
using static Sia.Math.Tests.TestAssert;

namespace Sia.Math.Tests;

public class TestMathExpLog
{
    public static TheoryData<double, double> ExpDoubleData => new()
    {
        { -10.0, 0.00004539992976248485 },
        { -1.2, 0.3011942119122021 },
        { 0.0, 1.0 },
        { 1.2, 3.3201169227365475 },
    };

    [Theory]
    [MemberData(nameof(ExpDoubleData))]
    public void Exp_Double(double x, double expected) => Approx(expected, math.exp(x));

    [Theory]
    [MemberData(nameof(ExpDoubleData))]
    public void Exp_Double2(double x, double expected) =>
        Approx(expected, math.exp(new double2(x, x)).x);

    [Theory]
    [MemberData(nameof(ExpDoubleData))]
    public void Exp_Double3(double x, double expected) =>
        Approx(expected, math.exp(new double3(x, x, x)).x);

    [Theory]
    [MemberData(nameof(ExpDoubleData))]
    public void Exp_Double4(double x, double expected) =>
        Approx(expected, math.exp(new double4(x, x, x, x)).x);

    [Theory]
    [InlineData(0.0f, 1.0f)]
    [InlineData(1.0f, 2.7182817f)]
    [InlineData(-1.2f, 0.3011942f)]
    [InlineData(1.2f, 3.320117f)]
    public void Exp_Float(float x, float expected) => Approx(expected, math.exp(x));

    [Theory]
    [InlineData(0.0f, 1.0f)]
    public void Exp_Float2(float x, float expected) =>
        Approx(expected, math.exp(new float2(x, x)).x);

    [Theory]
    [InlineData(1.0f, 2.7182817f)]
    public void Exp_Float3(float x, float expected) =>
        Approx(expected, math.exp(new float3(x, x, x)).x);

    [Theory]
    [InlineData(-1.2f, 0.3011942f)]
    public void Exp_Float4(float x, float expected) =>
        Approx(expected, math.exp(new float4(x, x, x, x)).x);

    public static TheoryData<double, double> Exp2DoubleData => new()
    {
        { -10.0, 0.0009765625 },
        { -1.2, 0.435275281648062 },
        { 0.0, 1.0 },
        { 1.2, 2.29739670999407 },
    };

    [Theory]
    [MemberData(nameof(Exp2DoubleData))]
    public void Exp2_Double(double x, double expected) => Approx(expected, math.exp2(x));

    [Theory]
    [MemberData(nameof(Exp2DoubleData))]
    public void Exp2_Double2(double x, double expected) =>
        Approx(expected, math.exp2(new double2(x, x)).x);

    [Theory]
    [MemberData(nameof(Exp2DoubleData))]
    public void Exp2_Double3(double x, double expected) =>
        Approx(expected, math.exp2(new double3(x, x, x)).x);

    [Theory]
    [MemberData(nameof(Exp2DoubleData))]
    public void Exp2_Double4(double x, double expected) =>
        Approx(expected, math.exp2(new double4(x, x, x, x)).x);

    [Theory]
    [InlineData(-10.0f, 0.0009765625f)]
    [InlineData(-1.2f, 0.4352753f)]
    [InlineData(0.0f, 1.0f)]
    [InlineData(1.2f, 2.29739666f)]
    public void Exp2_Float(float x, float expected) => Approx(expected, math.exp2(x));

    [Fact]
    public void Exp2_InfNaN()
    {
        Assert.Equal(0.0, math.exp2(double.NegativeInfinity));
        Assert.True(double.IsNaN(math.exp2(double.NaN)));
        Assert.Equal(double.PositiveInfinity, math.exp2(double.PositiveInfinity));
    }

    public static TheoryData<double, double> Exp10DoubleData => new()
    {
        { -10.0, 1E-10 },
        { -1.2, 0.063095734448019331 },
        { 0.0, 1.0 },
        { 1.2, 15.848931924611135 },
    };

    [Theory]
    [MemberData(nameof(Exp10DoubleData))]
    public void Exp10_Double(double x, double expected) => Approx(expected, math.exp10(x));

    [Theory]
    [MemberData(nameof(Exp10DoubleData))]
    public void Exp10_Double2(double x, double expected) =>
        Approx(expected, math.exp10(new double2(x, x)).x);

    [Theory]
    [InlineData(-10.0f, 1E-10f)]
    [InlineData(-1.2f, 0.06309573f)]
    [InlineData(0.0f, 1.0f)]
    [InlineData(1.2f, 15.8489323f)]
    public void Exp10_Float(float x, float expected) => Approx(expected, math.exp10(x));

    [Fact]
    public void Exp10_InfNaN()
    {
        Assert.Equal(0.0, math.exp10(double.NegativeInfinity));
        Assert.True(double.IsNaN(math.exp10(double.NaN)));
        Assert.Equal(double.PositiveInfinity, math.exp10(double.PositiveInfinity));
    }

    public static TheoryData<double, double> LogDoubleData => new()
    {
        { 1.2e-9, -20.540944280152457 },
        { 1.0, 0.0 },
        { 1.2e10, 23.20817248673441 },
    };

    [Theory]
    [MemberData(nameof(LogDoubleData))]
    public void Log_Double(double x, double expected) => Approx(expected, math.log(x));

    [Theory]
    [MemberData(nameof(LogDoubleData))]
    public void Log_Double2(double x, double expected) =>
        Approx(expected, math.log(new double2(x, x)).x);

    [Theory]
    [MemberData(nameof(LogDoubleData))]
    public void Log_Double3(double x, double expected) =>
        Approx(expected, math.log(new double3(x, x, x)).x);

    [Theory]
    [MemberData(nameof(LogDoubleData))]
    public void Log_Double4(double x, double expected) =>
        Approx(expected, math.log(new double4(x, x, x, x)).x);

    [Theory]
    [InlineData(1.0f, 0.0f)]
    [InlineData(2.7182817f, 1.0f)]
    public void Log_Float(float x, float expected) => Approx(expected, math.log(x));

    [Theory]
    [InlineData(1.0f, 0.0f)]
    public void Log_Float2(float x, float expected) =>
        Approx(expected, math.log(new float2(x, x)).x);

    [Fact]
    public void Log_Invalid_ReturnsNaN()
    {
        Assert.True(double.IsNaN(math.log(-1.0)));
        Assert.True(double.IsNaN(math.log(double.NegativeInfinity)));
        Assert.True(double.IsNaN(math.log(double.NaN)));
    }

    [Fact]
    public void Log_Infinity_ReturnsInfinity() =>
        Assert.Equal(double.PositiveInfinity, math.log(double.PositiveInfinity));

    [Theory]
    [InlineData(1.2e-9, -29.634318448152467)]
    [InlineData(1.0, 0.0)]
    [InlineData(12000000000.0, 33.482315354707417)]
    public void Log2_Double(double x, double expected) => Approx(expected, math.log2(x));

    [Theory]
    [InlineData(1.2e-9, -29.634318448152467)]
    public void Log2_Double2(double x, double expected) =>
        Approx(expected, math.log2(new double2(x, x)).x);

    [Theory]
    [InlineData(1.2e-09f, -29.63432f)]
    [InlineData(1.0f, 0.0f)]
    [InlineData(1.2e+10f, 33.4823151f)]
    public void Log2_Float(float x, float expected) => Approx(expected, math.log2(x));

    [Fact]
    public void Log2_Invalid_ReturnsNaN()
    {
        Assert.True(double.IsNaN(math.log2(-1.0)));
        Assert.True(double.IsNaN(math.log2(double.NegativeInfinity)));
        Assert.True(double.IsNaN(math.log2(double.NaN)));
    }

    [Fact]
    public void Log2_Infinity_ReturnsInfinity() =>
        Assert.Equal(double.PositiveInfinity, math.log2(double.PositiveInfinity));

    [Theory]
    [InlineData(1.2e-9, -8.9208187539523749)]
    [InlineData(1.0, 0.0)]
    [InlineData(12000000000.0, 10.079181246047623)]
    public void Log10_Double(double x, double expected) => Approx(expected, math.log10(x));

    [Theory]
    [InlineData(1.2e-9, -8.9208187539523749)]
    public void Log10_Double2(double x, double expected) =>
        Approx(expected, math.log10(new double2(x, x)).x);

    [Theory]
    [InlineData(1.2e-09f, -8.920818f)]
    [InlineData(1.0f, 0.0f)]
    [InlineData(1.2e+10f, 10.0791817f)]
    public void Log10_Float(float x, float expected) => Approx(expected, math.log10(x));

    [Fact]
    public void Log10_Invalid_ReturnsNaN()
    {
        Assert.True(double.IsNaN(math.log10(-1.0)));
        Assert.True(double.IsNaN(math.log10(double.NegativeInfinity)));
    }

    public static TheoryData<double, double, double> PowDoubleData => new()
    {
        { -3.4, 2.6, double.NaN },
        { -0.0, 2.6, 0.0 },
        { 0.0, 2.6, 0.0 },
        { 3.4, 2.6, 24.090465076169736 },
        { double.PositiveInfinity, 2.6, double.PositiveInfinity },
        { 3.4, 0.0, 1.0 },
        { double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity },
    };

    [Theory]
    [MemberData(nameof(PowDoubleData))]
    public void Pow_Double(double x, double y, double expected)
    {
        var result = math.pow(x, y);
        if (double.IsNaN(expected)) Assert.True(double.IsNaN(result));
        else if (double.IsInfinity(expected)) Assert.Equal(expected, result);
        else Approx(expected, result, 1e-8);
    }

    [Theory]
    [MemberData(nameof(PowDoubleData))]
    public void Pow_Double2(double x, double y, double expected)
    {
        var result = math.pow(new double2(x, x), new double2(y, y)).x;
        if (double.IsNaN(expected)) Assert.True(double.IsNaN(result));
        else if (double.IsInfinity(expected)) Assert.Equal(expected, result);
        else Approx(expected, result, 1e-8);
    }

    [Theory]
    [InlineData(3.4f, 2.6f, 24.090466f)]
    [InlineData(0.0f, 2.6f, 0.0f)]
    [InlineData(2.0f, 3.0f, 8.0f)]
    public void Pow_Float(float x, float y, float expected) =>
        Approx(expected, math.pow(x, y));

    public static TheoryData<double, double> SqrtDoubleData => new()
    {
        { 0.0, 0.0 },
        { 1e-10, 1e-5 },
        { 123.45, 11.11080555135405 },
    };

    [Theory]
    [MemberData(nameof(SqrtDoubleData))]
    public void Sqrt_Double(double x, double expected) => Approx(expected, math.sqrt(x));

    [Theory]
    [MemberData(nameof(SqrtDoubleData))]
    public void Sqrt_Double2(double x, double expected) =>
        Approx(expected, math.sqrt(new double2(x, x)).x);

    [Theory]
    [MemberData(nameof(SqrtDoubleData))]
    public void Sqrt_Double3(double x, double expected) =>
        Approx(expected, math.sqrt(new double3(x, x, x)).x);

    [Theory]
    [MemberData(nameof(SqrtDoubleData))]
    public void Sqrt_Double4(double x, double expected) =>
        Approx(expected, math.sqrt(new double4(x, x, x, x)).x);

    [Theory]
    [InlineData(0.0f, 0.0f)]
    [InlineData(4.0f, 2.0f)]
    [InlineData(2.0f, 1.4142135f)]
    public void Sqrt_Float(float x, float expected) => Approx(expected, math.sqrt(x));

    [Theory]
    [InlineData(0.0f, 0.0f)]
    public void Sqrt_Float2(float x, float expected) =>
        Approx(expected, math.sqrt(new float2(x, x)).x);

    [Fact]
    public void Sqrt_Negative_ReturnsNaN()
    {
        Assert.True(double.IsNaN(math.sqrt(-1.0)));
        Assert.True(double.IsNaN(math.sqrt(double.NegativeInfinity)));
    }

    [Fact]
    public void Sqrt_Infinity_ReturnsInfinity() =>
        Assert.Equal(double.PositiveInfinity, math.sqrt(double.PositiveInfinity));

    public static TheoryData<double, double> RSqrtDoubleData => new()
    {
        { 1e10, 1e-5 },
        { 123.45, 0.09000247510209843 },
    };

    [Theory]
    [MemberData(nameof(RSqrtDoubleData))]
    public void RSqrt_Double(double x, double expected) => Approx(expected, math.rsqrt(x));

    [Theory]
    [MemberData(nameof(RSqrtDoubleData))]
    public void RSqrt_Double2(double x, double expected) =>
        Approx(expected, math.rsqrt(new double2(x, x)).x);

    [Theory]
    [InlineData(1.0f, 1.0f)]
    [InlineData(4.0f, 0.5f)]
    public void RSqrt_Float(float x, float expected) => Approx(expected, math.rsqrt(x));

    [Theory]
    [InlineData(1.0f, 1.0f)]
    public void RSqrt_Float2(float x, float expected) =>
        Approx(expected, math.rsqrt(new float2(x, x)).x);

    [Fact]
    public void RSqrt_Zero_PosInf() => Assert.Equal(double.PositiveInfinity, math.rsqrt(0.0));

    public static TheoryData<double, double> RCpDoubleData => new()
    {
        { -123.45, -0.0081004455245038477 },
        { 123.45, 0.0081004455245038477 },
    };

    [Theory]
    [MemberData(nameof(RCpDoubleData))]
    public void RCp_Double(double x, double expected) => Approx(expected, math.rcp(x));

    [Theory]
    [MemberData(nameof(RCpDoubleData))]
    public void RCp_Double2(double x, double expected) =>
        Approx(expected, math.rcp(new double2(x, x)).x);

    [Theory]
    [InlineData(2.0f, 0.5f)]
    [InlineData(4.0f, 0.25f)]
    public void RCp_Float(float x, float expected) => Approx(expected, math.rcp(x));

    [Theory]
    [InlineData(2.0f, 0.5f)]
    public void RCp_Float2(float x, float expected) =>
        Approx(expected, math.rcp(new float2(x, x)).x);

    [Fact]
    public void RCp_Zero_PosInf() => Assert.Equal(double.PositiveInfinity, math.rcp(0.0));

    [Fact]
    public void RCp_Inf_Zero() => Assert.Equal(0.0, math.rcp(double.PositiveInfinity));
}
