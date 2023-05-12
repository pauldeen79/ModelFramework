namespace ModelFramework.Objects.Contracts
{
    public interface ICodeStatement : IMetadataContainer
    {
        ICodeStatementBuilder CreateBuilder();
        ICodeStatementModel CreateModel();
    }
}
