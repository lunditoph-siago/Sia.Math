namespace Sia.Math.CodeGenerators.Generation;

using System.Collections.Generic;

public static class TypeShapeCatalog
{
    public static IReadOnlyList<TypeShape> All { get; } = Create();

    private static TypeShape[] Create()
    {
        var shapes = new List<TypeShape>(60);

        for (var rows = 2; rows <= 4; rows++) {
            for (var columns = 1; columns <= 4; columns++) {
                shapes.Add(new TypeShape(
                    BaseType.Bool,
                    rows,
                    columns,
                    Features.BitwiseLogic));
                shapes.Add(new TypeShape(
                    BaseType.Int,
                    rows,
                    columns,
                    Features.All));
                shapes.Add(new TypeShape(
                    BaseType.UInt,
                    rows,
                    columns,
                    Features.All));
                shapes.Add(new TypeShape(
                    BaseType.Float,
                    rows,
                    columns,
                    Features.Arithmetic | Features.UnaryNegation));
                shapes.Add(new TypeShape(
                    BaseType.Double,
                    rows,
                    columns,
                    Features.Arithmetic | Features.UnaryNegation));
            }
        }

        return shapes.ToArray();
    }
}
