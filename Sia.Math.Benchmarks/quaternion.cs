using System.Numerics;
using BenchmarkDotNet.Attributes;

namespace Sia.Math.Benchmarks;

#region Mul

public class QuaternionMul : BenchBase
{
    private quaternion _sa, _sb;
    private Quaternion _qa, _qb;

    protected override void OnSetup()
    {
        _sa = quaternion.AxisAngle(math.normalize(new float3(NextF(), NextF(), NextF())), NextF() * 0.03f);
        _sb = quaternion.AxisAngle(math.normalize(new float3(NextF(), NextF(), NextF())), NextF() * 0.03f);
        _qa = new(_sa.value.x, _sa.value.y, _sa.value.z, _sa.value.w);
        _qb = new(_sb.value.x, _sb.value.y, _sb.value.z, _sb.value.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Mul")]
    public void Sys_Mul() { var r = _qa * _qb; Sink(r.X); }

    [Benchmark, BenchmarkCategory("Mul")]
    public void Sia_Mul() { var r = math.mul(_sa, _sb); Sink(r.value.x); }
}

#endregion

#region Normalize

public class QuaternionNormalize : BenchBase
{
    private quaternion _sa;
    private Quaternion _qa;

    protected override void OnSetup()
    {
        _sa = quaternion.AxisAngle(math.normalize(new float3(NextF(), NextF(), NextF())), NextF() * 0.03f);
        _qa = new(_sa.value.x, _sa.value.y, _sa.value.z, _sa.value.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Normalize")]
    public void Sys_Normalize() { var r = Quaternion.Normalize(_qa); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Normalize")]
    public void Sia_Normalize() { var r = math.normalize(_sa); Sink(r.value.x); }
}

#endregion

#region Conjugate

public class QuaternionConjugate : BenchBase
{
    private quaternion _sa;
    private Quaternion _qa;

    protected override void OnSetup()
    {
        _sa = quaternion.AxisAngle(math.normalize(new float3(NextF(), NextF(), NextF())), NextF() * 0.03f);
        _qa = new(_sa.value.x, _sa.value.y, _sa.value.z, _sa.value.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Conjugate")]
    public void Sys_Conjugate() { var r = Quaternion.Conjugate(_qa); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Conjugate")]
    public void Sia_Conjugate() { var r = math.conjugate(_sa); Sink(r.value.x); }
}

#endregion

#region Inverse

public class QuaternionInverse : BenchBase
{
    private quaternion _sa;
    private Quaternion _qa;

    protected override void OnSetup()
    {
        _sa = quaternion.AxisAngle(math.normalize(new float3(NextF(), NextF(), NextF())), NextF() * 0.03f);
        _qa = new(_sa.value.x, _sa.value.y, _sa.value.z, _sa.value.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Inverse")]
    public void Sys_Inverse() { var r = Quaternion.Inverse(_qa); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Inverse")]
    public void Sia_Inverse() { var r = math.inverse(_sa); Sink(r.value.x); }
}

#endregion

#region Dot

public class QuaternionDot : BenchBase
{
    private quaternion _sa, _sb;
    private Quaternion _qa, _qb;

    protected override void OnSetup()
    {
        _sa = quaternion.AxisAngle(math.normalize(new float3(NextF(), NextF(), NextF())), NextF() * 0.03f);
        _sb = quaternion.AxisAngle(math.normalize(new float3(NextF(), NextF(), NextF())), NextF() * 0.03f);
        _qa = new(_sa.value.x, _sa.value.y, _sa.value.z, _sa.value.w);
        _qb = new(_sb.value.x, _sb.value.y, _sb.value.z, _sb.value.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Dot")]
    public void Sys_Dot() => Sink(Quaternion.Dot(_qa, _qb));

    [Benchmark, BenchmarkCategory("Dot")]
    public void Sia_Dot() => Sink(math.dot(_sa, _sb));
}

#endregion

#region Lerp

public class QuaternionLerp : BenchBase
{
    private quaternion _sa, _sb;
    private Quaternion _qa, _qb;

    protected override void OnSetup()
    {
        _sa = quaternion.AxisAngle(math.normalize(new float3(NextF(), NextF(), NextF())), NextF() * 0.03f);
        _sb = quaternion.AxisAngle(math.normalize(new float3(NextF(), NextF(), NextF())), NextF() * 0.03f);
        _qa = new(_sa.value.x, _sa.value.y, _sa.value.z, _sa.value.w);
        _qb = new(_sb.value.x, _sb.value.y, _sb.value.z, _sb.value.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Lerp")]
    public void Sys_Lerp() { var r = Quaternion.Lerp(_qa, _qb, 0.5f); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Lerp")]
    public void Sia_Lerp() { var r = math.nlerp(_sa, _sb, 0.5f); Sink(r.value.x); }
}

#endregion

#region Slerp

public class QuaternionSlerp : BenchBase
{
    private quaternion _sa, _sb;
    private Quaternion _qa, _qb;

    protected override void OnSetup()
    {
        _sa = quaternion.AxisAngle(math.normalize(new float3(NextF(), NextF(), NextF())), NextF() * 0.03f);
        _sb = quaternion.AxisAngle(math.normalize(new float3(NextF(), NextF(), NextF())), NextF() * 0.03f);
        _qa = new(_sa.value.x, _sa.value.y, _sa.value.z, _sa.value.w);
        _qb = new(_sb.value.x, _sb.value.y, _sb.value.z, _sb.value.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Slerp")]
    public void Sys_Slerp() { var r = Quaternion.Slerp(_qa, _qb, 0.5f); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Slerp")]
    public void Sia_Slerp() { var r = math.slerp(_sa, _sb, 0.5f); Sink(r.value.x); }
}

#endregion

#region Rotate

public class QuaternionRotate : BenchBase
{
    private quaternion _sa;
    private float3 _sv;
    private Quaternion _qa;
    private Vector3 _qv;

    protected override void OnSetup()
    {
        _sa = quaternion.AxisAngle(math.normalize(new float3(NextF(), NextF(), NextF())), NextF() * 0.03f);
        _sv = new(NextF(), NextF(), NextF());
        _qa = new(_sa.value.x, _sa.value.y, _sa.value.z, _sa.value.w);
        _qv = new(_sv.x, _sv.y, _sv.z);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Rotate")]
    public void Sys_Rotate() { var r = Vector3.Transform(_qv, _qa); Sink(r.X); }

    [Benchmark, BenchmarkCategory("Rotate")]
    public void Sia_Rotate() { var r = math.rotate(_sa, _sv); Sink(r.x); }
}

#endregion
