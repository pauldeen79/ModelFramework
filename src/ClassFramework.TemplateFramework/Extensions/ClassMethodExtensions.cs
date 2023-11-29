namespace ClassFramework.TemplateFramework.Extensions;

public static class ClassMethodExtensions
{
    public static bool IsInterfaceMethod(this ClassMethod instance)
        => instance.Name.StartsWith('I') && instance.Name.Contains('.');
}
