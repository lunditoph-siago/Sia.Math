using Sia.Math;

namespace Sia.Math.Tests;

public class TransformTests
{
    private static readonly Random Rng = new(31415926);

    private static float NextFloat() => (float)(Rng.NextDouble() * 10.0 - 5.0);

    private static float3 RandomPoint() => new(NextFloat(), NextFloat(), NextFloat());

    private static quaternion RandomUnitQuaternion()
    {
        var q = new quaternion(NextFloat(), NextFloat(), NextFloat(), NextFloat());
        return math.normalize(q);
    }

    private static void AssertApprox(float3 expected, float3 actual, float tol = 1e-2f)
    {
        Assert.True(math.length(expected - actual) < tol, $"expected {expected}, got {actual}");
    }

    [Fact]
    public void AffineTransform_ToFloat4x4_AppliesRotationAndTranslationToPoint()
    {
        for (var i = 0; i < 32; i++)
        {
            var q = RandomUnitQuaternion();
            var translation = RandomPoint();
            var affine = new AffineTransform(translation, q);

            float4x4 m = affine;
            var p = RandomPoint();

            // row-vector convention: transform a homogeneous point via v*M
            var homogeneous = new float4(p, 1f);
            var transformed = math.mul(homogeneous, m);

            var expected = math.rotate(q, p) + translation;
            AssertApprox(expected, transformed.xyz);
            Assert.True(System.Math.Abs(transformed.w - 1f) < 1e-3f);
        }
    }

    [Fact]
    public void AffineTransform_WithScale_ScalesBeforeRotating()
    {
        for (var i = 0; i < 32; i++)
        {
            var q = RandomUnitQuaternion();
            var translation = RandomPoint();
            var scale = new float3(1f + (float)Rng.NextDouble(), 1f + (float)Rng.NextDouble(), 1f + (float)Rng.NextDouble());
            var affine = new AffineTransform(translation, q, scale);

            float4x4 m = affine;
            var p = RandomPoint();

            var homogeneous = new float4(p, 1f);
            var transformed = math.mul(homogeneous, m);

            var expected = math.rotate(q, p * scale) + translation;
            AssertApprox(expected, transformed.xyz);
        }
    }

    [Fact]
    public void RigidTransform_RoundTripsThroughFloat4x4()
    {
        for (var i = 0; i < 32; i++)
        {
            var q = RandomUnitQuaternion();
            var translation = RandomPoint();
            var original = new RigidTransform(q, translation);

            AffineTransform affine = new(translation, q);
            float4x4 m = affine;
            var reconstructed = new RigidTransform(m);

            var p = RandomPoint();
            var expected = math.transform(original, p);
            var actual = math.transform(reconstructed, p);
            AssertApprox(expected, actual);
        }
    }

    [Fact]
    public void MatrixTransform_Mul_MatchesRotateThenTranslate()
    {
        for (var i = 0; i < 32; i++)
        {
            var q = RandomUnitQuaternion();
            var translation = RandomPoint();
            var t = new MatrixTransform(translation, q);
            var p = RandomPoint();

            var actual = math.mul(t, p);
            var expected = math.rotate(q, p) + translation;
            AssertApprox(expected, actual);
        }
    }

    [Fact]
    public void MatrixTransform_Inverse_UndoesTransform()
    {
        for (var i = 0; i < 32; i++)
        {
            var q = RandomUnitQuaternion();
            var translation = RandomPoint();
            var t = new MatrixTransform(translation, q);
            var inv = math.inverse(t);
            var p = RandomPoint();

            var transformed = math.mul(t, p);
            var back = math.mul(inv, transformed);
            AssertApprox(p, back);
        }
    }

    [Fact]
    public void MatrixTransform_Composition_MatchesSequentialApplication()
    {
        for (var i = 0; i < 32; i++)
        {
            var qA = RandomUnitQuaternion();
            var tA = new MatrixTransform(RandomPoint(), qA);
            var qB = RandomUnitQuaternion();
            var tB = new MatrixTransform(RandomPoint(), qB);
            var p = RandomPoint();

            var composed = math.mul(tA, tB);
            var sequential = math.mul(tA, math.mul(tB, p));
            var direct = math.mul(composed, p);

            AssertApprox(sequential, direct);
        }
    }
}
