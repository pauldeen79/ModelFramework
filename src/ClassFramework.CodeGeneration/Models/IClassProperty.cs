namespace ClassFramework.CodeGeneration.Models;

public interface IClassProperty : IMetadataContainer, IExtendedVisibilityContainer, INameContainer, IAttributesContainer, ITypeContainer, IExplicitInterfaceNameContainer, IParentTypeContainer
{
    bool HasGetter { get; }
    bool HasSetter { get; }
    bool HasInitializer { get; }
    Visibility? GetterVisibility { get; }
    Visibility? SetterVisibility { get; }
    Visibility? InitializerVisibility { get; }
    IReadOnlyCollection<ICodeStatement> GetterCodeStatements { get; }
    IReadOnlyCollection<ICodeStatement> SetterCodeStatements { get; }
    IReadOnlyCollection<ICodeStatement> InitializerCodeStatements { get; }
}
