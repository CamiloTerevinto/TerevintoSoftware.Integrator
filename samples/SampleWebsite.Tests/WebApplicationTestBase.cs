using Microsoft.AspNetCore.Mvc.Testing;

namespace SampleWebsite.Tests;

public class WebApplicationTestBase
{
    private readonly WebApplicationFactory<Program> _webApplicationFactory = new CustomWebApplicationFactory();

    public HttpClient GetClient() => _webApplicationFactory.CreateClient();
}
