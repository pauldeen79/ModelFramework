namespace ModelFramework.Database.Contracts;

public interface ICheckConstraint : INameContainer, IMetadataContainer
{
    string Expression { get; }
}
