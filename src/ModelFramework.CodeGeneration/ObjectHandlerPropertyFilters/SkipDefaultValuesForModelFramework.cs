using System;
using System.Collections;
using System.Reflection;
using CrossCutting.Common.Extensions;
using CsharpExpressionDumper.Abstractions;
using CsharpExpressionDumper.Abstractions.Requests;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Extensions;

namespace ModelFramework.CodeGeneration.ObjectHandlerPropertyFilters
{
    public class SkipDefaultValuesForModelFramework : IObjectHandlerPropertyFilter
    {
        public bool IsValid(ObjectHandlerRequest command, PropertyInfo propertyInfo)
        {
            var classPropertyBuilder = command.Instance as ClassPropertyBuilder;
            if (classPropertyBuilder != null)
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

        private static bool ValueIsEmptyOrUnequalToDefaultValue(PropertyInfo propertyInfo, object? defaultValue, object actualValue)
        {
            if (typeof(ICollection).IsAssignableFrom(propertyInfo.PropertyType) && actualValue is ICollection c && c.Count == 0)
            {
                // Skip empty collections
                return false;
            }

            if (defaultValue == null && actualValue == null)
            {
                return false;
            }

            return defaultValue == null
                || actualValue == null
                || !actualValue.Equals(defaultValue);
        }

        private static object? GetDefaultValue(ObjectHandlerRequest command, PropertyInfo propertyInfo)
        {
            var classPropertyBuilder = command.Instance as ClassPropertyBuilder;
            if (classPropertyBuilder != null && propertyInfo.Name.In(nameof(ClassPropertyBuilder.HasGetter),
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
            else if (propertyInfo.Name == nameof(IVisibilityContainer.Visibility) && command.Instance is IVisibilityContainer)
            {
                // On ClassFieldBuilder the default visibility is private, for other types it's public
                return propertyInfo.DeclaringType == typeof(ClassFieldBuilder)
                    ? Visibility.Private
                    : Visibility.Public;
            }
            else
            {
                return propertyInfo.PropertyType.IsValueType && Nullable.GetUnderlyingType(propertyInfo.PropertyType) == null
                    ? Activator.CreateInstance(propertyInfo.PropertyType)
                    : null;
            }
        }
    }
}
