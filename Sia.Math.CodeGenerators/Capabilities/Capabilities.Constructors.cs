using System.Linq;
using System.Text;

namespace Sia.Math.CodeGenerators.Capabilities;

public static class Constructors
{
    private static readonly string[] s_Imports = ["System.Runtime.CompilerServices"];

    public static CodeFragment Generate(TypeShape shape)
    {
        if (shape.IsMatrix)
            return GenerateMatrixConstructors(shape);
        return GenerateVectorConstructors(shape);
    }

    private static CodeFragment GenerateVectorConstructors(TypeShape shape)
    {
        var typeBody = new StringBuilder();
        var mathBody = new StringBuilder();
        var components = new int[4];

        GenerateOverloads(shape, shape.Rows, 0, components, typeBody, mathBody);

        return new CodeFragment
        {
            Usings = s_Imports,
            TypeBody = typeBody.ToString().TrimEnd(),
            MathBody = mathBody.Length > 0 ? mathBody.ToString().TrimEnd() : null
        };
    }

    private static void GenerateOverloads(
        TypeShape shape, int remaining, int numParams, int[] paramComps,
        StringBuilder typeBody, StringBuilder mathBody)
    {
        if (remaining == 0)
        {
            EmitVectorCtor(shape, numParams, paramComps, typeBody, mathBody);
            return;
        }
        for (var i = 1; i <= remaining; i++)
        {
            paramComps[numParams] = i;
            GenerateOverloads(shape, remaining - i, numParams + 1, paramComps, typeBody, mathBody);
        }
    }

    private static void EmitVectorCtor(
        TypeShape shape, int numParams, int[] paramComps,
        StringBuilder typeBody, StringBuilder mathBody)
    {
        var typeParams = BuildTypeParams(shape.BaseType, numParams, paramComps);
        var descs = BuildDescriptions(shape, numParams, paramComps);

        foreach (var d in descs.Take(descs.Count - 1))
            typeBody.AppendLine($"        {d}");
        typeBody.AppendLine("        [MethodImpl(MethodImplOptions.AggressiveInlining)]");
        typeBody.AppendLine($"        public {shape.TypeName}({typeParams})");
        typeBody.AppendLine("        {");

        var compIdx = 0;
        for (var p = 0; p < numParams; p++)
        {
            var cnt = paramComps[p];
            var compStr = string.Concat(TypeShape.Components.Skip(compIdx).Take(cnt));
            for (var j = 0; j < cnt; j++)
            {
                var rhs = cnt > 1 ? $"{compStr}.{TypeShape.Components[j]}" : compStr;
                typeBody.AppendLine($"            this.{TypeShape.Components[compIdx]} = {rhs};");
                compIdx++;
            }
        }
        if (shape.NeedsPadding)
            typeBody.AppendLine("            this.__pad = 0;");
        typeBody.AppendLine("        }");

        var bodyBuilder = new StringBuilder();
        compIdx = 0;
        for (var p = 0; p < numParams; p++)
        {
            var cnt = paramComps[p];
            var compStr = string.Concat(TypeShape.Components.Skip(compIdx).Take(cnt));
            if (p != 0) bodyBuilder.Append(", ");
            bodyBuilder.Append(compStr);
            compIdx += cnt;
        }

        foreach (var d in descs)
            mathBody.AppendLine($"        {d}");
        mathBody.AppendLine("        [MethodImpl(MethodImplOptions.AggressiveInlining)]");
        mathBody.AppendLine($"        public static {shape.TypeName} {shape.TypeName}({typeParams}) => new {shape.TypeName}({bodyBuilder});");
    }

    private static string BuildTypeParams(BaseType baseType, int numParams, int[] paramComps)
    {
        var compIdx = 0;
        return string.Join(", ", paramComps.Take(numParams).Select(cnt =>
        {
            var paramType = baseType.ToTypeName(cnt, 1);
            var compStr = string.Concat(TypeShape.Components.Skip(compIdx).Take(cnt));
            compIdx += cnt;
            return $"{paramType} {compStr}";
        }));
    }

    private static System.Collections.Generic.List<string> BuildDescriptions(TypeShape shape, int numParams, int[] paramComps)
    {
        var parts = Enumerable.Range(0, numParams)
            .GroupBy(i => paramComps[i])
            .Select(g => shape.BaseType.ToTypeDescription(g.Key, 1, g.Count()))
            .ToList();

        var paramDesc = parts.Count > 1
            ? $"{string.Join(", ", parts.Take(parts.Count - 1))} and {parts.Last()}"
            : parts.Last();

        var descs = new System.Collections.Generic.List<string>
        {
            $"/// <summary>Constructs a <see cref=\"{shape.TypeName}\" /> vector from {paramDesc}.</summary>"
        };

        descs.AddRange(paramComps.Take(numParams).Select((cnt, i) =>
        {
            var compStr = string.Concat(TypeShape.Components.Skip(paramComps.Take(i).Sum()).Take(cnt));
            var plural = cnt > 1 ? "fields" : "field";
            return $"/// <param name=\"{compStr}\">The value to assign to the <see cref=\"{compStr}\" /> {plural}.</param>";
        }));

        descs.Add($"/// <returns>The <see cref=\"{shape.TypeName}\" /> constructed from arguments.</returns>");
        return descs;
    }

    private static CodeFragment GenerateMatrixConstructors(TypeShape shape)
    {
        var typeBody = new StringBuilder();
        var mathBody = new StringBuilder();

        var colFields = TypeShape.MatrixFields.Take(shape.Columns).ToArray();
        var colType = shape.BaseType.ToTypeName(shape.Rows, 1);
        var colParams = string.Join(", ", colFields.Select(f => $"{colType} {f}"));

        typeBody.AppendLine($"        /// <summary>Constructs a <see cref=\"{shape.TypeName}\" /> matrix from {shape.BaseType.ToTypeDescription(shape.Rows, 1, shape.Columns)}.</summary>");
        foreach (var f in colFields)
            typeBody.AppendLine($"        /// <param name=\"{f}\">The value to assign to the <see cref=\"{f}\" /> field.</param>");
        typeBody.AppendLine($"        /// <returns>The <see cref=\"{shape.TypeName}\" /> constructed from arguments.</returns>");
        typeBody.AppendLine("        [MethodImpl(MethodImplOptions.AggressiveInlining)]");
        typeBody.AppendLine($"        public {shape.TypeName}({colParams})");
        typeBody.AppendLine("        {");
        foreach (var f in colFields)
            typeBody.AppendLine($"            this.{f} = {f};");
        typeBody.AppendLine("        }");

        mathBody.AppendLine($"        [MethodImpl(MethodImplOptions.AggressiveInlining)]");
        mathBody.AppendLine($"        public static {shape.TypeName} {shape.TypeName}({colParams}) => new {shape.TypeName}({string.Join(", ", colFields)});");

        typeBody.AppendLine();
        mathBody.AppendLine();

        var rowParams = string.Join(", ", Enumerable.Range(0, shape.Rows * shape.Columns)
            .Select(i => $"{shape.BaseTypeName} m{i / shape.Columns}{i % shape.Columns}"));

        typeBody.AppendLine($"        /// <summary>Constructs a <see cref=\"{shape.TypeName}\" /> matrix from {shape.Rows * shape.Columns} <see cref=\"{shape.BaseTypeName}\" /> values given in row-major order.</summary>");
        foreach (var i in Enumerable.Range(0, shape.Rows * shape.Columns))
            typeBody.AppendLine($"        /// <param name=\"m{i / shape.Columns}{i % shape.Columns}\">The value to assign to the {(i % shape.Columns + 1).FormatOrdinals()} element in the {(i / shape.Columns + 1).FormatOrdinals()} row.</param>");
        typeBody.AppendLine($"        /// <returns>The <see cref=\"{shape.TypeName}\" /> constructed from arguments.</returns>");
        typeBody.AppendLine("        [MethodImpl(MethodImplOptions.AggressiveInlining)]");
        typeBody.AppendLine($"        public {shape.TypeName}({rowParams})");
        typeBody.AppendLine("        {");
        for (var c = 0; c < shape.Columns; c++)
        {
            var comps = string.Join(", ", Enumerable.Range(0, shape.Rows).Select(r => $"m{r}{c}"));
            typeBody.AppendLine($"            this.{TypeShape.MatrixFields[c]} = new {colType}({comps});");
        }
        typeBody.AppendLine("        }");

        var rowBody = string.Join(", ", Enumerable.Range(0, shape.Rows * shape.Columns)
            .Select(i => $"m{i / shape.Columns}{i % shape.Columns}"));
        mathBody.AppendLine($"        [MethodImpl(MethodImplOptions.AggressiveInlining)]");
        mathBody.AppendLine($"        public static {shape.TypeName} {shape.TypeName}({rowParams}) => new {shape.TypeName}({rowBody});");

        return new CodeFragment
        {
            Usings = s_Imports,
            TypeBody = typeBody.ToString().TrimEnd(),
            MathBody = mathBody.ToString().TrimEnd()
        };
    }
}
