namespace ModelFramework.Database.Contracts;

public interface IStoredProcedure : INameContainer, IMetadataContainer
{
    IReadOnlyCollection<ISqlStatement> Statements { get; }
    IReadOnlyCollection<IStoredProcedureParameter> Parameters { get; }
}
