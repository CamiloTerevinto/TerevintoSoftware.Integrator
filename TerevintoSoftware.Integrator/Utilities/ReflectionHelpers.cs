﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;
using TerevintoSoftware.Integrator.Models;

namespace TerevintoSoftware.Integrator.Utilities;

internal static class ReflectionHelpers
{
    private const BindingFlags _commonFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;

    internal static FixtureModel BuildModel(string baseTestNamespace, ControllerModel controller)
    {
        return new FixtureModel
        {
            Controller = controller,
            ClassNamespace = baseTestNamespace,
            RequiredUsings = FindReferencedUsingsForType(controller.Type)
        };
    }

    internal static List<string> FindReferencedUsingsForType(Type type)
    {
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }
        else if (!type.IsClass)
        {
            throw new ArgumentException($"{nameof(type)} must be a class.", nameof(type));
        }

        return type.GetFields(_commonFlags).Select(x => x.FieldType)
            .Concat(type.GetProperties(_commonFlags).Select(x => x.PropertyType))
            .Concat(type.GetInterfaces())
            .Concat(type.GetMethods(_commonFlags).SelectMany(x => x.GetParameters().Select(y => y.ParameterType).Append(x.ReturnType)))
            .Select(x => x.Namespace!)
            .Where(x => x != null)
            .Distinct()
            .Where(x => !x.StartsWith("Microsoft"))
            .ToList();
    }

    internal static IEnumerable<Type> FindControllerTypes(IEnumerable<Type> exportedTypes)
    {
        var controllerBaseType = typeof(ControllerBase);

        return exportedTypes.Where(controllerBaseType.IsAssignableFrom);
    }

    internal static List<ControllerModel> GetControllerActions(IEnumerable<Type> controllers)
    {
        var routeAttribute = typeof(RouteAttribute);
        var httpMethodAttribute = typeof(HttpMethodAttribute);
        var data = new List<ControllerModel>();

        foreach (var controllerType in controllers)
        {
            var controllerName = controllerType.Name.Replace("Controller", "");
            var controllerModel = new ControllerModel(controllerType, controllerName);
            controllerModel.CleanRoute(controllerType.GetCustomAttribute<RouteAttribute>()?.Template!);

            controllerModel.Actions = controllerType
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Select(method =>
                    {
                        var httpGetAttribute = method.GetCustomAttribute<HttpGetAttribute>(false);
                        var httpPostAttribute = method.GetCustomAttribute<HttpPostAttribute>(false);
                        var httpPutAttribute = method.GetCustomAttribute<HttpPutAttribute>(false);
                        var httpDeleteAttribute = method.GetCustomAttribute<HttpDeleteAttribute>(false);

                        var (httpMethod, template) = 
                            httpGetAttribute != null ? (HttpMethod.Get, httpGetAttribute.Template)
                            : httpPostAttribute != null ? (HttpMethod.Post, httpPostAttribute.Template)
                            : httpPutAttribute != null ? (HttpMethod.Put, httpPutAttribute.Template)
                            : httpDeleteAttribute != null ? (HttpMethod.Delete, httpDeleteAttribute.Template)
                            : (HttpMethod.Get, "");

                        return new ControllerActionModel(
                            method.Name, GetParametersData(controllerModel.Route, template ?? "", method.GetParameters()),
                            method.ReturnType.UnwrapReturnType(), httpMethod, template ?? "");
                    })
                .ToArray();

            data.Add(controllerModel);
        }

        return data;
    }

    internal static Type UnwrapReturnType(this Type returnType)
    {
        var taskType = typeof(Task);
        var genericActionResultType = typeof(IConvertToActionResult);
        var genericEnumerableType = typeof(IEnumerable<>);

        var nextType = returnType;

        if (nextType.IsGenericType && taskType.IsAssignableFrom(nextType))
        {
            nextType = nextType.GetGenericArguments()[0];
        }

        if (nextType.IsGenericType && genericActionResultType.IsAssignableFrom(nextType))
        {
            nextType = nextType.GetGenericArguments()[0];
        }

        if (nextType.IsGenericType && genericEnumerableType.IsAssignableFrom(nextType.GetGenericTypeDefinition()))
        {
            return typeof(List<>).MakeGenericType(nextType.GenericTypeArguments[0]);
        }

        return nextType;
    }

    internal static ActionParameterModel[] GetParametersData(string controllerRoute, string routeTemplate, ParameterInfo[] parameters)
    {
        return parameters
            .Select(x => new ActionParameterModel(x.Name!, x.ParameterType,
                ParameterInRouteTemplate(controllerRoute, routeTemplate, x) ? BindingSource.Path :
                    x.GetCustomAttribute<FromRouteAttribute>()?.BindingSource
                    ?? x.GetCustomAttribute<FromQueryAttribute>()?.BindingSource
                    ?? x.GetCustomAttribute<FromBodyAttribute>()?.BindingSource
                    ?? x.GetCustomAttribute<FromFormAttribute>()?.BindingSource
                    ?? x.GetCustomAttribute<FromHeaderAttribute>()?.BindingSource
                    ?? x.ParameterType.ToBindingSource()))
            .ToArray();
    }

    private static BindingSource ToBindingSource(this Type type)
    {
        if (type.Namespace == "System" && type != typeof(object))
        {
            return BindingSource.Query;
        }

        return BindingSource.Body;
    }

    private static bool ParameterInRouteTemplate(string controllerRoute, string actionRoute, ParameterInfo parameter)
    {
        var parameterName = "{" + parameter.Name;
        var finalUrl = UrlTemplateHelpers.BuildTemplateFromRoutes(controllerRoute, actionRoute);

        return finalUrl.Contains(parameterName + ":") || finalUrl.Contains(parameterName + "}");
    }
}
