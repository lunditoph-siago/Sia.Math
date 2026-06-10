using System.Runtime.InteropServices;

namespace Sia.Math.Tests;

public static class TestValue
{
    public static float SignedFloatQNaN()
    {
        var u = new UIntFloatUnion { floatValue = 0.0f, uintValue = 0xFFC0_0000u };
        return u.floatValue;
    }

    public static float UnsignedFloatQNaN()
    {
        var u = new UIntFloatUnion { floatValue = 0.0f, uintValue = 0x7FC0_0000u };
        return u.floatValue;
    }

    public static float SignedFloatZero()
    {
        var u = new UIntFloatUnion { floatValue = 0.0f, uintValue = 0x8000_0000u };
        return u.floatValue;
    }

    public static double SignedDoubleQNaN()
    {
        var u = new ULongDoubleUnion { doubleValue = 0.0, ulongValue = 0xFFF8_0000_0000_0000ul };
        return u.doubleValue;
    }

    public static double UnsignedDoubleQNaN()
    {
        var u = new ULongDoubleUnion { doubleValue = 0.0, ulongValue = 0x7FF8_0000_0000_0000ul };
        return u.doubleValue;
    }

    public static double SignedDoubleZero()
    {
        var u = new ULongDoubleUnion { doubleValue = 0.0, ulongValue = 0x8000_0000_0000_0000ul };
        return u.doubleValue;
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct UIntFloatUnion
    {
        [FieldOffset(0)] public uint uintValue;
        [FieldOffset(0)] public float floatValue;
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct ULongDoubleUnion
    {
        [FieldOffset(0)] public ulong ulongValue;
        [FieldOffset(0)] public double doubleValue;
    }
}
