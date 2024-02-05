using TerevintoSoftware.Integrator.Models;

namespace TerevintoSoftware.Integrator.Templates;

internal class FixtureTemplate(FixtureModel fixtureModel)
{
    private readonly TemplateBuilder _builder = new();
    private readonly FixtureModel _fixtureModel = fixtureModel;

    internal string GetTemplate()
    {
        AddUsingStatements(_fixtureModel.RequiredUsings);

        _builder.AddEmptyLine();

        AddNamespace(_fixtureModel.ClassNamespace);
        _builder.AddEmptyLine();

        AddClassDeclaration(_fixtureModel.Controller.Type.Name, _fixtureModel.Options.BaseClassName);
        _builder.BeginBlock();

        AddTests();

        _builder.EndBlock();

        return _builder.Build();
    }

    private void AddUsingStatements(List<string> usings)
    {
        const string template = "using {0};";

        if (_fixtureModel.ClassNamespace != _fixtureModel.Options.BaseClassNamespace)
        {
            usings.Add(_fixtureModel.Options.BaseClassNamespace);
        }

        usings.Add("NUnit.Framework");
        usings.Add("System.Net.Http.Json");

        foreach (var statement in usings.Order())
        {
            _builder.AddIndented(string.Format(template, statement));
        }
    }

    private void AddNamespace(string ns)
    {
        _builder.AddIndented($"namespace {ns};");
    }

    private void AddClassDeclaration(string className, string baseClassName)
    {
        _builder.AddIndented("[TestFixture]");
        _builder.AddIndented($"public class {className}IntegrationTests : {baseClassName}");
    }

    private void AddTests()
    {
        for (var i = 0; i < _fixtureModel.Controller.Actions.Length; i++)
        {
            var method = _fixtureModel.Controller.Actions[i];

            new TestTemplate(_fixtureModel, _builder).CreateTemplate(method);

            if (i != _fixtureModel.Controller.Actions.Length - 1)
            {
                _builder.AddEmptyLine();
            }
        }
    }
}
