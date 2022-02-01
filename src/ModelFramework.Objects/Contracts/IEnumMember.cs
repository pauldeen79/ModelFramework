namespace ModelFramework.Objects.Contracts;

public interface IEnumMember : IAttributesContainer, INameContainer, IMetadataContainer
{
    object? Value { get; }
}
