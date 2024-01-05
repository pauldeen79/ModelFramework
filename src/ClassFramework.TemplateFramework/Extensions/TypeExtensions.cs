namespace ClassFramework.TemplateFramework.Extensions;

public static class TypeExtensions
{
    public static string GetEntityClassName(this Type instance)
        => RemoveGenerics(instance.IsInterface && instance.Name.StartsWith('I')
            ? instance.Name[1..]
            : instance.Name);

    private static string RemoveGenerics(string name)
    {
        var index = name.IndexOf('`');
        if (index == -1)
        {
            return name;
        }

        return name[..index];
    }
}
