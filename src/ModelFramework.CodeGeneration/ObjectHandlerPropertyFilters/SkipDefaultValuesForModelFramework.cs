﻿namespace ModelFramework.CodeGeneration.ObjectHandlerPropertyFilters;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1062 // false positive because I've added null guards but code analysis doesn't understand this
#pragma warning restore IDE0079 // Remove unnecessary suppression
public class SkipDefaultValuesForModelFramework : IObjectHandlerPropertyFilter
{
    public bool IsValid(ObjectHandlerRequest command, PropertyInfo propertyInfo)
    {
        Guard.IsNotNull(command);
        Guard.IsNotNull(propertyInfo);
        var classPropertyBuilder = command.Instance as ClassPropertyBuilder;
        if (classPropertyBuilder is not null)
        {
            if (propertyInfo.Name == nameof(ClassPropertyBuilder.GetterVisibility) && (!classPropertyBuilder.HasGetter || classPropertyBuilder.GetterVisibility == classPropertyBuilder.Visibility))
            {
                return false;
            }
            else if (propertyInfo.Name == nameof(ClassPropertyBuilder.SetterVisibility) && (!classPropertyBuilder.HasSetter || classPropertyBuilder.SetterVisibility == classPropertyBuilder.Visibility))
            {
                return false;
            }
            else if (propertyInfo.Name == nameof(ClassPropertyBuilder.InitializerVisibility) && (!classPropertyBuilder.HasInitializer || classPropertyBuilder.InitializerVisibility == classPropertyBuilder.Visibility))
            {
                return false;
            }
        }
        var defaultValue = GetDefaultValue(command, propertyInfo);
        var actualValue = propertyInfo.GetValue(command.Instance);
        return ValueIsEmptyOrUnequalToDefaultValue(propertyInfo, defaultValue, actualValue);
    }

    private static bool ValueIsEmptyOrUnequalToDefaultValue(PropertyInfo propertyInfo, object? defaultValue, object? actualValue)
    {
        if (typeof(ICollection).IsAssignableFrom(propertyInfo.PropertyType) && actualValue is ICollection c && c.Count == 0)
        {
            // Skip empty collections
            return false;
        }

        if (typeof(StringBuilder).IsAssignableFrom(propertyInfo.PropertyType) && actualValue is StringBuilder sb && sb.Length == 0)
        {
            // Skip empty stringbuilders
            return false;
        }

        if (defaultValue is null && actualValue is null)
        {
            return false;
        }

        return defaultValue is null
            || actualValue is null
            || !actualValue.Equals(defaultValue);
    }

    private static object? GetDefaultValue(ObjectHandlerRequest command, PropertyInfo propertyInfo)
    {
        var classPropertyBuilder = command.Instance as ClassPropertyBuilder;
        if (classPropertyBuilder is not null && propertyInfo.Name.In(nameof(ClassPropertyBuilder.HasGetter),
                                                                     nameof(ClassPropertyBuilder.HasSetter)))
        {
            // HasGetter and HasSetter are true by default on ClassPropertyBuilder
            return true;
        }
        else if (propertyInfo.PropertyType == typeof(string) && !propertyInfo.IsNullable())
        {
            // For non-nullable strings, string.Empty is the default value
            return string.Empty;
        }
        else if (propertyInfo.PropertyType == typeof(StringBuilder) && !propertyInfo.IsNullable())
        {
            // For non-nullable string builders, string.Empty is the default value
            return string.Empty;
        }
        else if (propertyInfo.Name == nameof(IVisibilityContainer.Visibility) && command.Instance is IVisibilityContainer)
        {
            // On ClassFieldBuilder the default visibility is private, for other types it's public
            return propertyInfo.DeclaringType == typeof(ClassFieldBuilder)
                ? Visibility.Private
                : Visibility.Public;
        }
        else
        {
            return propertyInfo.PropertyType.IsValueType && Nullable.GetUnderlyingType(propertyInfo.PropertyType) is null
                ? Activator.CreateInstance(propertyInfo.PropertyType)
                : null;
        }
    }
}
