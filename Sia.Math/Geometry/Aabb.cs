using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace Sia.Math;

[DebuggerDisplay("{Min} - {Max}")]
[Serializable]
public record struct Aabb(float3 Min, float3 Max)
{
    private const uint XyzMask = 0b0111;

    public float3 Extents => Max - Min;

    public float3 HalfExtents => (Max - Min) * 0.5f;

    public float3 Center => (Max + Min) * 0.5f;

    public bool IsValid => AllXyz(Vector128.LessThanOrEqual(Min.data, Max.data));

    public static Aabb CreateFromCenterAndExtents(float3 center, float3 extents) =>
        CreateFromCenterAndHalfExtents(center, extents * 0.5f);

    public static Aabb CreateFromCenterAndHalfExtents(float3 center, float3 halfExtents) =>
        new(center - halfExtents, center + halfExtents);

    public float SurfaceArea
    {
        get
        {
            var diff = Max - Min;
            return 2 * math.dot(diff, diff.yzx);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Aabb Union(in Aabb a, in Aabb b) =>
        new(math.min(a.Min, b.Min), math.max(a.Max, b.Max));

    public void Intersect(Aabb aabb)
    {
        Min = math.max(Min, aabb.Min);
        Max = math.min(Max, aabb.Max);
    }

    public void Expand(float signedDistance)
    {
        Min -= signedDistance;
        Max += signedDistance;
    }

    public void Include(float3 point)
    {
        Min = math.min(Min, point);
        Max = math.max(Max, point);
    }

    public void Include(Aabb aabb)
    {
        Min = math.min(Min, aabb.Min);
        Max = math.max(Max, aabb.Max);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(float3 point)
    {
        var mask = Vector128.BitwiseAnd(
            Vector128.GreaterThanOrEqual(point.data, Min.data),
            Vector128.LessThanOrEqual(point.data, Max.data));
        return AllXyz(mask);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(Aabb aabb)
    {
        var mask = Vector128.BitwiseAnd(
            Vector128.LessThanOrEqual(Min.data, aabb.Min.data),
            Vector128.GreaterThanOrEqual(Max.data, aabb.Max.data));
        return AllXyz(mask);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Overlaps(Aabb aabb)
    {
        var mask = Vector128.BitwiseAnd(
            Vector128.GreaterThanOrEqual(Max.data, aabb.Min.data),
            Vector128.LessThanOrEqual(Min.data, aabb.Max.data));
        return AllXyz(mask);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool AllXyz(Vector128<float> mask) =>
        (mask.ExtractMostSignificantBits() & XyzMask) == XyzMask;
}
