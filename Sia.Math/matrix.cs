using System.Runtime.CompilerServices;
using static Sia.Math.math;

#pragma warning disable 8981

namespace Sia.Math;

public partial struct float2x2
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float2x2 Rotate(float angle)
    {
        sincos(angle, out var s, out var c);
        return float2x2(c, -s,
                        s,  c);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float2x2 Scale(float s) => float2x2(s,    0.0f,
                                                        0.0f, s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float2x2 Scale(float x, float y) => float2x2(x,    0.0f,
                                                                 0.0f, y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float2x2 Scale(float2 v) => Scale(v.x, v.y);
}

public partial struct float3x3
{
    public float3x3(in float4x4 m)
    {
        c0 = m.c0.xyz;
        c1 = m.c1.xyz;
        c2 = m.c2.xyz;
    }

    public float3x3(quaternion q)
    {
        var v = q.value;
        var v2 = v + v;

        var npn = new uint3(0x80000000, 0x00000000, 0x80000000);
        var nnp = new uint3(0x80000000, 0x80000000, 0x00000000);
        var pnn = new uint3(0x00000000, 0x80000000, 0x80000000);
        c0 = v2.y * math.asfloat(math.asuint(v.yxw) ^ npn) - v2.z * math.asfloat(math.asuint(v.zwx) ^ pnn) + new float3(1, 0, 0);
        c1 = v2.z * math.asfloat(math.asuint(v.wzy) ^ nnp) - v2.x * math.asfloat(math.asuint(v.yxw) ^ npn) + new float3(0, 1, 0);
        c2 = v2.x * math.asfloat(math.asuint(v.zwx) ^ pnn) - v2.y * math.asfloat(math.asuint(v.wzy) ^ nnp) + new float3(0, 0, 1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3x3 AxisAngle(float3 axis, float angle)
    {
        sincos(angle, out var sina, out var cosa);

        var u = axis;
        var u_inv_cosa = u - u * cosa;
        var t = float4(u * sina, cosa);

        var ppn = new uint3(0x00000000, 0x00000000, 0x80000000);
        var npp = new uint3(0x80000000, 0x00000000, 0x00000000);
        var pnp = new uint3(0x00000000, 0x80000000, 0x00000000);

        return float3x3(
            u.x * u_inv_cosa + asfloat(asuint(t.wzy) ^ ppn),
            u.y * u_inv_cosa + asfloat(asuint(t.zwx) ^ npp),
            u.z * u_inv_cosa + asfloat(asuint(t.yxw) ^ pnp)
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3x3 EulerXYZ(float3 xyz)
    {
        sincos(xyz, out var s, out var c);
        return float3x3(
            c.y * c.z,  c.z * s.x * s.y - c.x * s.z,    c.x * c.z * s.y + s.x * s.z,
            c.y * s.z,  c.x * c.z + s.x * s.y * s.z,    c.x * s.y * s.z - c.z * s.x,
            -s.y,       c.y * s.x,                      c.x * c.y
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3x3 EulerXZY(float3 xyz)
    {
        sincos(xyz, out var s, out var c);
        return float3x3(
            c.y * c.z,  s.x * s.y - c.x * c.y * s.z,    c.x * s.y + c.y * s.x * s.z,
            s.z,        c.x * c.z,                      -c.z * s.x,
            -c.z * s.y, c.y * s.x + c.x * s.y * s.z,    c.x * c.y - s.x * s.y * s.z
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3x3 EulerYXZ(float3 xyz)
    {
        sincos(xyz, out var s, out var c);
        return float3x3(
            c.y * c.z - s.x * s.y * s.z,    -c.x * s.z, c.z * s.y + c.y * s.x * s.z,
            c.z * s.x * s.y + c.y * s.z,    c.x * c.z,  s.y * s.z - c.y * c.z * s.x,
            -c.x * s.y,                     s.x,        c.x * c.y
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3x3 EulerYZX(float3 xyz)
    {
        sincos(xyz, out var s, out var c);
        return float3x3(
            c.y * c.z,                      -s.z,       c.z * s.y,
            s.x * s.y + c.x * c.y * s.z,    c.x * c.z,  c.x * s.y * s.z - c.y * s.x,
            c.y * s.x * s.z - c.x * s.y,    c.z * s.x,  c.x * c.y + s.x * s.y * s.z
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3x3 EulerZXY(float3 xyz)
    {
        sincos(xyz, out var s, out var c);
        return float3x3(
            c.y * c.z + s.x * s.y * s.z,    c.z * s.x * s.y - c.y * s.z,    c.x * s.y,
            c.x * s.z,                      c.x * c.z,                      -s.x,
            c.y * s.x * s.z - c.z * s.y,    c.y * c.z * s.x + s.y * s.z,    c.x * c.y
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3x3 EulerZYX(float3 xyz)
    {
        sincos(xyz, out var s, out var c);
        return float3x3(
            c.y * c.z,                      -c.y * s.z,                     s.y,
            c.z * s.x * s.y + c.x * s.z,    c.x * c.z - s.x * s.y * s.z,    -c.y * s.x,
            s.x * s.z - c.x * c.z * s.y,    c.z * s.x + c.x * s.y * s.z,    c.x * c.y
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3x3 EulerXYZ(float x, float y, float z) => EulerXYZ(float3(x, y, z));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3x3 EulerXZY(float x, float y, float z) => EulerXZY(float3(x, y, z));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3x3 EulerYXZ(float x, float y, float z) => EulerYXZ(float3(x, y, z));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3x3 EulerYZX(float x, float y, float z) => EulerYZX(float3(x, y, z));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3x3 EulerZXY(float x, float y, float z) => EulerZXY(float3(x, y, z));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3x3 EulerZYX(float x, float y, float z) => EulerZYX(float3(x, y, z));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3x3 Euler(float3 xyz, RotationOrder order = RotationOrder.Default) => order switch
    {
        RotationOrder.XYZ => EulerXYZ(xyz),
        RotationOrder.XZY => EulerXZY(xyz),
        RotationOrder.YXZ => EulerYXZ(xyz),
        RotationOrder.YZX => EulerYZX(xyz),
        RotationOrder.ZXY => EulerZXY(xyz),
        RotationOrder.ZYX => EulerZYX(xyz),
        _ => identity
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3x3 Euler(float x, float y, float z, RotationOrder order = RotationOrder.Default) => Euler(float3(x, y, z), order);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3x3 RotateX(float angle)
    {
        sincos(angle, out var s, out var c);
        return float3x3(1.0f, 0.0f, 0.0f,
                        0.0f, c,    -s,
                        0.0f, s,    c);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3x3 RotateY(float angle)
    {
        sincos(angle, out var s, out var c);
        return float3x3(c,    0.0f, s,
                        0.0f, 1.0f, 0.0f,
                        -s,   0.0f, c);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3x3 RotateZ(float angle)
    {
        sincos(angle, out var s, out var c);
        return float3x3(c,    -s,   0.0f,
                        s,    c,    0.0f,
                        0.0f, 0.0f, 1.0f);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3x3 Scale(float s) => float3x3(s,    0.0f, 0.0f,
                                                        0.0f, s,    0.0f,
                                                        0.0f, 0.0f, s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3x3 Scale(float x, float y, float z) => float3x3(x,    0.0f, 0.0f,
                                                                          0.0f, y,    0.0f,
                                                                          0.0f, 0.0f, z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3x3 Scale(float3 v) => Scale(v.x, v.y, v.z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3x3 LookRotation(float3 forward, float3 up)
    {
        var t = normalize(cross(up, forward));
        return float3x3(t, cross(forward, t), forward);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3x3 LookRotationSafe(float3 forward, float3 up)
    {
        var forwardLengthSq = dot(forward, forward);
        var upLengthSq = dot(up, up);

        forward *= rsqrt(forwardLengthSq);
        up *= rsqrt(upLengthSq);

        var t = cross(up, forward);
        var tLengthSq = dot(t, t);
        t *= rsqrt(tLengthSq);

        var mn = min(min(forwardLengthSq, upLengthSq), tLengthSq);
        var mx = max(max(forwardLengthSq, upLengthSq), tLengthSq);

        var accept = mn > 1e-35f && mx < 1e35f && isfinite(forwardLengthSq) && isfinite(upLengthSq) && isfinite(tLengthSq);
        return float3x3(
            select(float3(1, 0, 0), t, accept),
            select(float3(0, 1, 0), cross(forward, t), accept),
            select(float3(0, 0, 1), forward, accept));
    }
}

public partial struct float4x4
{
    public float4x4(in float3x3 rotation, float3 translation)
    {
        c0 = float4(rotation.c0, 0.0f);
        c1 = float4(rotation.c1, 0.0f);
        c2 = float4(rotation.c2, 0.0f);
        c3 = float4(translation, 1.0f);
    }

    public float4x4(quaternion rotation, float3 translation)
    {
        var rot = float3x3(rotation);
        c0 = float4(rot.c0, 0.0f);
        c1 = float4(rot.c1, 0.0f);
        c2 = float4(rot.c2, 0.0f);
        c3 = float4(translation, 1.0f);
    }

    public float4x4(in RigidTransform transform)
    {
        var rot = float3x3(transform.Rotation);
        c0 = float4(rot.c0, 0.0f);
        c1 = float4(rot.c1, 0.0f);
        c2 = float4(rot.c2, 0.0f);
        c3 = float4(transform.Translation, 1.0f);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 AxisAngle(float3 axis, float angle)
    {
        sincos(angle, out var sina, out var cosa);

        var u = float4(axis, 0.0f);
        var u_inv_cosa = u - u * cosa;
        var t = float4(u.xyz * sina, cosa);

        var ppnp = new uint4(0x00000000, 0x00000000, 0x80000000, 0x00000000);
        var nppp = new uint4(0x80000000, 0x00000000, 0x00000000, 0x00000000);
        var pnpp = new uint4(0x00000000, 0x80000000, 0x00000000, 0x00000000);
        var mask = new uint4(0xFFFFFFFF, 0xFFFFFFFF, 0xFFFFFFFF, 0x00000000);

        return float4x4(
            u.x * u_inv_cosa + asfloat((asuint(t.wzyx) ^ ppnp) & mask),
            u.y * u_inv_cosa + asfloat((asuint(t.zwxx) ^ nppp) & mask),
            u.z * u_inv_cosa + asfloat((asuint(t.yxwx) ^ pnpp) & mask),
            float4(0.0f, 0.0f, 0.0f, 1.0f)
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 EulerXYZ(float3 xyz)
    {
        sincos(xyz, out var s, out var c);
        return float4x4(
            c.y * c.z,  c.z * s.x * s.y - c.x * s.z,    c.x * c.z * s.y + s.x * s.z,    0.0f,
            c.y * s.z,  c.x * c.z + s.x * s.y * s.z,    c.x * s.y * s.z - c.z * s.x,    0.0f,
            -s.y,       c.y * s.x,                      c.x * c.y,                      0.0f,
            0.0f,       0.0f,                           0.0f,                           1.0f
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 EulerXZY(float3 xyz)
    {
        sincos(xyz, out var s, out var c);
        return float4x4(
            c.y * c.z,  s.x * s.y - c.x * c.y * s.z,    c.x * s.y + c.y * s.x * s.z,    0.0f,
            s.z,        c.x * c.z,                      -c.z * s.x,                     0.0f,
            -c.z * s.y, c.y * s.x + c.x * s.y * s.z,    c.x * c.y - s.x * s.y * s.z,    0.0f,
            0.0f,       0.0f,                           0.0f,                           1.0f
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 EulerYXZ(float3 xyz)
    {
        sincos(xyz, out var s, out var c);
        return float4x4(
            c.y * c.z - s.x * s.y * s.z,    -c.x * s.z, c.z * s.y + c.y * s.x * s.z,    0.0f,
            c.z * s.x * s.y + c.y * s.z,    c.x * c.z,  s.y * s.z - c.y * c.z * s.x,    0.0f,
            -c.x * s.y,                     s.x,        c.x * c.y,                      0.0f,
            0.0f,                           0.0f,       0.0f,                           1.0f
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 EulerYZX(float3 xyz)
    {
        sincos(xyz, out var s, out var c);
        return float4x4(
            c.y * c.z,                      -s.z,       c.z * s.y,                      0.0f,
            s.x * s.y + c.x * c.y * s.z,    c.x * c.z,  c.x * s.y * s.z - c.y * s.x,    0.0f,
            c.y * s.x * s.z - c.x * s.y,    c.z * s.x,  c.x * c.y + s.x * s.y * s.z,    0.0f,
            0.0f,                           0.0f,       0.0f,                           1.0f
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 EulerZXY(float3 xyz)
    {
        sincos(xyz, out var s, out var c);
        return float4x4(
            c.y * c.z + s.x * s.y * s.z,    c.z * s.x * s.y - c.y * s.z,    c.x * s.y,  0.0f,
            c.x * s.z,                      c.x * c.z,                      -s.x,       0.0f,
            c.y * s.x * s.z - c.z * s.y,    c.y * c.z * s.x + s.y * s.z,    c.x * c.y,  0.0f,
            0.0f,                           0.0f,                           0.0f,       1.0f
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 EulerZYX(float3 xyz)
    {
        sincos(xyz, out var s, out var c);
        return float4x4(
            c.y * c.z,                      -c.y * s.z,                     s.y,        0.0f,
            c.z * s.x * s.y + c.x * s.z,    c.x * c.z - s.x * s.y * s.z,    -c.y * s.x, 0.0f,
            s.x * s.z - c.x * c.z * s.y,    c.z * s.x + c.x * s.y * s.z,    c.x * c.y,  0.0f,
            0.0f,                           0.0f,                           0.0f,       1.0f
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 EulerXYZ(float x, float y, float z) => EulerXYZ(float3(x, y, z));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 EulerXZY(float x, float y, float z) => EulerXZY(float3(x, y, z));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 EulerYXZ(float x, float y, float z) => EulerYXZ(float3(x, y, z));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 EulerYZX(float x, float y, float z) => EulerYZX(float3(x, y, z));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 EulerZXY(float x, float y, float z) => EulerZXY(float3(x, y, z));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 EulerZYX(float x, float y, float z) => EulerZYX(float3(x, y, z));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 Euler(float3 xyz, RotationOrder order = RotationOrder.Default) => order switch
    {
        RotationOrder.XYZ => EulerXYZ(xyz),
        RotationOrder.XZY => EulerXZY(xyz),
        RotationOrder.YXZ => EulerYXZ(xyz),
        RotationOrder.YZX => EulerYZX(xyz),
        RotationOrder.ZXY => EulerZXY(xyz),
        RotationOrder.ZYX => EulerZYX(xyz),
        _ => identity
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 Euler(float x, float y, float z, RotationOrder order = RotationOrder.Default) => Euler(float3(x, y, z), order);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 RotateX(float angle)
    {
        sincos(angle, out var s, out var c);
        return float4x4(1.0f, 0.0f, 0.0f, 0.0f,
                        0.0f, c,    -s,   0.0f,
                        0.0f, s,    c,    0.0f,
                        0.0f, 0.0f, 0.0f, 1.0f);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 RotateY(float angle)
    {
        sincos(angle, out var s, out var c);
        return float4x4(c,    0.0f, s,    0.0f,
                        0.0f, 1.0f, 0.0f, 0.0f,
                        -s,   0.0f, c,    0.0f,
                        0.0f, 0.0f, 0.0f, 1.0f);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 RotateZ(float angle)
    {
        sincos(angle, out var s, out var c);
        return float4x4(c,    -s,   0.0f, 0.0f,
                        s,    c,    0.0f, 0.0f,
                        0.0f, 0.0f, 1.0f, 0.0f,
                        0.0f, 0.0f, 0.0f, 1.0f);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 Scale(float s) => float4x4(s,    0.0f, 0.0f, 0.0f,
                                                        0.0f, s,    0.0f, 0.0f,
                                                        0.0f, 0.0f, s,    0.0f,
                                                        0.0f, 0.0f, 0.0f, 1.0f);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 Scale(float x, float y, float z) => float4x4(x,    0.0f, 0.0f, 0.0f,
                                                                          0.0f, y,    0.0f, 0.0f,
                                                                          0.0f, 0.0f, z,    0.0f,
                                                                          0.0f, 0.0f, 0.0f, 1.0f);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 Scale(float3 scales) => Scale(scales.x, scales.y, scales.z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 Translate(float3 vector) => float4x4(
        float4(1.0f, 0.0f, 0.0f, 0.0f),
        float4(0.0f, 1.0f, 0.0f, 0.0f),
        float4(0.0f, 0.0f, 1.0f, 0.0f),
        float4(vector.x, vector.y, vector.z, 1.0f));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 LookAt(float3 eye, float3 target, float3 up)
    {
        var rot = float3x3.LookRotation(normalize(target - eye), up);

        float4x4 matrix;
        matrix.c0 = float4(rot.c0, 0.0f);
        matrix.c1 = float4(rot.c1, 0.0f);
        matrix.c2 = float4(rot.c2, 0.0f);
        matrix.c3 = float4(eye, 1.0f);
        return matrix;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 Ortho(float width, float height, float near, float far)
    {
        var rcpdx = 1.0f / width;
        var rcpdy = 1.0f / height;
        var rcpdz = 1.0f / (far - near);

        return float4x4(
            2.0f * rcpdx,   0.0f,           0.0f,       0.0f,
            0.0f,           2.0f * rcpdy,   0.0f,       0.0f,
            0.0f,           0.0f,          -rcpdz,     -near * rcpdz,
            0.0f,           0.0f,           0.0f,       1.0f
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 OrthoOffCenter(float left, float right, float bottom, float top, float near, float far)
    {
        var rcpdx = 1.0f / (right - left);
        var rcpdy = 1.0f / (top - bottom);
        var rcpdz = 1.0f / (far - near);

        return float4x4(
            2.0f * rcpdx,   0.0f,           0.0f,      -(right + left) * rcpdx,
            0.0f,           2.0f * rcpdy,   0.0f,      -(top + bottom) * rcpdy,
            0.0f,           0.0f,          -rcpdz,     -near * rcpdz,
            0.0f,           0.0f,           0.0f,       1.0f
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 PerspectiveFov(float verticalFov, float aspect, float near, float far)
    {
        var cotangent = 1.0f / tan(verticalFov * 0.5f);
        var rcpdz = 1.0f / (near - far);

        return float4x4(
            cotangent / aspect, 0.0f,       0.0f,          0.0f,
            0.0f,               cotangent,  0.0f,          0.0f,
            0.0f,               0.0f,       far * rcpdz,   near * far * rcpdz,
            0.0f,               0.0f,      -1.0f,          0.0f
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far)
    {
        var rcpdz = 1.0f / (near - far);
        var rcpWidth = 1.0f / (right - left);
        var rcpHeight = 1.0f / (top - bottom);

        return float4x4(
            2.0f * near * rcpWidth,     0.0f,                       (left + right) * rcpWidth,     0.0f,
            0.0f,                       2.0f * near * rcpHeight,    (bottom + top) * rcpHeight,    0.0f,
            0.0f,                       0.0f,                        far * rcpdz,                   near * far * rcpdz,
            0.0f,                       0.0f,                       -1.0f,                          0.0f
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 TRS(float3 translation, quaternion rotation, float3 scale)
    {
        var r = float3x3(rotation);
        return float4x4(float4(r.c0 * scale.x, 0.0f),
                        float4(r.c1 * scale.y, 0.0f),
                        float4(r.c2 * scale.z, 0.0f),
                        float4(translation, 1.0f));
    }
}

public static partial class math
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3x3 float3x3(quaternion rotation) => new(rotation);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3x3 float3x3(in float4x4 m) => new(m);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 float4x4(in float3x3 rotation, float3 translation) => new(rotation, translation);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 float4x4(quaternion rotation, float3 translation) => new(rotation, translation);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4x4 float4x4(in RigidTransform transform) => new(transform);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3x3 orthonormalize(in float3x3 i)
    {
        var u = i.c0;
        var v = i.c1 - i.c0 * dot(i.c1, i.c0);

        var lenU = length(u);
        var lenV = length(v);

        var c = lenU > 1e-30f && lenV > 1e-30f;

        float3x3 o;
        o.c0 = select(float3(1, 0, 0), u / lenU, c);
        o.c1 = select(float3(0, 1, 0), v / lenV, c);
        o.c2 = cross(o.c0, o.c1);

        return o;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3x3 mulScale(in float3x3 m, float3 s) => new(m.c0 * s.x, m.c1 * s.y, m.c2 * s.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3x3 scaleMul(float3 s, in float3x3 m) => new(m.c0 * s, m.c1 * s, m.c2 * s);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3x3 pseudoinverse(in float3x3 m)
    {
        var scaleSq = 0.333333f * (lengthsq(m.c0) + lengthsq(m.c1) + lengthsq(m.c2));
        if (scaleSq < svd.k_EpsilonNormal)
            return default;

        float3 scaleInv = rsqrt(scaleSq);
        var ms = mulScale(m, scaleInv);
        if (!adjInverse(ms, out var i, svd.k_EpsilonDeterminant))
            i = svd.svdInverse(ms);

        return mulScale(i, scaleInv);
    }
}
