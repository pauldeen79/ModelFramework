namespace ClassFramework.TemplateFramework.Extensions;

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
            var classMethod = instance as ClassMethod;

            if (classMethod is null || !classMethod.Partial)
            {
                builder.AddWithCondition("protected", extendedVisibilityContainer.Protected);
                builder.AddWithCondition(instance.Visibility.ToString().ToLower(CultureInfo.InvariantCulture), !(extendedVisibilityContainer.Protected && instance.Visibility != Visibility.Internal));
                builder.AddWithCondition("static", extendedVisibilityContainer.Static);
                builder.AddWithCondition("abstract", extendedVisibilityContainer.Abstract);
                builder.AddWithCondition("virtual", extendedVisibilityContainer.Virtual);
                builder.AddWithCondition("override", extendedVisibilityContainer.Override);

                var classField = instance as ClassField;
                builder.AddWithCondition("readonly", classField?.ReadOnly == true);
                builder.AddWithCondition("const", classField?.Constant == true);
            }

            builder.AddWithCondition("async", classMethod?.Async == true);
            builder.AddWithCondition("partial", classMethod?.Partial == true);
        }
        else
        {
            builder.Append(instance.Visibility.ToString().ToLower(CultureInfo.InvariantCulture));

            var cls = instance as Class;
            builder.AddWithCondition("sealed", cls?.Sealed == true);
            builder.AddWithCondition("abstract", cls?.Abstract == true);
            builder.AddWithCondition("static", cls?.Static == true);

            var typeBase = instance as TypeBase;
            builder.AddWithCondition("partial", typeBase?.Partial == true);
        }

        // Append trailing space when filled
        builder.AddWithCondition(string.Empty, builder.Length > 0);

        return builder.ToString();
    }
}
