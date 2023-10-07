namespace ClassFramework.Domain;

public static class NullableHelper
{
    public static bool IsNullable(Type memberType, MemberInfo declaringType, IEnumerable<CustomAttributeData> customAttributes, int index)
    {
        memberType = memberType.IsNotNull(nameof(memberType));
        declaringType = declaringType.IsNotNull(nameof(declaringType));
        customAttributes = customAttributes.IsNotNull(nameof(customAttributes));

        if (memberType.IsValueType)
        {
            return Nullable.GetUnderlyingType(memberType) is not null;
        }

        var nullable = customAttributes
            .FirstOrDefault(x => x.AttributeType.FullName == "System.Runtime.CompilerServices.NullableAttribute");
        if (nullable is not null && nullable.ConstructorArguments.Count == 1)
        {
            var attributeArgument = nullable.ConstructorArguments[0];
            if (attributeArgument.ArgumentType == typeof(byte[]))
            {
                var args = (ReadOnlyCollection<CustomAttributeTypedArgument>)attributeArgument.Value!;
                if (args.Count > index && args[index].ArgumentType == typeof(byte))
                {
                    return (byte)args[index].Value! == 2;
                }
            }
            else if (attributeArgument.ArgumentType == typeof(byte))
            {
                return (byte)attributeArgument.Value! == 2;
            }
        }

        for (var type = declaringType; type is not null; type = type.DeclaringType)
        {
            var context = type.CustomAttributes
                .FirstOrDefault(x => x.AttributeType.FullName == "System.Runtime.CompilerServices.NullableContextAttribute");
            if (context is not null &&
                context.ConstructorArguments.Count == 1 &&
                context.ConstructorArguments[0].ArgumentType == typeof(byte))
            {
                return (byte)context.ConstructorArguments[0].Value! == 2;
            }
        }

        // Couldn't find a suitable attribute
        return false;
    }
}
