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
        BuilderPipelineBuilderInheritanceSettings inheritanceSettings,
        bool isForWithStatement)
    {
        inheritanceSettings = inheritanceSettings.IsNotNull(nameof(inheritanceSettings));

        return parent.IsMemberValidForImmutableBuilderClass(
            parentTypeContainer,
            inheritanceSettings.EnableEntityInheritance,
            inheritanceSettings.EnableBuilderInheritance,
            isForWithStatement && !inheritanceSettings.RemoveDuplicateWithMethods, // only when duplicate methods need to be removed...
            inheritanceSettings.InheritanceComparisonFunction);
    }

    private static bool IsMemberValidForImmutableBuilderClass(
        this TypeBase parent,
        IParentTypeContainer parentTypeContainer,
        bool enableEntityInheritance,
        bool enableBuilderInhericance,
        bool isForWithStatement,
        Func<IParentTypeContainer, TypeBase, bool>? comparisonFunction = null)
    {
        if (!enableEntityInheritance)
        {
            // If entity inheritance is not enabled, then simply include all members
            return true;
        }

        if (enableBuilderInhericance && isForWithStatement)
        {
            // If builder inheritance is enabled, then we have to duplicate the property for With statements
            return true;
        }

        // If inheritance is enabled, then include the members if it's defined on the parent class
        return parentTypeContainer.IsDefinedOn(parent, comparisonFunction);
    }
}
