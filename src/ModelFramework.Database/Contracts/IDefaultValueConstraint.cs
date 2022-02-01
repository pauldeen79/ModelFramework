namespace ModelFramework.Database.Contracts;

public interface IDefaultValueConstraint : INameContainer, IMetadataContainer
{
    string FieldName { get; }
    string DefaultValue { get; }
}
