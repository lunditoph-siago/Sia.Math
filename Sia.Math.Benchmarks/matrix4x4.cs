using System.Numerics;
using BenchmarkDotNet.Attributes;

namespace Sia.Math.Benchmarks;

#region Mul

public class Matrix4x4Mul : BenchBase
{
    private float4x4 _sa, _sb;
    private Matrix4x4 _ma, _mb;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF());
        _sb = new(NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF());
        _ma = new(_sa.c0.x, _sa.c0.y, _sa.c0.z, _sa.c0.w, _sa.c1.x, _sa.c1.y, _sa.c1.z, _sa.c1.w, _sa.c2.x, _sa.c2.y, _sa.c2.z, _sa.c2.w, _sa.c3.x, _sa.c3.y, _sa.c3.z, _sa.c3.w);
        _mb = new(_sb.c0.x, _sb.c0.y, _sb.c0.z, _sb.c0.w, _sb.c1.x, _sb.c1.y, _sb.c1.z, _sb.c1.w, _sb.c2.x, _sb.c2.y, _sb.c2.z, _sb.c2.w, _sb.c3.x, _sb.c3.y, _sb.c3.z, _sb.c3.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Mul")]
    public void Sys_Mul() { var r = Matrix4x4.Multiply(_ma, _mb); Sink(r.M11); }

    [Benchmark, BenchmarkCategory("Mul")]
    public void Sia_Mul() { var r = math.mul(_sa, _sb); Sink(r.c0.x); }
}

#endregion

#region Transpose

public class Matrix4x4Transpose : BenchBase
{
    private float4x4 _sa;
    private Matrix4x4 _ma;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF());
        _ma = new(_sa.c0.x, _sa.c0.y, _sa.c0.z, _sa.c0.w, _sa.c1.x, _sa.c1.y, _sa.c1.z, _sa.c1.w, _sa.c2.x, _sa.c2.y, _sa.c2.z, _sa.c2.w, _sa.c3.x, _sa.c3.y, _sa.c3.z, _sa.c3.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Transpose")]
    public void Sys_Transpose() { var r = Matrix4x4.Transpose(_ma); Sink(r.M11); }

    [Benchmark, BenchmarkCategory("Transpose")]
    public void Sia_Transpose() { var r = math.transpose(_sa); Sink(r.c0.x); }
}

#endregion

#region Invert

public class Matrix4x4Invert : BenchBase
{
    private float4x4 _sa;
    private Matrix4x4 _ma;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF());
        _ma = new(_sa.c0.x, _sa.c0.y, _sa.c0.z, _sa.c0.w, _sa.c1.x, _sa.c1.y, _sa.c1.z, _sa.c1.w, _sa.c2.x, _sa.c2.y, _sa.c2.z, _sa.c2.w, _sa.c3.x, _sa.c3.y, _sa.c3.z, _sa.c3.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Invert")]
    public void Sys_Invert() { Matrix4x4.Invert(_ma, out var r); Sink(r.M11); }

    [Benchmark, BenchmarkCategory("Invert")]
    public void Sia_Invert() { var r = math.inverse(_sa); Sink(r.c0.x); }
}

#endregion

#region Transform

public class Matrix4x4Transform : BenchBase
{
    private float4x4 _sa;
    private float4 _sv;
    private Matrix4x4 _ma;
    private Vector4 _mv;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF());
        _sv = new(NextF(), NextF(), NextF(), NextF());
        _ma = new(_sa.c0.x, _sa.c0.y, _sa.c0.z, _sa.c0.w, _sa.c1.x, _sa.c1.y, _sa.c1.z, _sa.c1.w, _sa.c2.x, _sa.c2.y, _sa.c2.z, _sa.c2.w, _sa.c3.x, _sa.c3.y, _sa.c3.z, _sa.c3.w);
        _mv = new(_sv.x, _sv.y, _sv.z, _sv.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Transform")]
    public void Sys_Transform() { var r = Vector4.Transform(_mv, _ma); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Transform")]
    public void Sia_Transform() { var r = math.mul(_sa, _sv); Sink(r.x); }
}

#endregion
