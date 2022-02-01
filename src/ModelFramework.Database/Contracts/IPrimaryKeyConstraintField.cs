namespace ModelFramework.Database.Contracts;

public interface IPrimaryKeyConstraintField : INameContainer, IMetadataContainer
{
    bool IsDescending { get; }
}
