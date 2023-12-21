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

    public static T WithType<T>(this T instance, ITypeBuilder typeBuilder) where T : ITypeContainerBuilder
    {
        typeBuilder = typeBuilder.IsNotNull(nameof(typeBuilder));

        instance.TypeName = typeBuilder.GetFullName();
        instance.IsValueType = typeBuilder is IValueTypeBuilder;

        return instance;
    }
}
