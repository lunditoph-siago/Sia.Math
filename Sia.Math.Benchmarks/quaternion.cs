using System.Numerics;
using BenchmarkDotNet.Attributes;

namespace Sia.Math.Benchmarks;

#region CreateFromAxisAngle

[BenchmarkCategory("LowPriority")]
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

#region EulerXYZ

[BenchmarkCategory("LowPriority")]
public class QuaternionEulerXYZ : BenchBase
{
    private float3 _sxyz;
    private Vector3 _mxyz;

    protected override void OnSetup()
    {
        _sxyz = new(NextF() * 0.03f, NextF() * 0.03f, NextF() * 0.03f);
        _mxyz = new(_sxyz.x, _sxyz.y, _sxyz.z);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("EulerXYZ")]
    public void Sys_EulerXYZ()
    {
        var r = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, _mxyz.Z) * Quaternion.CreateFromAxisAngle(Vector3.UnitY, _mxyz.Y) * Quaternion.CreateFromAxisAngle(Vector3.UnitX, _mxyz.X);
        Sink(r.X);
    }

    [Benchmark, BenchmarkCategory("EulerXYZ")]
    public void Sia_EulerXYZ() { var r = quaternion.EulerXYZ(_sxyz); Sink(r.value.x); }
}

#endregion

#region EulerXZY

[BenchmarkCategory("LowPriority")]
public class QuaternionEulerXZY : BenchBase
{
    private float3 _sxyz;
    private Vector3 _mxyz;

    protected override void OnSetup()
    {
        _sxyz = new(NextF() * 0.03f, NextF() * 0.03f, NextF() * 0.03f);
        _mxyz = new(_sxyz.x, _sxyz.y, _sxyz.z);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("EulerXZY")]
    public void Sys_EulerXZY()
    {
        var r = Quaternion.CreateFromAxisAngle(Vector3.UnitY, _mxyz.Y) * Quaternion.CreateFromAxisAngle(Vector3.UnitZ, _mxyz.Z) * Quaternion.CreateFromAxisAngle(Vector3.UnitX, _mxyz.X);
        Sink(r.X);
    }

    [Benchmark, BenchmarkCategory("EulerXZY")]
    public void Sia_EulerXZY() { var r = quaternion.EulerXZY(_sxyz); Sink(r.value.x); }
}

#endregion

#region EulerYXZ

[BenchmarkCategory("LowPriority")]
public class QuaternionEulerYXZ : BenchBase
{
    private float3 _sxyz;
    private Vector3 _mxyz;

    protected override void OnSetup()
    {
        _sxyz = new(NextF() * 0.03f, NextF() * 0.03f, NextF() * 0.03f);
        _mxyz = new(_sxyz.x, _sxyz.y, _sxyz.z);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("EulerYXZ")]
    public void Sys_EulerYXZ()
    {
        var r = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, _mxyz.Z) * Quaternion.CreateFromAxisAngle(Vector3.UnitX, _mxyz.X) * Quaternion.CreateFromAxisAngle(Vector3.UnitY, _mxyz.Y);
        Sink(r.X);
    }

    [Benchmark, BenchmarkCategory("EulerYXZ")]
    public void Sia_EulerYXZ() { var r = quaternion.EulerYXZ(_sxyz); Sink(r.value.x); }
}

#endregion

#region EulerYZX

[BenchmarkCategory("LowPriority")]
public class QuaternionEulerYZX : BenchBase
{
    private float3 _sxyz;
    private Vector3 _mxyz;

    protected override void OnSetup()
    {
        _sxyz = new(NextF() * 0.03f, NextF() * 0.03f, NextF() * 0.03f);
        _mxyz = new(_sxyz.x, _sxyz.y, _sxyz.z);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("EulerYZX")]
    public void Sys_EulerYZX()
    {
        var r = Quaternion.CreateFromAxisAngle(Vector3.UnitX, _mxyz.X) * Quaternion.CreateFromAxisAngle(Vector3.UnitZ, _mxyz.Z) * Quaternion.CreateFromAxisAngle(Vector3.UnitY, _mxyz.Y);
        Sink(r.X);
    }

    [Benchmark, BenchmarkCategory("EulerYZX")]
    public void Sia_EulerYZX() { var r = quaternion.EulerYZX(_sxyz); Sink(r.value.x); }
}

#endregion

#region EulerZXY

[BenchmarkCategory("LowPriority")]
public class QuaternionEulerZXY : BenchBase
{
    private float3 _sxyz;
    private Vector3 _mxyz;

    protected override void OnSetup()
    {
        _sxyz = new(NextF() * 0.03f, NextF() * 0.03f, NextF() * 0.03f);
        _mxyz = new(_sxyz.x, _sxyz.y, _sxyz.z);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("EulerZXY")]
    public void Sys_EulerZXY()
    {
        var r = Quaternion.CreateFromAxisAngle(Vector3.UnitY, _mxyz.Y) * Quaternion.CreateFromAxisAngle(Vector3.UnitX, _mxyz.X) * Quaternion.CreateFromAxisAngle(Vector3.UnitZ, _mxyz.Z);
        Sink(r.X);
    }

    [Benchmark, BenchmarkCategory("EulerZXY")]
    public void Sia_EulerZXY() { var r = quaternion.EulerZXY(_sxyz); Sink(r.value.x); }
}

#endregion

#region EulerZYX

[BenchmarkCategory("LowPriority")]
public class QuaternionEulerZYX : BenchBase
{
    private float3 _sxyz;
    private Vector3 _mxyz;

    protected override void OnSetup()
    {
        _sxyz = new(NextF() * 0.03f, NextF() * 0.03f, NextF() * 0.03f);
        _mxyz = new(_sxyz.x, _sxyz.y, _sxyz.z);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("EulerZYX")]
    public void Sys_EulerZYX()
    {
        var r = Quaternion.CreateFromAxisAngle(Vector3.UnitX, _mxyz.X) * Quaternion.CreateFromAxisAngle(Vector3.UnitY, _mxyz.Y) * Quaternion.CreateFromAxisAngle(Vector3.UnitZ, _mxyz.Z);
        Sink(r.X);
    }

    [Benchmark, BenchmarkCategory("EulerZYX")]
    public void Sia_EulerZYX() { var r = quaternion.EulerZYX(_sxyz); Sink(r.value.x); }
}

#endregion

#region RotateX

[BenchmarkCategory("LowPriority")]
public class QuaternionRotateX : BenchBase
{
    private float _sangle;
    private float _mangle;

    protected override void OnSetup()
    {
        _sangle = NextF() * 0.03f;
        _mangle = _sangle;
    }

    [Benchmark(Baseline = true), BenchmarkCategory("RotateX")]
    public void Sys_RotateX() { var r = Quaternion.CreateFromAxisAngle(Vector3.UnitX, _mangle); Sink(r.X); }

    [Benchmark, BenchmarkCategory("RotateX")]
    public void Sia_RotateX() { var r = quaternion.RotateX(_sangle); Sink(r.value.x); }
}

#endregion

#region RotateY

[BenchmarkCategory("LowPriority")]
public class QuaternionRotateY : BenchBase
{
    private float _sangle;
    private float _mangle;

    protected override void OnSetup()
    {
        _sangle = NextF() * 0.03f;
        _mangle = _sangle;
    }

    [Benchmark(Baseline = true), BenchmarkCategory("RotateY")]
    public void Sys_RotateY() { var r = Quaternion.CreateFromAxisAngle(Vector3.UnitY, _mangle); Sink(r.X); }

    [Benchmark, BenchmarkCategory("RotateY")]
    public void Sia_RotateY() { var r = quaternion.RotateY(_sangle); Sink(r.value.x); }
}

#endregion

#region RotateZ

[BenchmarkCategory("LowPriority")]
public class QuaternionRotateZ : BenchBase
{
    private float _sangle;
    private float _mangle;

    protected override void OnSetup()
    {
        _sangle = NextF() * 0.03f;
        _mangle = _sangle;
    }

    [Benchmark(Baseline = true), BenchmarkCategory("RotateZ")]
    public void Sys_RotateZ() { var r = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, _mangle); Sink(r.X); }

    [Benchmark, BenchmarkCategory("RotateZ")]
    public void Sia_RotateZ() { var r = quaternion.RotateZ(_sangle); Sink(r.value.x); }
}

#endregion

#region LookRotation

[BenchmarkCategory("LowPriority")]
public class QuaternionLookRotation : BenchBase
{
    private float3 _sforward, _sup;
    private Vector3 _mforward, _mup;

    protected override void OnSetup()
    {
        _sforward = math.normalize(new float3(NextF(), NextF(), NextF()));
        _sup = new(0f, 1f, 0f);
        _mforward = new(_sforward.x, _sforward.y, _sforward.z);
        _mup = new(0f, 1f, 0f);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("LookRotation")]
    public void Sys_LookRotation()
    {
        var t = Vector3.Normalize(Vector3.Cross(_mup, _mforward));
        var u = Vector3.Cross(_mforward, t);
        var m = new Matrix4x4(
            t.X, t.Y, t.Z, 0f,
            u.X, u.Y, u.Z, 0f,
            _mforward.X, _mforward.Y, _mforward.Z, 0f,
            0f, 0f, 0f, 1f);
        var r = Quaternion.CreateFromRotationMatrix(m);
        Sink(r.X);
    }

    [Benchmark, BenchmarkCategory("LookRotation")]
    public void Sia_LookRotation() { var r = quaternion.LookRotation(_sforward, _sup); Sink(r.value.x); }
}

#endregion

#region LookRotationSafe

[BenchmarkCategory("LowPriority")]
public class QuaternionLookRotationSafe : BenchBase
{
    private float3 _sforward, _sup;
    private Vector3 _mforward, _mup;

    protected override void OnSetup()
    {
        _sforward = math.normalize(new float3(NextF(), NextF(), NextF()));
        _sup = new(0f, 1f, 0f);
        _mforward = new(_sforward.x, _sforward.y, _sforward.z);
        _mup = new(0f, 1f, 0f);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("LookRotationSafe")]
    public void Sys_LookRotationSafe()
    {
        var forwardLenSq = _mforward.LengthSquared();
        var upLenSq = _mup.LengthSquared();
        var forward = _mforward / MathF.Sqrt(forwardLenSq);
        var up = _mup / MathF.Sqrt(upLenSq);
        var t = Vector3.Cross(up, forward);
        var tLenSq = t.LengthSquared();
        t /= MathF.Sqrt(tLenSq);
        var u = Vector3.Cross(forward, t);
        var accept = forwardLenSq > 1e-35f && upLenSq > 1e-35f && tLenSq > 1e-35f;
        if (accept)
        {
            var m = new Matrix4x4(
                t.X, t.Y, t.Z, 0f,
                u.X, u.Y, u.Z, 0f,
                forward.X, forward.Y, forward.Z, 0f,
                0f, 0f, 0f, 1f);
            var r = Quaternion.CreateFromRotationMatrix(m);
            Sink(r.X);
        }
        else
        {
            Sink(Quaternion.Identity.X);
        }
    }

    [Benchmark, BenchmarkCategory("LookRotationSafe")]
    public void Sia_LookRotationSafe() { var r = quaternion.LookRotationSafe(_sforward, _sup); Sink(r.value.x); }
}

#endregion

#region CreateFromRotationMatrix

[BenchmarkCategory("LowPriority")]
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

#region RotationFromMatrix

[BenchmarkCategory("HighPriority")]
public class QuaternionRotationFromMatrix : BenchBase
{
    private float3x3 _sa;
    private Matrix4x4 _ma;

    protected override void OnSetup()
    {
        var axis = math.normalize(new float3(NextF(), NextF(), NextF()));
        var angle = NextF() * 0.03f;
        _ma = Matrix4x4.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(new(axis.x, axis.y, axis.z), angle));
        _sa = new(
            new float3(_ma.M11, _ma.M12, _ma.M13),
            new float3(_ma.M21, _ma.M22, _ma.M23),
            new float3(_ma.M31, _ma.M32, _ma.M33));
    }

    [Benchmark(Baseline = true), BenchmarkCategory("RotationFromMatrix")]
    public void Sys_RotationFromMatrix() { var r = Quaternion.CreateFromRotationMatrix(_ma); Sink(r.X); }

    [Benchmark, BenchmarkCategory("RotationFromMatrix")]
    public void Sia_RotationFromMatrix() { var r = math.rotation(_sa); Sink(r.value.x); }
}

#endregion

#region Mul

[BenchmarkCategory("Normal")]
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

#region Rotate

[BenchmarkCategory("LowPriority")]
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

#region Conjugate

[BenchmarkCategory("LowPriority")]
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

[BenchmarkCategory("LowPriority")]
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

[BenchmarkCategory("LowPriority")]
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

#region Length

[BenchmarkCategory("LowPriority")]
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

[BenchmarkCategory("LowPriority")]
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

#region Normalize

[BenchmarkCategory("Normal")]
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

#region NormalizeSafe

[BenchmarkCategory("LowPriority")]
public class QuaternionNormalizeSafe : BenchBase
{
    private quaternion _sa;
    private Quaternion _qa;

    protected override void OnSetup()
    {
        _sa = quaternion.AxisAngle(math.normalize(new float3(NextF(), NextF(), NextF())), NextF() * 0.03f);
        _qa = new(_sa.value.x, _sa.value.y, _sa.value.z, _sa.value.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("NormalizeSafe")]
    public void Sys_NormalizeSafe()
    {
        var lenSq = _qa.LengthSquared();
        var r = lenSq > 1.175494351e-38f ? _qa * (1f / MathF.Sqrt(lenSq)) : Quaternion.Identity;
        Sink(r.X);
    }

    [Benchmark, BenchmarkCategory("NormalizeSafe")]
    public void Sia_NormalizeSafe() { var r = math.normalizesafe(_sa); Sink(r.value.x); }
}

#endregion

#region UnitExp

[BenchmarkCategory("LowPriority")]
public class QuaternionUnitExp : BenchBase
{
    private quaternion _sa;
    private Quaternion _qa;

    protected override void OnSetup()
    {
        _sa = quaternion.AxisAngle(math.normalize(new float3(NextF(), NextF(), NextF())), NextF() * 0.03f);
        _qa = new(_sa.value.x, _sa.value.y, _sa.value.z, 0f);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("UnitExp")]
    public void Sys_UnitExp()
    {
        var v = new Vector3(_qa.X, _qa.Y, _qa.Z);
        var vRcpLen = 1f / v.Length();
        var vLen = 1f / vRcpLen;
        var sin = MathF.Sin(vLen);
        var cos = MathF.Cos(vLen);
        var r = new Quaternion(v * vRcpLen * sin, cos);
        Sink(r.X);
    }

    [Benchmark, BenchmarkCategory("UnitExp")]
    public void Sia_UnitExp() { var r = math.unitexp(_sa); Sink(r.value.x); }
}

#endregion

#region Exp

[BenchmarkCategory("LowPriority")]
public class QuaternionExp : BenchBase
{
    private quaternion _sa;
    private Quaternion _qa;

    protected override void OnSetup()
    {
        _sa = quaternion.AxisAngle(math.normalize(new float3(NextF(), NextF(), NextF())), NextF() * 0.03f);
        _qa = new(_sa.value.x, _sa.value.y, _sa.value.z, _sa.value.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Exp")]
    public void Sys_Exp()
    {
        var v = new Vector3(_qa.X, _qa.Y, _qa.Z);
        var vRcpLen = 1f / v.Length();
        var vLen = 1f / vRcpLen;
        var sin = MathF.Sin(vLen);
        var cos = MathF.Cos(vLen);
        var r = new Quaternion(v * vRcpLen * sin, cos) * MathF.Exp(_qa.W);
        Sink(r.X);
    }

    [Benchmark, BenchmarkCategory("Exp")]
    public void Sia_Exp() { var r = math.exp(_sa); Sink(r.value.x); }
}

#endregion

#region UnitLog

[BenchmarkCategory("LowPriority")]
public class QuaternionUnitLog : BenchBase
{
    private quaternion _sa;
    private Quaternion _qa;

    protected override void OnSetup()
    {
        _sa = quaternion.AxisAngle(math.normalize(new float3(NextF(), NextF(), NextF())), NextF() * 0.03f);
        _qa = new(_sa.value.x, _sa.value.y, _sa.value.z, _sa.value.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("UnitLog")]
    public void Sys_UnitLog()
    {
        var w = System.Math.Clamp(_qa.W, -1f, 1f);
        var s = MathF.Acos(w) / MathF.Sqrt(1f - w * w);
        var r = new Quaternion(new Vector3(_qa.X, _qa.Y, _qa.Z) * s, 0f);
        Sink(r.X);
    }

    [Benchmark, BenchmarkCategory("UnitLog")]
    public void Sia_UnitLog() { var r = math.unitlog(_sa); Sink(r.value.x); }
}

#endregion

#region Log

[BenchmarkCategory("LowPriority")]
public class QuaternionLog : BenchBase
{
    private quaternion _sa;
    private Quaternion _qa;

    protected override void OnSetup()
    {
        _sa = quaternion.AxisAngle(math.normalize(new float3(NextF(), NextF(), NextF())), NextF() * 0.03f);
        _qa = new(_sa.value.x, _sa.value.y, _sa.value.z, _sa.value.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("Log")]
    public void Sys_Log()
    {
        var v = new Vector3(_qa.X, _qa.Y, _qa.Z);
        var vLenSq = v.LengthSquared();
        var qLenSq = vLenSq + _qa.W * _qa.W;
        var s = MathF.Acos(System.Math.Clamp(_qa.W / MathF.Sqrt(qLenSq), -1f, 1f)) / MathF.Sqrt(vLenSq);
        var r = new Quaternion(v * s, 0.5f * MathF.Log(qLenSq));
        Sink(r.X);
    }

    [Benchmark, BenchmarkCategory("Log")]
    public void Sia_Log() { var r = math.log(_sa); Sink(r.value.x); }
}

#endregion

#region Angle

[BenchmarkCategory("LowPriority")]
public class QuaternionAngle : BenchBase
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

    [Benchmark(Baseline = true), BenchmarkCategory("Angle")]
    public void Sys_Angle()
    {
        var d = Quaternion.Normalize(Quaternion.Conjugate(_qa) * _qb);
        var diff = MathF.Asin(new Vector3(d.X, d.Y, d.Z).Length());
        Sink(diff + diff);
    }

    [Benchmark, BenchmarkCategory("Angle")]
    public void Sia_Angle() { var r = math.angle(_sa, _sb); Sink(r); }
}

#endregion

#region Lerp

[BenchmarkCategory("LowPriority")]
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

[BenchmarkCategory("LowPriority")]
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
