namespace ModelFramework.Objects.Extensions;

public static class ParentTypeContainerExtensions
{
    public static bool IsDefinedOn(this IParentTypeContainer instance, ITypeBase typeBase)
        => string.IsNullOrEmpty(instance.ParentTypeFullName)
        || instance.ParentTypeFullName == typeBase.GetFullName();
}
