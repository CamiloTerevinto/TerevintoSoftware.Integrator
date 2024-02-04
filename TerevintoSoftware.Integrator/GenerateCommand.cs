using Spectre.Console;
using Spectre.Console.Cli;
using TerevintoSoftware.Integrator.Configuration;
using TerevintoSoftware.Integrator.Utilities;

namespace TerevintoSoftware.Integrator;

public class GenerateCommand : AsyncCommand<GenerateCommandSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, GenerateCommandSettings settings)
    {
        var assemblyPath = AssemblyHelpers.FindAssemblyPath(settings.ProjectPath);

        AnsiConsole.MarkupLine($"[blue]Info:[/] using assembly path: {assemblyPath}");

        await TestGenerator.GenerateTestsAsync(new TestGenerationOptions(assemblyPath, settings.OutputPath, settings.TestNamespace));

        return 0;
    }
}
