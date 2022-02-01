namespace ModelFramework.Database.Contracts;

public interface IViewCondition : IMetadataContainer, IFileGroupNameContainer
{
    string Expression { get; }
    string Combination { get; }
}
