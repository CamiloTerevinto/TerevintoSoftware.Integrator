namespace TerevintoSoftware.Integrator.Configuration;

public class TestGenerationOptions
{
    /// <summary>
    /// The path to the project's assembly to be used for generation.
    /// </summary>
    public string AssemblyPath { get; }

    /// <summary>
    /// The directory where the generated site will be placed.
    /// </summary>
    public string OutputPath { get; }

    /// <summary>
    /// The base namespace to use for the test classes generated.
    /// </summary>
    public string BaseTestNamespace { get; set; }

    /// <summary>
    /// Creates a new instance of <see cref="TestGenerationOptions"/>.
    /// </summary>
    /// <param name="assemblyPath">The path to the assembly to use to find the classes to test in.</param>
    /// <param name="outputPath">The folder path to put the generated tests in.</param>
    /// <param name="baseTestNamespace">The prefix namespace to use in all generated tests.</param>
    public TestGenerationOptions(string assemblyPath, string outputPath, string baseTestNamespace)
    {
        AssemblyPath = assemblyPath;
        OutputPath = outputPath;
        BaseTestNamespace = baseTestNamespace;
    }
}
