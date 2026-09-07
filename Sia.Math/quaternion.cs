using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using static Sia.Math.math;

#pragma warning disable 8981

namespace Sia.Math;

[Serializable]
public struct quaternion : IEquatable<quaternion>, IFormattable
{
    /// <summary>The quaternion component values.</summary>
    public float4 value;

    /// <summary>A quaternion representing the identity transform.</summary>
    public static readonly quaternion identity = new(0.0f, 0.0f, 0.0f, 1.0f);

    /// <summary>Constructs a <see cref="quaternion" /> from four <see cref="float" /> values.</summary>
    /// <param name="x">The value to assign to the <see cref="x" /> field.</param>
    /// <param name="y">The value to assign to the <see cref="y" /> field.</param>
    /// <param name="z">The value to assign to the <see cref="z" /> field.</param>
    /// <param name="w">The value to assign to the <see cref="w" /> field.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public quaternion(float x, float y, float z, float w) { value.x = x; value.y = y; value.z = z; value.w = w; }

    /// <summary>Constructs a <see cref="quaternion" /> from a <see cref="float4" /> vector.</summary>
    /// <param name="v">The value to assign to the <see cref="v" /> field.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public quaternion(float4 v) => value = v;

    /// <summary>Implicitly converts a <see cref="float4" /> vector to a <see cref="quaternion" />.</summary>
    /// <param name="v">The <see cref="float4" /> to convert to <see cref="quaternion" />.</param>
    /// <returns>The <see cref="quaternion" /> converted from arguments.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator quaternion(float4 v) => new(v);

    /// <summary>Constructs a unit <see cref="quaternion" /> from a <see cref="float3x3" /> rotation matrix. The matrix must be orthonormal.</summary>
    /// <param name="m">The <see cref="float3x3" /> orthonormal rotation matrix.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public quaternion(in float3x3 m)
        => value = FromBasis(m.c0.data, m.c1.data, m.c2.data);

    /// <summary>Constructs a unit quaternion from an orthonormal <see cref="float4x4" /> matrix.</summary>
    /// <param name="m">The <see cref="float4x4" /> orthonormal rotation matrix.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public quaternion(in float4x4 m)
        => value = FromBasis(m.c0.data, m.c1.data, m.c2.data);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float4 FromBasis(Vector128<float> u, Vector128<float> v, Vector128<float> w)
    {
        var sign = Vector128.Create(0x80000000u).AsSingle();

        var ux = Vector128.Shuffle(u, Vector128.Create(0, 0, 0, 0));
        var uy = Vector128.Shuffle(u, Vector128.Create(1, 1, 1, 1));
        var uz = Vector128.Shuffle(u, Vector128.Create(2, 2, 2, 2));
        var vx = Vector128.Shuffle(v, Vector128.Create(0, 0, 0, 0));
        var vy = Vector128.Shuffle(v, Vector128.Create(1, 1, 1, 1));
        var vz = Vector128.Shuffle(v, Vector128.Create(2, 2, 2, 2));
        var wx = Vector128.Shuffle(w, Vector128.Create(0, 0, 0, 0));
        var wy = Vector128.Shuffle(w, Vector128.Create(1, 1, 1, 1));
        var wz = Vector128.Shuffle(w, Vector128.Create(2, 2, 2, 2));

        var uSign = Vector128.BitwiseAnd(ux, sign);
        var t = Vector128.Add(vy, Vector128.Xor(wz, uSign));

        var uMask = Vector128.ShiftRightArithmetic(ux.AsInt32(), 31).AsSingle();
        var tMask = Vector128.ShiftRightArithmetic(t.AsInt32(), 31).AsSingle();

        var tr = Vector128.Add(Vector128.Create(1.0f), Vector128.AndNot(ux, sign));

        var a = Lanes(tr, uy, wx, vz);
        var b = Lanes(t, vx, uz, wy);

        var flips = Vector128.Xor(
            Vector128.Xor(
                Vector128.Create(0x00000000u, 0x80000000u, 0x80000000u, 0x80000000u).AsSingle(),
                Vector128.BitwiseAnd(uMask, Vector128.Create(0x00000000u, 0x80000000u, 0x00000000u, 0x80000000u).AsSingle())),
            Vector128.BitwiseAnd(tMask, Vector128.Create(0x80000000u, 0x80000000u, 0x80000000u, 0x00000000u).AsSingle()));

        var r = Vector128.Add(a, Vector128.Xor(b, flips));   // +---, +++-, ++-+, +-++

        r = Vector128.ConditionalSelect(uMask, Vector128.Shuffle(r, Vector128.Create(2, 3, 0, 1)), r);
        r = Vector128.ConditionalSelect(tMask, r, Vector128.Shuffle(r, Vector128.Create(3, 2, 1, 0)));

        return normalize(new float4(r));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector128<float> Lanes(Vector128<float> x, Vector128<float> y, Vector128<float> z, Vector128<float> w)
    {
        var xy = Vector128.ConditionalSelect(Vector128.Create(0, -1, 0, 0).AsSingle(), y, x);
        var zw = Vector128.ConditionalSelect(Vector128.Create(0, 0, 0, -1).AsSingle(), w, z);
        return Vector128.ConditionalSelect(Vector128.Create(0, 0, -1, -1).AsSingle(), zw, xy);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion AxisAngle(float3 axis, float angle)
    {
        sincos(0.5f * angle, out var sina, out var cosa);
        return new quaternion(float4(axis * sina, cosa));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion EulerXYZ(float3 xyz)
    {
        // return mul(rotateZ(xyz.z), mul(rotateY(xyz.y), rotateX(xyz.x)));
        sincos(0.5f * xyz, out var s, out var c);
        return new quaternion(
            // s.x * c.y * c.z - s.y * s.z * c.x,
            // s.y * c.x * c.z + s.x * s.z * c.y,
            // s.z * c.x * c.y - s.x * s.y * c.z,
            // c.x * c.y * c.z + s.y * s.z * s.x
            float4(s.xyz, c.x) * c.yxxy * c.zzyz + s.yxxy * s.zzyz * float4(c.xyz, s.x) * float4(-1.0f, 1.0f, -1.0f, 1.0f)
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion EulerXZY(float3 xyz)
    {
        // return mul(rotateY(xyz.y), mul(rotateZ(xyz.z), rotateX(xyz.x)));
        sincos(0.5f * xyz, out var s, out var c);
        return new quaternion(
            // s.x * c.y * c.z + s.y * s.z * c.x,
            // s.y * c.x * c.z + s.x * s.z * c.y,
            // s.z * c.x * c.y - s.x * s.y * c.z,
            // c.x * c.y * c.z - s.y * s.z * s.x
            float4(s.xyz, c.x) * c.yxxy * c.zzyz + s.yxxy * s.zzyz * float4(c.xyz, s.x) * float4(1.0f, 1.0f, -1.0f, -1.0f)
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion EulerYXZ(float3 xyz)
    {
        // return mul(rotateZ(xyz.z), mul(rotateX(xyz.x), rotateY(xyz.y)));
        sincos(0.5f * xyz, out var s, out var c);
        return new quaternion(
            // s.x * c.y * c.z - s.y * s.z * c.x,
            // s.y * c.x * c.z + s.x * s.z * c.y,
            // s.z * c.x * c.y + s.x * s.y * c.z,
            // c.x * c.y * c.z - s.y * s.z * s.x
            float4(s.xyz, c.x) * c.yxxy * c.zzyz +
            s.yxxy * s.zzyz * float4(c.xyz, s.x) * float4(-1.0f, 1.0f, 1.0f, -1.0f)
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion EulerYZX(float3 xyz)
    {
        // return mul(rotateX(xyz.x), mul(rotateZ(xyz.z), rotateY(xyz.y)));
        sincos(0.5f * xyz, out var s, out var c);
        return new quaternion(
            // s.x * c.y * c.z - s.y * s.z * c.x,
            // s.y * c.x * c.z - s.x * s.z * c.y,
            // s.z * c.x * c.y + s.x * s.y * c.z,
            // c.x * c.y * c.z + s.y * s.z * s.x
            float4(s.xyz, c.x) * c.yxxy * c.zzyz + s.yxxy * s.zzyz * float4(c.xyz, s.x) * float4(-1.0f, -1.0f, 1.0f, 1.0f)
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion EulerZXY(float3 xyz)
    {
        // return mul(rotateY(xyz.y), mul(rotateX(xyz.x), rotateZ(xyz.z)));
        sincos(0.5f * xyz, out var s, out var c);
        return new quaternion(
            // s.x * c.y * c.z + s.y * s.z * c.x,
            // s.y * c.x * c.z - s.x * s.z * c.y,
            // s.z * c.x * c.y - s.x * s.y * c.z,
            // c.x * c.y * c.z + s.y * s.z * s.x
            float4(s.xyz, c.x) * c.yxxy * c.zzyz + s.yxxy * s.zzyz * float4(c.xyz, s.x) * float4(1.0f, -1.0f, -1.0f, 1.0f)
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion EulerZYX(float3 xyz)
    {
        // return mul(rotateX(xyz.x), mul(rotateY(xyz.y), rotateZ(xyz.z)));
        sincos(0.5f * xyz, out var s, out var c);
        return new quaternion(
            // s.x * c.y * c.z + s.y * s.z * c.x,
            // s.y * c.x * c.z - s.x * s.z * c.y,
            // s.z * c.x * c.y + s.x * s.y * c.z,
            // c.x * c.y * c.z - s.y * s.x * s.z
            float4(s.xyz, c.x) * c.yxxy * c.zzyz + s.yxxy * s.zzyz * float4(c.xyz, s.x) * float4(1.0f, -1.0f, 1.0f, -1.0f)
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion EulerXYZ(float x, float y, float z) => EulerXYZ(float3(x, y, z));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion EulerXZY(float x, float y, float z) => EulerXZY(float3(x, y, z));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion EulerYXZ(float x, float y, float z) => EulerYXZ(float3(x, y, z));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion EulerYZX(float x, float y, float z) => EulerYZX(float3(x, y, z));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion EulerZXY(float x, float y, float z) => EulerZXY(float3(x, y, z));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion EulerZYX(float x, float y, float z) => EulerZYX(float3(x, y, z));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion Euler(float3 xyz, RotationOrder order = RotationOrder.ZXY) => order switch
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
    public static quaternion Euler(float x, float y, float z, RotationOrder order = RotationOrder.Default) => Euler(float3(x, y, z), order);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion RotateX(float angle)
    {
        sincos(0.5f * angle, out var sina, out var cosa);
        return new quaternion(sina, 0.0f, 0.0f, cosa);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion RotateY(float angle)
    {
        sincos(0.5f * angle, out var sina, out var cosa);
        return new quaternion(0.0f, sina, 0.0f, cosa);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion RotateZ(float angle)
    {
        sincos(0.5f * angle, out var sina, out var cosa);
        return new quaternion(0.0f, 0.0f, sina, cosa);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion LookRotation(float3 forward, float3 up)
    {
        var t = normalize(cross(up, forward));
        return new quaternion(float3x3(t, cross(forward, t), forward));
    }

    public static quaternion LookRotationSafe(float3 forward, float3 up)
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
        return new quaternion(select(float4(0.0f, 0.0f, 0.0f, 1.0f), new quaternion(float3x3(t, cross(forward, t), forward)).value, accept));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(quaternion x) =>
        value.x == x.value.x && value.y == x.value.y && value.z == x.value.z && value.w == x.value.w;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? x) => x is quaternion converted && Equals(converted);

    /// <summary>Returns a hash code for the <see cref="quaternion" />.</summary>
    /// <returns>The hash code of the <see cref="quaternion" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => (int)hash(value);

    /// <summary>Returns the string representation of the <see cref="quaternion" /> using default formatting.</summary>
    /// <returns>The string representation of the <see cref="quaternion" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        return ToString("G", CultureInfo.CurrentCulture);
    }

    /// <summary>Returns a string representation of the <see cref="quaternion" /> using the specified format string to format individual elements and the specified format provider to define culture-specific formatting.</summary>
    /// <param name="format">A standard or custom numeric format string that defines the format of individual elements.</param>
    /// <param name="formatProvider">A format provider that supplies culture-specific formatting information.</param>
    /// <returns>The formatted string representation of the <see cref="quaternion" />.</returns>
    public readonly string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? formatProvider)
    {
        var separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
        return $"quaternion({value.x.ToString(format, formatProvider)}f{separator} {value.y.ToString(format, formatProvider)}f{separator} {value.z.ToString(format, formatProvider)}f{separator} {value.w.ToString(format, formatProvider)}f)";
    }
}

public static partial class math
{
    private static readonly Vector128<uint> s_quaternionConjugateMask = Vector128.Create(0x80000000u, 0x80000000u, 0x80000000u, 0u);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion quaternion(float x, float y, float z, float w) => new(x, y, z, w);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion quaternion(float4 v) => new(v);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion quaternion(float3x3 m) => new(m);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion quaternion(float4x4 m) => new(m);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion rotation(in float3x3 m)
    {
        var det = determinant(m);
        if (abs(1f - det) < svd.k_EpsilonDeterminant)
            return quaternion(m);

        if (abs(det) > svd.k_EpsilonDeterminant)
        {
            var tmp = mulScale(m, rsqrt(float3(lengthsq(m.c0), lengthsq(m.c1), lengthsq(m.c2))));
            if (abs(1f - determinant(tmp)) < svd.k_EpsilonDeterminant)
                return quaternion(tmp);
        }

        return svd.svdRotation(m);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion mul(quaternion a, quaternion b)
    {
        return quaternion(a.value.wwww * b.value + (a.value.xyzx * b.value.wwwx + a.value.yzxy * b.value.zxyy) * float4(1.0f, 1.0f, 1.0f, -1.0f) - a.value.zxyz * b.value.yzxz);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3 mul(quaternion q, float3 v)
    {
        var t = 2 * cross(q.value.xyz, v);
        return v + q.value.w * t + cross(q.value.xyz, t);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3 rotate(quaternion q, float3 v)
    {
        var t = 2 * cross(q.value.xyz, v);
        return v + q.value.w * t + cross(q.value.xyz, t);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3 forward(quaternion q) => mul(q, float3(0, 0, 1));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion conjugate(quaternion q)
    {
        var bits = Vector128.Xor(Vector128.AsUInt32(q.value.data), s_quaternionConjugateMask);
        var conjugated = Vector128.AsSingle(bits);
        return new quaternion(new float4(conjugated));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion inverse(quaternion q)
    {
        var value = q.value.data;
        var bits = Vector128.Xor(Vector128.AsUInt32(value), s_quaternionConjugateMask);
        var numerator = Vector128.AsSingle(bits);
        var denominator = Vector128.Create(Vector128.Dot(value, value));
        return new quaternion(new float4(numerator / denominator));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float dot(quaternion a, quaternion b)
    {
        return dot(a.value, b.value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool isfinite(quaternion q)
    {
        return all(isfinite(q.value));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float length(quaternion q)
    {
        return sqrt(dot(q.value, q.value));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float lengthsq(quaternion q)
    {
        return dot(q.value, q.value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion normalize(quaternion q)
    {
        var value = q.value.data;
        var length = Vector128.Sqrt(Vector128.Create(Vector128.Dot(value, value)));
        return new quaternion(new float4(value / length));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion normalizesafe(quaternion q)
    {
        var value = q.value.data;
        var lengthSquared = Vector128.Dot(value, value);
        if (!(lengthSquared > FLT_MIN_NORMAL)) return Math.quaternion.identity;

        var inverseLength = Vector128.Create(1.0f / MathF.Sqrt(lengthSquared));
        return new quaternion(new float4(value * inverseLength));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion normalizesafe(quaternion q, quaternion defaultvalue)
    {
        var value = q.value.data;
        var lengthSquared = Vector128.Dot(value, value);
        if (!(lengthSquared > FLT_MIN_NORMAL)) return defaultvalue;

        var inverseLength = Vector128.Create(1.0f / MathF.Sqrt(lengthSquared));
        return new quaternion(new float4(value * inverseLength));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion unitexp(quaternion q)
    {
        var v_rcp_len = rsqrt(dot(q.value.xyz, q.value.xyz));
        var v_len = rcp(v_rcp_len);
        sincos(v_len, out var sin_v_len, out var cos_v_len);
        return quaternion(float4(q.value.xyz * v_rcp_len * sin_v_len, cos_v_len));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion exp(quaternion q)
    {
        var v_rcp_len = rsqrt(dot(q.value.xyz, q.value.xyz));
        var v_len = rcp(v_rcp_len);
        sincos(v_len, out var sin_v_len, out var cos_v_len);
        return quaternion(float4(q.value.xyz * v_rcp_len * sin_v_len, cos_v_len) * exp(q.value.w));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion unitlog(quaternion q)
    {
        var w = clamp(q.value.w, -1.0f, 1.0f);
        var s = acos(w) * rsqrt(1.0f - w * w);
        return quaternion(float4(q.value.xyz * s, 0.0f));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion log(quaternion q)
    {
        var v_len_sq = dot(q.value.xyz, q.value.xyz);
        var q_len_sq = v_len_sq + q.value.w*q.value.w;

        var s = acos(clamp(q.value.w * rsqrt(q_len_sq), -1.0f, 1.0f)) * rsqrt(v_len_sq);
        return quaternion(float4(q.value.xyz * s, 0.5f * log(q_len_sq)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float angle(quaternion q1, quaternion q2)
    {
        var diff = asin(length(normalize(mul(conjugate(q1), q2)).value.xyz));
        return diff + diff;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion nlerp(quaternion q1, quaternion q2, float t)
    {
        var dt = dot(q1, q2);
        if(dt < 0.0f) q2.value = -q2.value;

        return normalize(quaternion(lerp(q1.value, q2.value, t)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static quaternion slerp(quaternion q1, quaternion q2, float t)
    {
        var dt = dot(q1, q2);
        if (dt < 0.0f)
        {
            dt = -dt;
            q2.value = -q2.value;
        }

        if (dt < 0.9995f)
        {
            var angle = acos(dt);
            var s = rsqrt(1.0f - dt * dt); // 1.0f / sin(angle)
            var w1 = sin(angle * (1.0f - t)) * s;
            var w2 = sin(angle * t) * s;
            return quaternion(q1.value * w1 + q2.value * w2);
        }

        // if the angle is small, use linear interpolation
        return nlerp(q1, q2, t);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint hash(quaternion q)
    {
        return hash(q.value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint4 hashwide(quaternion q)
    {
        return hashwide(q.value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float3x3 adj(in float3x3 m, out float det)
    {
        float3x3 adjT;
        adjT.c0 = cross(m.c1, m.c2);
        adjT.c1 = cross(m.c2, m.c0);
        adjT.c2 = cross(m.c0, m.c1);
        det = dot(m.c0, adjT.c0);

        return transpose(adjT);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool adjInverse(in float3x3 m, out float3x3 i, float epsilon = svd.k_EpsilonNormal)
    {
        i = adj(m, out var det);
        var c = abs(det) > epsilon;
        float3 detInv = select(float3(1f), rcp(det), c);
        i = scaleMul(detInv, i);
        return c;
    }
}
