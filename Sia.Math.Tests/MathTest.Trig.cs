using Sia.Math;
using static Sia.Math.Tests.TestValue;
using static Sia.Math.Tests.TestAssert;

namespace Sia.Math.Tests;

public class TestMathTrig
{
    public static TheoryData<double, double> SinDoubleData => new()
    {
        { -1000000.0, 0.34999350217129295 },
        { -1.2, -0.93203908596722635 },
        { 0.0, 0.0 },
        { 1.2, 0.93203908596722635 },
        { 1000000.0, -0.34999350217129295 },
    };

    [Theory]
    [MemberData(nameof(SinDoubleData))]
    public void Sin_Double(double x, double expected) => Approx(expected, math.sin(x));

    [Theory]
    [MemberData(nameof(SinDoubleData))]
    public void Sin_Double2(double x, double expected) =>
        Approx(expected, math.sin(new double2(x, x)).x);

    [Theory]
    [MemberData(nameof(SinDoubleData))]
    public void Sin_Double3(double x, double expected) =>
        Approx(expected, math.sin(new double3(x, x, x)).x);

    [Theory]
    [MemberData(nameof(SinDoubleData))]
    public void Sin_Double4(double x, double expected) =>
        Approx(expected, math.sin(new double4(x, x, x, x)).x);

    [Theory]
    [InlineData(0.0f, 0.0f)]
    [InlineData(1.2f, 0.9320391f)]
    [InlineData(-1.2f, -0.9320391f)]
    [InlineData(-1000000f, 0.3499935f)]
    [InlineData(1000000f, -0.3499935f)]
    public void Sin_Float(float x, float expected) => Approx(expected, math.sin(x));

    [Theory]
    [InlineData(0.0f, 0.0f)]
    [InlineData(1.2f, 0.9320391f)]
    public void Sin_Float2(float x, float expected) =>
        Approx(expected, math.sin(new float2(x, x)).x);

    [Theory]
    [InlineData(-1.2f, -0.9320391f)]
    public void Sin_Float3(float x, float expected) =>
        Approx(expected, math.sin(new float3(x, x, x)).x);

    [Theory]
    [InlineData(1.2f, 0.9320391f)]
    public void Sin_Float4(float x, float expected) =>
        Approx(expected, math.sin(new float4(x, x, x, x)).x);

    [Fact]
    public void Sin_InfNaN_ReturnsNaN()
    {
        Assert.True(double.IsNaN(math.sin(double.NegativeInfinity)));
        Assert.True(double.IsNaN(math.sin(double.NaN)));
        Assert.True(double.IsNaN(math.sin(double.PositiveInfinity)));
        Assert.True(float.IsNaN(math.sin(float.NegativeInfinity)));
        Assert.True(float.IsNaN(math.sin(float.PositiveInfinity)));
    }

    public static TheoryData<double, double> CosDoubleData => new()
    {
        { -1000000.0, 0.93675212753314479 },
        { -1.2, 0.36235775447667358 },
        { 0.0, 1.0 },
        { 1.2, 0.36235775447667358 },
        { 1000000.0, 0.93675212753314479 },
    };

    [Theory]
    [MemberData(nameof(CosDoubleData))]
    public void Cos_Double(double x, double expected) => Approx(expected, math.cos(x));

    [Theory]
    [MemberData(nameof(CosDoubleData))]
    public void Cos_Double2(double x, double expected) =>
        Approx(expected, math.cos(new double2(x, x)).x);

    [Theory]
    [MemberData(nameof(CosDoubleData))]
    public void Cos_Double3(double x, double expected) =>
        Approx(expected, math.cos(new double3(x, x, x)).x);

    [Theory]
    [MemberData(nameof(CosDoubleData))]
    public void Cos_Double4(double x, double expected) =>
        Approx(expected, math.cos(new double4(x, x, x, x)).x);

    [Theory]
    [InlineData(0.0f, 1.0f)]
    [InlineData(1.2f, 0.3623578f)]
    [InlineData(-1.2f, 0.3623578f)]
    public void Cos_Float(float x, float expected) => Approx(expected, math.cos(x));

    [Theory]
    [InlineData(0.0f, 1.0f)]
    public void Cos_Float2(float x, float expected) =>
        Approx(expected, math.cos(new float2(x, x)).x);

    [Theory]
    [InlineData(1.2f, 0.3623578f)]
    public void Cos_Float3(float x, float expected) =>
        Approx(expected, math.cos(new float3(x, x, x)).x);

    [Theory]
    [InlineData(-1.2f, 0.3623578f)]
    public void Cos_Float4(float x, float expected) =>
        Approx(expected, math.cos(new float4(x, x, x, x)).x);

    [Fact]
    public void Cos_InfNaN_ReturnsNaN()
    {
        Assert.True(double.IsNaN(math.cos(double.NegativeInfinity)));
        Assert.True(double.IsNaN(math.cos(double.PositiveInfinity)));
        Assert.True(float.IsNaN(math.cos(float.NegativeInfinity)));
    }

    public static TheoryData<double, double> TanDoubleData => new()
    {
        { -1000000.0, 0.373624453987599 },
        { -1.2, -2.57215162212632 },
        { 0.0, 0.0 },
        { 1.2, 2.57215162212632 },
        { 1000000.0, -0.373624453987599 },
    };

    [Theory]
    [MemberData(nameof(TanDoubleData))]
    public void Tan_Double(double x, double expected) => Approx(expected, math.tan(x), 1e-4);

    [Theory]
    [MemberData(nameof(TanDoubleData))]
    public void Tan_Double2(double x, double expected) =>
        Approx(expected, math.tan(new double2(x, x)).x, 1e-4);

    [Theory]
    [MemberData(nameof(TanDoubleData))]
    public void Tan_Double3(double x, double expected) =>
        Approx(expected, math.tan(new double3(x, x, x)).x, 1e-4);

    [Theory]
    [MemberData(nameof(TanDoubleData))]
    public void Tan_Double4(double x, double expected) =>
        Approx(expected, math.tan(new double4(x, x, x, x)).x, 1e-4);

    [Theory]
    [InlineData(0.0f, 0.0f)]
    [InlineData(1.2f, 2.5721517f)]
    [InlineData(-1.2f, -2.5721517f)]
    public void Tan_Float(float x, float expected) => Approx(expected, math.tan(x));

    [Theory]
    [InlineData(0.0f, 0.0f)]
    public void Tan_Float2(float x, float expected) =>
        Approx(expected, math.tan(new float2(x, x)).x);

    [Theory]
    [InlineData(0.0f, 0.0f)]
    [InlineData(0.5f, 0.5235988f)]
    [InlineData(-0.5f, -0.5235988f)]
    [InlineData(1.0f, 1.5707964f)]
    [InlineData(-1.0f, -1.5707964f)]
    public void Asin_Float(float x, float expected) => Approx(expected, math.asin(x));

    [Theory]
    [InlineData(0.0f, 0.0f)]
    public void Asin_Float2(float x, float expected) =>
        Approx(expected, math.asin(new float2(x, x)).x);

    [Theory]
    [InlineData(0.0, 0.0)]
    [InlineData(0.5, 0.52359877559829893)]
    [InlineData(-0.5, -0.52359877559829893)]
    [InlineData(1.0, 1.5707963267948966)]
    [InlineData(-1.0, -1.5707963267948966)]
    public void Asin_Double(double x, double expected) => Approx(expected, math.asin(x));

    [Theory]
    [InlineData(0.0, 0.0)]
    public void Asin_Double2(double x, double expected) =>
        Approx(expected, math.asin(new double2(x, x)).x);

    [Theory]
    [InlineData(0.0f, 1.5707964f)]
    [InlineData(0.5f, 1.0471976f)]
    [InlineData(-0.5f, 2.0943952f)]
    [InlineData(1.0f, 0.0f)]
    [InlineData(-1.0f, 3.1415927f)]
    public void Acos_Float(float x, float expected) => Approx(expected, math.acos(x));

    [Theory]
    [InlineData(0.0f, 1.5707964f)]
    public void Acos_Float2(float x, float expected) =>
        Approx(expected, math.acos(new float2(x, x)).x);

    [Theory]
    [InlineData(0.0, 1.5707963267948966)]
    [InlineData(0.5, 1.0471975511965979)]
    [InlineData(-0.5, 2.0943951023931957)]
    [InlineData(1.0, 0.0)]
    [InlineData(-1.0, 3.1415926535897931)]
    public void Acos_Double(double x, double expected) => Approx(expected, math.acos(x));

    public static TheoryData<double, double> AtanDoubleData => new()
    {
        { -1000000.0, -1.570795326794897 },
        { -1.2, -0.8760580505981934 },
        { 0.0, 0.0 },
        { 1.2, 0.8760580505981934 },
        { 1000000.0, 1.570795326794897 },
        { double.NegativeInfinity, -1.570796326794897 },
        { double.PositiveInfinity, 1.570796326794897 },
    };

    [Theory]
    [MemberData(nameof(AtanDoubleData))]
    public void Atan_Double(double x, double expected) => Approx(expected, math.atan(x));

    [Theory]
    [MemberData(nameof(AtanDoubleData))]
    public void Atan_Double2(double x, double expected) =>
        Approx(expected, math.atan(new double2(x, x)).x);

    [Theory]
    [MemberData(nameof(AtanDoubleData))]
    public void Atan_Double3(double x, double expected) =>
        Approx(expected, math.atan(new double3(x, x, x)).x);

    [Theory]
    [MemberData(nameof(AtanDoubleData))]
    public void Atan_Double4(double x, double expected) =>
        Approx(expected, math.atan(new double4(x, x, x, x)).x);

    [Theory]
    [InlineData(0.0f, 0.0f)]
    [InlineData(1.2f, 0.87605804f)]
    [InlineData(-1.2f, -0.87605804f)]
    public void Atan_Float(float x, float expected) => Approx(expected, math.atan(x));

    [Theory]
    [InlineData(0.0f, 0.0f)]
    public void Atan_Float2(float x, float expected) =>
        Approx(expected, math.atan(new float2(x, x)).x);

    public static TheoryData<double, double, double> Atan2DoubleData => new()
    {
        { 3.1, 2.4, 0.91199029067742043 },
        { 3.1, -2.4, 2.2296023629123729 },
        { -3.1, 2.4, -0.91199029067742043 },
        { -3.1, -2.4, -2.2296023629123729 },
        { 0.0, 0.0, 0.0 },
        { 1.0, double.NegativeInfinity, 3.1415926535897931 },
        { 1.0, double.PositiveInfinity, 0.0 },
        { double.NegativeInfinity, 1.0, -1.5707963267948966 },
        { double.PositiveInfinity, 1.0, 1.5707963267948966 },
    };

    [Theory]
    [MemberData(nameof(Atan2DoubleData))]
    public void Atan2_Double(double y, double x, double expected) =>
        Approx(expected, math.atan2(y, x));

    [Theory]
    [MemberData(nameof(Atan2DoubleData))]
    public void Atan2_Double2(double y, double x, double expected) =>
        Approx(expected, math.atan2(new double2(y, y), new double2(x, x)).x);

    [Theory]
    [InlineData(3.1f, 2.4f, 0.9119903f)]
    [InlineData(3.1f, -2.4f, 2.22960234f)]
    [InlineData(-3.1f, 2.4f, -0.9119903f)]
    [InlineData(-3.1f, -2.4f, -2.22960234f)]
    [InlineData(0.0f, 0.0f, 0.0f)]
    public void Atan2_Float(float y, float x, float expected) =>
        Approx(expected, math.atan2(y, x));

    [Fact]
    public void Atan2_NaN_ReturnsNaN()
    {
        Assert.True(double.IsNaN(math.atan2(1.0, double.NaN)));
        Assert.True(double.IsNaN(math.atan2(double.NaN, 1.0)));
        Assert.True(double.IsNaN(math.atan2(double.NaN, double.NaN)));
    }

    public static TheoryData<double, double> SinhDoubleData => new()
    {
        { -2.0, -3.626860407847018 },
        { -1.2, -1.509461355412173 },
        { 0.0, 0.0 },
        { 1.2, 1.509461355412173 },
        { 2.0, 3.626860407847018 },
    };

    [Theory]
    [MemberData(nameof(SinhDoubleData))]
    public void Sinh_Double(double x, double expected) => Approx(expected, math.sinh(x));

    [Theory]
    [MemberData(nameof(SinhDoubleData))]
    public void Sinh_Double2(double x, double expected) =>
        Approx(expected, math.sinh(new double2(x, x)).x);

    [Theory]
    [MemberData(nameof(SinhDoubleData))]
    public void Sinh_Double3(double x, double expected) =>
        Approx(expected, math.sinh(new double3(x, x, x)).x);

    [Theory]
    [MemberData(nameof(SinhDoubleData))]
    public void Sinh_Double4(double x, double expected) =>
        Approx(expected, math.sinh(new double4(x, x, x, x)).x);

    [Theory]
    [InlineData(0.0f, 0.0f)]
    [InlineData(1.2f, 1.5094614f)]
    [InlineData(-1.2f, -1.5094614f)]
    public void Sinh_Float(float x, float expected) => Approx(expected, math.sinh(x));

    [Theory]
    [InlineData(0.0f, 0.0f)]
    public void Sinh_Float2(float x, float expected) =>
        Approx(expected, math.sinh(new float2(x, x)).x);

    public static TheoryData<double, double> CoshDoubleData => new()
    {
        { -2.0, 3.7621956910836314 },
        { -1.2, 1.81065556732437 },
        { 0.0, 1.0 },
        { 1.2, 1.81065556732437 },
        { 2.0, 3.7621956910836314 },
    };

    [Theory]
    [MemberData(nameof(CoshDoubleData))]
    public void Cosh_Double(double x, double expected) => Approx(expected, math.cosh(x));

    [Theory]
    [MemberData(nameof(CoshDoubleData))]
    public void Cosh_Double2(double x, double expected) =>
        Approx(expected, math.cosh(new double2(x, x)).x);

    [Theory]
    [MemberData(nameof(CoshDoubleData))]
    public void Cosh_Double3(double x, double expected) =>
        Approx(expected, math.cosh(new double3(x, x, x)).x);

    [Theory]
    [MemberData(nameof(CoshDoubleData))]
    public void Cosh_Double4(double x, double expected) =>
        Approx(expected, math.cosh(new double4(x, x, x, x)).x);

    [Theory]
    [InlineData(0.0f, 1.0f)]
    [InlineData(1.2f, 1.8106556f)]
    [InlineData(-1.2f, 1.8106556f)]
    public void Cosh_Float(float x, float expected) => Approx(expected, math.cosh(x));

    [Theory]
    [InlineData(0.0f, 1.0f)]
    public void Cosh_Float2(float x, float expected) =>
        Approx(expected, math.cosh(new float2(x, x)).x);

    public static TheoryData<double, double> TanhDoubleData => new()
    {
        { -2.0, -0.96402758007581688 },
        { -1.2, -0.83365460701215526 },
        { 0.0, 0.0 },
        { 1.2, 0.83365460701215526 },
        { 2.0, 0.96402758007581688 },
    };

    [Theory]
    [MemberData(nameof(TanhDoubleData))]
    public void Tanh_Double(double x, double expected) => Approx(expected, math.tanh(x));

    [Theory]
    [InlineData(0.0f, 0.0f)]
    [InlineData(1.2f, 0.8336546f)]
    [InlineData(-1.2f, -0.8336546f)]
    public void Tanh_Float(float x, float expected) => Approx(expected, math.tanh(x));

    [Theory]
    [InlineData(0.0f, 0.0f)]
    public void Tanh_Float2(float x, float expected) =>
        Approx(expected, math.tanh(new float2(x, x)).x);

    [Theory]
    [InlineData(0.0f, 0.0f, 1.0f)]
    [InlineData(0.5f, 0.47942555f, 0.87758255f)]
    [InlineData(1.2f, 0.9320391f, 0.3623578f)]
    [InlineData(-1.2f, -0.9320391f, 0.3623578f)]
    [InlineData(-1000000f, 0.3499935f, 0.936752141f)]
    [InlineData(1000000f, -0.3499935f, 0.936752141f)]
    public void SinCos_Float(float x, float expectedSin, float expectedCos)
    {
        math.sincos(x, out var s, out var c);
        Approx(expectedSin, s);
        Approx(expectedCos, c);
    }

    [Theory]
    [InlineData(0.5, 0.479425538604203, 0.8775825618903728)]
    [InlineData(1.2, 0.9320390859672264, 0.36235775447667357)]
    public void SinCos_Double(double x, double expectedSin, double expectedCos)
    {
        math.sincos(x, out var s, out var c);
        Approx(expectedSin, s);
        Approx(expectedCos, c);
    }

    [Fact]
    public void SinCos_Float_InfNaN_ReturnsNaN()
    {
        math.sincos(float.NegativeInfinity, out var s, out var c);
        Assert.True(float.IsNaN(s));
        Assert.True(float.IsNaN(c));
    }

    [Theory]
    [InlineData(0.0f, 0.0f, 1.0f)]
    public void SinCos_Float2(float x, float expectedSin, float expectedCos)
    {
        math.sincos(new float2(x, x), out var s, out var c);
        Approx(expectedSin, s.x);
        Approx(expectedCos, c.x);
    }

    [Fact]
    public void SinCos_Float3()
    {
        math.sincos(new float3(0.5f, 1.2f, -1.2f), out var s, out var c);
        Approx(0.47942555f, s.x);
        Approx(0.9320391f, s.y);
        Approx(-0.9320391f, s.z);
    }

    [Fact]
    public void SinCos_Float4()
    {
        math.sincos(new float4(0.0f, 0.5f, 1.2f, -1.2f), out var s, out var c);
        Approx(0.0f, s.x);
        Approx(0.47942555f, s.y);
        Approx(0.87758255f, c.y);
    }

    [Fact]
    public void SinCos_Double2()
    {
        math.sincos(new double2(0.0, 0.5), out var s, out var c);
        Approx(0.0, s.x);
        Approx(0.479425538604203, s.y);
        Approx(1.0, c.x);
        Approx(0.8775825618903728, c.y);
    }

    [Fact]
    public void SinCos_Double3()
    {
        math.sincos(new double3(0.0, 0.5, 1.2), out var s, out var c);
        Approx(0.0, s.x);
        Approx(0.479425538604203, s.y);
        Approx(0.9320390859672264, s.z);
    }

    [Fact]
    public void SinCos_Double4()
    {
        math.sincos(new double4(0.0, 0.5, 1.2, -1.2), out var s, out var c);
        Approx(0.0, s.x);
        Approx(-0.9320390859672264, s.w);
    }
}
