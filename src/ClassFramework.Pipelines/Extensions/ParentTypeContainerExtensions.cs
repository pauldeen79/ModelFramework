namespace ClassFramework.Pipelines.Extensions;

public static class ParentTypeContainerExtensions
{
    public static bool IsDefinedOn(
        this IParentTypeContainer instance,
        TypeBase typeBase,
        Func<IParentTypeContainer, TypeBase, bool>? comparisonFunction = null)
    {
        typeBase = typeBase.IsNotNull(nameof(typeBase));

        return comparisonFunction is null
            ? string.IsNullOrEmpty(instance.ParentTypeFullName) || instance.ParentTypeFullName == typeBase.GetFullName()
            : comparisonFunction.Invoke(instance, typeBase);
    }
}
