namespace ModelFramework.Database.Contracts;

public interface IStoredProcedure : INameContainer, IMetadataContainer
{
    ValueCollection<ISqlStatement> Statements { get; }
    ValueCollection<IStoredProcedureParameter> Parameters { get; }
}
