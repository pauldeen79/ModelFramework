using ModelFramework.Common.Contracts;

namespace ModelFramework.Database.Contracts
{
    public interface IViewField : INameContainer, IMetadataContainer
    {
        string SourceSchemaName { get; }
        string SourceObjectName { get; }
        string Expression { get; }
        string Alias { get; }
    }
}
