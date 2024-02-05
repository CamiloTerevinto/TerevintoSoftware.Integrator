using TerevintoSoftware.Integrator.Utilities;

namespace TerevintoSoftware.Integrator.Tests.Utilities;

[TestFixture]
public class UrlTemplateHelperTests
{
    [TestCase("[controller]", "TestCase", "test-case")]
    [TestCase("[controller]/{id:guid}", "TestCase", "test-case/{id}")]
    [TestCase("test-case", "TestCase", "test-case")]
    [TestCase("test-case/{id:guid}", "TestCase", "test-case/{id}")]
    public void ControllerRouteTemplateIsCleaned(string controllerRoute, string controllerName, string expectedUrl)
    {
        Assert.That(UrlTemplateHelpers.CleanControllerTemplate(controllerRoute, controllerName), Is.EqualTo(expectedUrl));
    }

    [TestCase("test/id", "test/id")]
    [TestCase("test/{id}", "test/{id}")]
    [TestCase("test/{id:guid}", "test/{id}")]
    [TestCase("test/{id:guid}/{from}", "test/{id}/{from}")]
    [TestCase("test/{id:guid}/{from:int}", "test/{id}/{from}")]
    public void ContraintsAreRemovedFromUrl(string url, string expectedUrl)
    {
        Assert.That(UrlTemplateHelpers.RemoveConstraintsFromUrl(url), Is.EqualTo(expectedUrl));
    }
}
