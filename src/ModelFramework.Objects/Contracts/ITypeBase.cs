namespace ModelFramework.Objects.Contracts;

public interface ITypeBase : IMetadataContainer, IVisibilityContainer, INameContainer, IAttributesContainer
{
    string Namespace { get; }
    bool Partial { get; }
    IReadOnlyCollection<string> Interfaces { get; }
    IReadOnlyCollection<IClassProperty> Properties { get; }
    IReadOnlyCollection<IClassMethod> Methods { get; }
    IReadOnlyCollection<string> GenericTypeArguments { get; }
    IReadOnlyCollection<string> GenericTypeArgumentConstraints { get; }
}
