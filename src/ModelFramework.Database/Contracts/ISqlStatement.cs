namespace ModelFramework.Database.Contracts
{
    public interface ISqlStatement : IMetadataContainer
    {
        ISqlStatementBuilder CreateBuilder();
    }
}
