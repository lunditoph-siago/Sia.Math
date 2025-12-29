using System.Numerics;
using System.Runtime.Intrinsics;
using BenchmarkDotNet.Attributes;

namespace Sia.Math.Benchmarks;

#region Add

public class Float2Add : BenchBase
{
    private float2 _sa, _sb;
    private Vector2 _va, _vb;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF());
        _sb = new(NextF(), NextF());
        _va = new(_sa.x, _sa.y);
        _vb = new(_sb.x, _sb.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Add")]
    public void Sys_Add() { var r = _va + _vb; Sink(r.X + r.Y); }

    [Benchmark, BenchmarkCategory("Add")]
    public void Sia_Add() { var r = _sa + _sb; Sink(r.x + r.y); }
}

#endregion

#region Mul

public class Float2Mul : BenchBase
{
    private float2 _sa, _sb;
    private Vector2 _va, _vb;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF());
        _sb = new(NextF(), NextF());
        _va = new(_sa.x, _sa.y);
        _vb = new(_sb.x, _sb.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Mul")]
    public void Sys_Mul() { var r = _va * _vb; Sink(r.X); }

    [Benchmark, BenchmarkCategory("Mul")]
    public void Sia_Mul() { var r = _sa * _sb; Sink(r.x); }
}

#endregion

#region Dot

public class Float2Dot : BenchBase
{
    private float2 _sa, _sb;
    private Vector2 _va, _vb;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50);
        _sb = new(NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y);
        _vb = new(_sb.x, _sb.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Dot")]
    public void Sys_Dot() => Sink(Vector2.Dot(_va, _vb));

    [Benchmark, BenchmarkCategory("Dot")]
    public void Sia_Dot() => Sink(math.dot(_sa, _sb));
}

#endregion

#region Norm

public class Float2Norm : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Norm")]
    public void Sys_Norm() { var r = Vector2.Normalize(_va); Sink(r.X + r.Y); }

    [Benchmark, BenchmarkCategory("Norm")]
    public void Sia_Norm() { var r = math.normalize(_sa); Sink(r.x + r.y); }
}

#endregion

#region Min

public class Float2Min : BenchBase
{
    private float2 _sa, _sb;
    private Vector2 _va, _vb;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50);
        _sb = new(NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y);
        _vb = new(_sb.x, _sb.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Min")]
    public void Sys_Min() { var r = Vector2.Min(_va, _vb); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Min")]
    public void Sia_Min() { var r = math.min(_sa, _sb); Sink(r.x); }
}

#endregion

#region Max

public class Float2Max : BenchBase
{
    private float2 _sa, _sb;
    private Vector2 _va, _vb;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50);
        _sb = new(NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y);
        _vb = new(_sb.x, _sb.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Max")]
    public void Sys_Max() { var r = Vector2.Max(_va, _vb); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Max")]
    public void Sia_Max() { var r = math.max(_sa, _sb); Sink(r.x); }
}

#endregion

#region Abs

public class Float2Abs : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Abs")]
    public void Sys_Abs() { var r = Vector2.Abs(_va); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Abs")]
    public void Sia_Abs() { var r = math.abs(_sa); Sink(r.x); }
}

#endregion

#region Lerp

public class Float2Lerp : BenchBase
{
    private float2 _sa, _sb;
    private Vector2 _va, _vb;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50);
        _sb = new(NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y);
        _vb = new(_sb.x, _sb.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Lerp")]
    public void Sys_Lerp() { var r = Vector2.Lerp(_va, _vb, _va); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Lerp")]
    public void Sia_Lerp() { var r = math.lerp(_sa, _sb, _sa); Sink(r.x); }
}

#endregion

#region Exp2

public class Float2Exp2 : BenchBase
{
    private const float Ln2 = 0.6931472f;

    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() * 0.02f, NextF() * 0.02f);
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Exp2")]
    public void Sys_Exp2() { var r = Vector128.Exp(_va.AsVector128() * Ln2).AsVector2(); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Exp2")]
    public void Sia_Exp2() { var r = math.exp2(_sa); Sink(r.x); }
}

#endregion

#region Exp10

public class Float2Exp10 : BenchBase
{
    private const float Ln10 = 2.3025851f;

    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() * 0.02f, NextF() * 0.02f);
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Exp10")]
    public void Sys_Exp10() { var r = Vector128.Exp(_va.AsVector128() * Ln10).AsVector2(); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Exp10")]
    public void Sia_Exp10() { var r = math.exp10(_sa); Sink(r.x); }
}

#endregion

#region Log2

public class Float2Log2 : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Log2")]
    public void Sys_Log2() { var r = Vector128.Log2(_va.AsVector128()).AsVector2(); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Log2")]
    public void Sia_Log2() { var r = math.log2(_sa); Sink(r.x); }
}

#endregion

#region Log10

public class Float2Log10 : BenchBase
{
    private const float Log10E = 0.4342945f;

    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Log10")]
    public void Sys_Log10() { var r = (Vector128.Log(_va.AsVector128()) * Log10E).AsVector2(); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Log10")]
    public void Sia_Log10() { var r = math.log10(_sa); Sink(r.x); }
}

#endregion

#region Sqrt

public class Float2Sqrt : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Sqrt")]
    public void Sys_Sqrt() { var r = Vector2.SquareRoot(_va); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Sqrt")]
    public void Sia_Sqrt() { var r = math.sqrt(_sa); Sink(r.x); }
}

#endregion

#region Clamp

public class Float2Clamp : BenchBase
{
    private float2 _sa, _sb, _sc;
    private Vector2 _va, _vb, _vc;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50);
        _sb = new(NextF() + 50, NextF() + 50);
        _sc = new(NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y);
        _vb = new(_sb.x, _sb.y);
        _vc = new(_sc.x, _sc.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Clamp")]
    public void Sys_Clamp() { var r = Vector2.Clamp(_va, _vb, _vc); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Clamp")]
    public void Sia_Clamp() { var r = math.clamp(_sa, _sb, _sc); Sink(r.x); }
}

#endregion

#region Saturate

public class Float2Saturate : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Saturate")]
    public void Sys_Saturate() { var r = Vector2.Clamp(_va, Vector2.Zero, Vector2.One); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Saturate")]
    public void Sia_Saturate() { var r = math.saturate(_sa); Sink(r.x); }
}

#endregion

#region LengthSq

public class Float2LengthSq : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("LengthSq")]
    public void Sys_LengthSq() => Sink(_va.LengthSquared());

    [Benchmark, BenchmarkCategory("LengthSq")]
    public void Sia_LengthSq() => Sink(math.lengthsq(_sa));
}

#endregion

#region Length

public class Float2Length : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Length")]
    public void Sys_Length() => Sink(_va.Length());

    [Benchmark, BenchmarkCategory("Length")]
    public void Sia_Length() => Sink(math.length(_sa));
}

#endregion

#region Rcp

public class Float2Rcp : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Rcp")]
    public void Sys_Rcp() { var r = Vector2.One / _va; Sink(r.X); }

    [Benchmark, BenchmarkCategory("Rcp")]
    public void Sia_Rcp() { var r = math.rcp(_sa); Sink(r.x); }
}

#endregion

#region Rsqrt

public class Float2Rsqrt : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Rsqrt")]
    public void Sys_Rsqrt() { var r = Vector2.One / Vector2.SquareRoot(_va); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Rsqrt")]
    public void Sia_Rsqrt() { var r = math.rsqrt(_sa); Sink(r.x); }
}

#endregion

#region Mad

public class Float2Mad : BenchBase
{
    private float2 _sa, _sb, _sc;
    private Vector2 _va, _vb, _vc;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50);
        _sb = new(NextF() + 50, NextF() + 50);
        _sc = new(NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y);
        _vb = new(_sb.x, _sb.y);
        _vc = new(_sc.x, _sc.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Mad")]
    public void Sys_Mad() { var r = _va * _vb + _vc; Sink(r.X); }

    [Benchmark, BenchmarkCategory("Mad")]
    public void Sia_Mad() { var r = math.mad(_sa, _sb, _sc); Sink(r.x); }
}

#endregion

#region Csum

public class Float2Csum : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 50, NextF() + 50);
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Csum")]
    public void Sys_Csum() => Sink(_va.X + _va.Y);

    [Benchmark, BenchmarkCategory("Csum")]
    public void Sia_Csum() => Sink(math.csum(_sa));
}

#endregion

#region Unlerp

public class Float2Unlerp : BenchBase
{
    private float2 _sa, _sb, _sc;
    private Vector2 _va, _vb, _vc;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF());
        _sb = new(NextF(), NextF());
        _sc = new(NextF(), NextF());
        _va = new(_sa.x, _sa.y);
        _vb = new(_sb.x, _sb.y);
        _vc = new(_sc.x, _sc.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Unlerp")]
    public void Sys_Unlerp() { var r = (_vc - _va) / (_vb - _va); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Unlerp")]
    public void Sia_Unlerp() { var r = math.unlerp(_sa, _sb, _sc); Sink(r.x); }
}

#endregion

#region Remap

public class Float2Remap : BenchBase
{
    private float2 _sa, _sb, _sc, _sd, _se;
    private Vector2 _va, _vb, _vc, _vd, _ve;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF());
        _sb = new(NextF() + 2, NextF() + 2);
        _sc = new(NextF(), NextF());
        _sd = new(NextF() + 2, NextF() + 2);
        _se = new(NextF(), NextF());
        _va = new(_sa.x, _sa.y);
        _vb = new(_sb.x, _sb.y);
        _vc = new(_sc.x, _sc.y);
        _vd = new(_sd.x, _sd.y);
        _ve = new(_se.x, _se.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Remap")]
    public void Sys_Remap() { var t = (_ve - _va) / (_vb - _va); var r = Vector2.Lerp(_vc, _vd, t); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Remap")]
    public void Sia_Remap() { var r = math.remap(_sa, _sb, _sc, _sd, _se); Sink(r.x); }
}

#endregion

#region Step

public class Float2Step : BenchBase
{
    private float2 _sa, _sb;
    private Vector2 _va, _vb;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF());
        _sb = new(NextF(), NextF());
        _va = new(_sa.x, _sa.y);
        _vb = new(_sb.x, _sb.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Step")]
    public void Sys_Step()
    {
        var edge = _va.AsVector128();
        var x = _vb.AsVector128();
        var r = Vector128.ConditionalSelect(Vector128.GreaterThanOrEqual(x, edge), Vector128.Create(1f), Vector128.Create(0f)).AsVector2();
        Sink(r.X);
    }

    [Benchmark, BenchmarkCategory("Step")]
    public void Sia_Step() { var r = math.step(_sa, _sb); Sink(r.x); }
}

#endregion

#region SmoothStep

public class Float2SmoothStep : BenchBase
{
    private float2 _sa, _sb, _sc;
    private Vector2 _va, _vb, _vc;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF());
        _sb = new(NextF(), NextF());
        _sc = new(NextF(), NextF());
        _va = new(_sa.x, _sa.y);
        _vb = new(_sb.x, _sb.y);
        _vc = new(_sc.x, _sc.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("SmoothStep")]
    public void Sys_SmoothStep()
    {
        var t = Vector2.Clamp((_vc - _va) / (_vb - _va), Vector2.Zero, Vector2.One);
        var r = t * t * (new Vector2(3f) - 2f * t);
        Sink(r.X);
    }

    [Benchmark, BenchmarkCategory("SmoothStep")]
    public void Sia_SmoothStep() { var r = math.smoothstep(_sa, _sb, _sc); Sink(r.x); }
}

#endregion

#region Radians

public class Float2Radians : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF());
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Radians")]
    public void Sys_Radians() { var r = Vector128.DegreesToRadians(_va.AsVector128()).AsVector2(); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Radians")]
    public void Sia_Radians() { var r = math.radians(_sa); Sink(r.x); }
}

#endregion

#region Degrees

public class Float2Degrees : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF());
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Degrees")]
    public void Sys_Degrees() { var r = Vector128.RadiansToDegrees(_va.AsVector128()).AsVector2(); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Degrees")]
    public void Sia_Degrees() { var r = math.degrees(_sa); Sink(r.x); }
}

#endregion

#region SinCos

public class Float2SinCos : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF());
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("SinCos")]
    public void Sys_SinCos()
    {
        var v = _va.AsVector128();
        var s = Vector128.Sin(v).AsVector2();
        var c = Vector128.Cos(v).AsVector2();
        Sink(s.X + c.X);
    }

    [Benchmark, BenchmarkCategory("SinCos")]
    public void Sia_SinCos() { math.sincos(_sa, out var s, out var c); Sink(s.x + c.x); }
}

#endregion

#region Any

public class Float2Any : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF());
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Any")]
    public void Sys_Any() => Sink(_va.X != 0f || _va.Y != 0f);

    [Benchmark, BenchmarkCategory("Any")]
    public void Sia_Any() => Sink(math.any(_sa));
}

#endregion

#region All

public class Float2All : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF());
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("All")]
    public void Sys_All() => Sink(_va.X != 0f && _va.Y != 0f);

    [Benchmark, BenchmarkCategory("All")]
    public void Sia_All() => Sink(math.all(_sa));
}

#endregion

#region Select

public class Float2Select : BenchBase
{
    private float2 _sa, _sb;
    private Vector2 _va, _vb;
    private bool _test;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF());
        _sb = new(NextF(), NextF());
        _va = new(_sa.x, _sa.y);
        _vb = new(_sb.x, _sb.y);
        _test = NextF() > 0f;
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Select")]
    public void Sys_Select() { var r = _test ? _vb : _va; Sink(r.X); }

    [Benchmark, BenchmarkCategory("Select")]
    public void Sia_Select() { var r = math.select(_sa, _sb, _test); Sink(r.x); }
}

#endregion

#region NormalizeSafe

public class Float2NormalizeSafe : BenchBase
{
    private float2 _sa, _sb;
    private Vector2 _va, _vb;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF());
        _sb = new(NextF(), NextF());
        _va = new(_sa.x, _sa.y);
        _vb = new(_sb.x, _sb.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("NormalizeSafe")]
    public void Sys_NormalizeSafe()
    {
        var lenSq = Vector2.Dot(_va, _va);
        var r = lenSq > math.FLT_MIN_NORMAL ? _va / MathF.Sqrt(lenSq) : _vb;
        Sink(r.X);
    }

    [Benchmark, BenchmarkCategory("NormalizeSafe")]
    public void Sia_NormalizeSafe() { var r = math.normalizesafe(_sa, _sb); Sink(r.x); }
}

#endregion

#region Round

public class Float2Round : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF());
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Round")]
    public void Sys_Round() { var r = Vector128.Round(_va.AsVector128()).AsVector2(); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Round")]
    public void Sia_Round() { var r = math.round(_sa); Sink(r.x); }
}

#endregion

#region Trunc

public class Float2Trunc : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF());
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Trunc")]
    public void Sys_Trunc() { var r = Vector128.Truncate(_va.AsVector128()).AsVector2(); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Trunc")]
    public void Sia_Trunc() { var r = math.trunc(_sa); Sink(r.x); }
}

#endregion

#region Floor

public class Float2Floor : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF());
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Floor")]
    public void Sys_Floor() { var r = Vector128.Floor(_va.AsVector128()).AsVector2(); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Floor")]
    public void Sia_Floor() { var r = math.floor(_sa); Sink(r.x); }
}

#endregion

#region Ceil

public class Float2Ceil : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF());
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Ceil")]
    public void Sys_Ceil() { var r = Vector128.Ceiling(_va.AsVector128()).AsVector2(); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Ceil")]
    public void Sia_Ceil() { var r = math.ceil(_sa); Sink(r.x); }
}

#endregion

#region Frac

public class Float2Frac : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF());
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Frac")]
    public void Sys_Frac() { var v = _va.AsVector128(); var r = (v - Vector128.Floor(v)).AsVector2(); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Frac")]
    public void Sia_Frac() { var r = math.frac(_sa); Sink(r.x); }
}

#endregion

#region Sign

public class Float2Sign : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF());
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Sign")]
    public void Sys_Sign() { var r = new Vector2(MathF.Sign(_va.X), MathF.Sign(_va.Y)); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Sign")]
    public void Sia_Sign() { var r = math.sign(_sa); Sink(r.x); }
}

#endregion

#region Fmod

public class Float2Fmod : BenchBase
{
    private float2 _sa, _sb;
    private Vector2 _va, _vb;

    protected override void OnSetup()
    {
        _sa = new(NextF(), NextF());
        _sb = new(NextF() + 10, NextF() + 10);
        _va = new(_sa.x, _sa.y);
        _vb = new(_sb.x, _sb.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Fmod")]
    public void Sys_Fmod() { var r = new Vector2(_va.X % _vb.X, _va.Y % _vb.Y); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Fmod")]
    public void Sia_Fmod() { var r = math.fmod(_sa, _sb); Sink(r.x); }
}

#endregion

#region Sin

public class Float2Sin : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() * 0.5f + 0.5f, NextF() * 0.5f + 0.5f);
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Sin")]
    public void Sys_Sin() { var r = Vector128.Sin(_va.AsVector128()).AsVector2(); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Sin")]
    public void Sia_Sin() { var r = math.sin(_sa); Sink(r.x); }
}

#endregion

#region Cos

public class Float2Cos : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() * 0.5f + 0.5f, NextF() * 0.5f + 0.5f);
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Cos")]
    public void Sys_Cos() { var r = Vector128.Cos(_va.AsVector128()).AsVector2(); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Cos")]
    public void Sia_Cos() { var r = math.cos(_sa); Sink(r.x); }
}

#endregion

#region Exp

public class Float2Exp : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() * 0.5f + 0.5f, NextF() * 0.5f + 0.5f);
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Exp")]
    public void Sys_Exp() { var r = Vector128.Exp(_va.AsVector128()).AsVector2(); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Exp")]
    public void Sia_Exp() { var r = math.exp(_sa); Sink(r.x); }
}

#endregion

#region Log

public class Float2Log : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() * 0.5f + 0.5f, NextF() * 0.5f + 0.5f);
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Log")]
    public void Sys_Log() { var r = Vector128.Log(_va.AsVector128()).AsVector2(); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Log")]
    public void Sia_Log() { var r = math.log(_sa); Sink(r.x); }
}

#endregion

#region Tan

public class Float2Tan : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() / 100f, NextF() / 100f);
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Tan")]
    public void Sys_Tan() { var r = new Vector2(MathF.Tan(_va.X), MathF.Tan(_va.Y)); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Tan")]
    public void Sia_Tan() { var r = math.tan(_sa); Sink(r.x); }
}

#endregion

#region Asin

public class Float2Asin : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() / 100f, NextF() / 100f);
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Asin")]
    public void Sys_Asin() { var r = new Vector2(MathF.Asin(_va.X), MathF.Asin(_va.Y)); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Asin")]
    public void Sia_Asin() { var r = math.asin(_sa); Sink(r.x); }
}

#endregion

#region Acos

public class Float2Acos : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() / 100f, NextF() / 100f);
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Acos")]
    public void Sys_Acos() { var r = new Vector2(MathF.Acos(_va.X), MathF.Acos(_va.Y)); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Acos")]
    public void Sia_Acos() { var r = math.acos(_sa); Sink(r.x); }
}

#endregion

#region Atan

public class Float2Atan : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() / 50f, NextF() / 50f);
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Atan")]
    public void Sys_Atan() { var r = new Vector2(MathF.Atan(_va.X), MathF.Atan(_va.Y)); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Atan")]
    public void Sia_Atan() { var r = math.atan(_sa); Sink(r.x); }
}

#endregion

#region Atan2

public class Float2Atan2 : BenchBase
{
    private float2 _sa, _sb;
    private Vector2 _va, _vb;

    protected override void OnSetup()
    {
        _sa = new(NextF() / 50f, NextF() / 50f);
        _sb = new(NextF() + 2, NextF() + 2);
        _va = new(_sa.x, _sa.y);
        _vb = new(_sb.x, _sb.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Atan2")]
    public void Sys_Atan2() { var r = new Vector2(MathF.Atan2(_va.X, _vb.X), MathF.Atan2(_va.Y, _vb.Y)); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Atan2")]
    public void Sia_Atan2() { var r = math.atan2(_sa, _sb); Sink(r.x); }
}

#endregion

#region Pow

public class Float2Pow : BenchBase
{
    private float2 _sa, _sb;
    private Vector2 _va, _vb;

    protected override void OnSetup()
    {
        _sa = new(NextF() + 2, NextF() + 2);
        _sb = new(NextF() * 0.03f, NextF() * 0.03f);
        _va = new(_sa.x, _sa.y);
        _vb = new(_sb.x, _sb.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Pow")]
    public void Sys_Pow() { var r = new Vector2(MathF.Pow(_va.X, _vb.X), MathF.Pow(_va.Y, _vb.Y)); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Pow")]
    public void Sia_Pow() { var r = math.pow(_sa, _sb); Sink(r.x); }
}

#endregion

#region Sinh

public class Float2Sinh : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() * 0.03f, NextF() * 0.03f);
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Sinh")]
    public void Sys_Sinh() { var r = new Vector2(MathF.Sinh(_va.X), MathF.Sinh(_va.Y)); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Sinh")]
    public void Sia_Sinh() { var r = math.sinh(_sa); Sink(r.x); }
}

#endregion

#region Cosh

public class Float2Cosh : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() * 0.03f, NextF() * 0.03f);
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Cosh")]
    public void Sys_Cosh() { var r = new Vector2(MathF.Cosh(_va.X), MathF.Cosh(_va.Y)); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Cosh")]
    public void Sia_Cosh() { var r = math.cosh(_sa); Sink(r.x); }
}

#endregion

#region Tanh

public class Float2Tanh : BenchBase
{
    private float2 _sa;
    private Vector2 _va;

    protected override void OnSetup()
    {
        _sa = new(NextF() * 0.03f, NextF() * 0.03f);
        _va = new(_sa.x, _sa.y);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Tanh")]
    public void Sys_Tanh() { var r = new Vector2(MathF.Tanh(_va.X), MathF.Tanh(_va.Y)); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Tanh")]
    public void Sia_Tanh() { var r = math.tanh(_sa); Sink(r.x); }
}

#endregion
