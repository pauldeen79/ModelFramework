namespace ClassFramework.Domain.Extensions;

public static class TypeExtensions
{
    /// <summary>
    /// Removes generics from a typename. (`1)
    /// </summary>
    /// <param name="typeName">Typename with or without generics</param>
    /// <returns>Typename without generics (`1)</returns>
    public static string WithoutGenerics(this System.Type instance)
    {
        var name = instance.IsGenericParameter
            ? instance.Name
            : instance.FullName.WhenNullOrEmpty($"{instance.Namespace}.{instance.Name}");
        var index = name.IndexOf('`');
        return index == -1
            ? name.FixTypeName()
            : name.Substring(0, index).FixTypeName();
    }
}
