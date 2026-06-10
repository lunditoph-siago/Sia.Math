using Sia.Math;
using static Sia.Math.Tests.TestValue;

namespace Sia.Math.Tests;

public class TestMathBitManipulation
{
    [Theory]
    [InlineData(0x01234567, 12)]
    [InlineData(0x456789AB, 16)]
    [InlineData(-0x01234567, 21)]
    [InlineData(-0x456789AB, 17)]
    [InlineData(-1, 32)]
    public void CountBits_Int(int x, int expected) => Assert.Equal(expected, math.countbits(x));

    [Theory]
    [InlineData(0x01234567, 12)]
    [InlineData(0x456789AB, 16)]
    public void CountBits_Int2(int x, int expected) =>
        Assert.Equal(expected, math.countbits(new int2(x, x)).x);

    [Theory]
    [InlineData(0x01234567, 12)]
    public void CountBits_Int3(int x, int expected) =>
        Assert.Equal(expected, math.countbits(new int3(x, x, x)).x);

    [Theory]
    [InlineData(-1, 32)]
    public void CountBits_Int4(int x, int expected) =>
        Assert.Equal(expected, math.countbits(new int4(x, x, x, x)).x);

    [Theory]
    [InlineData(0u, 0)]
    [InlineData(0x01234567u, 12)]
    [InlineData(0x456789ABu, 16)]
    [InlineData(0x89ABCDEFu, 20)]
    [InlineData(0xCDEF0123u, 16)]
    [InlineData(0xFFFFFFFFu, 32)]
    public void CountBits_UInt(uint x, int expected) => Assert.Equal(expected, math.countbits(x));

    [Theory]
    [InlineData(0u, 0)]
    public void CountBits_UInt2(uint x, int expected) =>
        Assert.Equal(expected, math.countbits(new uint2(x, x)).x);

    [Theory]
    [InlineData(0xFFFFFFFFu, 32)]
    public void CountBits_UInt3(uint x, int expected) =>
        Assert.Equal(expected, math.countbits(new uint3(x, x, x)).x);

    [Theory]
    [InlineData(0u, 0)]
    public void CountBits_UInt4(uint x, int expected) =>
        Assert.Equal(expected, math.countbits(new uint4(x, x, x, x)).x);

    [Theory]
    [InlineData(0L, 0)]
    [InlineData(0x0123456789ABCDEFL, 32)]
    [InlineData(-0x0123456789ABCDEFL, 33)]
    [InlineData(-1L, 64)]
    public void CountBits_Long(long x, int expected) => Assert.Equal(expected, math.countbits(x));

    [Theory]
    [InlineData(0UL, 0)]
    [InlineData(0x0123456789ABCDEFUL, 32)]
    [InlineData(0x89ABCDEF01234567UL, 32)]
    [InlineData(0xFFFFFFFFFFFFFFFFUL, 64)]
    public void CountBits_ULong(ulong x, int expected) => Assert.Equal(expected, math.countbits(x));

    [Theory]
    [InlineData(0, 32)]
    [InlineData(1, 31)]
    [InlineData(2, 30)]
    [InlineData(3, 30)]
    [InlineData(0x5321, 17)]
    [InlineData(0x04435321, 5)]
    [InlineData(-1, 0)]
    [InlineData(-2147483648, 0)]
    public void LzCnt_Int(int x, int expected) => Assert.Equal(expected, math.lzcnt(x));

    [Theory]
    [InlineData(0, 32)]
    public void LzCnt_Int2(int x, int expected) =>
        Assert.Equal(expected, math.lzcnt(new int2(x, x)).x);

    [Theory]
    [InlineData(-1, 0)]
    public void LzCnt_Int3(int x, int expected) =>
        Assert.Equal(expected, math.lzcnt(new int3(x, x, x)).x);

    [Theory]
    [InlineData(0x5321, 17)]
    public void LzCnt_Int4(int x, int expected) =>
        Assert.Equal(expected, math.lzcnt(new int4(x, x, x, x)).x);

    [Theory]
    [InlineData(0u, 32)]
    [InlineData(1u, 31)]
    [InlineData(2u, 30)]
    [InlineData(3u, 30)]
    [InlineData(0x5321u, 17)]
    [InlineData(0x04435321u, 5)]
    [InlineData(0x84435320u, 0)]
    [InlineData(0xFFFFFFFFu, 0)]
    public void LzCnt_UInt(uint x, int expected) => Assert.Equal(expected, math.lzcnt(x));

    [Theory]
    [InlineData(0u, 32)]
    public void LzCnt_UInt2(uint x, int expected) =>
        Assert.Equal(expected, math.lzcnt(new uint2(x, x)).x);

    [Theory]
    [InlineData(0xFFFFFFFFu, 0)]
    public void LzCnt_UInt3(uint x, int expected) =>
        Assert.Equal(expected, math.lzcnt(new uint3(x, x, x)).x);

    [Theory]
    [InlineData(0u, 32)]
    public void LzCnt_UInt4(uint x, int expected) =>
        Assert.Equal(expected, math.lzcnt(new uint4(x, x, x, x)).x);

    [Theory]
    [InlineData(0L, 64)]
    [InlineData(1L, 63)]
    [InlineData(0x1FFF1234L, 35)]
    [InlineData(0x1FFFF1234L, 31)]
    [InlineData(0x1FFFFFFF1234L, 19)]
    [InlineData(-1L, 0)]
    [InlineData(-9223372036854775808L, 0)]
    public void LzCnt_Long(long x, int expected) => Assert.Equal(expected, math.lzcnt(x));

    [Theory]
    [InlineData(0UL, 64)]
    [InlineData(1UL, 63)]
    [InlineData(0x1FFF1234UL, 35)]
    [InlineData(0x1FFFF1234UL, 31)]
    [InlineData(0x1FFFFFFF1234UL, 19)]
    [InlineData(0xFFFFFFFFFFFFFFFFUL, 0)]
    [InlineData(0x8000000000000000UL, 0)]
    public void LzCnt_ULong(ulong x, int expected) => Assert.Equal(expected, math.lzcnt(x));

    [Theory]
    [InlineData(0, 32)]
    [InlineData(1, 0)]
    [InlineData(2, 1)]
    [InlineData(3, 0)]
    [InlineData(0x53210, 4)]
    [InlineData(0x44420000, 17)]
    [InlineData(-2, 1)]
    [InlineData(-2147483647, 0)]
    [InlineData(-2147483648, 31)]
    public void TzCnt_Int(int x, int expected) => Assert.Equal(expected, math.tzcnt(x));

    [Theory]
    [InlineData(1, 0)]
    public void TzCnt_Int2(int x, int expected) =>
        Assert.Equal(expected, math.tzcnt(new int2(x, x)).x);

    [Theory]
    [InlineData(0, 32)]
    public void TzCnt_Int3(int x, int expected) =>
        Assert.Equal(expected, math.tzcnt(new int3(x, x, x)).x);

    [Theory]
    [InlineData(0x53210, 4)]
    public void TzCnt_Int4(int x, int expected) =>
        Assert.Equal(expected, math.tzcnt(new int4(x, x, x, x)).x);

    [Theory]
    [InlineData(0u, 32)]
    [InlineData(1u, 0)]
    [InlineData(2u, 1)]
    [InlineData(3u, 0)]
    [InlineData(0x53210u, 4)]
    [InlineData(0x44420000u, 17)]
    [InlineData(0xFFFFFFFEu, 1)]
    [InlineData(0x80000001u, 0)]
    [InlineData(0x80000000u, 31)]
    public void TzCnt_UInt(uint x, int expected) => Assert.Equal(expected, math.tzcnt(x));

    [Theory]
    [InlineData(1u, 0)]
    public void TzCnt_UInt2(uint x, int expected) =>
        Assert.Equal(expected, math.tzcnt(new uint2(x, x)).x);

    [Theory]
    [InlineData(0u, 32)]
    public void TzCnt_UInt3(uint x, int expected) =>
        Assert.Equal(expected, math.tzcnt(new uint3(x, x, x)).x);

    [Theory]
    [InlineData(0x53210u, 4)]
    public void TzCnt_UInt4(uint x, int expected) =>
        Assert.Equal(expected, math.tzcnt(new uint4(x, x, x, x)).x);

    [Theory]
    [InlineData(0L, 64)]
    [InlineData(1L, 0)]
    [InlineData(2L, 1)]
    [InlineData(0x44420000L, 17)]
    [InlineData(0x444200000000L, 33)]
    [InlineData(-9223372036854775808L, 63)]
    [InlineData(-9223372036854775807L, 0)]
    public void TzCnt_Long(long x, int expected) => Assert.Equal(expected, math.tzcnt(x));

    [Theory]
    [InlineData(0UL, 64)]
    [InlineData(1UL, 0)]
    [InlineData(2UL, 1)]
    [InlineData(0x44420000UL, 17)]
    [InlineData(0x444200000000UL, 33)]
    [InlineData(0x8000000000000000UL, 63)]
    [InlineData(0x8000000000000001UL, 0)]
    public void TzCnt_ULong(ulong x, int expected) => Assert.Equal(expected, math.tzcnt(x));

    [Theory]
    [InlineData(unchecked((int)0x90684AC0), 0x03521609)]
    [InlineData(0x1260dafa, 0x5f5b0648)]
    [InlineData(unchecked((int)0xB1BF5DD2), 0x4bbafd8d)]
    [InlineData(0x74239b12, 0x48d9c42e)]
    public void ReverseBits_Int(int x, int expected) => Assert.Equal(expected, math.reversebits(x));

    [Fact]
    public void ReverseBits_Int2()
    {
        int x = unchecked((int)0x90684AC0);
        int expected = 0x03521609;
        Assert.Equal(expected, math.reversebits(new int2(x, x)).x);
    }

    [Theory]
    [InlineData(0x1260dafa, 0x5f5b0648)]
    public void ReverseBits_Int3(int x, int expected) =>
        Assert.Equal(expected, math.reversebits(new int3(x, x, x)).x);

    [Theory]
    [InlineData(0x74239b12, 0x48d9c42e)]
    public void ReverseBits_Int4(int x, int expected) =>
        Assert.Equal(expected, math.reversebits(new int4(x, x, x, x)).x);

    [Theory]
    [InlineData(0x90684ac0u, 0x03521609u)]
    [InlineData(0x1260dafau, 0x5f5b0648u)]
    [InlineData(0xb1bf5dd2u, 0x4bbafd8du)]
    [InlineData(0x74239b12u, 0x48d9c42eu)]
    public void ReverseBits_UInt(uint x, uint expected) => Assert.Equal(expected, math.reversebits(x));

    [Theory]
    [InlineData(0x90684ac0u, 0x03521609u)]
    public void ReverseBits_UInt2(uint x, uint expected) =>
        Assert.Equal(expected, math.reversebits(new uint2(x, x)).x);

    [Theory]
    [InlineData(0x1260dafau, 0x5f5b0648u)]
    public void ReverseBits_UInt3(uint x, uint expected) =>
        Assert.Equal(expected, math.reversebits(new uint3(x, x, x)).x);

    [Theory]
    [InlineData(0x74239b12u, 0x48d9c42eu)]
    public void ReverseBits_UInt4(uint x, uint expected) =>
        Assert.Equal(expected, math.reversebits(new uint4(x, x, x, x)).x);

    [Theory]
    [InlineData(0x1260dafab1bf5dd2L, 0x4bbafd8d5f5b0648L)]
    public void ReverseBits_Long(long x, long expected) => Assert.Equal(expected, math.reversebits(x));

    [Theory]
    [InlineData(0x1260dafab1bf5dd2UL, 0x4bbafd8d5f5b0648UL)]
    public void ReverseBits_ULong(ulong x, ulong expected) => Assert.Equal(expected, math.reversebits(x));

    [Theory]
    [InlineData(false, false, false, false, 0)]
    [InlineData(true, false, false, false, 0x01)]
    [InlineData(false, true, false, false, 0x02)]
    [InlineData(true, true, false, false, 0x03)]
    [InlineData(false, false, true, false, 0x04)]
    [InlineData(false, false, false, true, 0x08)]
    [InlineData(true, true, true, false, 0x07)]
    [InlineData(true, true, true, true, 0x0F)]
    public void BitMask(bool x, bool y, bool z, bool w, int expected) =>
        Assert.Equal(expected, math.bitmask(new bool4(x, y, z, w)));
}
