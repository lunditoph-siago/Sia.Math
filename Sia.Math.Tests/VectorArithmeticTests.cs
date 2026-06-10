using Sia.Math;
using static Sia.Math.Tests.TestValue;
using static Sia.Math.Tests.TestAssert;

namespace Sia.Math.Tests;

public class FloatVectorArithmeticTests
{
    private static readonly TestRng Rng = new(12345);

    [Fact]
    public void Float2_Arithmetic_MatchesScalarReference()
    {
        for (var i = 0; i < 256; i++)
        {
            var ax = Rng.Float(); var ay = Rng.Float();
            var bx = Rng.Float(); var by = Rng.Float();
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
            Approx(ax * bx + ay * by, math.dot(a, b));
        }
    }

    [Fact]
    public void Float3_Arithmetic_MatchesScalarReference()
    {
        for (var i = 0; i < 256; i++)
        {
            var ax = Rng.Float(); var ay = Rng.Float(); var az = Rng.Float();
            var bx = Rng.Float(); var by = Rng.Float(); var bz = Rng.Float();
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
            Approx(ax * bx + ay * by + az * bz, math.dot(a, b));

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
            var ax = Rng.Float(); var ay = Rng.Float(); var az = Rng.Float(); var aw = Rng.Float();
            var bx = Rng.Float(); var by = Rng.Float(); var bz = Rng.Float(); var bw = Rng.Float();
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
            Approx(ax * bx + ay * by + az * bz + aw * bw, math.dot(a, b));
        }
    }

    [Fact]
    public void Float3_Normalize_ProducesUnitLength()
    {
        for (var i = 0; i < 64; i++)
        {
            var v = new float3(Rng.Float(50f, 150f), Rng.Float(50f, 150f), Rng.Float(50f, 150f));
            var n = math.normalize(v);
            Approx(1f, math.length(n));
            Approx(math.sqrt(v.x * v.x + v.y * v.y + v.z * v.z), math.length(v));
            Approx(v.x * v.x + v.y * v.y + v.z * v.z, math.lengthsq(v));
        }
    }
}

public class DoubleVectorArithmeticTests
{
    private static readonly TestRng Rng = new(161803);

    [Fact]
    public void Double2_Arithmetic_MatchesScalarReference()
    {
        for (var i = 0; i < 256; i++)
        {
            var ax = Rng.Double(); var ay = Rng.Double();
            var bx = Rng.Double(); var by = Rng.Double();
            var a = new double2(ax, ay);
            var b = new double2(bx, by);

            Approx(ax + bx, (a + b).x);
            Approx(ay + by, (a + b).y);
            Approx(ax - bx, (a - b).x);
            Approx(ax * bx, (a * b).x);
            Approx(ax / bx, (a / b).x);
            Approx(ax * bx + ay * by, math.dot(a, b));
            Approx(-ax, (-a).x);
        }
    }

    [Fact]
    public void Double3_Arithmetic_MatchesScalarReference()
    {
        for (var i = 0; i < 256; i++)
        {
            var ax = Rng.Double(); var ay = Rng.Double(); var az = Rng.Double();
            var bx = Rng.Double(); var by = Rng.Double(); var bz = Rng.Double();
            var a = new double3(ax, ay, az);
            var b = new double3(bx, by, bz);

            Approx(ax + bx, (a + b).x);
            Approx(ay + by, (a + b).y);
            Approx(az + bz, (a + b).z);
            Approx(ax - bx, (a - b).x);
            Approx(ax * bx, (a * b).x);
            Approx(ax / bx, (a / b).x);
            Approx(ax * bx + ay * by + az * bz, math.dot(a, b));
            Approx(-az, (-a).z);

            var cross = math.cross(a, b);
            Approx(ay * bz - az * by, cross.x);
            Approx(az * bx - ax * bz, cross.y);
            Approx(ax * by - ay * bx, cross.z);
        }
    }

    [Fact]
    public void Double4_Arithmetic_MatchesScalarReference()
    {
        for (var i = 0; i < 256; i++)
        {
            var ax = Rng.Double(); var ay = Rng.Double(); var az = Rng.Double(); var aw = Rng.Double();
            var bx = Rng.Double(); var by = Rng.Double(); var bz = Rng.Double(); var bw = Rng.Double();
            var a = new double4(ax, ay, az, aw);
            var b = new double4(bx, by, bz, bw);

            Approx(ax + bx, (a + b).x);
            Approx(aw + bw, (a + b).w);
            Approx(ax - bx, (a - b).x);
            Approx(ax * bx, (a * b).x);
            Approx(ax / bx, (a / b).x);
            Approx(ax * bx + ay * by + az * bz + aw * bw, math.dot(a, b));
            Approx(-aw, (-a).w);
        }
    }

    [Fact]
    public void Double3_Normalize_ProducesUnitLength()
    {
        for (var i = 0; i < 64; i++)
        {
            var v = new double3(Rng.Double(50, 150), Rng.Double(50, 150), Rng.Double(50, 150));
            var n = math.normalize(v);
            Approx(1.0, math.length(n), 1e-6);
        }
    }
}

public class IntVectorArithmeticTests
{
    private static readonly TestRng Rng = new(54321);

    [Fact]
    public void Int2_Arithmetic_MatchesScalarReference()
    {
        for (var i = 0; i < 256; i++)
        {
            var ax = Rng.Int(); var ay = Rng.Int();
            var bx = Rng.Int(); var by = Rng.Int();
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
            var ax = Rng.Int(); var ay = Rng.Int(); var az = Rng.Int();
            var bx = Rng.Int(); var by = Rng.Int(); var bz = Rng.Int();
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
            var ax = Rng.Int(); var ay = Rng.Int(); var az = Rng.Int(); var aw = Rng.Int();
            var bx = Rng.Int(); var by = Rng.Int(); var bz = Rng.Int(); var bw = Rng.Int();
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

public class UIntVectorArithmeticTests
{
    private static readonly TestRng Rng = new(98765);

    [Fact]
    public void UInt2_Arithmetic_MatchesScalarReference()
    {
        for (var i = 0; i < 256; i++)
        {
            var ax = Rng.UInt(); var ay = Rng.UInt();
            var bx = Rng.UInt(); var by = Rng.UInt();
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
            var ax = Rng.UInt(); var ay = Rng.UInt(); var az = Rng.UInt();
            var bx = Rng.UInt(); var by = Rng.UInt(); var bz = Rng.UInt();
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
            var ax = Rng.UInt(); var ay = Rng.UInt(); var az = Rng.UInt(); var aw = Rng.UInt();
            var bx = Rng.UInt(); var by = Rng.UInt(); var bz = Rng.UInt(); var bw = Rng.UInt();
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

public class UnaryAndBitwiseVectorTests
{
    private static readonly TestRng Rng = new(24680);

    [Fact]
    public void Float_UnaryNegation_MatchesScalarReference()
    {
        for (var i = 0; i < 64; i++)
        {
            var x = Rng.Float(); var y = Rng.Float(); var z = Rng.Float(); var w = Rng.Float();
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
            var x = Rng.Int(); var y = Rng.Int(); var z = Rng.Int();
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
            var x = Rng.UInt(int.MaxValue); var y = Rng.UInt(int.MaxValue); var z = Rng.UInt(int.MaxValue);
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
            var ax = Rng.Int(); var ay = Rng.Int(); var az = Rng.Int();
            var bx = Rng.Int(); var by = Rng.Int(); var bz = Rng.Int();
            var a = new int3(ax, ay, az);
            var b = new int3(bx, by, bz);
            var n = Rng.Int(0, 8);

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
            var ax = Rng.UInt(int.MaxValue); var ay = Rng.UInt(int.MaxValue); var az = Rng.UInt(int.MaxValue);
            var bx = Rng.UInt(int.MaxValue); var by = Rng.UInt(int.MaxValue); var bz = Rng.UInt(int.MaxValue);
            var a = new uint3(ax, ay, az);
            var b = new uint3(bx, by, bz);
            var n = Rng.Int(0, 8);

            Assert.Equal(ax & bx, (a & b).x);
            Assert.Equal(ay | by, (a | b).y);
            Assert.Equal(az ^ bz, (a ^ b).z);
            Assert.Equal(~ax, (~a).x);
            Assert.Equal(ax << n, (a << n).x);
            Assert.Equal(ax >> n, (a >> n).x);
        }
    }
}
