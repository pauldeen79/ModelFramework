namespace ModelFramework.Objects.Extensions;

public static class ParentTypeContainerExtensions
{
    public static bool IsDefinedOn(this IParentTypeContainer instance,
                                   ITypeBase typeBase,
                                   string name,
                                   Func<IParentTypeContainer, string, ITypeBase, bool>? comparisonFunction = null)
        => comparisonFunction == null
            ? string.IsNullOrEmpty(instance.ParentTypeFullName) || instance.ParentTypeFullName == typeBase.GetFullName()
            : comparisonFunction.Invoke(instance, name, typeBase);
}
