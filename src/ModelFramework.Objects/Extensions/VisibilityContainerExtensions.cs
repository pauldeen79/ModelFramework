﻿namespace ModelFramework.Objects.Extensions;

public static class VisibilityContainerExtensions
{
    public static string GetModifiers<T>(this T instance)
        where T : IMetadataContainer, IVisibilityContainer
    {
        var customModifiers = instance.Metadata.GetStringValue(MetadataNames.CustomModifiers);
        if (!string.IsNullOrEmpty(customModifiers))
        {
            return customModifiers + " ";
        }
        var builder = new StringBuilder();
        if (instance is IExtendedVisibilityContainer extendedVisibilityContainer)
        {
            var classMethod = instance as IClassMethod;

            if (classMethod is null || !classMethod.Partial)
            {
                builder.AddWithCondition("protected", extendedVisibilityContainer.Protected);
                if (!(extendedVisibilityContainer.Protected && instance.Visibility != Visibility.Internal))
                {
                    builder.Append(instance.Visibility.ToString().ToLower(CultureInfo.InvariantCulture));
                }
                builder.AddWithCondition("static", extendedVisibilityContainer.Static);
                builder.AddWithCondition("abstract", extendedVisibilityContainer.Abstract);
                builder.AddWithCondition("virtual", extendedVisibilityContainer.Virtual);
                builder.AddWithCondition("override", extendedVisibilityContainer.Override);

                if (instance is IClassField classField)
                {
                    builder.AddWithCondition("readonly", classField.ReadOnly);
                    builder.AddWithCondition("const", classField.Constant);
                }
            }

            if (classMethod is not null)
            {
                builder.AddWithCondition("async", classMethod.Async);
                builder.AddWithCondition("partial", classMethod.Partial);
            }
        }
        else
        {
            builder.Append(instance.Visibility.ToString().ToLower(CultureInfo.InvariantCulture));
            if (instance is IClass cls)
            {
                builder.AddWithCondition("sealed", cls.Sealed);
                builder.AddWithCondition("abstract", cls.Abstract);
                builder.AddWithCondition("static", cls.Static);
            }
            if (instance is ITypeBase typeBase)
            {
                builder.AddWithCondition("partial", typeBase.Partial);
            }
        }

        if (builder.Length > 0)
        {
            builder.Append(" ");
        }

        return builder.ToString();
    }
}
