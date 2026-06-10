namespace Sia.Math.Tests;

public class TestRng(int seed)
{
    private readonly Random _rng = new(seed);

    public float Float(float min = -100f, float max = 100f) =>
        (float)(_rng.NextDouble() * (max - min) + min);

    public double Double(double min = -100.0, double max = 100.0) =>
        _rng.NextDouble() * (max - min) + min;

    public int Int(int min = -1000, int max = 1000) => _rng.Next(min, max);

    public float Float01() => (float)_rng.NextDouble();

    public uint UInt(int max = 1000) => (uint)_rng.Next(0, max > 0 ? max : int.MaxValue);
}
