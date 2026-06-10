using Sia.Math;
using static Sia.Math.Tests.TestValue;
using static Sia.Math.Tests.TestAssert;

namespace Sia.Math.Tests;

public class TestMathInterpolation
{
    public static TheoryData<double, double, double, double> LerpDoubleData => new()
    {
        { -123.45, 439.43, -1.5, -967.77 },
        { -123.45, 439.43, 0.5, 157.99 },
        { -123.45, 439.43, 5.5, 2972.39 },
    };

    [Theory]
    [MemberData(nameof(LerpDoubleData))]
    public void Lerp_Double(double a, double b, double t, double expected) =>
        Approx(expected, math.lerp(a, b, t), 1e-8);

    [Theory]
    [MemberData(nameof(LerpDoubleData))]
    public void Lerp_Double2_ScalarS(double a, double b, double t, double expected) =>
        Approx(expected, math.lerp(new double2(a, a), new double2(b, b), t).x, 1e-8);

    [Theory]
    [MemberData(nameof(LerpDoubleData))]
    public void Lerp_Double2_VectorS(double a, double b, double t, double expected) =>
        Approx(expected, math.lerp(new double2(a, a), new double2(b, b), new double2(t, t)).x, 1e-8);

    [Theory]
    [MemberData(nameof(LerpDoubleData))]
    public void Lerp_Double3(double a, double b, double t, double expected) =>
        Approx(expected, math.lerp(new double3(a, a, a), new double3(b, b, b), t).x, 1e-8);

    [Theory]
    [MemberData(nameof(LerpDoubleData))]
    public void Lerp_Double4(double a, double b, double t, double expected) =>
        Approx(expected, math.lerp(new double4(a, a, a, a), new double4(b, b, b, b), t).x, 1e-8);

    [Fact]
    public void Lerp_Double_NaN_t_ReturnsNaN() =>
        Assert.True(double.IsNaN(math.lerp(-123.45, 439.43, double.NaN)));

    [Theory]
    [InlineData(0.0f, 1.0f, 0.5f, 0.5f)]
    [InlineData(-123.45f, 439.43f, 0.5f, 157.99f)]
    public void Lerp_Float(float a, float b, float t, float expected) =>
        Approx(expected, math.lerp(a, b, t));

    [Theory]
    [InlineData(0.0f, 1.0f, 0.5f, 0.5f)]
    public void Lerp_Float2(float a, float b, float t, float expected) =>
        Approx(expected, math.lerp(new float2(a, a), new float2(b, b), new float2(t, t)).x);

    public static TheoryData<double, double, double, double> UnlerpDoubleData => new()
    {
        { -123.45, 439.43, -254.3, -0.23246517907902217 },
        { -123.45, 439.43, 0.0, 0.21931850483229107 },
        { -123.45, 439.43, 632.1, 1.3422932063672541 },
        { 439.43, -123.45, -254.3, 1.2324651790790221 },
        { 439.43, -123.45, 0.0, 0.7806814951677089 },
        { 439.43, -123.45, 632.1, -0.3422932063672541 },
    };

    [Theory]
    [MemberData(nameof(UnlerpDoubleData))]
    public void Unlerp_Double(double a, double b, double x, double expected) =>
        Approx(expected, math.unlerp(a, b, x), 1e-6);

    [Theory]
    [InlineData(-123.45, 439.43, -254.3, -0.23246517907902217)]
    public void Unlerp_Double2(double a, double b, double x, double expected) =>
        Approx(expected, math.unlerp(new double2(a, a), new double2(b, b), new double2(x, x)).x, 1e-6);

    public static TheoryData<double, double, double, double, double, double> RemapDoubleData => new()
    {
        { -123.45, 439.43, 541.3, 631.5, -200.0, 529.03306921546334 },
        { -123.45, 439.43, 541.3, 631.5, -100.0, 545.057799175668 },
        { -123.45, 439.43, 541.3, 631.5, 500.0, 641.206178936896 },
        { 439.43, -123.45, 541.3, 631.5, -200.0, 643.76693078453673 },
        { 439.43, -123.45, 541.3, 631.5, -100.0, 627.742200824332 },
        { 439.43, -123.45, 541.3, 631.5, 500.0, 531.59382106310409 },
    };

    [Theory]
    [MemberData(nameof(RemapDoubleData))]
    public void Remap_Double(double a, double b, double c, double d, double x, double expected) =>
        Approx(expected, math.remap(a, b, c, d, x), 1e-6);

    [Theory]
    [MemberData(nameof(RemapDoubleData))]
    public void Remap_Double2(double a, double b, double c, double d, double x, double expected) =>
        Approx(expected, math.remap(new double2(a, a), new double2(b, b), new double2(c, c), new double2(d, d), new double2(x, x)).x, 1e-6);

    public static TheoryData<int, int, int, int> ClampIntData => new()
    {
        { int.MinValue, -123, 439, -123 },
        { -254, -123, 439, -123 },
        { 246, -123, 439, 246 },
        { 632, -123, 439, 439 },
        { -254, 439, -123, 439 },
        { 246, 439, -123, 439 },
        { 632, 439, -123, 439 },
        { int.MaxValue, -123, 439, 439 },
    };

    [Theory]
    [MemberData(nameof(ClampIntData))]
    public void Clamp_Int(int v, int a, int b, int expected) => Assert.Equal(expected, math.clamp(v, a, b));

    [Theory]
    [MemberData(nameof(ClampIntData))]
    public void Clamp_Int2(int v, int a, int b, int expected) =>
        Assert.Equal(expected, math.clamp(new int2(v, v), new int2(a, a), new int2(b, b)).x);

    public static TheoryData<uint, uint, uint, uint> ClampUIntData => new()
    {
        { 0u, 123u, 439u, 123u },
        { 54u, 123u, 439u, 123u },
        { 246u, 123u, 439u, 246u },
        { 632u, 123u, 439u, 439u },
        { uint.MaxValue, 123u, 439u, 439u },
    };

    [Theory]
    [MemberData(nameof(ClampUIntData))]
    public void Clamp_UInt(uint v, uint a, uint b, uint expected) => Assert.Equal(expected, math.clamp(v, a, b));

    public static TheoryData<double, double, double, double> ClampDoubleData => new()
    {
        { double.NegativeInfinity, -123.45, 439.43, -123.45 },
        { -254.3, -123.45, 439.43, -123.45 },
        { 246.3, -123.45, 439.43, 246.3 },
        { 632.1, -123.45, 439.43, 439.43 },
        { -254.3, 439.43, -123.45, 439.43 },
        { 632.1, 439.43, -123.45, 439.43 },
        { double.PositiveInfinity, -123.45, 439.43, 439.43 },
    };

    [Theory]
    [MemberData(nameof(ClampDoubleData))]
    public void Clamp_Double(double v, double a, double b, double expected) =>
        Assert.Equal(expected, math.clamp(v, a, b));

    [Theory]
    [MemberData(nameof(ClampDoubleData))]
    public void Clamp_Double2(double v, double a, double b, double expected) =>
        Assert.Equal(expected, math.clamp(new double2(v, v), new double2(a, a), new double2(b, b)).x);

    public static TheoryData<double, double> SaturateDoubleData => new()
    {
        { double.NegativeInfinity, 0.0 },
        { -123.45, 0.0 },
        { 0.0, 0.0 },
        { 0.5, 0.5 },
        { 1.0, 1.0 },
        { 123.45, 1.0 },
        { double.PositiveInfinity, 1.0 },
    };

    [Theory]
    [MemberData(nameof(SaturateDoubleData))]
    public void Saturate_Double(double x, double expected) => Assert.Equal(expected, math.saturate(x));

    [Theory]
    [InlineData(0.0f, 0.0f)]
    [InlineData(0.5f, 0.5f)]
    [InlineData(-1.0f, 0.0f)]
    [InlineData(2.0f, 1.0f)]
    public void Saturate_Float(float x, float expected) => Assert.Equal(expected, math.saturate(x));

    [Theory]
    [InlineData(-123.45f, -1f)]
    [InlineData(-1E-20f, -1f)]
    [InlineData(0f, 0f)]
    [InlineData(1E-10f, 1f)]
    [InlineData(123.45f, 1f)]
    [InlineData(float.NegativeInfinity, -1f)]
    [InlineData(float.PositiveInfinity, 1f)]
    public void Sign_Float(float x, float expected) => Assert.Equal(expected, math.sign(x));

    [Theory]
    [InlineData(-123.45f, -1f)]
    public void Sign_Float2(float x, float expected) =>
        Assert.Equal(expected, math.sign(new float2(x, x)).x);

    [Theory]
    [InlineData(-123.45, -1.0)]
    [InlineData(0.0, 0.0)]
    [InlineData(123.45, 1.0)]
    [InlineData(double.NegativeInfinity, -1.0)]
    [InlineData(double.PositiveInfinity, 1.0)]
    public void Sign_Double(double x, double expected) => Assert.Equal(expected, math.sign(x));

    [Theory]
    [InlineData(-123.45, -1.0)]
    public void Sign_Double2(double x, double expected) =>
        Assert.Equal(expected, math.sign(new double2(x, x)).x);

    [Fact]
    public void Sign_Float_NaN_ReturnsZero() => Assert.Equal(0f, math.sign(SignedFloatQNaN()));

    public static TheoryData<double, double> RadiansDoubleData => new()
    {
        { -123.45, -2.15460896158699986 },
        { 0.0, 0.0 },
        { 123.45, 2.15460896158699986 },
    };

    [Theory]
    [MemberData(nameof(RadiansDoubleData))]
    public void Radians_Double(double x, double expected) =>
        Approx(expected, math.radians(x), 1e-12);

    [Theory]
    [MemberData(nameof(RadiansDoubleData))]
    public void Radians_Double2(double x, double expected) =>
        Approx(expected, math.radians(new double2(x, x)).x, 1e-12);

    [Theory]
    [MemberData(nameof(RadiansDoubleData))]
    public void Radians_Double3(double x, double expected) =>
        Approx(expected, math.radians(new double3(x, x, x)).x, 1e-12);

    [Theory]
    [MemberData(nameof(RadiansDoubleData))]
    public void Radians_Double4(double x, double expected) =>
        Approx(expected, math.radians(new double4(x, x, x, x)).x, 1e-12);

    [Theory]
    [InlineData(0.0f, 0.0f)]
    [InlineData(180.0f, 3.1415927f)]
    public void Radians_Float(float x, float expected) => Approx(expected, math.radians(x));

    [Theory]
    [InlineData(0.0f, 0.0f)]
    public void Radians_Float2(float x, float expected) =>
        Approx(expected, math.radians(new float2(x, x)).x);

    public static TheoryData<double, double> DegreesDoubleData => new()
    {
        { -123.45, -7073.1639808900125 },
        { 0.0, 0.0 },
        { 123.45, 7073.1639808900125 },
    };

    [Theory]
    [MemberData(nameof(DegreesDoubleData))]
    public void Degrees_Double(double x, double expected) =>
        Approx(expected, math.degrees(x), 1e-6);

    [Theory]
    [MemberData(nameof(DegreesDoubleData))]
    public void Degrees_Double2(double x, double expected) =>
        Approx(expected, math.degrees(new double2(x, x)).x, 1e-6);

    [Theory]
    [MemberData(nameof(DegreesDoubleData))]
    public void Degrees_Double3(double x, double expected) =>
        Approx(expected, math.degrees(new double3(x, x, x)).x, 1e-6);

    [Theory]
    [MemberData(nameof(DegreesDoubleData))]
    public void Degrees_Double4(double x, double expected) =>
        Approx(expected, math.degrees(new double4(x, x, x, x)).x, 1e-6);

    [Theory]
    [InlineData(0.0f, 0.0f)]
    [InlineData(3.1415927f, 180.0f)]
    public void Degrees_Float(float x, float expected) => Approx(expected, math.degrees(x));

    [Theory]
    [InlineData(0.0f, 0.0f)]
    public void Degrees_Float2(float x, float expected) =>
        Approx(expected, math.degrees(new float2(x, x)).x);

    [Theory]
    [InlineData(-123.45f, -200f, 0f)]
    [InlineData(-123.45f, 200f, 1f)]
    [InlineData(123.45f, -200f, 0f)]
    [InlineData(123.45f, 200f, 1f)]
    [InlineData(-123.45f, float.NaN, 0f)]
    [InlineData(float.NaN, -200f, 0f)]
    public void Step_Float(float y, float x, float expected) => Assert.Equal(expected, math.step(y, x));

    [Theory]
    [InlineData(-123.45f, -200f, 0f)]
    public void Step_Float2(float y, float x, float expected) =>
        Assert.Equal(expected, math.step(new float2(y, y), new float2(x, x)).x);

    [Theory]
    [InlineData(-123.45, -200.0, 0.0)]
    [InlineData(-123.45, 200.0, 1.0)]
    [InlineData(123.45, -200.0, 0.0)]
    [InlineData(123.45, 200.0, 1.0)]
    public void Step_Double(double y, double x, double expected) => Assert.Equal(expected, math.step(y, x));

    [Theory]
    [InlineData(-123.45f, 345.6f, -200f, 0f)]
    [InlineData(-123.45f, 345.6f, -100f, 0.00724848127f)]
    [InlineData(-123.45f, 345.6f, 400f, 1f)]
    [InlineData(345.6f, -123.45f, -200f, 1f)]
    [InlineData(345.6f, -123.45f, -100f, 0.992751539f)]
    [InlineData(345.6f, -123.45f, 400f, 0f)]
    public void SmoothStep_Float(float a, float b, float x, float expected) =>
        Approx(expected, math.smoothstep(a, b, x));

    [Theory]
    [InlineData(-123.45f, 345.6f, 400f, 1f)]
    public void SmoothStep_Float2(float a, float b, float x, float expected) =>
        Approx(expected, math.smoothstep(new float2(a, a), new float2(b, b), new float2(x, x)).x);

    [Theory]
    [InlineData(-123.45, 345.6, -100.0, 0.0072484810488798995)]
    [InlineData(-123.45, 345.6, 400.0, 1.0)]
    [InlineData(345.6, -123.45, -100.0, 0.99275151895112013)]
    [InlineData(345.6, -123.45, 400.0, 0.0)]
    public void SmoothStep_Double(double a, double b, double x, double expected) =>
        Approx(expected, math.smoothstep(a, b, x));

    [Theory]
    [InlineData(1234, 5678, 91011, 7097663)]
    [InlineData(1234, 5678, -91011, 6915641)]
    [InlineData(1234, -5678, 91011, -6915641)]
    [InlineData(-1234, -5678, 91011, 7097663)]
    [InlineData(-1234, -5678, -91011, 6915641)]
    public void Mad_Int(int a, int b, int c, int expected) => Assert.Equal(expected, math.mad(a, b, c));

    [Theory]
    [InlineData(1234, 5678, 91011, 7097663)]
    public void Mad_Int2(int a, int b, int c, int expected) =>
        Assert.Equal(expected, math.mad(new int2(a, a), new int2(b, b), new int2(c, c)).x);

    [Theory]
    [InlineData(1234u, 5678u, 91011u, 7097663u)]
    [InlineData(98765u, 56789u, 91011u, 1313889300u)]
    public void Mad_UInt(uint a, uint b, uint c, uint expected) => Assert.Equal(expected, math.mad(a, b, c));

    [Theory]
    [InlineData(-123.45f, 345.6f, 4.321f, -42660f)]
    public void Mad_Float(float a, float b, float c, float expected) =>
        Approx(expected, math.mad(a, b, c));

    [Theory]
    [InlineData(-123.45, 345.6, 4.321, -42659.999)]
    public void Mad_Double(double a, double b, double c, double expected) =>
        Approx(expected, math.mad(a, b, c), 1e-3);

    [Theory]
    [InlineData(-323.4f, -123.6f, -76.2f)]
    [InlineData(-323.4f, 123.6f, -76.2f)]
    [InlineData(323.4f, -123.6f, 76.2f)]
    [InlineData(323.4f, 123.6f, 76.2f)]
    public void Fmod_Float(float x, float y, float expected) => Assert.Equal(expected, math.fmod(x, y), 3);

    [Theory]
    [InlineData(-323.4f, -123.6f, -76.2f)]
    public void Fmod_Float2(float x, float y, float expected) =>
        Assert.Equal(expected, math.fmod(new float2(x, x), new float2(y, y)).x, 3);

    [Theory]
    [InlineData(-323.4, -123.6, -76.2)]
    [InlineData(323.4, 123.6, 76.2)]
    public void Fmod_Double(double x, double y, double expected) => Assert.Equal(expected, math.fmod(x, y), 3);

    [Theory]
    [InlineData(-323.4, -123.6, -76.2)]
    public void Fmod_Double2(double x, double y, double expected) =>
        Assert.Equal(expected, math.fmod(new double2(x, x), new double2(y, y)).x, 3);

    [Fact]
    public void Fmod_InfOrZero_ReturnsNaN()
    {
        Assert.True(float.IsNaN(math.fmod(float.PositiveInfinity, 1.0f)));
        Assert.True(float.IsNaN(math.fmod(1.0f, 0.0f)));
        Assert.True(float.IsNaN(math.fmod(0.0f, 0.0f)));
    }
}
