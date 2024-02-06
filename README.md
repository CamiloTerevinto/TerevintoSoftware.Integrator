# TerevintoSoftware.Integrator

[![Nuget version](https://img.shields.io/nuget/v/TerevintoSoftware.Integrator)](https://www.nuget.org/packages/TerevintoSoftware.Integrator/)

This project aims to provide a way for c# developers to quickly create ASP.NET Core Integration Tests for an entire project at once.  
This is meant to help people that have a solution without any tests to quickly get up and running, avoiding hours of writing repetitive boilerplate code.

This currently supports only NUnit (v3) tests, and it was tested with fairly simple/common scenarios (see the SampleLibrary folder).

## Sample usage

1. Install the tool: `dotnet tool install TerevintoSoftware.Integrator`
2. (optional) See the available options with `dotnet-integrator --help`
3. Run the generator: `dotnet-integrator "path/to/mvc/project/folder" "path/to/tests/project-or-folder" -n YourApp.Tests.Tests`

Do note that the paths can be absolute or relative, and the tool uses the built (Release or Debug) assembly from the input project.

Considering an endpoint like:

```csharp
[HttpGet("{id}/async-wrapped")]
public async Task<ActionResult<WeatherForecast>> GetWrappedAsync(int id)
{
}
```

The tool would generate a test like:

```csharp
[Test]
public async Task Test_Get_GetWrappedAsync()
{
    // Arrange
    int id = default;
    WeatherForecast expectedResult = default;
    var httpClient = GetClient();

    // Act
    var requestUri = $"weather/{id}/async-wrapped";
    var httpResult = await httpClient.GetAsync(requestUri);

    // Assert
    Assert.That(httpResult.IsSuccessStatusCode, Is.True);
    var contentResult = await httpResult.Content.ReadFromJsonAsync<WeatherForecast>();
    Assert.That(contentResult, Is.EqualTo(expectedResult));
}
```

See more examples in the Samples folder.

## How to build

* Install Visual Studio 2022 (.NET 8 required), if needed. 
* Install git, if needed.
* Clone this repository.
* Build from Visual Studio or through `dotnet build`.

### Running tests

Once the solution is compiled, tests can be run either from Visual Studio's Test Explorer window, or through `dotnet test`.

## License

The .NET Tool and this solution are licensed under the [MIT license](/LICENSE).

## Bug reports and feature requests

Please use the [issue tracker](https://github.com/CamiloTerevinto/TerevintoSoftware.Integrator/issues) and ensure your question/feedback was not previously reported.
