using Sia.Math;
using static Sia.Math.Tests.TestAssert;

namespace Sia.Math.Tests;

public class QuaternionTests
{
    private static readonly Random Rng = new(112358);

    private static float NextFloat() => (float)(Rng.NextDouble() * 2.0 - 1.0);

    private static quaternion RandomUnitQuaternion()
    {
        var q = new quaternion(NextFloat(), NextFloat(), NextFloat(), NextFloat());
        return math.normalize(q);
    }

    [Fact]
    public void Normalize_ProducesUnitLength()
    {
        for (var i = 0; i < 64; i++)
        {
            var q = RandomUnitQuaternion();
            Assert.True(System.Math.Abs(math.length(q) - 1f) < 1e-4f);
        }
    }

    [Fact]
    public void Mul_WithIdentity_IsNoOp()
    {
        for (var i = 0; i < 64; i++)
        {
            var q = RandomUnitQuaternion();
            var r = math.mul(q, quaternion.identity);
            Assert.True(System.Math.Abs(q.value.x - r.value.x) < 1e-4f);
            Assert.True(System.Math.Abs(q.value.y - r.value.y) < 1e-4f);
            Assert.True(System.Math.Abs(q.value.z - r.value.z) < 1e-4f);
            Assert.True(System.Math.Abs(q.value.w - r.value.w) < 1e-4f);
        }
    }

    [Fact]
    public void Mul_ByInverse_IsIdentity()
    {
        for (var i = 0; i < 64; i++)
        {
            var q = RandomUnitQuaternion();
            var r = math.mul(q, math.inverse(q));
            var dotToIdentity = System.Math.Abs(math.dot(r, quaternion.identity));
            Assert.True(dotToIdentity > 1f - 1e-3f);
        }
    }

    [Fact]
    public void RotateVector_PreservesLength()
    {
        for (var i = 0; i < 64; i++)
        {
            var q = RandomUnitQuaternion();
            var v = new float3(NextFloat() * 10f, NextFloat() * 10f, NextFloat() * 10f);
            var rotated = math.rotate(q, v);
            Approx(math.length(v), math.length(rotated));
        }
    }

    [Fact]
    public void Slerp_AtEndpoints_MatchesInputs()
    {
        for (var i = 0; i < 32; i++)
        {
            var a = RandomUnitQuaternion();
            var b = RandomUnitQuaternion();

            var atZero = math.slerp(a, b, 0f);
            var dotAtZero = System.Math.Abs(math.dot(atZero, a));
            Assert.True(dotAtZero > 1f - 1e-3f);

            var atOne = math.slerp(a, b, 1f);
            var dotAtOne = System.Math.Abs(math.dot(atOne, b));
            Assert.True(dotAtOne > 1f - 1e-3f);
        }
    }

    [Fact]
    public void AxisAngle_RoundTrips_ThroughMatrix()
    {
        for (var i = 0; i < 32; i++)
        {
            var axis = math.normalize(new float3(NextFloat(), NextFloat(), NextFloat()) + new float3(0.01f));
            var angle = NextFloat() * math.PI;
            var q = quaternion.AxisAngle(axis, angle);

            var m = new float3x3(q);
            var roundTripped = new quaternion(m);

            var d = System.Math.Abs(math.dot(q, roundTripped));
            Assert.True(d > 1f - 1e-2f);
        }
    }
}
