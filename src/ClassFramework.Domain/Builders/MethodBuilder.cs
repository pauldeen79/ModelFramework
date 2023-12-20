namespace ClassFramework.Domain.Builders;

public partial class MethodBuilder
{
    public MethodBuilder WithReturnType(Type type)
    {
        type = type.IsNotNull(nameof(type));

        ReturnTypeName = type.FullName;
        ReturnTypeIsValueType = type.IsValueType;

        return this;
    }

    public MethodBuilder WithReturnType(TypeBase typeBase)
    {
        typeBase = typeBase.IsNotNull(nameof(typeBase));

        ReturnTypeName = typeBase.GetFullName();
        ReturnTypeIsValueType = typeBase is IValueType;

        return this;
    }
}
