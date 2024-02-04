using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace TerevintoSoftware.Integrator;

public class GenerateCommandSettings : CommandSettings
{
    [CommandArgument(0, "<PROJECT_PATH>")]
    [Description("The path to the project containing the ASP.NET Core MVC website.")]
    public string ProjectPath { get; set; } = string.Empty;

    [CommandArgument(1, "<OUTPUT_PATH>")]
    [Description("The path to the output directory, typically a folder within the Tests project.")]
    public string OutputPath { get; set; } = string.Empty;

    [CommandOption("-n|--namespace")]
    [Description("The namespace to use for test classes.")]
    public string TestNamespace { get; set; } = string.Empty;

    public override ValidationResult Validate()
    {
        if (string.IsNullOrEmpty(ProjectPath))
        {
            return ValidationResult.Error("A project path is required");
        }

        ProjectPath = Path.GetFullPath(ProjectPath);

        if (!Directory.Exists(ProjectPath))
        {
            return ValidationResult.Error($"The project path '{ProjectPath}' does not exist.");
        }

        if (string.IsNullOrEmpty(OutputPath))
        {
            return ValidationResult.Error("The output path is required.");
        }

        OutputPath = Path.GetFullPath(OutputPath);

        if (string.IsNullOrEmpty (TestNamespace))
        {
            return ValidationResult.Error("The namespace for tests is required");
        }

        return ValidationResult.Success();
    }
}
