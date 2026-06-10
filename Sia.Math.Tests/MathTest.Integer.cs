using Sia.Math;
using static Sia.Math.Tests.TestValue;

namespace Sia.Math.Tests;

public class TestMathInteger
{
    public static TheoryData<int, int> AbsIntData => new()
    {
        { 0, 0 }, { -7, 7 }, { 11, 11 },
        { -2147483647, 2147483647 }, { -2147483648, -2147483648 },
    };

    [Theory]
    [MemberData(nameof(AbsIntData))]
    public void Abs_Int(int x, int expected) => Assert.Equal(expected, math.abs(x));

    [Theory]
    [MemberData(nameof(AbsIntData))]
    public void Abs_Int2(int x, int expected)
    {
        var r = math.abs(new int2(x, x));
        Assert.Equal(expected, r.x);
        Assert.Equal(expected, r.y);
    }

    [Theory]
    [MemberData(nameof(AbsIntData))]
    public void Abs_Int3(int x, int expected) =>
        Assert.Equal(expected, math.abs(new int3(x, x, x)).x);

    [Theory]
    [MemberData(nameof(AbsIntData))]
    public void Abs_Int4(int x, int expected) =>
        Assert.Equal(expected, math.abs(new int4(x, x, x, x)).x);

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 4)]
    [InlineData(1019642234, 1073741824)]
    public void CeilPow2_Int(int x, int expected) => Assert.Equal(expected, math.ceilpow2(x));

    [Theory]
    [InlineData(3, 4)]
    public void CeilPow2_Int2(int x, int expected) =>
        Assert.Equal(expected, math.ceilpow2(new int2(x, x)).x);

    [Theory]
    [InlineData(1, 1)]
    public void CeilPow2_Int3(int x, int expected) =>
        Assert.Equal(expected, math.ceilpow2(new int3(x, x, x)).x);

    [Theory]
    [InlineData(1019642234, 1073741824)]
    public void CeilPow2_Int4(int x, int expected) =>
        Assert.Equal(expected, math.ceilpow2(new int4(x, x, x, x)).x);

    [Theory]
    [InlineData(0u, 0u)]
    [InlineData(1u, 1u)]
    [InlineData(2u, 2u)]
    [InlineData(3u, 4u)]
    [InlineData(1019642234u, 1073741824u)]
    [InlineData(1823423423u, 2147483648u)]
    [InlineData(4294967295u, 0u)]
    public void CeilPow2_UInt(uint x, uint expected) => Assert.Equal(expected, math.ceilpow2(x));

    [Theory]
    [InlineData(3u, 4u)]
    public void CeilPow2_UInt2(uint x, uint expected) =>
        Assert.Equal(expected, math.ceilpow2(new uint2(x, x)).x);

    [Theory]
    [InlineData(4294967295u, 0u)]
    public void CeilPow2_UInt3(uint x, uint expected) =>
        Assert.Equal(expected, math.ceilpow2(new uint3(x, x, x)).x);

    [Theory]
    [InlineData(1823423423u, 2147483648u)]
    public void CeilPow2_UInt4(uint x, uint expected) =>
        Assert.Equal(expected, math.ceilpow2(new uint4(x, x, x, x)).x);

    [Theory]
    [InlineData(1, 0)]
    [InlineData(2, 1)]
    [InlineData(3, 1)]
    [InlineData(4, 2)]
    [InlineData(5, 2)]
    [InlineData(32767, 14)]
    [InlineData(32768, 15)]
    [InlineData(32769, 15)]
    [InlineData(2147483647, 30)]
    public void FloorLog2_Int(int x, int expected) => Assert.Equal(expected, math.floorlog2(x));

    [Theory]
    [InlineData(1, 0)]
    public void FloorLog2_Int2(int x, int expected) =>
        Assert.Equal(expected, math.floorlog2(new int2(x, x)).x);

    [Theory]
    [InlineData(32768, 15)]
    public void FloorLog2_Int3(int x, int expected) =>
        Assert.Equal(expected, math.floorlog2(new int3(x, x, x)).x);

    [Theory]
    [InlineData(2147483647, 30)]
    public void FloorLog2_Int4(int x, int expected) =>
        Assert.Equal(expected, math.floorlog2(new int4(x, x, x, x)).x);

    [Theory]
    [InlineData(1u, 0)]
    [InlineData(2u, 1)]
    [InlineData(3u, 1)]
    [InlineData(4u, 2)]
    [InlineData(32768u, 15)]
    public void FloorLog2_UInt(uint x, int expected) => Assert.Equal(expected, math.floorlog2(x));

    [Theory]
    [InlineData(1u, 0)]
    public void FloorLog2_UInt2(uint x, int expected) =>
        Assert.Equal(expected, math.floorlog2(new uint2(x, x)).x);

    [Theory]
    [InlineData(4u, 2)]
    public void FloorLog2_UInt3(uint x, int expected) =>
        Assert.Equal(expected, math.floorlog2(new uint3(x, x, x)).x);

    [Theory]
    [InlineData(32768u, 15)]
    public void FloorLog2_UInt4(uint x, int expected) =>
        Assert.Equal(expected, math.floorlog2(new uint4(x, x, x, x)).x);

    [Theory]
    [InlineData(1, 0)]
    [InlineData(2, 1)]
    [InlineData(3, 2)]
    [InlineData(4, 2)]
    [InlineData(5, 3)]
    [InlineData(63, 6)]
    [InlineData(64, 6)]
    [InlineData(65, 7)]
    [InlineData(16777215, 24)]
    [InlineData(16777216, 24)]
    [InlineData(16777217, 25)]
    [InlineData(2147483646, 31)]
    [InlineData(2147483647, 31)]
    public void CeilLog2_Int(int x, int expected) => Assert.Equal(expected, math.ceillog2(x));

    [Theory]
    [InlineData(1, 0)]
    public void CeilLog2_Int2(int x, int expected) =>
        Assert.Equal(expected, math.ceillog2(new int2(x, x)).x);

    [Theory]
    [InlineData(64, 6)]
    public void CeilLog2_Int3(int x, int expected) =>
        Assert.Equal(expected, math.ceillog2(new int3(x, x, x)).x);

    [Theory]
    [InlineData(16777216, 24)]
    public void CeilLog2_Int4(int x, int expected) =>
        Assert.Equal(expected, math.ceillog2(new int4(x, x, x, x)).x);

    [Theory]
    [InlineData(1u, 0)]
    [InlineData(2u, 1)]
    [InlineData(3u, 2)]
    [InlineData(4u, 2)]
    [InlineData(5u, 3)]
    [InlineData(63u, 6)]
    [InlineData(64u, 6)]
    [InlineData(65u, 7)]
    [InlineData(16777215u, 24)]
    [InlineData(16777216u, 24)]
    [InlineData(16777217u, 25)]
    [InlineData(4294967294u, 32)]
    [InlineData(4294967295u, 32)]
    public void CeilLog2_UInt(uint x, int expected) => Assert.Equal(expected, math.ceillog2(x));

    [Theory]
    [InlineData(1u, 0)]
    public void CeilLog2_UInt2(uint x, int expected) =>
        Assert.Equal(expected, math.ceillog2(new uint2(x, x)).x);

    [Theory]
    [InlineData(64u, 6)]
    public void CeilLog2_UInt3(uint x, int expected) =>
        Assert.Equal(expected, math.ceillog2(new uint3(x, x, x)).x);

    [Theory]
    [InlineData(4294967295u, 32)]
    public void CeilLog2_UInt4(uint x, int expected) =>
        Assert.Equal(expected, math.ceillog2(new uint4(x, x, x, x)).x);

    public static TheoryData<float, float> AbsFloatData => new()
    {
        { 0.0f, 0.0f }, { -1.1f, 1.1f }, { 2.2f, 2.2f },
        { float.NegativeInfinity, float.PositiveInfinity },
        { float.PositiveInfinity, float.PositiveInfinity },
    };

    [Theory]
    [MemberData(nameof(AbsFloatData))]
    public void Abs_Float(float x, float expected) => Assert.Equal(expected, math.abs(x));

    [Theory]
    [MemberData(nameof(AbsFloatData))]
    public void Abs_Float2(float x, float expected) =>
        Assert.Equal(expected, math.abs(new float2(x, x)).x);

    [Theory]
    [MemberData(nameof(AbsFloatData))]
    public void Abs_Float3(float x, float expected) =>
        Assert.Equal(expected, math.abs(new float3(x, x, x)).x);

    [Theory]
    [MemberData(nameof(AbsFloatData))]
    public void Abs_Float4(float x, float expected) =>
        Assert.Equal(expected, math.abs(new float4(x, x, x, x)).x);

    public static TheoryData<double, double> AbsDoubleData => new()
    {
        { 0.0, 0.0 }, { -1.1, 1.1 }, { 2.2, 2.2 },
        { double.NegativeInfinity, double.PositiveInfinity },
        { double.PositiveInfinity, double.PositiveInfinity },
    };

    [Theory]
    [MemberData(nameof(AbsDoubleData))]
    public void Abs_Double(double x, double expected) => Assert.Equal(expected, math.abs(x));

    [Theory]
    [MemberData(nameof(AbsDoubleData))]
    public void Abs_Double2(double x, double expected) =>
        Assert.Equal(expected, math.abs(new double2(x, x)).x);

    [Theory]
    [MemberData(nameof(AbsDoubleData))]
    public void Abs_Double3(double x, double expected) =>
        Assert.Equal(expected, math.abs(new double3(x, x, x)).x);

    [Theory]
    [MemberData(nameof(AbsDoubleData))]
    public void Abs_Double4(double x, double expected) =>
        Assert.Equal(expected, math.abs(new double4(x, x, x, x)).x);
}
