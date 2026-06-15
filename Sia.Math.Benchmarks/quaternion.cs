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

#region Length

public class QuaternionLength : BenchBase
{
    private quaternion _sa;
    private Quaternion _qa;

    protected override void OnSetup()
    {
        _sa = quaternion.AxisAngle(math.normalize(new float3(NextF(), NextF(), NextF())), NextF() * 0.03f);
        _qa = new(_sa.value.x, _sa.value.y, _sa.value.z, _sa.value.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Length")]
    public void Sys_Length() => Sink(_qa.Length());

    [Benchmark, BenchmarkCategory("Length")]
    public void Sia_Length() => Sink(math.length(_sa));
}

#endregion

#region LengthSq

public class QuaternionLengthSq : BenchBase
{
    private quaternion _sa;
    private Quaternion _qa;

    protected override void OnSetup()
    {
        _sa = quaternion.AxisAngle(math.normalize(new float3(NextF(), NextF(), NextF())), NextF() * 0.03f);
        _qa = new(_sa.value.x, _sa.value.y, _sa.value.z, _sa.value.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("LengthSq")]
    public void Sys_LengthSq() => Sink(_qa.LengthSquared());

    [Benchmark, BenchmarkCategory("LengthSq")]
    public void Sia_LengthSq() => Sink(math.lengthsq(_sa));
}

#endregion

#region CreateFromAxisAngle

public class QuaternionCreateFromAxisAngle : BenchBase
{
    private float3 _saxis;
    private float _sangle;
    private Vector3 _vaxis;

    protected override void OnSetup()
    {
        _saxis = math.normalize(new float3(NextF(), NextF(), NextF()));
        _sangle = NextF() * 0.03f;
        _vaxis = new(_saxis.x, _saxis.y, _saxis.z);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("CreateFromAxisAngle")]
    public void Sys_CreateFromAxisAngle() { var r = Quaternion.CreateFromAxisAngle(_vaxis, _sangle); Sink(r.X); }

    [Benchmark, BenchmarkCategory("CreateFromAxisAngle")]
    public void Sia_CreateFromAxisAngle() { var r = quaternion.AxisAngle(_saxis, _sangle); Sink(r.value.x); }
}

#endregion

#region CreateFromRotationMatrix

public class QuaternionCreateFromRotationMatrix : BenchBase
{
    private float4x4 _sa;
    private Matrix4x4 _ma;

    protected override void OnSetup()
    {
        var axis = math.normalize(new float3(NextF(), NextF(), NextF()));
        var angle = NextF() * 0.03f;
        _ma = Matrix4x4.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(new(axis.x, axis.y, axis.z), angle));
        _sa = new(
            _ma.M11, _ma.M12, _ma.M13, _ma.M14,
            _ma.M21, _ma.M22, _ma.M23, _ma.M24,
            _ma.M31, _ma.M32, _ma.M33, _ma.M34,
            _ma.M41, _ma.M42, _ma.M43, _ma.M44);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("CreateFromRotationMatrix")]
    public void Sys_CreateFromRotationMatrix() { var r = Quaternion.CreateFromRotationMatrix(_ma); Sink(r.X); }

    [Benchmark, BenchmarkCategory("CreateFromRotationMatrix")]
    public void Sia_CreateFromRotationMatrix() { var r = math.quaternion(_sa); Sink(r.value.x); }
}

#endregion
