namespace ModelFramework.Objects.Extensions;

public static class PropertyInfoExtensions
{
    public static bool IsInitOnly(this PropertyInfo property)
    {
        if (!property.CanWrite)
        {
            return false;
        }

        var setMethod = property.SetMethod;

        // Get the modifiers applied to the return parameter.
        var setMethodReturnParameterModifiers = setMethod.ReturnParameter.GetRequiredCustomModifiers();

        // Init-only properties are marked with the IsExternalInit type.
        return setMethodReturnParameterModifiers.Any(t => t.FullName == "System.Runtime.CompilerServices.IsExternalInit");
    }

    public static bool IsNullable(this PropertyInfo property)
        => NullableHelper.IsNullable(property.PropertyType, property.DeclaringType, property.CustomAttributes);
}
