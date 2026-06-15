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

#region TransformPoint

public class Matrix4x4TransformPoint : BenchBase
{
    private float4x4 _sa;
    private float3 _sv;
    private Matrix4x4 _ma;
    private Vector3 _mv;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF());
        _sv = new(NextF(), NextF(), NextF());
        _ma = new(_sa.c0.x, _sa.c0.y, _sa.c0.z, _sa.c0.w, _sa.c1.x, _sa.c1.y, _sa.c1.z, _sa.c1.w, _sa.c2.x, _sa.c2.y, _sa.c2.z, _sa.c2.w, _sa.c3.x, _sa.c3.y, _sa.c3.z, _sa.c3.w);
        _mv = new(_sv.x, _sv.y, _sv.z);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("TransformPoint")]
    public void Sys_TransformPoint() { var r = Vector3.Transform(_mv, _ma); Sink(r.X); }

    [Benchmark, BenchmarkCategory("TransformPoint")]
    public void Sia_TransformPoint() { var r = math.mul(_sa, new float4(_sv, 1f)); Sink(r.x); }
}

#endregion

#region TransformNormal

public class Matrix4x4TransformNormal : BenchBase
{
    private float4x4 _sa;
    private float3 _sv;
    private Matrix4x4 _ma;
    private Vector3 _mv;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF());
        _sv = new(NextF(), NextF(), NextF());
        _ma = new(_sa.c0.x, _sa.c0.y, _sa.c0.z, _sa.c0.w, _sa.c1.x, _sa.c1.y, _sa.c1.z, _sa.c1.w, _sa.c2.x, _sa.c2.y, _sa.c2.z, _sa.c2.w, _sa.c3.x, _sa.c3.y, _sa.c3.z, _sa.c3.w);
        _mv = new(_sv.x, _sv.y, _sv.z);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("TransformNormal")]
    public void Sys_TransformNormal() { var r = Vector3.TransformNormal(_mv, _ma); Sink(r.X); }

    [Benchmark, BenchmarkCategory("TransformNormal")]
    public void Sia_TransformNormal() { var r = math.mul(new float3x3(_sa), _sv); Sink(r.x); }
}

#endregion

#region Add

public class Matrix4x4Add : BenchBase
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

    [Benchmark(Baseline = true), BenchmarkCategory("Add")]
    public void Sys_Add() { var r = Matrix4x4.Add(_ma, _mb); Sink(r.M11); }

    [Benchmark, BenchmarkCategory("Add")]
    public void Sia_Add() { var r = _sa + _sb; Sink(r.c0.x); }
}

#endregion

#region Subtract

public class Matrix4x4Subtract : BenchBase
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

    [Benchmark(Baseline = true), BenchmarkCategory("Subtract")]
    public void Sys_Subtract() { var r = Matrix4x4.Subtract(_ma, _mb); Sink(r.M11); }

    [Benchmark, BenchmarkCategory("Subtract")]
    public void Sia_Subtract() { var r = _sa - _sb; Sink(r.c0.x); }
}

#endregion

#region Negate

public class Matrix4x4Negate : BenchBase
{
    private float4x4 _sa;
    private Matrix4x4 _ma;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF());
        _ma = new(_sa.c0.x, _sa.c0.y, _sa.c0.z, _sa.c0.w, _sa.c1.x, _sa.c1.y, _sa.c1.z, _sa.c1.w, _sa.c2.x, _sa.c2.y, _sa.c2.z, _sa.c2.w, _sa.c3.x, _sa.c3.y, _sa.c3.z, _sa.c3.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Negate")]
    public void Sys_Negate() { var r = Matrix4x4.Negate(_ma); Sink(r.M11); }

    [Benchmark, BenchmarkCategory("Negate")]
    public void Sia_Negate() { var r = -_sa; Sink(r.c0.x); }
}

#endregion

#region ScalarMul

public class Matrix4x4ScalarMul : BenchBase
{
    private float4x4 _sa;
    private float _s;
    private Matrix4x4 _ma;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF(), NextF());
        _s = NextF();
        _ma = new(_sa.c0.x, _sa.c0.y, _sa.c0.z, _sa.c0.w, _sa.c1.x, _sa.c1.y, _sa.c1.z, _sa.c1.w, _sa.c2.x, _sa.c2.y, _sa.c2.z, _sa.c2.w, _sa.c3.x, _sa.c3.y, _sa.c3.z, _sa.c3.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("ScalarMul")]
    public void Sys_ScalarMul() { var r = Matrix4x4.Multiply(_ma, _s); Sink(r.M11); }

    [Benchmark, BenchmarkCategory("ScalarMul")]
    public void Sia_ScalarMul() { var r = _sa * _s; Sink(r.c0.x); }
}

#endregion

#region Lerp

public class Matrix4x4Lerp : BenchBase
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

    [Benchmark(Baseline = true), BenchmarkCategory("Lerp")]
    public void Sys_Lerp() { var r = Matrix4x4.Lerp(_ma, _mb, 0.5f); Sink(r.M11); }

    [Benchmark, BenchmarkCategory("Lerp")]
    public void Sia_Lerp() { var r = _sa + (_sb - _sa) * 0.5f; Sink(r.c0.x); }
}

#endregion

#region Equals

public class Matrix4x4Equals : BenchBase
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

    [Benchmark(Baseline = true), BenchmarkCategory("Equals")]
    public void Sys_Equals() => Sink(_ma.Equals(_mb));

    [Benchmark, BenchmarkCategory("Equals")]
    public void Sia_Equals() => Sink(_sa.Equals(_sb));
}

#endregion
