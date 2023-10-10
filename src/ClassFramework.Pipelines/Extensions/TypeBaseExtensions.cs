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
        PipelineBuilderInheritanceSettings inheritanceSettings,
        bool enableEntityInheritance,
        bool isForWithStatement)
    {
        inheritanceSettings = inheritanceSettings.IsNotNull(nameof(inheritanceSettings));

        return parent.IsMemberValidForImmutableBuilderClass(
            parentTypeContainer,
            enableEntityInheritance,
            inheritanceSettings.InheritanceComparisonFunction);
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
        => instance.GetCustomValueForInheritedClass(settings.IsNotNull(nameof(settings)).ClassSettings.InheritanceSettings.EnableInheritance, customValue.IsNotNull(nameof(customValue)));

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
