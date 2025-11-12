using Sia.Math;

namespace Sia.Math.Tests;

public class DoubleVectorArithmeticTests
{
    private static readonly Random Rng = new(161803);

    private static double NextDouble() => Rng.NextDouble() * 200.0 - 100.0;

    private static void AssertApprox(double expected, double actual, double relTol = 1e-9)
    {
        Assert.True(System.Math.Abs(expected - actual) <= relTol * System.Math.Max(1.0, System.Math.Abs(expected)),
            $"expected {expected}, got {actual}");
    }

    [Fact]
    public void Double2_Arithmetic_MatchesScalarReference()
    {
        for (var i = 0; i < 256; i++)
        {
            var ax = NextDouble(); var ay = NextDouble();
            var bx = NextDouble(); var by = NextDouble();
            var a = new double2(ax, ay);
            var b = new double2(bx, by);

            AssertApprox(ax + bx, (a + b).x);
            AssertApprox(ay + by, (a + b).y);
            AssertApprox(ax - bx, (a - b).x);
            AssertApprox(ax * bx, (a * b).x);
            AssertApprox(ax / bx, (a / b).x);
            AssertApprox(ax * bx + ay * by, math.dot(a, b));
            AssertApprox(-ax, (-a).x);
        }
    }

    [Fact]
    public void Double3_Arithmetic_MatchesScalarReference()
    {
        for (var i = 0; i < 256; i++)
        {
            var ax = NextDouble(); var ay = NextDouble(); var az = NextDouble();
            var bx = NextDouble(); var by = NextDouble(); var bz = NextDouble();
            var a = new double3(ax, ay, az);
            var b = new double3(bx, by, bz);

            AssertApprox(ax + bx, (a + b).x);
            AssertApprox(ay + by, (a + b).y);
            AssertApprox(az + bz, (a + b).z);
            AssertApprox(ax - bx, (a - b).x);
            AssertApprox(ax * bx, (a * b).x);
            AssertApprox(ax / bx, (a / b).x);
            AssertApprox(ax * bx + ay * by + az * bz, math.dot(a, b));
            AssertApprox(-az, (-a).z);

            var cross = math.cross(a, b);
            AssertApprox(ay * bz - az * by, cross.x);
            AssertApprox(az * bx - ax * bz, cross.y);
            AssertApprox(ax * by - ay * bx, cross.z);
        }
    }

    [Fact]
    public void Double4_Arithmetic_MatchesScalarReference()
    {
        for (var i = 0; i < 256; i++)
        {
            var ax = NextDouble(); var ay = NextDouble(); var az = NextDouble(); var aw = NextDouble();
            var bx = NextDouble(); var by = NextDouble(); var bz = NextDouble(); var bw = NextDouble();
            var a = new double4(ax, ay, az, aw);
            var b = new double4(bx, by, bz, bw);

            AssertApprox(ax + bx, (a + b).x);
            AssertApprox(aw + bw, (a + b).w);
            AssertApprox(ax - bx, (a - b).x);
            AssertApprox(ax * bx, (a * b).x);
            AssertApprox(ax / bx, (a / b).x);
            AssertApprox(ax * bx + ay * by + az * bz + aw * bw, math.dot(a, b));
            AssertApprox(-aw, (-a).w);
        }
    }

    [Fact]
    public void Double3_Normalize_ProducesUnitLength()
    {
        for (var i = 0; i < 64; i++)
        {
            var v = new double3(NextDouble() + 50.0, NextDouble() + 50.0, NextDouble() + 50.0);
            var n = math.normalize(v);
            AssertApprox(1.0, math.length(n), 1e-6);
        }
    }
}
