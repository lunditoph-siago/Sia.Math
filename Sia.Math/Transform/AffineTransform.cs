using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;

#pragma warning disable 8981

namespace Sia.Math;

[Serializable]
public record struct AffineTransform(float3 Translation, float3x3 RotationScale) : IFormattable
{
    public static readonly AffineTransform Identity = new(float3.zero, float3x3.identity);

    public static readonly AffineTransform Zero;

    public AffineTransform(float3 translation, quaternion rotation)
        : this(translation, new float3x3(rotation)) { }

    public AffineTransform(float3 translation, quaternion rotation, float3 scale)
        : this(translation, new float3x3(rotation)) =>
        RotationScale = new(RotationScale.c0 * scale.x, RotationScale.c1 * scale.y, RotationScale.c2 * scale.z);

    public AffineTransform(float3x3 RotationScale) : this(float3.zero, RotationScale) { }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator float4x4(AffineTransform m) =>
        new(new float4(m.RotationScale.c0, 0f), new float4(m.RotationScale.c1, 0f),
            new float4(m.RotationScale.c2, 0f), new float4(m.Translation, 1f));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return (int)math.hash(this);
    }

    public readonly override string ToString()
    {
        return ToString("G", CultureInfo.CurrentCulture);
    }

    public readonly string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? formatProvider)
    {
        var separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
        return $"AffineTransform(({RotationScale.c0.x.ToString(format, formatProvider)}f{separator} {RotationScale.c1.x.ToString(format, formatProvider)}f{separator} {RotationScale.c2.x.ToString(format, formatProvider)}f{separator}  {RotationScale.c0.y.ToString(format, formatProvider)}f{separator} {RotationScale.c1.y.ToString(format, formatProvider)}f{separator} {RotationScale.c2.y.ToString(format, formatProvider)}f{separator}  {RotationScale.c0.z.ToString(format, formatProvider)}f{separator} {RotationScale.c1.z.ToString(format, formatProvider)}f{separator} {RotationScale.c2.z.ToString(format, formatProvider)}f){separator} ({Translation.x.ToString(format, formatProvider)}f{separator} {Translation.y.ToString(format, formatProvider)}f{separator} {Translation.z.ToString(format, formatProvider)}f))";
    }
}

public static partial class math
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint hash(AffineTransform v)
    {
        return hash(v.RotationScale) + 0xC5C5394Bu * hash(v.Translation);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint4 hashwide(AffineTransform v)
    {
        return hashwide(v.RotationScale).xyzz + 0xC5C5394Bu * hashwide(v.Translation).xyzz;
    }
}
