namespace ClassFramework.Domain.Builders.Extensions;

public static class ParametersContainerBuilderExtensions
{
    public static T AddParameter<T>(this T instance, string name, Type type) where T : IParametersContainerBuilder
    {
        instance.Parameters.Add(new ParameterBuilder().WithName(name).WithType(type));
        return instance;
    }

    public static T AddParameter<T>(this T instance, string name, Type type, bool isNullable) where T : IParametersContainerBuilder
    {
        instance.Parameters.Add(new ParameterBuilder().WithName(name).WithType(type).WithIsNullable(isNullable));
        return instance;
    }

    public static T AddParameter<T>(this T instance, string name, string typeName) where T : IParametersContainerBuilder
    {
        instance.Parameters.Add(new ParameterBuilder().WithName(name).WithTypeName(typeName));
        return instance;
    }

    public static T AddParameter<T>(this T instance, string name, string typeName, bool isNullable) where T : IParametersContainerBuilder
    {
        instance.Parameters.Add(new ParameterBuilder().WithName(name).WithTypeName(typeName).WithIsNullable(isNullable));
        return instance;
    }
}
