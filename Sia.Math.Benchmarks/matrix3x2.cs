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

#region Mul

public class Matrix3x2Mul : BenchBase
{
    private float2x2 _sla, _slb;
    private float2 _sta, _stb;
    private Matrix3x2 _ma, _mb;

    protected override void OnSetup()
    {
        _sla = new(NextF(), NextF(), NextF(), NextF());
        _slb = new(NextF(), NextF(), NextF(), NextF());
        _sta = new(NextF(), NextF());
        _stb = new(NextF(), NextF());
        _ma = new(_sla.c0.x, _sla.c1.x, _sla.c0.y, _sla.c1.y, _sta.x, _sta.y);
        _mb = new(_slb.c0.x, _slb.c1.x, _slb.c0.y, _slb.c1.y, _stb.x, _stb.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Mul")]
    public void Sys_Mul() { var r = _ma * _mb; Sink(r.M11); }

    [Benchmark, BenchmarkCategory("Mul")]
    public void Sia_Mul()
    {
        var l = math.mul(_sla, _slb);
        var t = math.mul(_sta, _slb) + _stb;
        Sink(l.c0.x + t.x);
    }
}

#endregion

#region Add

public class Matrix3x2Add : BenchBase
{
    private float3x2 _sa, _sb;
    private Matrix3x2 _ma, _mb;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF(), NextF(), NextF());
        _sb = new(NextF(), NextF(), NextF(), NextF(), NextF(), NextF());
        _ma = new(_sa.c0.x, _sa.c1.x, _sa.c0.y, _sa.c1.y, _sa.c0.z, _sa.c1.z);
        _mb = new(_sb.c0.x, _sb.c1.x, _sb.c0.y, _sb.c1.y, _sb.c0.z, _sb.c1.z);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Add")]
    public void Sys_Add() { var r = Matrix3x2.Add(_ma, _mb); Sink(r.M11); }

    [Benchmark, BenchmarkCategory("Add")]
    public void Sia_Add() { var r = _sa + _sb; Sink(r.c0.x); }
}

#endregion

#region Subtract

public class Matrix3x2Subtract : BenchBase
{
    private float3x2 _sa, _sb;
    private Matrix3x2 _ma, _mb;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF(), NextF(), NextF());
        _sb = new(NextF(), NextF(), NextF(), NextF(), NextF(), NextF());
        _ma = new(_sa.c0.x, _sa.c1.x, _sa.c0.y, _sa.c1.y, _sa.c0.z, _sa.c1.z);
        _mb = new(_sb.c0.x, _sb.c1.x, _sb.c0.y, _sb.c1.y, _sb.c0.z, _sb.c1.z);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Subtract")]
    public void Sys_Subtract() { var r = Matrix3x2.Subtract(_ma, _mb); Sink(r.M11); }

    [Benchmark, BenchmarkCategory("Subtract")]
    public void Sia_Subtract() { var r = _sa - _sb; Sink(r.c0.x); }
}

#endregion

#region Negate

public class Matrix3x2Negate : BenchBase
{
    private float3x2 _sa;
    private Matrix3x2 _ma;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF(), NextF(), NextF());
        _ma = new(_sa.c0.x, _sa.c1.x, _sa.c0.y, _sa.c1.y, _sa.c0.z, _sa.c1.z);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Negate")]
    public void Sys_Negate() { var r = Matrix3x2.Negate(_ma); Sink(r.M11); }

    [Benchmark, BenchmarkCategory("Negate")]
    public void Sia_Negate() { var r = -_sa; Sink(r.c0.x); }
}

#endregion

#region ScalarMul

public class Matrix3x2ScalarMul : BenchBase
{
    private float3x2 _sa;
    private float _s;
    private Matrix3x2 _ma;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF(), NextF(), NextF());
        _s = NextF();
        _ma = new(_sa.c0.x, _sa.c1.x, _sa.c0.y, _sa.c1.y, _sa.c0.z, _sa.c1.z);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("ScalarMul")]
    public void Sys_ScalarMul() { var r = Matrix3x2.Multiply(_ma, _s); Sink(r.M11); }

    [Benchmark, BenchmarkCategory("ScalarMul")]
    public void Sia_ScalarMul() { var r = _sa * _s; Sink(r.c0.x); }
}

#endregion

#region Lerp

public class Matrix3x2Lerp : BenchBase
{
    private float3x2 _sa, _sb;
    private Matrix3x2 _ma, _mb;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF(), NextF(), NextF());
        _sb = new(NextF(), NextF(), NextF(), NextF(), NextF(), NextF());
        _ma = new(_sa.c0.x, _sa.c1.x, _sa.c0.y, _sa.c1.y, _sa.c0.z, _sa.c1.z);
        _mb = new(_sb.c0.x, _sb.c1.x, _sb.c0.y, _sb.c1.y, _sb.c0.z, _sb.c1.z);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Lerp")]
    public void Sys_Lerp() { var r = Matrix3x2.Lerp(_ma, _mb, 0.5f); Sink(r.M11); }

    [Benchmark, BenchmarkCategory("Lerp")]
    public void Sia_Lerp() { var r = _sa + (_sb - _sa) * 0.5f; Sink(r.c0.x); }
}

#endregion

#region Equals

public class Matrix3x2Equals : BenchBase
{
    private float3x2 _sa, _sb;
    private Matrix3x2 _ma, _mb;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF(), NextF(), NextF());
        _sb = new(NextF(), NextF(), NextF(), NextF(), NextF(), NextF());
        _ma = new(_sa.c0.x, _sa.c1.x, _sa.c0.y, _sa.c1.y, _sa.c0.z, _sa.c1.z);
        _mb = new(_sb.c0.x, _sb.c1.x, _sb.c0.y, _sb.c1.y, _sb.c0.z, _sb.c1.z);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Equals")]
    public void Sys_Equals() => Sink(_ma.Equals(_mb));

    [Benchmark, BenchmarkCategory("Equals")]
    public void Sia_Equals() => Sink(_sa.Equals(_sb));
}

#endregion
