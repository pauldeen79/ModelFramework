namespace ClassFramework.Domain.Builders.Extensions;

public static class TypeContainerBuilderExtensions
{
    public static T WithType<T>(this T instance, Type type) where T : ITypeContainerBuilder
    {
        type = type.IsNotNull(nameof(type));

        instance.TypeName = type.FullName;
        instance.IsValueType = type.IsValueType;

        return instance;
    }

    public static T WithType<T>(this T instance, TypeBase typeBase) where T : ITypeContainerBuilder
    {
        typeBase = typeBase.IsNotNull(nameof(typeBase));

        instance.TypeName = typeBase.GetFullName();
        instance.IsValueType = typeBase is IValueType;

        return instance;
    }
}
