namespace ClassFramework.Domain.Builders.Extensions;

public static class TypeContainerBuilderExtensions
{
    public static T WithType<T>(this T instance, Type type) where T : ITypeContainerBuilder
    {
        instance.TypeName = type.IsNotNull(nameof(type)).FullName;
        return instance;
    }
}
