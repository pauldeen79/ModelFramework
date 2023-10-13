namespace ModelFramework.Objects.Extensions;

public static class ParentTypeContainerExtensions
{
    public static bool IsDefinedOn(this IParentTypeContainer instance,
                                   ITypeBase typeBase,
                                   INameContainer nameContainer,
                                   Func<IParentTypeContainer, INameContainer, ITypeBase, bool>? comparisonFunction = null)
        => comparisonFunction == null
            ? string.IsNullOrEmpty(instance.ParentTypeFullName) || instance.ParentTypeFullName == typeBase.GetFullName()
            : comparisonFunction.Invoke(instance, nameContainer, typeBase);
}
