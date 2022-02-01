namespace ModelFramework.Objects.Contracts;

public interface IClassProperty : IMetadataContainer, IExtendedVisibilityContainer, INameContainer, IAttributesContainer, ITypeContainer, IExplicitInterfaceNameContainer
{
    bool HasGetter { get; }
    bool HasSetter { get; }
    bool HasInitializer { get; }
    Visibility? GetterVisibility { get; }
    Visibility? SetterVisibility { get; }
    Visibility? InitializerVisibility { get; }
    ValueCollection<ICodeStatement> GetterCodeStatements { get; }
    ValueCollection<ICodeStatement> SetterCodeStatements { get; }
    ValueCollection<ICodeStatement> InitializerCodeStatements { get; }
}
