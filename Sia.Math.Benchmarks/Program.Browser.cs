#if BROWSER
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;

namespace Sia.Math.Benchmarks;

public sealed record BenchDef(
    string Group,
    string Name,
    int Iterations,
    Action Sys,
    Action Sia);

public static partial class BenchmarkRunner
{
    private const int PoolSize = 256;
    private static readonly float[] _pool;
    private static int _cursor;
    public static float Sunk;

    static BenchmarkRunner()
    {
        _pool = new float[PoolSize * 8];
        var rng = new Random(42);
        for (var i = 0; i < _pool.Length; i++)
            _pool[i] = (float)(rng.NextDouble() * 200.0 - 100.0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float NextF() => _pool[_cursor = (_cursor + 1) % PoolSize];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double NextD() => (double)NextF();

    [JSImport("initProgress", "main.js")]
    private static partial void InitProgress(int total);

    [JSImport("updateProgress", "main.js")]
    private static partial void UpdateProgress(int current, int total);

    [JSImport("addGroup", "main.js")]
    private static partial void AddGroup(string name);

    [JSImport("addResult", "main.js")]
    private static partial void AddResult(string name, double meanNs, double ratio);

    [JSImport("setStatus", "main.js")]
    private static partial void SetStatus(string text);

    public static void Main() { }

    [JSExport]
    public static async Task RunAll()
    {
        _cursor = 0;
        SetStatus("Running…");

        try
        {
            var defs = BenchRegistry.GetAll().ToList();
            InitProgress(defs.Count);
            var lastGroup = "";
            int idx = 0;
            foreach (var def in defs)
            {
                if (def.Group != lastGroup)
                {
                    AddGroup(def.Group);
                    lastGroup = def.Group;
                }
                Paired(def.Name, def.Iterations, def.Sys, def.Sia);
                idx++;
                UpdateProgress(idx, defs.Count);
                await Task.Yield();
            }
            SetStatus("Done");
        }
        catch (Exception ex)
        {
            SetStatus($"Error: {ex.Message}");
        }
    }

    private const int WarmupIters = 200;

    private static void Paired(string name, int iters, Action sysAct, Action siaAct)
    {
        for (var i = 0; i < WarmupIters; i++) { sysAct(); siaAct(); }

        var start = Stopwatch.GetTimestamp();
        for (var i = 0; i < iters; i++) sysAct();
        var sysTicks = Stopwatch.GetTimestamp() - start;

        start = Stopwatch.GetTimestamp();
        for (var i = 0; i < iters; i++) siaAct();
        var siaTicks = Stopwatch.GetTimestamp() - start;

        var sysNs = sysTicks * 1e9 / (iters * Stopwatch.Frequency);
        var siaNs = siaTicks * 1e9 / (iters * Stopwatch.Frequency);
        AddResult(name, siaNs, sysNs > 0 ? siaNs / sysNs : 1.0);
    }
}
#endif
