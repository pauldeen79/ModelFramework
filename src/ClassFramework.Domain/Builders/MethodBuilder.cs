﻿namespace ClassFramework.Domain.Builders;

public partial class MethodBuilder
{
    public MethodBuilder WithReturnType(Type type)
    {
        type = type.IsNotNull(nameof(type));

        ReturnTypeName = type.FullName;
        ReturnTypeIsValueType = type.IsValueType;

        return this;
    }

    public MethodBuilder WithReturnType(IType type)
    {
        type = type.IsNotNull(nameof(type));

        ReturnTypeName = type.GetFullName();
        ReturnTypeIsValueType = type is IValueType;

        return this;
    }
}
