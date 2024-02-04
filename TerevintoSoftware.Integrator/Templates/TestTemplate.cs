using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TerevintoSoftware.Integrator.Models;
using TerevintoSoftware.Integrator.Utilities;

namespace TerevintoSoftware.Integrator.Templates;

internal class TestTemplate(FixtureModel fixtureModel, TemplateBuilder builder)
{
    private readonly FixtureModel _fixtureModel = fixtureModel;
    private readonly TemplateBuilder _builder = builder;

    internal string CreateTemplate(ControllerActionModel actionModel)
    {
        const string testAttributeTemplate = "[Test]";
        const string methodSignatureTemplate = "public async Task Test_{0}_{1}()";

        var name = actionModel.Name;

        _builder.AddIndented(testAttributeTemplate);

        _builder.AddFormatIndented(methodSignatureTemplate, actionModel.HttpMethod.ToString().ToSentenceCase(), name);

        _builder.BeginBlock();

        AddApiTestMethod(actionModel);

        _builder.EndBlock();

        return _builder.Build();
    }

    private void AddApiTestMethod(ControllerActionModel actionModel)
    {
        const string arrangeComment = "// Arrange";
        const string variableTemplate = "{0} {1} = {2};";
        const string actComment = "// Act";
        const string assertComment = "// Assert";

        var urlTemplate = $"{_fixtureModel.Controller.Route}/{actionModel.RouteTemplate}";
        var resultExpected = actionModel.ReturnType != typeof(IActionResult);

        _builder.AddIndented(arrangeComment);

        foreach (var param in actionModel.Parameters)
        {
            _builder.AddFormatIndented(variableTemplate, FriendlyTypeName(param.Type), param.Name, "default");
        }

        if (resultExpected)
        {
            _builder.AddFormatIndented(variableTemplate, FriendlyTypeName(actionModel.ReturnType), "expectedResult", "default");
        }

        _builder.AddFormatIndented(variableTemplate, "var", "httpClient", $"{_fixtureModel.Options.ObtainHttpClientMethodName}()");

        _builder.AddEmptyLine();
        _builder.AddIndented(actComment);
        _builder.AddFormatIndented(variableTemplate, "var", "requestUri", "$\"" + urlTemplate + "\"");
        
        AddEndpointCall(actionModel);

        _builder.AddEmptyLine();
        _builder.AddIndented(assertComment);
        _builder.AddIndented("Assert.That(httpResult.IsSuccessStatusCode, Is.True);");

        if (resultExpected)
        {
            _builder.AddFormatIndented(variableTemplate, "var", "contentResult", 
                $"await httpResult.Content.ReadFromJsonAsync<{actionModel.ReturnType}>()");
            _builder.AddIndented("Assert.That(contentResult, Is.EqualTo(expectedResult));");
        }
    }

    private void AddEndpointCall(ControllerActionModel actionModel)
    {
        if (actionModel.HttpMethod == HttpMethod.Get)
        {
            _builder.AddIndented("var httpResult = await httpClient.GetAsync(requestUri);");
        }
        else if (actionModel.HttpMethod == HttpMethod.Post)
        {
            var inputParameter = actionModel.Parameters.Single(p => p.BindingSource == BindingSource.Body);
            _builder.AddFormatIndented("var httpResult = await httpClient.PostAsJsonAsync(requestUri, {0});", inputParameter.Name);
        }
        else if (actionModel.HttpMethod == HttpMethod.Put)
        {
            var inputParameter = actionModel.Parameters.Single(p => p.BindingSource == BindingSource.Body);
            _builder.AddFormatIndented("var httpResult = await httpClient.PutAsJsonAsync(requestUri, {0});", inputParameter.Name);
        }
        else if (actionModel.HttpMethod == HttpMethod.Delete)
        {
            _builder.AddIndented("var httpResult = await httpClient.DeleteAsync(requestUri);");
        }
    }

    private static string FriendlyTypeName(Type type)
    {
        if (type == typeof(int))
        {
            return "int";
        }

        return type.Name;
    }
}
