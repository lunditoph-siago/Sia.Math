using System.Linq;
using System.Text;

namespace Sia.Math.CodeGenerators.Capabilities;

public static class Shuffle
{
    public static CodeFragment Generate(TypeShape shape)
    {
        if (shape.IsMatrix) return CodeFragment.Empty;

        var mathBody = new StringBuilder();

        mathBody.AppendLine("        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]");
        mathBody.AppendLine($"        internal static {shape.BaseTypeName} select_shuffle_component({shape.TypeName} a, {shape.TypeName} b, ShuffleComponent component) => component switch {{");
        var comps = TypeShape.ShuffleComponents.Take(shape.Rows)
            .Concat(TypeShape.ShuffleComponents.Skip(4).Take(shape.Rows));
        foreach (var c in comps)
        {
            var idx = System.Array.IndexOf(TypeShape.ShuffleComponents, c);
            var target = idx >= 4 ? "b" : "a";
            var field = shape.Rows > 1 ? $".{TypeShape.VectorFields[idx % 4]}" : "";
            mathBody.AppendLine($"            ShuffleComponent.{c} => {target}{field},");
        }
        mathBody.AppendLine("            _ => throw new System.ArgumentException($\"Invalid shuffle component: {component}\")");
        mathBody.AppendLine("        };");

        for (var comp = 1; comp <= 4; comp++)
        {
            var resultType = shape.BaseType.ToTypeName(comp, 1);
            mathBody.AppendLine();
            mathBody.AppendLine($"        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]");
            var @params = string.Join(", ", Enumerable.Range(0, comp).Select(i => $"ShuffleComponent {TypeShape.VectorFields[i]}"));
            mathBody.AppendLine($"        public static {resultType} shuffle({shape.TypeName} left, {shape.TypeName} right, {@params}) =>");
            if (comp == 1)
                mathBody.AppendLine($"            select_shuffle_component(left, right, {TypeShape.VectorFields[0]});");
            else
                mathBody.AppendLine($"            new {resultType}({string.Join(", ", Enumerable.Range(0, comp).Select(i => $"select_shuffle_component(left, right, {TypeShape.VectorFields[i]})"))});");
        }

        return new CodeFragment
        {
            Usings = ["System.Runtime.CompilerServices"],
            MathBody = mathBody.ToString().TrimEnd()
        };
    }
}
