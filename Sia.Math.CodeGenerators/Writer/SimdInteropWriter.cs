using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace Sia.Math.CodeGenerators.Writer;

public class SimdInteropWriter(VectorType type) : ITypeSourceWriter
{
    public HashSet<string> Imports { get; } = ["System.Runtime.CompilerServices", "System.Runtime.Intrinsics"];
    public HashSet<string> Inherits { get; } = [];

    public Action<IndentedTextWriter> TypeSourceWriter => source =>
    {
        if (!SimdSupport.IsEligibleVector(type.BaseType, type.Rows, type.Columns))
        {
            return;
        }

        var vectorName = SimdSupport.NativeVectorTypeName(type.BaseType, type.Rows);
        var vectorClass = SimdSupport.NativeVectorClassName(type.BaseType, type.Rows);
        var laneCount = SimdSupport.NativeLaneCount(type.BaseType, type.Rows);
        var exactFit = SimdSupport.IsExactFit(type.BaseType, type.Rows) || SimdSupport.IsPaddedFit(type.BaseType, type.Rows);

        source.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
        if (exactFit)
        {
            source.WriteLine("internal readonly {0} AsSimd() => Unsafe.As<{1}, {0}>(ref Unsafe.AsRef(in this));", vectorName, type.TypeName);
        }
        else
        {
            var zero = type.BaseType.ToTypedLiteral(0);
            var args = string.Join(", ", VectorType.VectorFields.Take(type.Rows)
                .Concat(Enumerable.Repeat(zero, laneCount - type.Rows)));
            source.WriteLine("internal readonly {0} AsSimd() => {1}.Create({2});", vectorName, vectorClass, args);
        }

        source.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
        if (exactFit)
        {
            source.WriteLine("internal readonly {0} AsSimdUnsafe() => AsSimd();", vectorName);
        }
        else
        {
            source.WriteLine("internal readonly {0} AsSimdUnsafe() => Unsafe.ReadUnaligned<{0}>(ref Unsafe.As<{1}, byte>(ref Unsafe.AsRef(in this)));", vectorName, type.TypeName);
        }

        source.WriteLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
        if (exactFit)
        {
            source.WriteLine("internal {0}({1} v) => this = Unsafe.As<{1}, {0}>(ref v);", type.TypeName, vectorName);
        }
        else
        {
            var assigns = string.Join(" ", VectorType.VectorFields.Take(type.Rows)
                .Select((fieldName, i) => $"{fieldName} = v.GetElement({i});"));
            source.WriteLine("internal {0}({1} v) {{ {2} }}", type.TypeName, vectorName, assigns);
        }
    };
}
