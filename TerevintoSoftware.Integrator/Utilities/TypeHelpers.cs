namespace TerevintoSoftware.Integrator.Utilities;

public static class TypeHelpers
{
    public static string GetTypeName(this Type type)
    {
        return FormatGenericTypeName(type);
    }

    // Obtained from: https://stackoverflow.com/a/6402954
    private static string FormatGenericTypeName(Type type)
    {
        if (type.GetGenericArguments().Length == 0)
        {
            return type.ToFriendlyName();
        }

        var genericArguments = type.GetGenericArguments();
        var typeDefinition = type.Name;
        var unmangledName = typeDefinition[..typeDefinition.IndexOf('`')];

        return unmangledName + "<" + string.Join(",", genericArguments.Select(FormatGenericTypeName)) + ">";
    }

    private static string ToFriendlyName(this Type type)
    {
        return type.Name switch
        {
            "String" => "string",
            "Int32" => "int",
            "Int64" => "long",
            _ => type.Name
        };
    }
}
