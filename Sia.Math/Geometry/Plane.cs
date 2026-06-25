using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.InteropServices;

namespace Sia.Math;

[DebuggerDisplay("{Normal}, {Distance}")]
[Serializable]
[StructLayout(LayoutKind.Sequential, Size = 16)]
public readonly struct Plane : IEquatable<Plane>
{
    private readonly float4 _value;

    public float3 Normal {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(_value.data);
    }

    public float Distance {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _value.data.GetElement(3);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Plane(Vector128<float> value) => _value = new float4(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Plane(float3 normal, float distance)
    {
        var packed = normal.data.WithElement(3, distance);
        var length = Vector128.Sqrt(Vector128.Create(math.dot(normal, normal)));
        _value = new float4(packed / length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Plane(float3 normal, float3 pointInPlane) : this(normal, -math.dot(normal, pointInPlane)) { }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Plane(float3 vector1InPlane, float3 vector2InPlane, float3 pointInPlane)
        : this(math.cross(vector1InPlane, vector2InPlane), pointInPlane) { }

    public Plane Flipped {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(-_value.data);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float SignedDistanceToPoint(float3 point) =>
        Vector128.Dot(_value.data, point.data.WithElement(3, 1.0f));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float3 Projection(float3 point)
    {
        var signedDistance = SignedDistanceToPoint(point);
        return new float3(Vector128.FusedMultiplyAdd(
            _value.data,
            Vector128.Create(-signedDistance),
            point.data));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Plane CreateFromUnitNormalAndDistance(float3 unitNormal, float distance) =>
        new(unitNormal.data.WithElement(3, distance));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Plane CreateFromUnitNormalAndPointInPlane(float3 unitNormal, float3 pointInPlane) =>
        CreateFromUnitNormalAndDistance(unitNormal, -math.dot(unitNormal, pointInPlane));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator float4(Plane plane) => plane._value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Plane other) => _value.Equals(other._value);

    public override bool Equals(object? obj) => obj is Plane other && Equals(other);

    public override int GetHashCode() => _value.GetHashCode();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Plane left, Plane right) => left.Equals(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Plane left, Plane right) => !left.Equals(right);
}
