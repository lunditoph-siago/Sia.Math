using Sia.Math;
using static Sia.Math.Tests.TestValue;

namespace Sia.Math.Tests;

public class TestMathComparison
{
    public static TheoryData<int, int, int> MinMaxIntData => new()
    {
        { int.MinValue, int.MinValue, int.MinValue },
        { int.MinValue, -1, int.MinValue },
        { -1, int.MinValue, int.MinValue },
        { -1234, -3456, -3456 },
        { -3456, -1234, -3456 },
        { -1234, 3456, -1234 },
        { 3456, -1234, -1234 },
        { 1234, 3456, 1234 },
        { 3456, 1234, 1234 },
        { 1, int.MaxValue, 1 },
        { int.MaxValue, 1, 1 },
        { int.MaxValue, int.MinValue, int.MinValue },
        { int.MaxValue, int.MaxValue, int.MaxValue },
    };

    public static TheoryData<uint, uint, uint> MinMaxUIntData => new()
    {
        { 0u, 0u, 0u },
        { 0u, 1u, 0u },
        { 100u, 200u, 100u },
        { 200u, 100u, 100u },
        { 0u, uint.MaxValue, 0u },
        { uint.MaxValue, 0u, 0u },
        { uint.MaxValue, uint.MaxValue, uint.MaxValue },
    };

    public static TheoryData<double, double, double> MinMaxDoubleData => new()
    {
        { double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity },
        { double.NegativeInfinity, -1.0, double.NegativeInfinity },
        { -1.0, double.NegativeInfinity, double.NegativeInfinity },
        { -1234.56, -3456.7, -3456.7 },
        { -3456.7, -1234.56, -3456.7 },
        { -1234.56, 3456.7, -1234.56 },
        { 3456.7, -1234.56, -1234.56 },
        { 1234.56, 3456.7, 1234.56 },
        { 3456.7, 1234.56, 1234.56 },
        { 1.0, double.PositiveInfinity, 1.0 },
        { double.PositiveInfinity, 1.0, 1.0 },
        { double.PositiveInfinity, double.NegativeInfinity, double.NegativeInfinity },
        { double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity },
    };

    [Theory]
    [MemberData(nameof(MinMaxIntData))]
    public void Min_Int(int a, int b, int expected) => Assert.Equal(expected, math.min(a, b));

    [Theory]
    [MemberData(nameof(MinMaxIntData))]
    public void Min_Int2(int a, int b, int expected) =>
        Assert.Equal(expected, math.min(new int2(a, a), new int2(b, b)).x);

    [Theory]
    [MemberData(nameof(MinMaxIntData))]
    public void Min_Int3(int a, int b, int expected) =>
        Assert.Equal(expected, math.min(new int3(a, a, a), new int3(b, b, b)).x);

    [Theory]
    [MemberData(nameof(MinMaxIntData))]
    public void Min_Int4(int a, int b, int expected) =>
        Assert.Equal(expected, math.min(new int4(a, a, a, a), new int4(b, b, b, b)).x);

    [Theory]
    [MemberData(nameof(MinMaxUIntData))]
    public void Min_UInt(uint a, uint b, uint expected) => Assert.Equal(expected, math.min(a, b));

    [Theory]
    [MemberData(nameof(MinMaxUIntData))]
    public void Min_UInt2(uint a, uint b, uint expected) =>
        Assert.Equal(expected, math.min(new uint2(a, a), new uint2(b, b)).x);

    [Theory]
    [MemberData(nameof(MinMaxDoubleData))]
    public void Min_Double(double a, double b, double expected) => Assert.Equal(expected, math.min(a, b));

    [Fact]
    public void Min_Double_NaN_ReturnsOther()
    {
        Assert.Equal(2.3, math.min(2.3, double.NaN));
        Assert.Equal(2.3, math.min(double.NaN, 2.3));
    }

    [Fact]
    public void Min_Float3_NaN_Mixed()
    {
        var a = new float3(2.3f, float.NaN, 5.0f);
        var b = new float3(float.NaN, 4.1f, float.NaN);
        var r = math.min(a, b);
        Assert.Equal(2.3f, r.x);
        Assert.Equal(4.1f, r.y);
        Assert.Equal(5.0f, r.z);
    }

    [Theory]
    [MemberData(nameof(MinMaxIntData))]
    public void Max_Int(int a, int b, int _) =>
        Assert.Equal(global::System.Math.Max(a, b), math.max(a, b));

    [Theory]
    [InlineData(int.MinValue, int.MinValue, int.MinValue)]
    [InlineData(int.MinValue, -1, -1)]
    [InlineData(-1, int.MinValue, -1)]
    [InlineData(-1234, -3456, -1234)]
    [InlineData(-3456, -1234, -1234)]
    [InlineData(-1234, 3456, 3456)]
    [InlineData(3456, -1234, 3456)]
    [InlineData(1234, 3456, 3456)]
    [InlineData(3456, 1234, 3456)]
    [InlineData(1, int.MaxValue, int.MaxValue)]
    [InlineData(int.MaxValue, 1, int.MaxValue)]
    [InlineData(int.MaxValue, int.MinValue, int.MaxValue)]
    [InlineData(int.MaxValue, int.MaxValue, int.MaxValue)]
    public void Max_Int_Raw(int a, int b, int expected) => Assert.Equal(expected, math.max(a, b));

    [Theory]
    [InlineData(int.MinValue, -1, -1)]
    [InlineData(-1234, -3456, -1234)]
    [InlineData(1234, 3456, 3456)]
    [InlineData(1, int.MaxValue, int.MaxValue)]
    public void Max_Int2(int a, int b, int expected) =>
        Assert.Equal(expected, math.max(new int2(a, a), new int2(b, b)).x);

    [Theory]
    [InlineData(-3456, -1234, -1234)]
    [InlineData(int.MinValue, -1, -1)]
    public void Max_Int3(int a, int b, int expected) =>
        Assert.Equal(expected, math.max(new int3(a, a, a), new int3(b, b, b)).x);

    [Theory]
    [InlineData(int.MaxValue, 1, int.MaxValue)]
    [InlineData(-1234, 3456, 3456)]
    public void Max_Int4(int a, int b, int expected) =>
        Assert.Equal(expected, math.max(new int4(a, a, a, a), new int4(b, b, b, b)).x);

    [Theory]
    [InlineData(100u, 200u, 200u)]
    [InlineData(200u, 100u, 200u)]
    [InlineData(0u, uint.MaxValue, uint.MaxValue)]
    [InlineData(uint.MaxValue, 0u, uint.MaxValue)]
    public void Max_UInt(uint a, uint b, uint expected) => Assert.Equal(expected, math.max(a, b));

    [Theory]
    [InlineData(100u, 200u, 200u)]
    [InlineData(uint.MaxValue, 0u, uint.MaxValue)]
    public void Max_UInt2(uint a, uint b, uint expected) =>
        Assert.Equal(expected, math.max(new uint2(a, a), new uint2(b, b)).x);

    [Theory]
    [InlineData(double.NegativeInfinity, -1.0, -1.0)]
    [InlineData(-1234.56, -3456.7, -1234.56)]
    [InlineData(-1234.56, 3456.7, 3456.7)]
    [InlineData(1234.56, 3456.7, 3456.7)]
    [InlineData(1.0, double.PositiveInfinity, double.PositiveInfinity)]
    [InlineData(double.PositiveInfinity, double.NegativeInfinity, double.PositiveInfinity)]
    public void Max_Double(double a, double b, double expected) => Assert.Equal(expected, math.max(a, b));

    [Fact]
    public void Max_Double_NaN_ReturnsOther()
    {
        Assert.Equal(2.3, math.max(2.3, double.NaN));
        Assert.Equal(2.3, math.max(double.NaN, 2.3));
    }

    [Fact]
    public void Max_Float4_NaN_Mixed()
    {
        var a = new float4(2.3f, float.NaN, 5.0f, float.NaN);
        var b = new float4(float.NaN, 4.1f, float.NaN, float.NaN);
        var r = math.max(a, b);
        Assert.Equal(2.3f, r.x);
        Assert.Equal(4.1f, r.y);
        Assert.Equal(5.0f, r.z);
        Assert.True(float.IsNaN(r.w));
    }

    [Theory]
    [InlineData(1.0f, 2.0f, false, 1.0f)]
    [InlineData(1.0f, 2.0f, true, 2.0f)]
    [InlineData(0.0f, 0.0f, false, 0.0f)]
    public void Select_Float(float a, float b, bool test, float expected) =>
        Assert.Equal(expected, math.select(a, b, test));

    [Theory]
    [InlineData(1.0f, 2.0f, true, true, 2.0f, 2.0f)]
    [InlineData(1.0f, 2.0f, true, false, 2.0f, 1.0f)]
    [InlineData(1.0f, 2.0f, false, false, 1.0f, 1.0f)]
    public void Select_Float2(float a, float b, bool tx, bool ty, float ex, float ey) =>
        Assert.Equal(new float2(ex, ey), math.select(new float2(a, a), new float2(b, b), new bool2(tx, ty)));

    [Theory]
    [InlineData(10, 99, false, 10)]
    [InlineData(10, 99, true, 99)]
    public void Select_Int(int a, int b, bool test, int expected) =>
        Assert.Equal(expected, math.select(a, b, test));

    [Fact]
    public void Select_Int3()
    {
        var r = math.select(new int3(10, 10, 10), new int3(99, 99, 99), new bool3(true, true, false));
        Assert.Equal(new int3(99, 99, 10), r);
    }

    [Theory]
    [InlineData(1.0, 2.0, false, 1.0)]
    [InlineData(1.0, 2.0, true, 2.0)]
    public void Select_Double(double a, double b, bool test, double expected) =>
        Assert.Equal(expected, math.select(a, b, test));

    [Theory]
    [InlineData(0f, 0f, false)]
    [InlineData(0f, 1f, true)]
    [InlineData(1f, 0f, true)]
    [InlineData(-1f, 0f, true)]
    public void Any_Float2(float x, float y, bool expected) =>
        Assert.Equal(expected, math.any(new float2(x, y)));

    [Theory]
    [InlineData(0, 0, 0, false)]
    [InlineData(0, 1, 0, true)]
    [InlineData(1, 0, 0, true)]
    public void Any_Int3(int x, int y, int z, bool expected) =>
        Assert.Equal(expected, math.any(new int3(x, y, z)));

    [Theory]
    [InlineData(true, false, true)]
    [InlineData(false, true, true)]
    [InlineData(false, false, false)]
    public void Any_Bool2(bool x, bool y, bool expected) => Assert.Equal(expected, math.any(new bool2(x, y)));

    [Theory]
    [InlineData(true, false, false, true)]
    [InlineData(false, false, false, false)]
    public void Any_Bool3(bool x, bool y, bool z, bool expected) =>
        Assert.Equal(expected, math.any(new bool3(x, y, z)));

    [Theory]
    [InlineData(false, false, false, false, false)]
    [InlineData(false, false, false, true, true)]
    public void Any_Bool4(bool x, bool y, bool z, bool w, bool expected) =>
        Assert.Equal(expected, math.any(new bool4(x, y, z, w)));

    [Theory]
    [InlineData(1f, 1f, true)]
    [InlineData(1f, 0f, false)]
    [InlineData(0f, 1f, false)]
    [InlineData(0f, 0f, false)]
    public void All_Float2(float x, float y, bool expected) =>
        Assert.Equal(expected, math.all(new float2(x, y)));

    [Theory]
    [InlineData(1, 1, 1, true)]
    [InlineData(1, 0, 1, false)]
    [InlineData(0, 1, 1, false)]
    [InlineData(0, 0, 0, false)]
    public void All_Int3(int x, int y, int z, bool expected) =>
        Assert.Equal(expected, math.all(new int3(x, y, z)));

    [Theory]
    [InlineData(true, true, true)]
    [InlineData(true, false, false)]
    [InlineData(false, true, false)]
    [InlineData(false, false, false)]
    public void All_Bool2(bool x, bool y, bool expected) => Assert.Equal(expected, math.all(new bool2(x, y)));

    [Theory]
    [InlineData(true, true, true, true)]
    [InlineData(true, true, false, false)]
    public void All_Bool3(bool x, bool y, bool z, bool expected) =>
        Assert.Equal(expected, math.all(new bool3(x, y, z)));

    [Theory]
    [InlineData(true, true, true, true, true)]
    [InlineData(true, true, true, false, false)]
    public void All_Bool4(bool x, bool y, bool z, bool w, bool expected) =>
        Assert.Equal(expected, math.all(new bool4(x, y, z, w)));

    [Fact]
    public void Any_Double2() => Assert.True(math.any(new double2(0.0, 1.0)));

    [Fact]
    public void Any_UInt3() => Assert.True(math.any(new uint3(0u, 0u, 1u)));

    [Fact]
    public void All_Double2() => Assert.True(math.all(new double2(1.0, 2.0)));

    [Fact]
    public void All_UInt4() => Assert.False(math.all(new uint4(1u, 0u, 3u, 4u)));
}
