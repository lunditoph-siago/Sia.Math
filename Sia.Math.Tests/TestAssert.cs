using Xunit;

namespace Sia.Math.Tests;

public static class TestAssert
{
    public static void Approx(float expected, float actual, float relTol = 1e-4f) =>
        Assert.True(global::System.Math.Abs(expected - actual) <= relTol * global::System.Math.Max(1f, global::System.Math.Abs(expected)),
            $"expected {expected}, got {actual}");

    public static void Approx(double expected, double actual, double relTol = 1e-6) =>
        Assert.True(global::System.Math.Abs(expected - actual) <= relTol * global::System.Math.Max(1.0, global::System.Math.Abs(expected)),
            $"expected {expected}, got {actual}");

    public static void Approx(Sia.Math.float3 expected, Sia.Math.float3 actual, float tol = 1e-2f) =>
        Assert.True(Sia.Math.math.length(expected - actual) < tol, $"expected {expected}, got {actual}");
}
