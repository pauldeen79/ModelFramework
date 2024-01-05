namespace ClassFramework.TemplateFramework.Extensions;

public static class TypeBaseExtensions
{
    public static string GetEntityClassName(this TypeBase instance)
        => instance is Interface && instance.Name.StartsWith('I')
            ? instance.Name.Substring(1)
            : instance.Name;
}
