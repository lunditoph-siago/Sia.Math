namespace Sia.Math.CodeGenerators;

using System;
using System.Collections.Generic;
using System.Text;

public sealed class SourceBuilder
{
    private readonly StringBuilder _text = new();
    private readonly string _indentText;

    public int Indent {
        get; private set;
    }

    public SourceBuilder(int indent = 0, string indentText = "    ")
    {
        if (indent < 0) {
            throw new ArgumentOutOfRangeException(nameof(indent));
        }

        Indent = indent;
        _indentText = indentText;
    }

    public SourceBuilder Line(string? text = null)
    {
        if (!string.IsNullOrEmpty(text)) {
            for (var i = 0; i < Indent; i++) {
                _text.Append(_indentText);
            }

            _text.Append(text);
        }

        _text.AppendLine();
        return this;
    }

    public SourceBuilder Lines(IEnumerable<string> lines)
    {
        foreach (var line in lines) {
            Line(line);
        }

        return this;
    }

    public SourceBuilder Block(string header, Action<SourceBuilder> body)
    {
        Line(header);
        Line("{");
        Indent++;
        body(this);
        Indent--;
        Line("}");
        return this;
    }

    public SourceBuilder Indented(Action<SourceBuilder> body)
    {
        Indent++;
        body(this);
        Indent--;
        return this;
    }

    public override string ToString() => _text.ToString();
}
