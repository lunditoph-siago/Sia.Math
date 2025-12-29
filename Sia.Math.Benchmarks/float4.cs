using System.Numerics;
using System.Runtime.Intrinsics;
using BenchmarkDotNet.Attributes;

namespace Sia.Math.Benchmarks;

#region Add

public class Float4Add : BenchBase
{
    private float4 _sa, _sb;
    private Vector4 _va, _vb;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF());
        _sb = new(NextF(), NextF(), NextF(), NextF());
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
        _vb = new(_sb.x, _sb.y, _sb.z, _sb.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Add")]
    public void Sys_Add() { var r = _va + _vb; Sink(r.X + r.Y); }

    [Benchmark, BenchmarkCategory("Add")]
    public void Sia_Add() { var r = _sa + _sb; Sink(r.x + r.y); }
}

#endregion

#region Mul

public class Float4Mul : BenchBase
{
    private float4 _sa, _sb;
    private Vector4 _va, _vb;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF());
        _sb = new(NextF(), NextF(), NextF(), NextF());
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
        _vb = new(_sb.x, _sb.y, _sb.z, _sb.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Mul")]
    public void Sys_Mul() { var r = _va * _vb; Sink(r.X); }

    [Benchmark, BenchmarkCategory("Mul")]
    public void Sia_Mul() { var r = _sa * _sb; Sink(r.x); }
}

#endregion

#region Dot

public class Float4Dot : BenchBase
{
    private float4 _sa, _sb;
    private Vector4 _va, _vb;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50, NextF() + 50, NextF() + 50);
        _sb = new(NextF() + 50, NextF() + 50, NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
        _vb = new(_sb.x, _sb.y, _sb.z, _sb.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Dot")]
    public void Sys_Dot() => Sink(Vector4.Dot(_va, _vb));

    [Benchmark, BenchmarkCategory("Dot")]
    public void Sia_Dot() => Sink(math.dot(_sa, _sb));
}

#endregion

#region Norm

public class Float4Norm : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50, NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Norm")]
    public void Sys_Norm() { var r = Vector4.Normalize(_va); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Norm")]
    public void Sia_Norm() { var r = math.normalize(_sa); Sink(r.x); }
}

#endregion

#region Min

public class Float4Min : BenchBase
{
    private float4 _sa, _sb;
    private Vector4 _va, _vb;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50, NextF() + 50, NextF() + 50);
        _sb = new(NextF() + 50, NextF() + 50, NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
        _vb = new(_sb.x, _sb.y, _sb.z, _sb.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Min")]
    public void Sys_Min() { var r = Vector4.Min(_va, _vb); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Min")]
    public void Sia_Min() { var r = math.min(_sa, _sb); Sink(r.x); }
}

#endregion

#region Max

public class Float4Max : BenchBase
{
    private float4 _sa, _sb;
    private Vector4 _va, _vb;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50, NextF() + 50, NextF() + 50);
        _sb = new(NextF() + 50, NextF() + 50, NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
        _vb = new(_sb.x, _sb.y, _sb.z, _sb.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Max")]
    public void Sys_Max() { var r = Vector4.Max(_va, _vb); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Max")]
    public void Sia_Max() { var r = math.max(_sa, _sb); Sink(r.x); }
}

#endregion

#region Abs

public class Float4Abs : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50, NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Abs")]
    public void Sys_Abs() { var r = Vector4.Abs(_va); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Abs")]
    public void Sia_Abs() { var r = math.abs(_sa); Sink(r.x); }
}

#endregion

#region Lerp

public class Float4Lerp : BenchBase
{
    private float4 _sa, _sb;
    private Vector4 _va, _vb;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50, NextF() + 50, NextF() + 50);
        _sb = new(NextF() + 50, NextF() + 50, NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
        _vb = new(_sb.x, _sb.y, _sb.z, _sb.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Lerp")]
    public void Sys_Lerp() { var r = Vector4.Lerp(_va, _vb, _va); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Lerp")]
    public void Sia_Lerp() { var r = math.lerp(_sa, _sb, _sa); Sink(r.x); }
}

#endregion

#region Exp2

public class Float4Exp2 : BenchBase
{
    private const float Ln2 = 0.6931472f;

    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() * 0.02f, NextF() * 0.02f, NextF() * 0.02f, NextF() * 0.02f);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Exp2")]
    public void Sys_Exp2() { var r = Vector128.Exp(_va.AsVector128() * Ln2).AsVector4(); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Exp2")]
    public void Sia_Exp2() { var r = math.exp2(_sa); Sink(r.x); }
}

#endregion

#region Exp10

public class Float4Exp10 : BenchBase
{
    private const float Ln10 = 2.3025851f;

    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() * 0.02f, NextF() * 0.02f, NextF() * 0.02f, NextF() * 0.02f);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Exp10")]
    public void Sys_Exp10() { var r = Vector128.Exp(_va.AsVector128() * Ln10).AsVector4(); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Exp10")]
    public void Sia_Exp10() { var r = math.exp10(_sa); Sink(r.x); }
}

#endregion

#region Log2

public class Float4Log2 : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50, NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Log2")]
    public void Sys_Log2() { var r = Vector128.Log2(_va.AsVector128()).AsVector4(); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Log2")]
    public void Sia_Log2() { var r = math.log2(_sa); Sink(r.x); }
}

#endregion

#region Log10

public class Float4Log10 : BenchBase
{
    private const float Log10E = 0.4342945f;

    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50, NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Log10")]
    public void Sys_Log10() { var r = (Vector128.Log(_va.AsVector128()) * Log10E).AsVector4(); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Log10")]
    public void Sia_Log10() { var r = math.log10(_sa); Sink(r.x); }
}

#endregion

#region Sqrt

public class Float4Sqrt : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50, NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Sqrt")]
    public void Sys_Sqrt() { var r = Vector4.SquareRoot(_va); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Sqrt")]
    public void Sia_Sqrt() { var r = math.sqrt(_sa); Sink(r.x); }
}

#endregion

#region Clamp

public class Float4Clamp : BenchBase
{
    private float4 _sa, _sb, _sc;
    private Vector4 _va, _vb, _vc;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50, NextF() + 50, NextF() + 50);
        _sb = new(NextF() + 50, NextF() + 50, NextF() + 50, NextF() + 50);
        _sc = new(NextF() + 50, NextF() + 50, NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
        _vb = new(_sb.x, _sb.y, _sb.z, _sb.w);
        _vc = new(_sc.x, _sc.y, _sc.z, _sc.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Clamp")]
    public void Sys_Clamp() { var r = Vector4.Clamp(_va, _vb, _vc); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Clamp")]
    public void Sia_Clamp() { var r = math.clamp(_sa, _sb, _sc); Sink(r.x); }
}

#endregion

#region Saturate

public class Float4Saturate : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50, NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Saturate")]
    public void Sys_Saturate() { var r = Vector4.Clamp(_va, Vector4.Zero, Vector4.One); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Saturate")]
    public void Sia_Saturate() { var r = math.saturate(_sa); Sink(r.x); }
}

#endregion

#region LengthSq

public class Float4LengthSq : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50, NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("LengthSq")]
    public void Sys_LengthSq() => Sink(_va.LengthSquared());

    [Benchmark, BenchmarkCategory("LengthSq")]
    public void Sia_LengthSq() => Sink(math.lengthsq(_sa));
}

#endregion

#region Length

public class Float4Length : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50, NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Length")]
    public void Sys_Length() => Sink(_va.Length());

    [Benchmark, BenchmarkCategory("Length")]
    public void Sia_Length() => Sink(math.length(_sa));
}

#endregion

#region Rcp

public class Float4Rcp : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50, NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Rcp")]
    public void Sys_Rcp() { var r = Vector4.One / _va; Sink(r.X); }

    [Benchmark, BenchmarkCategory("Rcp")]
    public void Sia_Rcp() { var r = math.rcp(_sa); Sink(r.x); }
}

#endregion

#region Rsqrt

public class Float4Rsqrt : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50, NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Rsqrt")]
    public void Sys_Rsqrt() { var r = Vector4.One / Vector4.SquareRoot(_va); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Rsqrt")]
    public void Sia_Rsqrt() { var r = math.rsqrt(_sa); Sink(r.x); }
}

#endregion

#region Mad

public class Float4Mad : BenchBase
{
    private float4 _sa, _sb, _sc;
    private Vector4 _va, _vb, _vc;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50, NextF() + 50, NextF() + 50);
        _sb = new(NextF() + 50, NextF() + 50, NextF() + 50, NextF() + 50);
        _sc = new(NextF() + 50, NextF() + 50, NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
        _vb = new(_sb.x, _sb.y, _sb.z, _sb.w);
        _vc = new(_sc.x, _sc.y, _sc.z, _sc.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Mad")]
    public void Sys_Mad() { var r = _va * _vb + _vc; Sink(r.X); }

    [Benchmark, BenchmarkCategory("Mad")]
    public void Sia_Mad() { var r = math.mad(_sa, _sb, _sc); Sink(r.x); }
}

#endregion

#region Csum

public class Float4Csum : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50, NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Csum")]
    public void Sys_Csum() => Sink(_va.X + _va.Y + _va.Z + _va.W);

    [Benchmark, BenchmarkCategory("Csum")]
    public void Sia_Csum() => Sink(math.csum(_sa));
}

#endregion

#region Unlerp

public class Float4Unlerp : BenchBase
{
    private float4 _sa, _sb, _sc;
    private Vector4 _va, _vb, _vc;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF());
        _sb = new(NextF(), NextF(), NextF(), NextF());
        _sc = new(NextF(), NextF(), NextF(), NextF());
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
        _vb = new(_sb.x, _sb.y, _sb.z, _sb.w);
        _vc = new(_sc.x, _sc.y, _sc.z, _sc.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Unlerp")]
    public void Sys_Unlerp() { var r = (_vc - _va) / (_vb - _va); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Unlerp")]
    public void Sia_Unlerp() { var r = math.unlerp(_sa, _sb, _sc); Sink(r.x); }
}

#endregion

#region Remap

public class Float4Remap : BenchBase
{
    private float4 _sa, _sb, _sc, _sd, _se;
    private Vector4 _va, _vb, _vc, _vd, _ve;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF());
        _sb = new(NextF() + 2, NextF() + 2, NextF() + 2, NextF() + 2);
        _sc = new(NextF(), NextF(), NextF(), NextF());
        _sd = new(NextF() + 2, NextF() + 2, NextF() + 2, NextF() + 2);
        _se = new(NextF(), NextF(), NextF(), NextF());
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
        _vb = new(_sb.x, _sb.y, _sb.z, _sb.w);
        _vc = new(_sc.x, _sc.y, _sc.z, _sc.w);
        _vd = new(_sd.x, _sd.y, _sd.z, _sd.w);
        _ve = new(_se.x, _se.y, _se.z, _se.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Remap")]
    public void Sys_Remap() { var t = (_ve - _va) / (_vb - _va); var r = Vector4.Lerp(_vc, _vd, t); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Remap")]
    public void Sia_Remap() { var r = math.remap(_sa, _sb, _sc, _sd, _se); Sink(r.x); }
}

#endregion

#region Step

public class Float4Step : BenchBase
{
    private float4 _sa, _sb;
    private Vector4 _va, _vb;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF());
        _sb = new(NextF(), NextF(), NextF(), NextF());
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
        _vb = new(_sb.x, _sb.y, _sb.z, _sb.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Step")]
    public void Sys_Step()
    {
        var edge = _va.AsVector128();
        var x = _vb.AsVector128();
        var r = Vector128.ConditionalSelect(Vector128.GreaterThanOrEqual(x, edge), Vector128.Create(1f), Vector128.Create(0f)).AsVector4();
        Sink(r.X);
    }

    [Benchmark, BenchmarkCategory("Step")]
    public void Sia_Step() { var r = math.step(_sa, _sb); Sink(r.x); }
}

#endregion

#region SmoothStep

public class Float4SmoothStep : BenchBase
{
    private float4 _sa, _sb, _sc;
    private Vector4 _va, _vb, _vc;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF());
        _sb = new(NextF(), NextF(), NextF(), NextF());
        _sc = new(NextF(), NextF(), NextF(), NextF());
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
        _vb = new(_sb.x, _sb.y, _sb.z, _sb.w);
        _vc = new(_sc.x, _sc.y, _sc.z, _sc.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("SmoothStep")]
    public void Sys_SmoothStep()
    {
        var t = Vector4.Clamp((_vc - _va) / (_vb - _va), Vector4.Zero, Vector4.One);
        var r = t * t * (new Vector4(3f) - 2f * t);
        Sink(r.X);
    }

    [Benchmark, BenchmarkCategory("SmoothStep")]
    public void Sia_SmoothStep() { var r = math.smoothstep(_sa, _sb, _sc); Sink(r.x); }
}

#endregion

#region Radians

public class Float4Radians : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF());
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Radians")]
    public void Sys_Radians() { var r = Vector128.DegreesToRadians(_va.AsVector128()).AsVector4(); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Radians")]
    public void Sia_Radians() { var r = math.radians(_sa); Sink(r.x); }
}

#endregion

#region Degrees

public class Float4Degrees : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF());
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Degrees")]
    public void Sys_Degrees() { var r = Vector128.RadiansToDegrees(_va.AsVector128()).AsVector4(); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Degrees")]
    public void Sia_Degrees() { var r = math.degrees(_sa); Sink(r.x); }
}

#endregion

#region SinCos

public class Float4SinCos : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF());
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("SinCos")]
    public void Sys_SinCos()
    {
        var v = _va.AsVector128();
        var s = Vector128.Sin(v).AsVector4();
        var c = Vector128.Cos(v).AsVector4();
        Sink(s.X + c.X);
    }

    [Benchmark, BenchmarkCategory("SinCos")]
    public void Sia_SinCos() { math.sincos(_sa, out var s, out var c); Sink(s.x + c.x); }
}

#endregion

#region Any

public class Float4Any : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF());
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Any")]
    public void Sys_Any() => Sink(_va.X != 0f || _va.Y != 0f || _va.Z != 0f || _va.W != 0f);

    [Benchmark, BenchmarkCategory("Any")]
    public void Sia_Any() => Sink(math.any(_sa));
}

#endregion

#region All

public class Float4All : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF());
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("All")]
    public void Sys_All() => Sink(_va.X != 0f && _va.Y != 0f && _va.Z != 0f && _va.W != 0f);

    [Benchmark, BenchmarkCategory("All")]
    public void Sia_All() => Sink(math.all(_sa));
}

#endregion

#region Select

public class Float4Select : BenchBase
{
    private float4 _sa, _sb;
    private Vector4 _va, _vb;
    private bool _test;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF());
        _sb = new(NextF(), NextF(), NextF(), NextF());
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
        _vb = new(_sb.x, _sb.y, _sb.z, _sb.w);
        _test = NextF() > 0f;
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Select")]
    public void Sys_Select() { var r = _test ? _vb : _va; Sink(r.X); }

    [Benchmark, BenchmarkCategory("Select")]
    public void Sia_Select() { var r = math.select(_sa, _sb, _test); Sink(r.x); }
}

#endregion

#region NormalizeSafe

public class Float4NormalizeSafe : BenchBase
{
    private float4 _sa, _sb;
    private Vector4 _va, _vb;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF());
        _sb = new(NextF(), NextF(), NextF(), NextF());
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
        _vb = new(_sb.x, _sb.y, _sb.z, _sb.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("NormalizeSafe")]
    public void Sys_NormalizeSafe()
    {
        var lenSq = Vector4.Dot(_va, _va);
        var r = lenSq > math.FLT_MIN_NORMAL ? _va / MathF.Sqrt(lenSq) : _vb;
        Sink(r.X);
    }

    [Benchmark, BenchmarkCategory("NormalizeSafe")]
    public void Sia_NormalizeSafe() { var r = math.normalizesafe(_sa, _sb); Sink(r.x); }
}

#endregion

#region Round

public class Float4Round : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF());
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Round")]
    public void Sys_Round() { var r = Vector128.Round(_va.AsVector128()).AsVector4(); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Round")]
    public void Sia_Round() { var r = math.round(_sa); Sink(r.x); }
}

#endregion

#region Trunc

public class Float4Trunc : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF());
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Trunc")]
    public void Sys_Trunc() { var r = Vector128.Truncate(_va.AsVector128()).AsVector4(); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Trunc")]
    public void Sia_Trunc() { var r = math.trunc(_sa); Sink(r.x); }
}

#endregion

#region Floor

public class Float4Floor : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF());
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Floor")]
    public void Sys_Floor() { var r = Vector128.Floor(_va.AsVector128()).AsVector4(); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Floor")]
    public void Sia_Floor() { var r = math.floor(_sa); Sink(r.x); }
}

#endregion

#region Ceil

public class Float4Ceil : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF());
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Ceil")]
    public void Sys_Ceil() { var r = Vector128.Ceiling(_va.AsVector128()).AsVector4(); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Ceil")]
    public void Sia_Ceil() { var r = math.ceil(_sa); Sink(r.x); }
}

#endregion

#region Frac

public class Float4Frac : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF());
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Frac")]
    public void Sys_Frac() { var v = _va.AsVector128(); var r = (v - Vector128.Floor(v)).AsVector4(); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Frac")]
    public void Sia_Frac() { var r = math.frac(_sa); Sink(r.x); }
}

#endregion

#region Sign

public class Float4Sign : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF());
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Sign")]
    public void Sys_Sign() { var r = new Vector4(MathF.Sign(_va.X), MathF.Sign(_va.Y), MathF.Sign(_va.Z), MathF.Sign(_va.W)); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Sign")]
    public void Sia_Sign() { var r = math.sign(_sa); Sink(r.x); }
}

#endregion

#region Fmod

public class Float4Fmod : BenchBase
{
    private float4 _sa, _sb;
    private Vector4 _va, _vb;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF(), NextF(), NextF());
        _sb = new(NextF() + 10, NextF() + 10, NextF() + 10, NextF() + 10);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
        _vb = new(_sb.x, _sb.y, _sb.z, _sb.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Fmod")]
    public void Sys_Fmod() { var r = new Vector4(_va.X % _vb.X, _va.Y % _vb.Y, _va.Z % _vb.Z, _va.W % _vb.W); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Fmod")]
    public void Sia_Fmod() { var r = math.fmod(_sa, _sb); Sink(r.x); }
}

#endregion

#region Sin

public class Float4Sin : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() * 0.5f + 0.5f, NextF() * 0.5f + 0.5f, NextF() * 0.5f + 0.5f, NextF() * 0.5f + 0.5f);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Sin")]
    public void Sys_Sin() { var r = Vector128.Sin(_va.AsVector128()).AsVector4(); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Sin")]
    public void Sia_Sin() { var r = math.sin(_sa); Sink(r.x); }
}

#endregion

#region Cos

public class Float4Cos : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() * 0.5f + 0.5f, NextF() * 0.5f + 0.5f, NextF() * 0.5f + 0.5f, NextF() * 0.5f + 0.5f);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Cos")]
    public void Sys_Cos() { var r = Vector128.Cos(_va.AsVector128()).AsVector4(); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Cos")]
    public void Sia_Cos() { var r = math.cos(_sa); Sink(r.x); }
}

#endregion

#region Exp

public class Float4Exp : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() * 0.5f + 0.5f, NextF() * 0.5f + 0.5f, NextF() * 0.5f + 0.5f, NextF() * 0.5f + 0.5f);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Exp")]
    public void Sys_Exp() { var r = Vector128.Exp(_va.AsVector128()).AsVector4(); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Exp")]
    public void Sia_Exp() { var r = math.exp(_sa); Sink(r.x); }
}

#endregion

#region Log

public class Float4Log : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() * 0.5f + 0.5f, NextF() * 0.5f + 0.5f, NextF() * 0.5f + 0.5f, NextF() * 0.5f + 0.5f);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Log")]
    public void Sys_Log() { var r = Vector128.Log(_va.AsVector128()).AsVector4(); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Log")]
    public void Sia_Log() { var r = math.log(_sa); Sink(r.x); }
}

#endregion

#region Tan

public class Float4Tan : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() / 100f, NextF() / 100f, NextF() / 100f, NextF() / 100f);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Tan")]
    public void Sys_Tan() { var r = new Vector4(MathF.Tan(_va.X), MathF.Tan(_va.Y), MathF.Tan(_va.Z), MathF.Tan(_va.W)); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Tan")]
    public void Sia_Tan() { var r = math.tan(_sa); Sink(r.x); }
}

#endregion

#region Asin

public class Float4Asin : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() / 100f, NextF() / 100f, NextF() / 100f, NextF() / 100f);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Asin")]
    public void Sys_Asin() { var r = new Vector4(MathF.Asin(_va.X), MathF.Asin(_va.Y), MathF.Asin(_va.Z), MathF.Asin(_va.W)); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Asin")]
    public void Sia_Asin() { var r = math.asin(_sa); Sink(r.x); }
}

#endregion

#region Acos

public class Float4Acos : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() / 100f, NextF() / 100f, NextF() / 100f, NextF() / 100f);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Acos")]
    public void Sys_Acos() { var r = new Vector4(MathF.Acos(_va.X), MathF.Acos(_va.Y), MathF.Acos(_va.Z), MathF.Acos(_va.W)); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Acos")]
    public void Sia_Acos() { var r = math.acos(_sa); Sink(r.x); }
}

#endregion

#region Atan

public class Float4Atan : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() / 50f, NextF() / 50f, NextF() / 50f, NextF() / 50f);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Atan")]
    public void Sys_Atan() { var r = new Vector4(MathF.Atan(_va.X), MathF.Atan(_va.Y), MathF.Atan(_va.Z), MathF.Atan(_va.W)); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Atan")]
    public void Sia_Atan() { var r = math.atan(_sa); Sink(r.x); }
}

#endregion

#region Atan2

public class Float4Atan2 : BenchBase
{
    private float4 _sa, _sb;
    private Vector4 _va, _vb;

    protected override void OnSetup()
    {
        _sa = new(NextF() / 50f, NextF() / 50f, NextF() / 50f, NextF() / 50f);
        _sb = new(NextF() + 2, NextF() + 2, NextF() + 2, NextF() + 2);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
        _vb = new(_sb.x, _sb.y, _sb.z, _sb.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Atan2")]
    public void Sys_Atan2() { var r = new Vector4(MathF.Atan2(_va.X, _vb.X), MathF.Atan2(_va.Y, _vb.Y), MathF.Atan2(_va.Z,_vb.Z), MathF.Atan2(_va.W, _vb.W)); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Atan2")]
    public void Sia_Atan2() { var r = math.atan2(_sa, _sb); Sink(r.x); }
}

#endregion

#region Pow

public class Float4Pow : BenchBase
{
    private float4 _sa, _sb;
    private Vector4 _va, _vb;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 2, NextF() + 2, NextF() + 2, NextF() + 2);
        _sb = new(NextF() * 0.03f, NextF() * 0.03f, NextF() * 0.03f, NextF() * 0.03f);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
        _vb = new(_sb.x, _sb.y, _sb.z, _sb.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Pow")]
    public void Sys_Pow() { var r = new Vector4(MathF.Pow(_va.X, _vb.X), MathF.Pow(_va.Y, _vb.Y), MathF.Pow(_va.Z, _vb.Z), MathF.Pow(_va.W, _vb.W)); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Pow")]
    public void Sia_Pow() { var r = math.pow(_sa, _sb); Sink(r.x); }
}

#endregion

#region Sinh

public class Float4Sinh : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() * 0.03f, NextF() * 0.03f, NextF() * 0.03f, NextF() * 0.03f);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Sinh")]
    public void Sys_Sinh() { var r = new Vector4(MathF.Sinh(_va.X), MathF.Sinh(_va.Y), MathF.Sinh(_va.Z), MathF.Sinh(_va.W)); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Sinh")]
    public void Sia_Sinh() { var r = math.sinh(_sa); Sink(r.x); }
}

#endregion

#region Cosh

public class Float4Cosh : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() * 0.03f, NextF() * 0.03f, NextF() * 0.03f, NextF() * 0.03f);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Cosh")]
    public void Sys_Cosh() { var r = new Vector4(MathF.Cosh(_va.X), MathF.Cosh(_va.Y), MathF.Cosh(_va.Z), MathF.Cosh(_va.W)); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Cosh")]
    public void Sia_Cosh() { var r = math.cosh(_sa); Sink(r.x); }
}

#endregion

#region Tanh

public class Float4Tanh : BenchBase
{
    private float4 _sa;
    private Vector4 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() * 0.03f, NextF() * 0.03f, NextF() * 0.03f, NextF() * 0.03f);
        _va = new(_sa.x, _sa.y, _sa.z, _sa.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Tanh")]
    public void Sys_Tanh() { var r = new Vector4(MathF.Tanh(_va.X), MathF.Tanh(_va.Y), MathF.Tanh(_va.Z), MathF.Tanh(_va.W)); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Tanh")]
    public void Sia_Tanh() { var r = math.tanh(_sa); Sink(r.x); }
}

#endregion
