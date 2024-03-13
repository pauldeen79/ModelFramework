namespace DatabaseFramework.CodeGeneration.Models;

public interface IStoredProcedure : INameContainer, IMetadataContainer
{
    [Required] IReadOnlyCollection<ISqlStatement> Statements { get; }
    [Required] IReadOnlyCollection<IStoredProcedureParameter> Parameters { get; }
}
