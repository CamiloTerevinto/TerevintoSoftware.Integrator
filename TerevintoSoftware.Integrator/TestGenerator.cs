using Spectre.Console;
using System.Reflection;
using TerevintoSoftware.Integrator.Configuration;
using TerevintoSoftware.Integrator.Models;
using TerevintoSoftware.Integrator.Templates;
using TerevintoSoftware.Integrator.Utilities;

namespace TerevintoSoftware.Integrator;

public static class TestGenerator
{
    public static async Task GenerateTestsAsync(TestGenerationOptions testGenerationOptions)
    {
        var assembly = Assembly.LoadFrom(testGenerationOptions.AssemblyPath);
        var controllers = ReflectionHelpers.FindControllerTypes(assembly.GetExportedTypes()).ToArray();
        var controllerData = ReflectionHelpers.GetControllerActions(controllers);

        AnsiConsole.MarkupLine($"[blue]Info:[/] found [yellow]{controllerData.Count}[/] controllers that can be used for generating tests");

        await RunAsync(controllerData, testGenerationOptions);

        AnsiConsole.MarkupLine("[green]Success:[/] finished generating tests");
    }

    private static async Task RunAsync(List<ControllerModel> controllers, TestGenerationOptions testGenerationOptions)
    {
#if DEBUG
        foreach (var controller in controllers)
        {
            await GenerateTestsForType(controller, testGenerationOptions);
        }
#else
        await Parallel.ForEachAsync(controllers, async (controller, ct) =>
        {
            await GenerateTestForType(controller, testGenerationOptions);
        });
#endif
    }

    private static async Task GenerateTestsForType(ControllerModel controller, TestGenerationOptions generationOptions)
    {
        try
        {
            var options = new FixtureGenerationOptions("WebApplicationTestBase", "SampleWebsite.Tests", "GetClient");
            var model = ReflectionHelpers.BuildModel(generationOptions.BaseTestNamespace, controller);
            model.Options = options;
            var template = new FixtureTemplate(model);

            var result = template.GetTemplate();

            var folders = model.ClassNamespace.Replace(generationOptions.BaseTestNamespace, "").Split('.', StringSplitOptions.RemoveEmptyEntries);
            var joinedPath = string.Join(Path.DirectorySeparatorChar, folders);
            var folderPath = Path.Combine(generationOptions.OutputPath, joinedPath);

            Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, $"{model.Controller.Type.Name}Test.cs");

            await File.WriteAllTextAsync(filePath, result);

            AnsiConsole.MarkupLine($"[blue]Info:[/] {controller.Actions.Length} tests generated for {controller.Type.Name}");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"Generating a test for the type {controller.Name} failed due to: {ex.Message}");
            AnsiConsole.WriteException(ex);
        }
    }
}
