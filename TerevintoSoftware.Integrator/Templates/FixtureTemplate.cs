using TerevintoSoftware.Integrator.Models;
using TerevintoSoftware.Integrator.Templates;

namespace TerevintoSoftware.Integrator.Templates;

internal class FixtureTemplate(FixtureModel fixtureModel)
{
    private class Context
    {
        public record Dependency(Type Type, string Name);

        public bool UsingMocks { get; }
        public IReadOnlyCollection<Dependency> Interfaces { get; }
        public IReadOnlyCollection<Dependency> Classes { get; }

        public Context(IReadOnlyCollection<Type> declaredDependencies)
        {
            if (declaredDependencies.Count == 0)
            {
                Interfaces = Array.Empty<Dependency>();
                Classes = Array.Empty<Dependency>();

                return;
            }

            var types = declaredDependencies.Where(x => x.IsInterface || x.IsClass).Select(x =>
            {
                string dependencyName;

                if (x.IsInterface)
                {
                    dependencyName = x.Name[1..];
                }
                else
                {
                    dependencyName = x.Name;
                }

                dependencyName = dependencyName[0].ToString().ToLower() + dependencyName[1..];

                return new Dependency(x, dependencyName);
            }).ToArray();

            Interfaces = types.Where(x => x.Type.IsInterface).ToArray();
            Classes = types.Where(x => x.Type.IsClass).ToArray();
            UsingMocks = Interfaces.Count > 0;
        }
    }

    private readonly TemplateBuilder _builder = new();
    private readonly Context _context = new(fixtureModel.Dependencies);
    private readonly FixtureModel _fixtureModel = fixtureModel;

    internal string GetTemplate()
    {
        AddUsings(_fixtureModel.RequiredUsings);

        _builder.AddEmptyLine();

        AddNamespace(_fixtureModel.ClassNamespace);
        _builder.AddEmptyLine();

        AddClassDeclaration(_fixtureModel.Controller.Type.Name, _fixtureModel.Options.BaseClassName);
        _builder.BeginBlock();

        AddConstructor(_fixtureModel.Controller.Type.Name);

        _builder.AddEmptyLine();

        AddTests();

        _builder.EndBlock();

        return _builder.Build();
    }

    private void AddUsings(List<string> usings)
    {
        const string template = "using {0};";

        if (_context.UsingMocks)
        {
            usings.Add("Moq");
        }

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

    private void AddConstructor(string className)
    {
        if (_context.Classes.Count == 0 && _context.Interfaces.Count == 0)
        {
            // There's no need for a constructor if there are no dependencies
            return;
        }

        const string template = "public {0}Test()";

        _builder.AddFormatIndented(template, className);
        _builder.BeginBlock();

        if (_context.UsingMocks)
        {
            const string mockRepositoryTemplate = "_mockRepository = new MockRepository(MockBehavior.Default);";
            const string mockTemplate = "_{0} = _mockRepository.Create<{1}>();";

            _builder.AddIndented(mockRepositoryTemplate);

            foreach (var dependency in _context.Interfaces)
            {
                _builder.AddFormatIndented(mockTemplate, dependency.Name, dependency.Type.Name);
            }
        }

        const string classTemplate = "_{0} = new {1}();";

        foreach (var dependency in _context.Classes)
        {
            _builder.AddFormatIndented(classTemplate, dependency.Name, dependency.Type.Name);
        }

        _builder.EndBlock();

        _builder.AddEmptyLine();
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
