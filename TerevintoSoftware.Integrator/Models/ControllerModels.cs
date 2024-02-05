using Microsoft.AspNetCore.Mvc.ModelBinding;
using TerevintoSoftware.Integrator.Utilities;

namespace TerevintoSoftware.Integrator.Models;

public record ActionParameterModel(string Name, Type Type, BindingSource BindingSource);
public record ControllerActionModel(string Name, ActionParameterModel[] Parameters, Type ReturnType, HttpMethod HttpMethod, string RouteTemplate);

public class ControllerModel(Type type, string name)
{
    public Type Type { get; } = type;
    public string Name { get; } = name;
    public string Route { get; private set; } = "";
    public ControllerActionModel[] Actions { get; set; }

    public void CleanRoute(string routeFromTemplate)
    {
        var controllerRoute = routeFromTemplate ?? Name.ToKebabCase();

        Route = UrlTemplateHelpers.CleanControllerTemplate(controllerRoute, Name);
    }
}
