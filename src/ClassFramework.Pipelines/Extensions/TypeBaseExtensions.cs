namespace ClassFramework.Pipelines.Extensions;

public static class TypeBaseExtensions
{
    public static string FormatInstanceName(
        this TypeBase instance,
        bool forCreate,
        Func<TypeBase, bool, string>? formatInstanceTypeNameDelegate)
    {
        if (formatInstanceTypeNameDelegate is not null)
        {
            var retVal = formatInstanceTypeNameDelegate(instance, forCreate);
            if (!string.IsNullOrEmpty(retVal))
            {
                return retVal;
            }
        }

        return instance.GetFullName().GetCsharpFriendlyTypeName();
    }

    public static bool IsMemberValidForImmutableBuilderClass(
        this TypeBase parent,
        IParentTypeContainer parentTypeContainer,
        PipelineBuilderSettings settings)
    {
        parentTypeContainer = parentTypeContainer.IsNotNull(nameof(parentTypeContainer));
        settings = settings.IsNotNull(nameof(settings));

        return parent.IsMemberValidForImmutableBuilderClass(
            parentTypeContainer,
            settings.ClassSettings.InheritanceSettings.EnableInheritance,
            settings.InheritanceSettings.InheritanceComparisonFunction);
    }

    public static string GetGenericTypeArgumentsString(this TypeBase instance)
        => instance.GenericTypeArguments.Count > 0
            ? $"<{string.Join(", ", instance.GenericTypeArguments)}>"
            : string.Empty;

    public static string GetGenericTypeArgumentConstraintsString(this TypeBase instance)
        => instance.GenericTypeArgumentConstraints.Any()
            ? string.Concat
            (
                Environment.NewLine,
                "        ",
                string.Join(string.Concat(Environment.NewLine, "        "), instance.GenericTypeArgumentConstraints)
            )
            : string.Empty;

    public static string GetCustomValueForInheritedClass(
        this TypeBase instance,
        PipelineBuilderSettings settings,
        Func<Class, string> customValue)
    {
        settings = settings.IsNotNull(nameof(settings));
        customValue = customValue.IsNotNull(nameof(customValue));

        return instance.GetCustomValueForInheritedClass(settings.ClassSettings.InheritanceSettings.EnableInheritance, customValue);
    }

    public static IEnumerable<ClassProperty> GetImmutableBuilderConstructorProperties(
        this TypeBase instance,
        BuilderContext context,
        bool hasPublicParameterlessConstructor)
    {
        context = context.IsNotNull(nameof(context));

        if (hasPublicParameterlessConstructor)
        {
            return instance.Properties.Where(x => x.HasSetter || x.HasInitializer);
        }

        var cls = instance as Class;
        if (cls is null)
        {
            throw new ArgumentException("Cannot get immutable builder constructor properties for interface");
        }

        var ctor = cls.Constructors.FirstOrDefault(x => x.Visibility == Domain.Domains.Visibility.Public && x.Parameters.Count > 0);
        if (ctor is null)
        {
            // No public constructor, so we can't add properties to initialization.
            return Enumerable.Empty<ClassProperty>();
        }

        if (context.IsBuilderForOverrideEntity && context.Settings.InheritanceSettings.BaseClass is not null)
        {
            // Try to get property from either the base class c'tor or the class c'tor itself
            return ctor
                .Parameters
                .Select(x => instance.Properties.FirstOrDefault(y => y.Name.Equals(x.Name, StringComparison.OrdinalIgnoreCase))
                    ?? context.Settings.InheritanceSettings.BaseClass!.Properties.FirstOrDefault(y => y.Name.Equals(x.Name, StringComparison.OrdinalIgnoreCase)))
                .Where(x => x is not null);
        }

        return ctor
            .Parameters
            .Select(x => instance.Properties.FirstOrDefault(y => y.Name.Equals(x.Name, StringComparison.OrdinalIgnoreCase)))
            .Where(x => x is not null);
    }

    private static string GetCustomValueForInheritedClass(
        this TypeBase instance,
        bool enableInheritance,
        Func<Class, string> customValue)
    {
        if (!enableInheritance)
        {
            // Inheritance is not enabled
            return string.Empty;
        }

        var cls = instance as Class;
        if (cls is null)
        {
            // Type is an interface
            return string.Empty;
        }

        if (string.IsNullOrEmpty(cls.BaseClass))
        {
            // Class is not inherited
            return string.Empty;
        }

        return customValue(cls);
    }

    private static bool IsMemberValidForImmutableBuilderClass(
        this TypeBase parent,
        IParentTypeContainer parentTypeContainer,
        bool enableEntityInheritance,
        Func<IParentTypeContainer, TypeBase, bool>? comparisonFunction = null)
    {
        if (!enableEntityInheritance)
        {
            // If entity inheritance is not enabled, then simply include all members
            return true;
        }

        // If inheritance is enabled, then include the members if it's defined on the parent class
        return parentTypeContainer.IsDefinedOn(parent, comparisonFunction);
    }
}
