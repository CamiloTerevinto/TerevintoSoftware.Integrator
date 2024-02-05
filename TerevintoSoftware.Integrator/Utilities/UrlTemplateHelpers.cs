using System.Text.RegularExpressions;

namespace TerevintoSoftware.Integrator.Utilities;

public static partial class UrlTemplateHelpers
{
    public static string BuildTemplateFromRoutes(string controllerRoute, string actionRoute)
    {
        if (string.IsNullOrEmpty(actionRoute))
        {
            return controllerRoute;
        }

        actionRoute = RemoveConstraintsFromUrl(actionRoute);

        if (actionRoute.StartsWith('/'))
        {
            return actionRoute;
        }

        return $"{controllerRoute}/{actionRoute}";
    }

    public static string CleanControllerTemplate(string controllerRoute, string controllerName)
    {
        if (controllerRoute.Contains("[controller]"))
        {
            controllerRoute = controllerRoute.Replace("[controller]", controllerName.ToKebabCase());
        }

        return RemoveConstraintsFromUrl(controllerRoute);
    }

    public static string RemoveConstraintsFromUrl(string url)
    {
        return FindConstraintsInTemplates().Replace(url, "{$1}");
    }

    [GeneratedRegex(@"\{([^{}]*):[^{}]*\}")]
    private static partial Regex FindConstraintsInTemplates();
}
