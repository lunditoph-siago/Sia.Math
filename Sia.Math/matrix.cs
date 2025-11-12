using System.Runtime.CompilerServices;

#pragma warning disable 8981

namespace Sia.Math;

public partial struct float3x3
{
    public float3x3(float4x4 m)
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
}

partial class math
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float3x3 float3x3(quaternion rotation) => new(rotation);
}
