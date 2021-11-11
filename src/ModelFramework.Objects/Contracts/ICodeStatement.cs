using ModelFramework.Common.Contracts;

namespace ModelFramework.Objects.Contracts
{
    public interface ICodeStatement : IMetadataContainer
    {
        ICodeStatementBuilder CreateBuilder();
    }
}