using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TerevintoSoftware.Integrator.Models;

public record ActionParameterModel(string Name, Type Type, BindingSource BindingSource);
public record ControllerActionModel(string Name, ActionParameterModel[] Parameters, Type ReturnType, HttpMethod HttpMethod, string RouteTemplate);
public record ControllerModel(string Name, Type Type, string Route, ControllerActionModel[] Actions);

