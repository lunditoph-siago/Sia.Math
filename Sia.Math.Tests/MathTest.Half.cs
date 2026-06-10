using Sia.Math;
using static Sia.Math.Tests.TestValue;

namespace Sia.Math.Tests;

public class TestMathHalf
{
    [Theory]
    [InlineData(0x0000, 0x00000000u)]
    [InlineData(0x0203, 0x3800C000u)]
    [InlineData(0x4321, 0x40642000u)]
    [InlineData(0x7BFF, 0x477FE000u)]
    [InlineData(0x7C00, 0x7F800000u)]
    public void F16ToF32(uint half, uint expectedFloatBits) =>
        Assert.Equal(expectedFloatBits, math.asuint(math.f16tof32(half)));

    [Fact]
    public void F16ToF32_NaN()
    {
        Assert.True(float.IsNaN(math.f16tof32(0x7C01)));
        Assert.True(float.IsNaN(math.f16tof32(0xFC01)));
    }

    [Theory]
    [InlineData(0x8000, 0x80000000u)]
    [InlineData(0x8203, 0xB800C000u)]
    [InlineData(0xC321, 0xC0642000u)]
    [InlineData(0xFBFF, 0xC77FE000u)]
    [InlineData(0xFC00, 0xFF800000u)]
    public void F16ToF32_Negative(uint half, uint expectedFloatBits) =>
        Assert.Equal(expectedFloatBits, math.asuint(math.f16tof32(half)));

    [Fact]
    public void F16ToF32_UInt2()
    {
        var r = math.f16tof32(new uint2(0x4321, 0x7BFF));
        var bits = math.asuint(r);
        Assert.Equal(0x40642000u, bits.x);
        Assert.Equal(0x477FE000u, bits.y);
    }

    [Fact]
    public void F16ToF32_UInt3()
    {
        var r = math.f16tof32(new uint3(0x0000, 0x4321, 0x7BFF));
        var bits = math.asuint(r);
        Assert.Equal(0x00000000u, bits.x);
        Assert.Equal(0x40642000u, bits.y);
        Assert.Equal(0x477FE000u, bits.z);
    }

    [Fact]
    public void F16ToF32_UInt4()
    {
        var r = math.f16tof32(new uint4(0x0000, 0x4321, 0x7BFF, 0x7C00));
        var bits = math.asuint(r);
        Assert.Equal(0x00000000u, bits.x);
        Assert.Equal(0x40642000u, bits.y);
        Assert.Equal(0x477FE000u, bits.z);
        Assert.Equal(0x7F800000u, bits.w);
    }

    [Fact]
    public void F16ToF32_UInt4_Negative()
    {
        var r = math.f16tof32(new uint4(0x8000, 0xC321, 0xFBFF, 0xFC00));
        var bits = math.asuint(r);
        Assert.Equal(0x80000000u, bits.x);
        Assert.Equal(0xC0642000u, bits.y);
        Assert.Equal(0xC77FE000u, bits.z);
        Assert.Equal(0xFF800000u, bits.w);
    }

    [Theory]
    [InlineData(0.0f, 0x0000u)]
    [InlineData(2.98e-08f, 0x0000u)]
    [InlineData(5.96046448e-08f, 0x0001u)]
    [InlineData(123.4f, 0x57B6u)]
    [InlineData(65504.0f, 0x7BFFu)]
    [InlineData(65520.0f, 0x7C00u)]
    public void F32ToF16(float x, uint expected) => Assert.Equal(expected, math.f32tof16(x));

    [Fact]
    public void F32ToF16_PositiveInfinity() => Assert.Equal(0x7C00u, math.f32tof16(float.PositiveInfinity));

    [Fact]
    public void F32ToF16_SignedQNaN() => Assert.Equal(0xFE00u, math.f32tof16(SignedFloatQNaN()));

    [Theory]
    [InlineData(-2.98e-08f, 0x8000u)]
    [InlineData(-5.96046448e-08f, 0x8001u)]
    [InlineData(-123.4f, 0xD7B6u)]
    [InlineData(-65504.0f, 0xFBFFu)]
    [InlineData(-65520.0f, 0xFC00u)]
    public void F32ToF16_Negative(float x, uint expected) => Assert.Equal(expected, math.f32tof16(x));

    [Fact]
    public void F32ToF16_NegativeInfinity() => Assert.Equal(0xFC00u, math.f32tof16(float.NegativeInfinity));

    [Fact]
    public void F32ToF16_Float2()
    {
        var r = math.f32tof16(new float2(0.0f, 123.4f));
        Assert.Equal(0x0000u, r.x);
        Assert.Equal(0x57B6u, r.y);
    }

    [Fact]
    public void F32ToF16_Float3()
    {
        var r = math.f32tof16(new float3(0.0f, 123.4f, 65520.0f));
        Assert.Equal(0x0000u, r.x);
        Assert.Equal(0x57B6u, r.y);
        Assert.Equal(0x7C00u, r.z);
    }

    [Fact]
    public void F32ToF16_Float4()
    {
        var r = math.f32tof16(new float4(0.0f, 123.4f, 65520.0f, float.PositiveInfinity));
        Assert.Equal(0x0000u, r.x);
        Assert.Equal(0x57B6u, r.y);
        Assert.Equal(0x7C00u, r.z);
        Assert.Equal(0x7C00u, r.w);
    }

    [Fact]
    public void F32ToF16_Float4_Negative()
    {
        var r = math.f32tof16(new float4(-123.4f, -65504.0f, -65520.0f, float.NegativeInfinity));
        Assert.Equal(0xD7B6u, r.x);
        Assert.Equal(0xFBFFu, r.y);
        Assert.Equal(0xFC00u, r.z);
        Assert.Equal(0xFC00u, r.w);
    }
}
