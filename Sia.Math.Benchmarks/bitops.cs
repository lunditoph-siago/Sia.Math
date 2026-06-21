using System.Numerics;
using BenchmarkDotNet.Attributes;

namespace Sia.Math.Benchmarks;

#region PopCount

[BenchmarkCategory("LowPriority")]
public class UIntPopCount : BenchBase
{
    private uint _sa;

    protected override void OnSetup() => _sa = math.asuint(NextF());

    [Benchmark(Baseline = true), BenchmarkCategory("PopCount")]
    public void Sys_PopCount() => Sink(BitOperations.PopCount(_sa));

    [Benchmark, BenchmarkCategory("PopCount")]
    public void Sia_PopCount() => Sink(math.countbits(_sa));
}

[BenchmarkCategory("LowPriority")]
public class UInt2PopCount : BenchBase
{
    private uint2 _sa;

    protected override void OnSetup() => _sa = new(math.asuint(NextF()), math.asuint(NextF()));

    [Benchmark(Baseline = true), BenchmarkCategory("PopCount")]
    public void Sys_PopCount() => Sink(BitOperations.PopCount(_sa.x) + BitOperations.PopCount(_sa.y));

    [Benchmark, BenchmarkCategory("PopCount")]
    public void Sia_PopCount() { var r = math.countbits(_sa); Sink(r.x + r.y); }
}

[BenchmarkCategory("LowPriority")]
public class UInt3PopCount : BenchBase
{
    private uint3 _sa;

    protected override void OnSetup() => _sa = new(math.asuint(NextF()), math.asuint(NextF()), math.asuint(NextF()));

    [Benchmark(Baseline = true), BenchmarkCategory("PopCount")]
    public void Sys_PopCount() => Sink(BitOperations.PopCount(_sa.x) + BitOperations.PopCount(_sa.y) + BitOperations.PopCount(_sa.z));

    [Benchmark, BenchmarkCategory("PopCount")]
    public void Sia_PopCount() { var r = math.countbits(_sa); Sink(r.x + r.y + r.z); }
}

[BenchmarkCategory("LowPriority")]
public class UInt4PopCount : BenchBase
{
    private uint4 _sa;

    protected override void OnSetup() => _sa = new(math.asuint(NextF()), math.asuint(NextF()), math.asuint(NextF()), math.asuint(NextF()));

    [Benchmark(Baseline = true), BenchmarkCategory("PopCount")]
    public void Sys_PopCount() => Sink(BitOperations.PopCount(_sa.x) + BitOperations.PopCount(_sa.y) + BitOperations.PopCount(_sa.z) + BitOperations.PopCount(_sa.w));

    [Benchmark, BenchmarkCategory("PopCount")]
    public void Sia_PopCount() { var r = math.countbits(_sa); Sink(r.x + r.y + r.z + r.w); }
}

#endregion

#region LeadingZeroCount

[BenchmarkCategory("LowPriority")]
public class UIntLeadingZeroCount : BenchBase
{
    private uint _sa;

    protected override void OnSetup() => _sa = math.asuint(NextF());

    [Benchmark(Baseline = true), BenchmarkCategory("LeadingZeroCount")]
    public void Sys_LeadingZeroCount() => Sink(BitOperations.LeadingZeroCount(_sa));

    [Benchmark, BenchmarkCategory("LeadingZeroCount")]
    public void Sia_LeadingZeroCount() => Sink(math.lzcnt(_sa));
}

[BenchmarkCategory("LowPriority")]
public class UInt2LeadingZeroCount : BenchBase
{
    private uint2 _sa;

    protected override void OnSetup() => _sa = new(math.asuint(NextF()), math.asuint(NextF()));

    [Benchmark(Baseline = true), BenchmarkCategory("LeadingZeroCount")]
    public void Sys_LeadingZeroCount() => Sink(BitOperations.LeadingZeroCount(_sa.x) + BitOperations.LeadingZeroCount(_sa.y));

    [Benchmark, BenchmarkCategory("LeadingZeroCount")]
    public void Sia_LeadingZeroCount() { var r = math.lzcnt(_sa); Sink(r.x + r.y); }
}

[BenchmarkCategory("LowPriority")]
public class UInt3LeadingZeroCount : BenchBase
{
    private uint3 _sa;

    protected override void OnSetup() => _sa = new(math.asuint(NextF()), math.asuint(NextF()), math.asuint(NextF()));

    [Benchmark(Baseline = true), BenchmarkCategory("LeadingZeroCount")]
    public void Sys_LeadingZeroCount() => Sink(BitOperations.LeadingZeroCount(_sa.x) + BitOperations.LeadingZeroCount(_sa.y) + BitOperations.LeadingZeroCount(_sa.z));

    [Benchmark, BenchmarkCategory("LeadingZeroCount")]
    public void Sia_LeadingZeroCount() { var r = math.lzcnt(_sa); Sink(r.x + r.y + r.z); }
}

[BenchmarkCategory("LowPriority")]
public class UInt4LeadingZeroCount : BenchBase
{
    private uint4 _sa;

    protected override void OnSetup() => _sa = new(math.asuint(NextF()), math.asuint(NextF()), math.asuint(NextF()), math.asuint(NextF()));

    [Benchmark(Baseline = true), BenchmarkCategory("LeadingZeroCount")]
    public void Sys_LeadingZeroCount() => Sink(BitOperations.LeadingZeroCount(_sa.x) + BitOperations.LeadingZeroCount(_sa.y) + BitOperations.LeadingZeroCount(_sa.z) + BitOperations.LeadingZeroCount(_sa.w));

    [Benchmark, BenchmarkCategory("LeadingZeroCount")]
    public void Sia_LeadingZeroCount() { var r = math.lzcnt(_sa); Sink(r.x + r.y + r.z + r.w); }
}

#endregion

#region TrailingZeroCount

[BenchmarkCategory("LowPriority")]
public class UIntTrailingZeroCount : BenchBase
{
    private uint _sa;

    protected override void OnSetup() => _sa = math.asuint(NextF());

    [Benchmark(Baseline = true), BenchmarkCategory("TrailingZeroCount")]
    public void Sys_TrailingZeroCount() => Sink(BitOperations.TrailingZeroCount(_sa));

    [Benchmark, BenchmarkCategory("TrailingZeroCount")]
    public void Sia_TrailingZeroCount() => Sink(math.tzcnt(_sa));
}

[BenchmarkCategory("LowPriority")]
public class UInt2TrailingZeroCount : BenchBase
{
    private uint2 _sa;

    protected override void OnSetup() => _sa = new(math.asuint(NextF()), math.asuint(NextF()));

    [Benchmark(Baseline = true), BenchmarkCategory("TrailingZeroCount")]
    public void Sys_TrailingZeroCount() => Sink(BitOperations.TrailingZeroCount(_sa.x) + BitOperations.TrailingZeroCount(_sa.y));

    [Benchmark, BenchmarkCategory("TrailingZeroCount")]
    public void Sia_TrailingZeroCount() { var r = math.tzcnt(_sa); Sink(r.x + r.y); }
}

[BenchmarkCategory("LowPriority")]
public class UInt3TrailingZeroCount : BenchBase
{
    private uint3 _sa;

    protected override void OnSetup() => _sa = new(math.asuint(NextF()), math.asuint(NextF()), math.asuint(NextF()));

    [Benchmark(Baseline = true), BenchmarkCategory("TrailingZeroCount")]
    public void Sys_TrailingZeroCount() => Sink(BitOperations.TrailingZeroCount(_sa.x) + BitOperations.TrailingZeroCount(_sa.y) + BitOperations.TrailingZeroCount(_sa.z));

    [Benchmark, BenchmarkCategory("TrailingZeroCount")]
    public void Sia_TrailingZeroCount() { var r = math.tzcnt(_sa); Sink(r.x + r.y + r.z); }
}

[BenchmarkCategory("LowPriority")]
public class UInt4TrailingZeroCount : BenchBase
{
    private uint4 _sa;

    protected override void OnSetup() => _sa = new(math.asuint(NextF()), math.asuint(NextF()), math.asuint(NextF()), math.asuint(NextF()));

    [Benchmark(Baseline = true), BenchmarkCategory("TrailingZeroCount")]
    public void Sys_TrailingZeroCount() => Sink(BitOperations.TrailingZeroCount(_sa.x) + BitOperations.TrailingZeroCount(_sa.y) + BitOperations.TrailingZeroCount(_sa.z) + BitOperations.TrailingZeroCount(_sa.w));

    [Benchmark, BenchmarkCategory("TrailingZeroCount")]
    public void Sia_TrailingZeroCount() { var r = math.tzcnt(_sa); Sink(r.x + r.y + r.z + r.w); }
}

#endregion

#region Log2

[BenchmarkCategory("LowPriority")]
public class UIntLog2 : BenchBase
{
    private uint _sa;

    protected override void OnSetup() => _sa = math.asuint(NextF());

    [Benchmark(Baseline = true), BenchmarkCategory("Log2")]
    public void Sys_Log2() => Sink(BitOperations.Log2(_sa));

    [Benchmark, BenchmarkCategory("Log2")]
    public void Sia_Log2() => Sink(math.floorlog2(_sa));
}

[BenchmarkCategory("LowPriority")]
public class UInt2Log2 : BenchBase
{
    private uint2 _sa;

    protected override void OnSetup() => _sa = new(math.asuint(NextF()), math.asuint(NextF()));

    [Benchmark(Baseline = true), BenchmarkCategory("Log2")]
    public void Sys_Log2() => Sink(BitOperations.Log2(_sa.x) + BitOperations.Log2(_sa.y));

    [Benchmark, BenchmarkCategory("Log2")]
    public void Sia_Log2() { var r = math.floorlog2(_sa); Sink(r.x + r.y); }
}

[BenchmarkCategory("LowPriority")]
public class UInt3Log2 : BenchBase
{
    private uint3 _sa;

    protected override void OnSetup() => _sa = new(math.asuint(NextF()), math.asuint(NextF()), math.asuint(NextF()));

    [Benchmark(Baseline = true), BenchmarkCategory("Log2")]
    public void Sys_Log2() => Sink(BitOperations.Log2(_sa.x) + BitOperations.Log2(_sa.y) + BitOperations.Log2(_sa.z));

    [Benchmark, BenchmarkCategory("Log2")]
    public void Sia_Log2() { var r = math.floorlog2(_sa); Sink(r.x + r.y + r.z); }
}

[BenchmarkCategory("LowPriority")]
public class UInt4Log2 : BenchBase
{
    private uint4 _sa;

    protected override void OnSetup() => _sa = new(math.asuint(NextF()), math.asuint(NextF()), math.asuint(NextF()), math.asuint(NextF()));

    [Benchmark(Baseline = true), BenchmarkCategory("Log2")]
    public void Sys_Log2() => Sink(BitOperations.Log2(_sa.x) + BitOperations.Log2(_sa.y) + BitOperations.Log2(_sa.z) + BitOperations.Log2(_sa.w));

    [Benchmark, BenchmarkCategory("Log2")]
    public void Sia_Log2() { var r = math.floorlog2(_sa); Sink(r.x + r.y + r.z + r.w); }
}

#endregion

#region RoundUpToPowerOf2

[BenchmarkCategory("LowPriority")]
public class UIntRoundUpToPowerOf2 : BenchBase
{
    private uint _sa;

    protected override void OnSetup() => _sa = math.asuint(NextF());

    [Benchmark(Baseline = true), BenchmarkCategory("RoundUpToPowerOf2")]
    public void Sys_RoundUpToPowerOf2() => Sink(BitOperations.RoundUpToPowerOf2(_sa));

    [Benchmark, BenchmarkCategory("RoundUpToPowerOf2")]
    public void Sia_RoundUpToPowerOf2() => Sink(math.ceilpow2(_sa));
}

[BenchmarkCategory("LowPriority")]
public class UInt2RoundUpToPowerOf2 : BenchBase
{
    private uint2 _sa;

    protected override void OnSetup() => _sa = new(math.asuint(NextF()), math.asuint(NextF()));

    [Benchmark(Baseline = true), BenchmarkCategory("RoundUpToPowerOf2")]
    public void Sys_RoundUpToPowerOf2() => Sink(BitOperations.RoundUpToPowerOf2(_sa.x) + BitOperations.RoundUpToPowerOf2(_sa.y));

    [Benchmark, BenchmarkCategory("RoundUpToPowerOf2")]
    public void Sia_RoundUpToPowerOf2() { var r = math.ceilpow2(_sa); Sink(r.x + r.y); }
}

[BenchmarkCategory("LowPriority")]
public class UInt3RoundUpToPowerOf2 : BenchBase
{
    private uint3 _sa;

    protected override void OnSetup() => _sa = new(math.asuint(NextF()), math.asuint(NextF()), math.asuint(NextF()));

    [Benchmark(Baseline = true), BenchmarkCategory("RoundUpToPowerOf2")]
    public void Sys_RoundUpToPowerOf2() => Sink(BitOperations.RoundUpToPowerOf2(_sa.x) + BitOperations.RoundUpToPowerOf2(_sa.y) + BitOperations.RoundUpToPowerOf2(_sa.z));

    [Benchmark, BenchmarkCategory("RoundUpToPowerOf2")]
    public void Sia_RoundUpToPowerOf2() { var r = math.ceilpow2(_sa); Sink(r.x + r.y + r.z); }
}

[BenchmarkCategory("LowPriority")]
public class UInt4RoundUpToPowerOf2 : BenchBase
{
    private uint4 _sa;

    protected override void OnSetup() => _sa = new(math.asuint(NextF()), math.asuint(NextF()), math.asuint(NextF()), math.asuint(NextF()));

    [Benchmark(Baseline = true), BenchmarkCategory("RoundUpToPowerOf2")]
    public void Sys_RoundUpToPowerOf2() => Sink(BitOperations.RoundUpToPowerOf2(_sa.x) + BitOperations.RoundUpToPowerOf2(_sa.y) + BitOperations.RoundUpToPowerOf2(_sa.z) + BitOperations.RoundUpToPowerOf2(_sa.w));

    [Benchmark, BenchmarkCategory("RoundUpToPowerOf2")]
    public void Sia_RoundUpToPowerOf2() { var r = math.ceilpow2(_sa); Sink(r.x + r.y + r.z + r.w); }
}

#endregion

#region IsPow2

[BenchmarkCategory("LowPriority")]
public class UIntIsPow2 : BenchBase
{
    private uint _sa;

    protected override void OnSetup() => _sa = math.asuint(NextF());

    [Benchmark(Baseline = true), BenchmarkCategory("IsPow2")]
    public void Sys_IsPow2() => Sink(BitOperations.IsPow2(_sa));

    [Benchmark, BenchmarkCategory("IsPow2")]
    public void Sia_IsPow2() => Sink(math.ispow2(_sa));
}

[BenchmarkCategory("LowPriority")]
public class UInt2IsPow2 : BenchBase
{
    private uint2 _sa;

    protected override void OnSetup() => _sa = new(math.asuint(NextF()), math.asuint(NextF()));

    [Benchmark(Baseline = true), BenchmarkCategory("IsPow2")]
    public void Sys_IsPow2() => Sink(BitOperations.IsPow2(_sa.x) || BitOperations.IsPow2(_sa.y));

    [Benchmark, BenchmarkCategory("IsPow2")]
    public void Sia_IsPow2() => Sink(math.any(math.ispow2(_sa)));
}

[BenchmarkCategory("LowPriority")]
public class UInt3IsPow2 : BenchBase
{
    private uint3 _sa;

    protected override void OnSetup() => _sa = new(math.asuint(NextF()), math.asuint(NextF()), math.asuint(NextF()));

    [Benchmark(Baseline = true), BenchmarkCategory("IsPow2")]
    public void Sys_IsPow2() => Sink(BitOperations.IsPow2(_sa.x) || BitOperations.IsPow2(_sa.y) || BitOperations.IsPow2(_sa.z));

    [Benchmark, BenchmarkCategory("IsPow2")]
    public void Sia_IsPow2() => Sink(math.any(math.ispow2(_sa)));
}

[BenchmarkCategory("LowPriority")]
public class UInt4IsPow2 : BenchBase
{
    private uint4 _sa;

    protected override void OnSetup() => _sa = new(math.asuint(NextF()), math.asuint(NextF()), math.asuint(NextF()), math.asuint(NextF()));

    [Benchmark(Baseline = true), BenchmarkCategory("IsPow2")]
    public void Sys_IsPow2() => Sink(BitOperations.IsPow2(_sa.x) || BitOperations.IsPow2(_sa.y) || BitOperations.IsPow2(_sa.z) || BitOperations.IsPow2(_sa.w));

    [Benchmark, BenchmarkCategory("IsPow2")]
    public void Sia_IsPow2() => Sink(math.any(math.ispow2(_sa)));
}

#endregion
