using System.Runtime.CompilerServices;
#if !BROWSER
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
#endif

namespace Sia.Math.Benchmarks;

#if !BROWSER
[MemoryDiagnoser(false)]
[DisassemblyDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
#endif
public abstract class BenchBase
{
    private const int PoolSize = 1024;

    private float[] _pool = null!;
    private int _cursor;
    protected float Sunk;

#if !BROWSER
    [GlobalSetup]
#endif
    public void Setup()
    {
        _pool = new float[PoolSize * 8];
        var rng = new Random(42);
        for (var i = 0; i < _pool.Length; i++)
            _pool[i] = (float)(rng.NextDouble() * 200.0 - 100.0);
        _cursor = 0;
        OnSetup();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected float NextF() => _pool[_cursor = (_cursor + 1) % PoolSize];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected double NextD() => _pool[_cursor = (_cursor + 1) % PoolSize];

    [MethodImpl(MethodImplOptions.NoInlining)]
    protected void Sink(float x) => Sunk = x;

    [MethodImpl(MethodImplOptions.NoInlining)]
    protected void Sink(double x) => Sunk = (float)x;

    [MethodImpl(MethodImplOptions.NoInlining)]
    protected void Sink(bool x) => Sunk = x ? 1f : 0f;

    protected virtual void OnSetup() { }
}
