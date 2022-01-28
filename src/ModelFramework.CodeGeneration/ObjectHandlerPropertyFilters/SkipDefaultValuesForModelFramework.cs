using System;
using System.Collections;
using System.Reflection;
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
            if (propertyInfo.Name == nameof(ClassPropertyBuilder.GetterVisibility))
            {
                var classPropertyBuilder = command.Instance as ClassPropertyBuilder;
                if (classPropertyBuilder != null && !classPropertyBuilder.HasGetter)
                {
                    return false;
                }
            }
            else if (propertyInfo.Name == nameof(ClassPropertyBuilder.SetterVisibility))
            {
                var classPropertyBuilder = command.Instance as ClassPropertyBuilder;
                if (classPropertyBuilder != null && !classPropertyBuilder.HasSetter)
                {
                    return false;
                }
            }
            else if (propertyInfo.Name == nameof(ClassPropertyBuilder.InitializerVisibility))
            {
                var classPropertyBuilder = command.Instance as ClassPropertyBuilder;
                if (classPropertyBuilder != null && !classPropertyBuilder.HasInitializer)
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
            if (propertyInfo.Name == nameof(ClassPropertyBuilder.HasGetter)
                || propertyInfo.Name == nameof(ClassPropertyBuilder.HasSetter))
            {
                // HasGetter and HasSetter are true by default
                return true;
            }
            else if (propertyInfo.PropertyType == typeof(string) && !propertyInfo.IsNullable())
            {
                return string.Empty;
            }
            else if (propertyInfo.Name == nameof(IVisibilityContainer.Visibility))
            {
                // Not really necessary at this time because we're currently only using ClassBuilder and ClassPropertyBuilder instances...
                return propertyInfo.DeclaringType == typeof(ClassFieldBuilder)
                    ? Visibility.Private
                    : Visibility.Public;
            }
            else if (propertyInfo.Name == nameof(ClassPropertyBuilder.GetterVisibility))
            {
                return propertyInfo.DeclaringType?.GetProperty(nameof(ClassPropertyBuilder.Visibility))?.GetValue(command.Instance);
            }
            else if (propertyInfo.Name == nameof(ClassPropertyBuilder.SetterVisibility))
            {
                return propertyInfo.DeclaringType?.GetProperty(nameof(ClassPropertyBuilder.Visibility))?.GetValue(command.Instance);
            }
            else if (propertyInfo.Name == nameof(ClassPropertyBuilder.InitializerVisibility))
            {
                return propertyInfo.DeclaringType?.GetProperty(nameof(ClassPropertyBuilder.Visibility))?.GetValue(command.Instance);
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
