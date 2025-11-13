using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Sia.Math;

[DebuggerDisplay("{Normal}, {Distance}")]
[Serializable]
public record struct Plane(float3 normal, float distance)
{
    public float3 Normal { get; set; } = normal * math.rsqrt(math.lengthsq(normal));
    public float Distance { get; set; } = distance * math.rsqrt(math.lengthsq(normal));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Plane(float3 normal, float3 pointInPlane) : this(normal, -math.dot(normal, pointInPlane)) { }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Plane(float3 vector1InPlane, float3 vector2InPlane, float3 pointInPlane)
        : this(math.cross(vector1InPlane, vector2InPlane), pointInPlane) { }

    public Plane Flipped => new(-Normal, -Distance);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float SignedDistanceToPoint(float3 point) => math.dot(Normal, point) + Distance;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float3 Projection(float3 point) => point - Normal * SignedDistanceToPoint(point);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Plane CreateFromUnitNormalAndDistance(float3 unitNormal, float distance)
    {
        Plane plane = default;
        plane.Normal = unitNormal;
        plane.Distance = distance;
        return plane;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Plane CreateFromUnitNormalAndPointInPlane(float3 unitNormal, float3 pointInPlane) =>
        CreateFromUnitNormalAndDistance(unitNormal, -math.dot(unitNormal, pointInPlane));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator float4(Plane plane) => new(plane.Normal, plane.Distance);
}
