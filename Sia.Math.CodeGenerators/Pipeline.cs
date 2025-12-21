using System;
using System.Linq;

namespace Sia.Math.CodeGenerators;

public static class Pipeline
{
    public static Func<TypeShape, CodeFragment> Compose(
        params Func<TypeShape, CodeFragment>[] capabilities)
        => shape => capabilities.Aggregate(CodeFragment.Empty,
            (frag, cap) => frag + cap(shape));
}
