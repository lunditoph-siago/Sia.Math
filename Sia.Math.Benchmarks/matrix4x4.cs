using System.Numerics;
using BenchmarkDotNet.Attributes;

namespace Sia.Math.Benchmarks;

#region CreateAxisAngle

[BenchmarkCategory("LowPriority")]
public class Matrix4x4CreateAxisAngle : BenchBase
{
    private float3 _saxis;
    private float _sangle;
    private Vector3 _maxis;
    private float _mangle;

    protected override void OnSetup()
    {
        _saxis = math.normalize(new float3(NextF(), NextF(), NextF()));
        _sangle = NextF() * 0.03f;
        _maxis = new(_saxis.x, _saxis.y, _saxis.z);
        _mangle = _sangle;
    }

    [Benchmark(Baseline = true), BenchmarkCategory("CreateAxisAngle")]
    public void Sys_CreateAxisAngle() { var r = Matrix4x4.CreateFromAxisAngle(_maxis, _mangle); Sink(r.M11); }

    [Benchmark, BenchmarkCategory("CreateAxisAngle")]
    public void Sia_CreateAxisAngle() { var r = float4x4.AxisAngle(_saxis, _sangle); Sink(r.c0.x); }
}

#endregion

#region CreateEulerXYZ

[BenchmarkCategory("LowPriority")]
public class Matrix4x4CreateEulerXYZ : BenchBase
{
    private float3 _sxyz;
    private Vector3 _mxyz;

    protected override void OnSetup()
    {
        _sxyz = new(NextF() * 0.03f, NextF() * 0.03f, NextF() * 0.03f);
        _mxyz = new(_sxyz.x, _sxyz.y, _sxyz.z);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("CreateEulerXYZ")]
    public void Sys_CreateEulerXYZ()
    {
        var r = Matrix4x4.CreateRotationZ(_mxyz.Z) * Matrix4x4.CreateRotationY(_mxyz.Y) * Matrix4x4.CreateRotationX(_mxyz.X);
        Sink(r.M11);
    }

    [Benchmark, BenchmarkCategory("CreateEulerXYZ")]
    public void Sia_CreateEulerXYZ() { var r = float4x4.EulerXYZ(_sxyz); Sink(r.c0.x); }
}

#endregion

#region CreateEulerXZY

[BenchmarkCategory("LowPriority")]
public class Matrix4x4CreateEulerXZY : BenchBase
{
    private float3 _sxyz;
    private Vector3 _mxyz;

    protected override void OnSetup()
    {
        _sxyz = new(NextF() * 0.03f, NextF() * 0.03f, NextF() * 0.03f);
        _mxyz = new(_sxyz.x, _sxyz.y, _sxyz.z);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("CreateEulerXZY")]
    public void Sys_CreateEulerXZY()
    {
        var r = Matrix4x4.CreateRotationY(_mxyz.Y) * Matrix4x4.CreateRotationZ(_mxyz.Z) * Matrix4x4.CreateRotationX(_mxyz.X);
        Sink(r.M11);
    }

    [Benchmark, BenchmarkCategory("CreateEulerXZY")]
    public void Sia_CreateEulerXZY() { var r = float4x4.EulerXZY(_sxyz); Sink(r.c0.x); }
}

#endregion

#region CreateEulerYXZ

[BenchmarkCategory("LowPriority")]
public class Matrix4x4CreateEulerYXZ : BenchBase
{
    private float3 _sxyz;
    private Vector3 _mxyz;

    protected override void OnSetup()
    {
        _sxyz = new(NextF() * 0.03f, NextF() * 0.03f, NextF() * 0.03f);
        _mxyz = new(_sxyz.x, _sxyz.y, _sxyz.z);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("CreateEulerYXZ")]
    public void Sys_CreateEulerYXZ()
    {
        var r = Matrix4x4.CreateRotationZ(_mxyz.Z) * Matrix4x4.CreateRotationX(_mxyz.X) * Matrix4x4.CreateRotationY(_mxyz.Y);
        Sink(r.M11);
    }

    [Benchmark, BenchmarkCategory("CreateEulerYXZ")]
    public void Sia_CreateEulerYXZ() { var r = float4x4.EulerYXZ(_sxyz); Sink(r.c0.x); }
}

#endregion

#region CreateEulerYZX

[BenchmarkCategory("LowPriority")]
public class Matrix4x4CreateEulerYZX : BenchBase
{
    private float3 _sxyz;
    private Vector3 _mxyz;

    protected override void OnSetup()
    {
        _sxyz = new(NextF() * 0.03f, NextF() * 0.03f, NextF() * 0.03f);
        _mxyz = new(_sxyz.x, _sxyz.y, _sxyz.z);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("CreateEulerYZX")]
    public void Sys_CreateEulerYZX()
    {
        var r = Matrix4x4.CreateRotationX(_mxyz.X) * Matrix4x4.CreateRotationZ(_mxyz.Z) * Matrix4x4.CreateRotationY(_mxyz.Y);
        Sink(r.M11);
    }

    [Benchmark, BenchmarkCategory("CreateEulerYZX")]
    public void Sia_CreateEulerYZX() { var r = float4x4.EulerYZX(_sxyz); Sink(r.c0.x); }
}

#endregion

#region CreateEulerZXY

[BenchmarkCategory("LowPriority")]
public class Matrix4x4CreateEulerZXY : BenchBase
{
    private float3 _sxyz;
    private Vector3 _mxyz;

    protected override void OnSetup()
    {
        _sxyz = new(NextF() * 0.03f, NextF() * 0.03f, NextF() * 0.03f);
        _mxyz = new(_sxyz.x, _sxyz.y, _sxyz.z);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("CreateEulerZXY")]
    public void Sys_CreateEulerZXY()
    {
        var r = Matrix4x4.CreateRotationY(_mxyz.Y) * Matrix4x4.CreateRotationX(_mxyz.X) * Matrix4x4.CreateRotationZ(_mxyz.Z);
        Sink(r.M11);
    }

    [Benchmark, BenchmarkCategory("CreateEulerZXY")]
    public void Sia_CreateEulerZXY() { var r = float4x4.EulerZXY(_sxyz); Sink(r.c0.x); }
}

#endregion

#region CreateEulerZYX

[BenchmarkCategory("LowPriority")]
public class Matrix4x4CreateEulerZYX : BenchBase
{
    private float3 _sxyz;
    private Vector3 _mxyz;

    protected override void OnSetup()
    {
        _sxyz = new(NextF() * 0.03f, NextF() * 0.03f, NextF() * 0.03f);
        _mxyz = new(_sxyz.x, _sxyz.y, _sxyz.z);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("CreateEulerZYX")]
    public void Sys_CreateEulerZYX()
    {
        var r = Matrix4x4.CreateRotationX(_mxyz.X) * Matrix4x4.CreateRotationY(_mxyz.Y) * Matrix4x4.CreateRotationZ(_mxyz.Z);
        Sink(r.M11);
    }

    [Benchmark, BenchmarkCategory("CreateEulerZYX")]
    public void Sia_CreateEulerZYX() { var r = float4x4.EulerZYX(_sxyz); Sink(r.c0.x); }
}

#endregion

#region CreateRotateX

[BenchmarkCategory("LowPriority")]
public class Matrix4x4CreateRotateX : BenchBase
{
    private float _sangle;
    private float _mangle;

    protected override void OnSetup()
    {
        _sangle = NextF() * 0.03f;
        _mangle = _sangle;
    }

    [Benchmark(Baseline = true), BenchmarkCategory("CreateRotateX")]
    public void Sys_CreateRotateX() { var r = Matrix4x4.CreateRotationX(_mangle); Sink(r.M22); }

    [Benchmark, BenchmarkCategory("CreateRotateX")]
    public void Sia_CreateRotateX() { var r = float4x4.RotateX(_sangle); Sink(r.c1.y); }
}

#endregion

#region CreateRotateY

[BenchmarkCategory("LowPriority")]
public class Matrix4x4CreateRotateY : BenchBase
{
    private float _sangle;
    private float _mangle;

    protected override void OnSetup()
    {
        _sangle = NextF() * 0.03f;
        _mangle = _sangle;
    }

    [Benchmark(Baseline = true), BenchmarkCategory("CreateRotateY")]
    public void Sys_CreateRotateY() { var r = Matrix4x4.CreateRotationY(_mangle); Sink(r.M11); }

    [Benchmark, BenchmarkCategory("CreateRotateY")]
    public void Sia_CreateRotateY() { var r = float4x4.RotateY(_sangle); Sink(r.c0.x); }
}

#endregion

#region CreateRotateZ

[BenchmarkCategory("LowPriority")]
public class Matrix4x4CreateRotateZ : BenchBase
{
    private float _sangle;
    private float _mangle;

    protected override void OnSetup()
    {
        _sangle = NextF() * 0.03f;
        _mangle = _sangle;
    }

    [Benchmark(Baseline = true), BenchmarkCategory("CreateRotateZ")]
    public void Sys_CreateRotateZ() { var r = Matrix4x4.CreateRotationZ(_mangle); Sink(r.M11); }

    [Benchmark, BenchmarkCategory("CreateRotateZ")]
    public void Sia_CreateRotateZ() { var r = float4x4.RotateZ(_sangle); Sink(r.c0.x); }
}

#endregion

#region CreateOrtho

[BenchmarkCategory("LowPriority")]
public class Matrix4x4CreateOrtho : BenchBase
{
    private float _swidth, _sheight, _snear, _sfar;

    protected override void OnSetup()
    {
        _swidth = 50f + NextF();
        _sheight = 50f + NextF();
        _snear = 0.1f;
        _sfar = 100f + NextF();
    }

    [Benchmark(Baseline = true), BenchmarkCategory("CreateOrtho")]
    public void Sys_CreateOrtho() { var r = Matrix4x4.CreateOrthographic(_swidth, _sheight, _snear, _sfar); Sink(r.M11); }

    [Benchmark, BenchmarkCategory("CreateOrtho")]
    public void Sia_CreateOrtho() { var r = float4x4.Ortho(_swidth, _sheight, _snear, _sfar); Sink(r.c0.x); }
}

#endregion

#region CreateOrthoOffCenter

[BenchmarkCategory("LowPriority")]
public class Matrix4x4CreateOrthoOffCenter : BenchBase
{
    private float _sleft, _sright, _sbottom, _stop, _snear, _sfar;

    protected override void OnSetup()
    {
        _sleft = -50f + NextF() * 0.1f;
        _sright = 50f + NextF() * 0.1f;
        _sbottom = -50f + NextF() * 0.1f;
        _stop = 50f + NextF() * 0.1f;
        _snear = 0.1f;
        _sfar = 100f + NextF();
    }

    [Benchmark(Baseline = true), BenchmarkCategory("CreateOrthoOffCenter")]
    public void Sys_CreateOrthoOffCenter() { var r = Matrix4x4.CreateOrthographicOffCenter(_sleft, _sright, _sbottom, _stop, _snear, _sfar); Sink(r.M11); }

    [Benchmark, BenchmarkCategory("CreateOrthoOffCenter")]
    public void Sia_CreateOrthoOffCenter() { var r = float4x4.OrthoOffCenter(_sleft, _sright, _sbottom, _stop, _snear, _sfar); Sink(r.c0.x); }
}

#endregion

#region CreatePerspectiveFov

[BenchmarkCategory("LowPriority")]
public class Matrix4x4CreatePerspectiveFov : BenchBase
{
    private float _sfov, _saspect, _snear, _sfar;

    protected override void OnSetup()
    {
        _sfov = 1.0f;
        _saspect = 1.5f;
        _snear = 0.1f;
        _sfar = 100f + NextF();
    }

    [Benchmark(Baseline = true), BenchmarkCategory("CreatePerspectiveFov")]
    public void Sys_CreatePerspectiveFov() { var r = Matrix4x4.CreatePerspectiveFieldOfView(_sfov, _saspect, _snear, _sfar); Sink(r.M11); }

    [Benchmark, BenchmarkCategory("CreatePerspectiveFov")]
    public void Sia_CreatePerspectiveFov() { var r = float4x4.PerspectiveFov(_sfov, _saspect, _snear, _sfar); Sink(r.c0.x); }
}

#endregion

#region CreatePerspectiveOffCenter

[BenchmarkCategory("LowPriority")]
public class Matrix4x4CreatePerspectiveOffCenter : BenchBase
{
    private float _sleft, _sright, _sbottom, _stop, _snear, _sfar;

    protected override void OnSetup()
    {
        _sleft = -1f + NextF() * 0.001f;
        _sright = 1f + NextF() * 0.001f;
        _sbottom = -1f + NextF() * 0.001f;
        _stop = 1f + NextF() * 0.001f;
        _snear = 0.1f;
        _sfar = 100f + NextF();
    }

    [Benchmark(Baseline = true), BenchmarkCategory("CreatePerspectiveOffCenter")]
    public void Sys_CreatePerspectiveOffCenter() { var r = Matrix4x4.CreatePerspectiveOffCenter(_sleft, _sright, _sbottom, _stop, _snear, _sfar); Sink(r.M11); }

    [Benchmark, BenchmarkCategory("CreatePerspectiveOffCenter")]
    public void Sia_CreatePerspectiveOffCenter() { var r = float4x4.PerspectiveOffCenter(_sleft, _sright, _sbottom, _stop, _snear, _sfar); Sink(r.c0.x); }
}

#endregion

#region CreateTRS

[BenchmarkCategory("LowPriority")]
public class Matrix4x4CreateTRS : BenchBase
{
    private float3 _stranslation, _sscale;
    private quaternion _srotation;
    private Vector3 _mtranslation, _mscale;
    private Quaternion _mrotation;

    protected override void OnSetup()
    {
        _stranslation = new(NextF(), NextF(), NextF());
        _sscale = new(1f + NextF() * 0.01f, 1f + NextF() * 0.01f, 1f + NextF() * 0.01f);
        _srotation = quaternion.AxisAngle(math.normalize(new float3(NextF(), NextF(), NextF())), NextF() * 0.03f);
        _mtranslation = new(_stranslation.x, _stranslation.y, _stranslation.z);
        _mscale = new(_sscale.x, _sscale.y, _sscale.z);
        _mrotation = new(_srotation.value.x, _srotation.value.y, _srotation.value.z, _srotation.value.w);
    }

    [Benchmark(Baseline = true), BenchmarkCategory("CreateTRS")]
    public void Sys_CreateTRS()
    {
        var r = Matrix4x4.CreateScale(_mscale) * Matrix4x4.CreateFromQuaternion(_mrotation) * Matrix4x4.CreateTranslation(_mtranslation);
        Sink(r.M11);
    }

    [Benchmark, BenchmarkCategory("CreateTRS")]
    public void Sia_CreateTRS() { var r = float4x4.TRS(_stranslation, _srotation, _sscale); Sink(r.c0.x); }
}

#endregion

#region Mul

[BenchmarkCategory("LowPriority")]
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

[BenchmarkCategory("LowPriority")]
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

[BenchmarkCategory("HighPriority")]
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

[BenchmarkCategory("LowPriority")]
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

[BenchmarkCategory("LowPriority")]
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

[BenchmarkCategory("LowPriority")]
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

[BenchmarkCategory("LowPriority")]
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

[BenchmarkCategory("LowPriority")]
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

[BenchmarkCategory("LowPriority")]
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

[BenchmarkCategory("LowPriority")]
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

[BenchmarkCategory("LowPriority")]
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

[BenchmarkCategory("LowPriority")]
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
