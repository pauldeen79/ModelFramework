using ModelFramework.Common.Contracts;

namespace ModelFramework.Database.Contracts
{
    public interface IViewSource : INameContainer, IMetadataContainer
    {
        string Alias { get; }
        string SourceSchemaName { get; }
        string SourceObjectName { get; }
    }
}
