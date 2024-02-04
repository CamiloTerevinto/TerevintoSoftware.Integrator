using System.Text;

namespace TerevintoSoftware.Integrator.Templates;

internal class TemplateBuilder(int initialIndentationLevel = 0)
{
    internal int CurrentIndentationLevel { get; private set; } = initialIndentationLevel;
    private readonly StringBuilder _builder = new();

    internal void BeginBlock()
    {
        AddIndented("{");
        CurrentIndentationLevel++;
    }

    internal void EndBlock()
    {
        CurrentIndentationLevel--;
        AddIndented("}");
    }

    internal void AddEmptyLine()
    {
        _builder.AppendLine();
    }

    internal void AddIndented(string value)
    {
        _builder.AppendLine(new string(' ', CurrentIndentationLevel * 4) + value);
    }

    internal void AddFormatIndented(string value, params object[] args)
    {
        _builder.AppendFormat(new string(' ', CurrentIndentationLevel * 4) + value + Environment.NewLine, args);
    }

    internal string Build()
    {
        return _builder.ToString();
    }
}
