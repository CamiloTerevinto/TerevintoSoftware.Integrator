#nullable disable

namespace TerevintoSoftware.Integrator.Models;

public class FixtureModel
{
    public FixtureGenerationOptions Options { get; set; }
    public ControllerModel Controller { get; set; }
    public string ClassNamespace { get; set; }
    public List<string> RequiredUsings { get; set; }
}

public record FixtureGenerationOptions(string BaseClassName, string BaseClassNamespace, string ObtainHttpClientMethodName)
{
}