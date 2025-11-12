using Sia.Math;

namespace Sia.Math.Tests;

public class FloatVectorArithmeticTests
{
    private static readonly Random Rng = new(12345);

    private static float NextFloat() => (float)(Rng.NextDouble() * 200.0 - 100.0);

    // Hardware dot-product sums lanes in a different order than left-to-right scalar addition
    // (pairwise/horizontal-add), so it can legitimately differ from the naive reference in the
    // low bits of a float's ~7 significant digits. Use a relative tolerance instead of fixed
    // decimal places, which assume a magnitude the reduction doesn't have here.
    private static void AssertApprox(float expected, float actual, float relTol = 1e-4f) =>
        Assert.True(System.Math.Abs(expected - actual) <= relTol * System.Math.Max(1f, System.Math.Abs(expected)),
            $"expected {expected}, got {actual}");

    [Fact]
    public void Float2_Arithmetic_MatchesScalarReference()
    {
        for (var i = 0; i < 256; i++)
        {
            var ax = NextFloat(); var ay = NextFloat();
            var bx = NextFloat(); var by = NextFloat();
            var a = new float2(ax, ay);
            var b = new float2(bx, by);

            Assert.Equal(ax + bx, (a + b).x, 3);
            Assert.Equal(ay + by, (a + b).y, 3);
            Assert.Equal(ax - bx, (a - b).x, 3);
            Assert.Equal(ay - by, (a - b).y, 3);
            Assert.Equal(ax * bx, (a * b).x, 3);
            Assert.Equal(ay * by, (a * b).y, 3);
            Assert.Equal(ax / bx, (a / b).x, 3);
            Assert.Equal(ay / by, (a / b).y, 3);
            AssertApprox(ax * bx + ay * by, math.dot(a, b));
        }
    }

    [Fact]
    public void Float3_Arithmetic_MatchesScalarReference()
    {
        for (var i = 0; i < 256; i++)
        {
            var ax = NextFloat(); var ay = NextFloat(); var az = NextFloat();
            var bx = NextFloat(); var by = NextFloat(); var bz = NextFloat();
            var a = new float3(ax, ay, az);
            var b = new float3(bx, by, bz);

            Assert.Equal(ax + bx, (a + b).x, 3);
            Assert.Equal(ay + by, (a + b).y, 3);
            Assert.Equal(az + bz, (a + b).z, 3);
            Assert.Equal(ax - bx, (a - b).x, 3);
            Assert.Equal(ay - by, (a - b).y, 3);
            Assert.Equal(az - bz, (a - b).z, 3);
            Assert.Equal(ax * bx, (a * b).x, 3);
            Assert.Equal(ay * by, (a * b).y, 3);
            Assert.Equal(az * bz, (a * b).z, 3);
            Assert.Equal(ax / bx, (a / b).x, 3);
            Assert.Equal(ay / by, (a / b).y, 3);
            Assert.Equal(az / bz, (a / b).z, 3);
            AssertApprox(ax * bx + ay * by + az * bz, math.dot(a, b));

            var cross = math.cross(a, b);
            Assert.Equal(ay * bz - az * by, cross.x, 3);
            Assert.Equal(az * bx - ax * bz, cross.y, 3);
            Assert.Equal(ax * by - ay * bx, cross.z, 3);
        }
    }

    [Fact]
    public void Float4_Arithmetic_MatchesScalarReference()
    {
        for (var i = 0; i < 256; i++)
        {
            var ax = NextFloat(); var ay = NextFloat(); var az = NextFloat(); var aw = NextFloat();
            var bx = NextFloat(); var by = NextFloat(); var bz = NextFloat(); var bw = NextFloat();
            var a = new float4(ax, ay, az, aw);
            var b = new float4(bx, by, bz, bw);

            Assert.Equal(ax + bx, (a + b).x, 3);
            Assert.Equal(ay + by, (a + b).y, 3);
            Assert.Equal(az + bz, (a + b).z, 3);
            Assert.Equal(aw + bw, (a + b).w, 3);
            Assert.Equal(ax - bx, (a - b).x, 3);
            Assert.Equal(ay - by, (a - b).y, 3);
            Assert.Equal(az - bz, (a - b).z, 3);
            Assert.Equal(aw - bw, (a - b).w, 3);
            Assert.Equal(ax * bx, (a * b).x, 3);
            Assert.Equal(ay * by, (a * b).y, 3);
            Assert.Equal(az * bz, (a * b).z, 3);
            Assert.Equal(aw * bw, (a * b).w, 3);
            Assert.Equal(ax / bx, (a / b).x, 3);
            Assert.Equal(ay / by, (a / b).y, 3);
            Assert.Equal(az / bz, (a / b).z, 3);
            Assert.Equal(aw / bw, (a / b).w, 3);
            AssertApprox(ax * bx + ay * by + az * bz + aw * bw, math.dot(a, b));
        }
    }

    [Fact]
    public void Float3_Normalize_ProducesUnitLength()
    {
        for (var i = 0; i < 64; i++)
        {
            var v = new float3(NextFloat() + 50f, NextFloat() + 50f, NextFloat() + 50f);
            var n = math.normalize(v);
            AssertApprox(1f, math.length(n));
            AssertApprox(math.sqrt(v.x * v.x + v.y * v.y + v.z * v.z), math.length(v));
            AssertApprox(v.x * v.x + v.y * v.y + v.z * v.z, math.lengthsq(v));
        }
    }
}

public class IntVectorArithmeticTests
{
    private static readonly Random Rng = new(54321);

    private static int NextInt() => Rng.Next(-1000, 1000);

    [Fact]
    public void Int2_Arithmetic_MatchesScalarReference()
    {
        for (var i = 0; i < 256; i++)
        {
            var ax = NextInt(); var ay = NextInt();
            var bx = NextInt(); var by = NextInt();
            var a = new int2(ax, ay);
            var b = new int2(bx, by);

            Assert.Equal(ax + bx, (a + b).x);
            Assert.Equal(ay + by, (a + b).y);
            Assert.Equal(ax - bx, (a - b).x);
            Assert.Equal(ay - by, (a - b).y);
            Assert.Equal(ax * bx, (a * b).x);
            Assert.Equal(ay * by, (a * b).y);
            Assert.Equal(ax * bx + ay * by, math.dot(a, b));
        }
    }

    [Fact]
    public void Int3_Arithmetic_MatchesScalarReference()
    {
        for (var i = 0; i < 256; i++)
        {
            var ax = NextInt(); var ay = NextInt(); var az = NextInt();
            var bx = NextInt(); var by = NextInt(); var bz = NextInt();
            var a = new int3(ax, ay, az);
            var b = new int3(bx, by, bz);

            Assert.Equal(ax + bx, (a + b).x);
            Assert.Equal(ay + by, (a + b).y);
            Assert.Equal(az + bz, (a + b).z);
            Assert.Equal(ax - bx, (a - b).x);
            Assert.Equal(ay - by, (a - b).y);
            Assert.Equal(az - bz, (a - b).z);
            Assert.Equal(ax * bx, (a * b).x);
            Assert.Equal(ay * by, (a * b).y);
            Assert.Equal(az * bz, (a * b).z);
            Assert.Equal(ax * bx + ay * by + az * bz, math.dot(a, b));
        }
    }

    [Fact]
    public void Int4_Arithmetic_MatchesScalarReference()
    {
        for (var i = 0; i < 256; i++)
        {
            var ax = NextInt(); var ay = NextInt(); var az = NextInt(); var aw = NextInt();
            var bx = NextInt(); var by = NextInt(); var bz = NextInt(); var bw = NextInt();
            var a = new int4(ax, ay, az, aw);
            var b = new int4(bx, by, bz, bw);

            Assert.Equal(ax + bx, (a + b).x);
            Assert.Equal(ay + by, (a + b).y);
            Assert.Equal(az + bz, (a + b).z);
            Assert.Equal(aw + bw, (a + b).w);
            Assert.Equal(ax - bx, (a - b).x);
            Assert.Equal(ay - by, (a - b).y);
            Assert.Equal(az - bz, (a - b).z);
            Assert.Equal(aw - bw, (a - b).w);
            Assert.Equal(ax * bx, (a * b).x);
            Assert.Equal(ay * by, (a * b).y);
            Assert.Equal(az * bz, (a * b).z);
            Assert.Equal(aw * bw, (a * b).w);
            Assert.Equal(ax * bx + ay * by + az * bz + aw * bw, math.dot(a, b));
        }
    }
}

public class UnaryAndBitwiseVectorTests
{
    private static readonly Random Rng = new(24680);

    private static int NextInt() => Rng.Next(-1000, 1000);
    private static uint NextUInt() => (uint)Rng.Next(0, int.MaxValue);
    private static float NextFloat() => (float)(Rng.NextDouble() * 200.0 - 100.0);

    [Fact]
    public void Float_UnaryNegation_MatchesScalarReference()
    {
        for (var i = 0; i < 64; i++)
        {
            var x = NextFloat(); var y = NextFloat(); var z = NextFloat(); var w = NextFloat();
            Assert.Equal(-x, (-new float2(x, y)).x, 3);
            Assert.Equal(-y, (-new float2(x, y)).y, 3);
            Assert.Equal(-x, (-new float3(x, y, z)).x, 3);
            Assert.Equal(-z, (-new float3(x, y, z)).z, 3);
            Assert.Equal(-w, (-new float4(x, y, z, w)).w, 3);
        }
    }

    [Fact]
    public void Int_UnaryNegation_MatchesScalarReference()
    {
        for (var i = 0; i < 64; i++)
        {
            var x = NextInt(); var y = NextInt(); var z = NextInt();
            var v = -new int3(x, y, z);
            Assert.Equal(-x, v.x);
            Assert.Equal(-y, v.y);
            Assert.Equal(-z, v.z);
        }
    }

    [Fact]
    public void UInt_UnaryNegation_MatchesScalarWraparoundReference()
    {
        for (var i = 0; i < 64; i++)
        {
            var x = NextUInt(); var y = NextUInt(); var z = NextUInt();
            var v = -new uint3(x, y, z);
            Assert.Equal((uint)-x, v.x);
            Assert.Equal((uint)-y, v.y);
            Assert.Equal((uint)-z, v.z);
        }
    }

    [Fact]
    public void Int_BitwiseAndShift_MatchesScalarReference()
    {
        for (var i = 0; i < 64; i++)
        {
            var ax = NextInt(); var ay = NextInt(); var az = NextInt();
            var bx = NextInt(); var by = NextInt(); var bz = NextInt();
            var a = new int3(ax, ay, az);
            var b = new int3(bx, by, bz);
            var n = Rng.Next(0, 8);

            Assert.Equal(ax & bx, (a & b).x);
            Assert.Equal(ay | by, (a | b).y);
            Assert.Equal(az ^ bz, (a ^ b).z);
            Assert.Equal(~ax, (~a).x);
            Assert.Equal(ax << n, (a << n).x);
            Assert.Equal(ax >> n, (a >> n).x);
        }
    }

    [Fact]
    public void UInt_BitwiseAndShift_MatchesScalarReference()
    {
        for (var i = 0; i < 64; i++)
        {
            var ax = NextUInt(); var ay = NextUInt(); var az = NextUInt();
            var bx = NextUInt(); var by = NextUInt(); var bz = NextUInt();
            var a = new uint3(ax, ay, az);
            var b = new uint3(bx, by, bz);
            var n = Rng.Next(0, 8);

            Assert.Equal(ax & bx, (a & b).x);
            Assert.Equal(ay | by, (a | b).y);
            Assert.Equal(az ^ bz, (a ^ b).z);
            Assert.Equal(~ax, (~a).x);
            Assert.Equal(ax << n, (a << n).x);
            Assert.Equal(ax >> n, (a >> n).x);
        }
    }
}

public class UIntVectorArithmeticTests
{
    private static readonly Random Rng = new(98765);

    private static uint NextUInt() => (uint)Rng.Next(0, 1000);

    [Fact]
    public void UInt2_Arithmetic_MatchesScalarReference()
    {
        for (var i = 0; i < 256; i++)
        {
            var ax = NextUInt(); var ay = NextUInt();
            var bx = NextUInt(); var by = NextUInt();
            var a = new uint2(ax, ay);
            var b = new uint2(bx, by);

            Assert.Equal(ax + bx, (a + b).x);
            Assert.Equal(ay + by, (a + b).y);
            Assert.Equal(ax * bx, (a * b).x);
            Assert.Equal(ay * by, (a * b).y);
            Assert.Equal(ax * bx + ay * by, math.dot(a, b));
        }
    }

    [Fact]
    public void UInt3_Arithmetic_MatchesScalarReference()
    {
        for (var i = 0; i < 256; i++)
        {
            var ax = NextUInt(); var ay = NextUInt(); var az = NextUInt();
            var bx = NextUInt(); var by = NextUInt(); var bz = NextUInt();
            var a = new uint3(ax, ay, az);
            var b = new uint3(bx, by, bz);

            Assert.Equal(ax + bx, (a + b).x);
            Assert.Equal(ay + by, (a + b).y);
            Assert.Equal(az + bz, (a + b).z);
            Assert.Equal(ax * bx, (a * b).x);
            Assert.Equal(ay * by, (a * b).y);
            Assert.Equal(az * bz, (a * b).z);
            Assert.Equal(ax * bx + ay * by + az * bz, math.dot(a, b));
        }
    }

    [Fact]
    public void UInt4_Arithmetic_MatchesScalarReference()
    {
        for (var i = 0; i < 256; i++)
        {
            var ax = NextUInt(); var ay = NextUInt(); var az = NextUInt(); var aw = NextUInt();
            var bx = NextUInt(); var by = NextUInt(); var bz = NextUInt(); var bw = NextUInt();
            var a = new uint4(ax, ay, az, aw);
            var b = new uint4(bx, by, bz, bw);

            Assert.Equal(ax + bx, (a + b).x);
            Assert.Equal(ay + by, (a + b).y);
            Assert.Equal(az + bz, (a + b).z);
            Assert.Equal(aw + bw, (a + b).w);
            Assert.Equal(ax * bx, (a * b).x);
            Assert.Equal(ay * by, (a * b).y);
            Assert.Equal(az * bz, (a * b).z);
            Assert.Equal(aw * bw, (a * b).w);
            Assert.Equal(ax * bx + ay * by + az * bz + aw * bw, math.dot(a, b));
        }
    }
}
