namespace ModelFramework.Objects.Contracts;

public interface IAttribute : IMetadataContainer, INameContainer
{
    IReadOnlyCollection<IAttributeParameter> Parameters { get; }
}
