using System.Numerics;
using BenchmarkDotNet.Attributes;

namespace Sia.Math.Benchmarks;

#region Transform

public class Matrix3x2Transform : BenchBase
{
    private float3x2 _sa;
    private float2 _sv;
    private Matrix3x2 _ma;
    private Vector2 _mv;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF(), NextF(), NextF());
        _sv = new(NextF(), NextF());
        _ma = new(_sa.c0.x, _sa.c1.x, _sa.c0.y, _sa.c1.y, _sa.c0.z, _sa.c1.z);
        _mv = new(_sv.x, _sv.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Transform")]
    public void Sys_Transform() { var r = Vector2.Transform(_mv, _ma); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Transform")]
    public void Sia_Transform() { var r = math.mul(_sa, _sv); Sink(r.x); }
}

#endregion

#region Invert

public class Matrix3x2Invert : BenchBase
{
    private float2x2 _sb;
    private Matrix3x2 _ma;

    protected override void OnSetup()
    {
        _sb = new(NextF(), NextF(), NextF(), NextF());
        _ma = new(_sb.c0.x, _sb.c0.y, _sb.c1.x, _sb.c1.y, _sb.c0.x, _sb.c0.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Invert")]
    public void Sys_Invert() { Matrix3x2.Invert(_ma, out var r); Sink(r.M11); }

    [Benchmark, BenchmarkCategory("Invert")]
    public void Sia_Invert() { var r = math.inverse(_sb); Sink(r.c0.x); }
}

#endregion
