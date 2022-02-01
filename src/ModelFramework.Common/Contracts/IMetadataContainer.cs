namespace ModelFramework.Common.Contracts;

public interface IMetadataContainer
{
    ValueCollection<IMetadata> Metadata { get; }
}
