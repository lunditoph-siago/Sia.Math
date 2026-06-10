using Sia.Math;
using static Sia.Math.Tests.TestAssert;

namespace Sia.Math.Tests;

public class TransformTests
{
    private static readonly TestRng Rng = new(31415926);

    private static float3 RandomPoint() => new(Rng.Float(-5f, 5f), Rng.Float(-5f, 5f), Rng.Float(-5f, 5f));

    private static quaternion RandomUnitQuaternion()
    {
        var q = new quaternion(Rng.Float(-5f, 5f), Rng.Float(-5f, 5f), Rng.Float(-5f, 5f), Rng.Float(-5f, 5f));
        return math.normalize(q);
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

            var homogeneous = new float4(p, 1f);
            var transformed = math.mul(homogeneous, m);

            var expected = math.rotate(q, p) + translation;
            Approx(expected, transformed.xyz);
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
            var scale = new float3(1f + Rng.Float01(), 1f + Rng.Float01(), 1f + Rng.Float01());
            var affine = new AffineTransform(translation, q, scale);

            float4x4 m = affine;
            var p = RandomPoint();

            var homogeneous = new float4(p, 1f);
            var transformed = math.mul(homogeneous, m);

            var expected = math.rotate(q, p * scale) + translation;
            Approx(expected, transformed.xyz);
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
            Approx(math.transform(original, p), math.transform(reconstructed, p));
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

            Approx(math.rotate(q, p) + translation, math.mul(t, p));
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
            Approx(p, math.mul(inv, transformed));
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
            Approx(math.mul(tA, math.mul(tB, p)), math.mul(composed, p));
        }
    }
}
