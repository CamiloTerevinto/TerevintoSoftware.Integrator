using NUnit.Framework;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SampleWebsite.Tests;

[TestFixture]
public class WeatherControllerIntegrationTests : WebApplicationTestBase
{
    [Test]
    public async Task Test_Get_GetNull()
    {
        // Arrange
        var httpClient = GetClient();

        // Act
        var requestUri = $"weather";
        var httpResult = await httpClient.GetAsync(requestUri);

        // Assert
        Assert.That(httpResult.IsSuccessStatusCode, Is.True);
    }

    [Test]
    public async Task Test_Get_GetIenumerable()
    {
        // Arrange
        List<WeatherForecast> expectedResult = default;
        var httpClient = GetClient();

        // Act
        var requestUri = $"weather/ienumerable";
        var httpResult = await httpClient.GetAsync(requestUri);

        // Assert
        Assert.That(httpResult.IsSuccessStatusCode, Is.True);
        var contentResult = await httpResult.Content.ReadFromJsonAsync<List<WeatherForecast>>();
        Assert.That(contentResult, Is.EqualTo(expectedResult));
    }

    [Test]
    public async Task Test_Get_GetArray()
    {
        // Arrange
        WeatherForecast[] expectedResult = default;
        var httpClient = GetClient();

        // Act
        var requestUri = $"weather/array";
        var httpResult = await httpClient.GetAsync(requestUri);

        // Assert
        Assert.That(httpResult.IsSuccessStatusCode, Is.True);
        var contentResult = await httpResult.Content.ReadFromJsonAsync<WeatherForecast[]>();
        Assert.That(contentResult, Is.EqualTo(expectedResult));
    }

    [Test]
    public async Task Test_Get_GetList()
    {
        // Arrange
        List<WeatherForecast> expectedResult = default;
        var httpClient = GetClient();

        // Act
        var requestUri = $"weather/list";
        var httpResult = await httpClient.GetAsync(requestUri);

        // Assert
        Assert.That(httpResult.IsSuccessStatusCode, Is.True);
        var contentResult = await httpResult.Content.ReadFromJsonAsync<List<WeatherForecast>>();
        Assert.That(contentResult, Is.EqualTo(expectedResult));
    }

    [Test]
    public async Task Test_Get_GetEmpty()
    {
        // Arrange
        int id = default;
        var httpClient = GetClient();

        // Act
        var requestUri = $"weather/{id}";
        var httpResult = await httpClient.GetAsync(requestUri);

        // Assert
        Assert.That(httpResult.IsSuccessStatusCode, Is.True);
    }

    [Test]
    public async Task Test_Get_Get()
    {
        // Arrange
        int id = default;
        WeatherForecast expectedResult = default;
        var httpClient = GetClient();

        // Act
        var requestUri = $"weather/{id}/sync";
        var httpResult = await httpClient.GetAsync(requestUri);

        // Assert
        Assert.That(httpResult.IsSuccessStatusCode, Is.True);
        var contentResult = await httpResult.Content.ReadFromJsonAsync<WeatherForecast>();
        Assert.That(contentResult, Is.EqualTo(expectedResult));
    }

    [Test]
    public async Task Test_Get_GetWrapped()
    {
        // Arrange
        int id = default;
        WeatherForecast expectedResult = default;
        var httpClient = GetClient();

        // Act
        var requestUri = $"weather/{id}/sync-wrapped";
        var httpResult = await httpClient.GetAsync(requestUri);

        // Assert
        Assert.That(httpResult.IsSuccessStatusCode, Is.True);
        var contentResult = await httpResult.Content.ReadFromJsonAsync<WeatherForecast>();
        Assert.That(contentResult, Is.EqualTo(expectedResult));
    }

    [Test]
    public async Task Test_Get_GetAsync()
    {
        // Arrange
        int id = default;
        WeatherForecast expectedResult = default;
        var httpClient = GetClient();

        // Act
        var requestUri = $"weather/{id}/async";
        var httpResult = await httpClient.GetAsync(requestUri);

        // Assert
        Assert.That(httpResult.IsSuccessStatusCode, Is.True);
        var contentResult = await httpResult.Content.ReadFromJsonAsync<WeatherForecast>();
        Assert.That(contentResult, Is.EqualTo(expectedResult));
    }

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

    [Test]
    public async Task Test_Post_Create()
    {
        // Arrange
        WeatherForecast forecast = default;
        WeatherForecast expectedResult = default;
        var httpClient = GetClient();

        // Act
        var requestUri = $"weather";
        var httpResult = await httpClient.PostAsJsonAsync(requestUri, forecast);

        // Assert
        Assert.That(httpResult.IsSuccessStatusCode, Is.True);
        var contentResult = await httpResult.Content.ReadFromJsonAsync<WeatherForecast>();
        Assert.That(contentResult, Is.EqualTo(expectedResult));
    }

    [Test]
    public async Task Test_Put_Update()
    {
        // Arrange
        int id = default;
        WeatherForecast forecast = default;
        WeatherForecast expectedResult = default;
        var httpClient = GetClient();

        // Act
        var requestUri = $"weather/{id}";
        var httpResult = await httpClient.PutAsJsonAsync(requestUri, forecast);

        // Assert
        Assert.That(httpResult.IsSuccessStatusCode, Is.True);
        var contentResult = await httpResult.Content.ReadFromJsonAsync<WeatherForecast>();
        Assert.That(contentResult, Is.EqualTo(expectedResult));
    }

    [Test]
    public async Task Test_Delete_Delete()
    {
        // Arrange
        int id = default;
        var httpClient = GetClient();

        // Act
        var requestUri = $"weather/{id}";
        var httpResult = await httpClient.DeleteAsync(requestUri);

        // Assert
        Assert.That(httpResult.IsSuccessStatusCode, Is.True);
    }
}
