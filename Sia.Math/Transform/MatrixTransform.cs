using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

#pragma warning disable 8981

namespace Sia.Math;

[Serializable]
public record struct MatrixTransform(float3 Translation, float3x3 Rotation)
{
    public static MatrixTransform Identity => new(float3.zero, float3x3.identity);

    public MatrixTransform(float3 translation, quaternion rotation)
        : this(translation, new float3x3(rotation)) { }

    public MatrixTransform(RigidTransform transform)
        : this(transform.Translation, new float3x3(transform.Rotation)) { }

    public float3x3 InverseRotation => math.transpose(Rotation);
}

public static partial class math
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MatrixTransform inverse(in MatrixTransform a)
    {
        var inverseRotation = transpose(a.Rotation);
        return new MatrixTransform(mul(inverseRotation, -a.Translation), inverseRotation);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3 mul(in MatrixTransform a, float3 x)
    {
        var result = Vector128.FusedMultiplyAdd(
            a.Rotation.c0.data,
            Vector128.Create(x.x),
            a.Translation.data);
        result = Vector128.FusedMultiplyAdd(
            a.Rotation.c1.data,
            Vector128.Create(x.y),
            result);
        result = Vector128.FusedMultiplyAdd(
            a.Rotation.c2.data,
            Vector128.Create(x.z),
            result);
        return new float3(result);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MatrixTransform mul(in MatrixTransform cFromB, in MatrixTransform bFromA)
    {
        return new MatrixTransform(
            mul(cFromB.Rotation, bFromA.Translation) + cFromB.Translation,
            mul(cFromB.Rotation, bFromA.Rotation));
    }
}
