using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace Sia.Math;

[Serializable]
public readonly struct Frustum(Plane left, Plane right, Plane bottom, Plane top, Plane near, Plane far)
{
    private const int PlaneMask = 0b0011_1111;

    public readonly Plane Left = left;
    public readonly Plane Right = right;
    public readonly Plane Bottom = bottom;
    public readonly Plane Top = top;
    public readonly Plane Near = near;
    public readonly Plane Far = far;

    private readonly Vector256<float> _normalX = Vector256.Create(
        left.Normal.x, right.Normal.x, bottom.Normal.x, top.Normal.x, near.Normal.x, far.Normal.x, 0.0f, 0.0f);
    private readonly Vector256<float> _normalY = Vector256.Create(
        left.Normal.y, right.Normal.y, bottom.Normal.y, top.Normal.y, near.Normal.y, far.Normal.y, 0.0f, 0.0f);
    private readonly Vector256<float> _normalZ = Vector256.Create(
        left.Normal.z, right.Normal.z, bottom.Normal.z, top.Normal.z, near.Normal.z, far.Normal.z, 0.0f, 0.0f);
    private readonly Vector256<float> _distance = Vector256.Create(
        left.Distance, right.Distance, bottom.Distance, top.Distance, near.Distance, far.Distance, 0.0f, 0.0f);

    public static Frustum CreateFromViewProjection(in float4x4 viewProjection)
    {
        var rows = math.transpose(viewProjection);

        return new Frustum(
            left: FromRow(rows.c3 + rows.c0),
            right: FromRow(rows.c3 - rows.c0),
            bottom: FromRow(rows.c3 + rows.c1),
            top: FromRow(rows.c3 - rows.c1),
            near: FromRow(rows.c2),
            far: FromRow(rows.c3 - rows.c2));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Plane FromRow(float4 row) =>
        new(new float3(row.x, row.y, row.z), row.w);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(float3 point)
    {
        var dot = Vector256.FusedMultiplyAdd(_normalX, Vector256.Create(point.x),
            Vector256.FusedMultiplyAdd(_normalY, Vector256.Create(point.y),
            Vector256.FusedMultiplyAdd(_normalZ, Vector256.Create(point.z), _distance)));
        var mask = Vector256.GreaterThanOrEqual(dot, Vector256<float>.Zero);
        return (mask.ExtractMostSignificantBits() & PlaneMask) == PlaneMask;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Intersects(Aabb aabb)
    {
        var min = aabb.Min;
        var max = aabb.Max;

        var posX = Vector256.ConditionalSelect(
            Vector256.GreaterThanOrEqual(_normalX, Vector256<float>.Zero), Vector256.Create(max.x), Vector256.Create(min.x));
        var posY = Vector256.ConditionalSelect(
            Vector256.GreaterThanOrEqual(_normalY, Vector256<float>.Zero), Vector256.Create(max.y), Vector256.Create(min.y));
        var posZ = Vector256.ConditionalSelect(
            Vector256.GreaterThanOrEqual(_normalZ, Vector256<float>.Zero), Vector256.Create(max.z), Vector256.Create(min.z));

        var dot = Vector256.FusedMultiplyAdd(_normalX, posX,
            Vector256.FusedMultiplyAdd(_normalY, posY,
            Vector256.FusedMultiplyAdd(_normalZ, posZ, _distance)));
        var mask = Vector256.GreaterThanOrEqual(dot, Vector256<float>.Zero);
        return (mask.ExtractMostSignificantBits() & PlaneMask) == PlaneMask;
    }
}
