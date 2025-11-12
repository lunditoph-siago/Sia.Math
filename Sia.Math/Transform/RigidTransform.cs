using System.Globalization;
using System.Runtime.CompilerServices;

#pragma warning disable 8981

namespace Sia.Math;

[Serializable]
public record struct RigidTransform(quaternion Rotation, float3 Translation)
{
     public static readonly RigidTransform Identity = new(quaternion.identity, float3.zero);

     [MethodImpl(MethodImplOptions.AggressiveInlining)]
     public RigidTransform(float3x3 rotation, float3 translation) : this(new quaternion(rotation), translation) { }

     [MethodImpl(MethodImplOptions.AggressiveInlining)]
     public RigidTransform(float4x4 transform) : this(new quaternion(transform), transform.c3.xyz) { }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RigidTransform AxisAngle(float3 axis, float angle) => new(quaternion.AxisAngle(axis, angle), float3.zero);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RigidTransform EulerXYZ(float3 xyz) => new(quaternion.EulerXYZ(xyz), float3.zero);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RigidTransform EulerXZY(float3 xyz) => new(quaternion.EulerXZY(xyz), float3.zero);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RigidTransform EulerYXZ(float3 xyz) => new(quaternion.EulerYXZ(xyz), float3.zero);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RigidTransform EulerYZX(float3 xyz) => new(quaternion.EulerYZX(xyz), float3.zero);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RigidTransform EulerZXY(float3 xyz) => new(quaternion.EulerZXY(xyz), float3.zero);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RigidTransform EulerZYX(float3 xyz) => new(quaternion.EulerZYX(xyz), float3.zero);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RigidTransform Euler(float3 xyz, math.RotationOrder order = math.RotationOrder.ZXY) => order switch
    {
        math.RotationOrder.XYZ => EulerXYZ(xyz),
        math.RotationOrder.XZY => EulerXZY(xyz),
        math.RotationOrder.YXZ => EulerYXZ(xyz),
        math.RotationOrder.YZX => EulerYZX(xyz),
        math.RotationOrder.ZXY => EulerZXY(xyz),
        math.RotationOrder.ZYX => EulerZYX(xyz),
        _ => Identity
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RigidTransform RotateY(float angle) => new(quaternion.RotateY(angle), float3.zero);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RigidTransform RotateZ(float angle) => new(quaternion.RotateZ(angle), float3.zero);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RigidTransform Translate(float3 vector) => new(quaternion.identity, vector);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return (int)math.hash(this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly override string ToString()
    {
        return ToString("G", CultureInfo.CurrentCulture);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly string ToString(string format, IFormatProvider formatProvider)
    {
        var separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
        return $"float4x4(({Rotation.value.x.ToString(format, formatProvider)}f{separator} {Rotation.value.y.ToString(format, formatProvider)}f{separator} {Rotation.value.z.ToString(format, formatProvider)}f, {Rotation.value.w.ToString(format, formatProvider)}f),  ({Translation.x.ToString(format, formatProvider)}f, {Translation.y.ToString(format, formatProvider)}f{separator} {Translation.z.ToString(format, formatProvider)}f))";
    }
}

public static partial class math
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RigidTransform inverse(RigidTransform t)
    {
        var invRotation = inverse(t.Rotation);
        var invTranslation = mul(invRotation, -t.Translation);
        return new RigidTransform(invRotation, invTranslation);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RigidTransform mul(RigidTransform a, RigidTransform b)
    {
        return new RigidTransform(mul(a.Rotation, b.Rotation), mul(a.Rotation, b.Translation) + a.Translation);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float4 mul(RigidTransform a, float4 pos)
    {
        return float4(mul(a.Rotation, pos.xyz) + a.Translation * pos.w, pos.w);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3 rotate(RigidTransform a, float3 dir)
    {
        return mul(a.Rotation, dir);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3 transform(RigidTransform a, float3 pos)
    {
        return mul(a.Rotation, pos) + a.Translation;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint hash(RigidTransform v)
    {
        return hash(v.Rotation) + 0xC5C5394Bu * hash(v.Translation);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint4 hashwide(RigidTransform v)
    {
        return hashwide(v.Rotation) + 0xC5C5394Bu * hashwide(v.Translation).xyzz;
    }
}
