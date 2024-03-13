namespace DatabaseFramework.CodeGeneration.Models;

internal interface IStoredProcedure : INameContainer, IMetadataContainer
{
    [Required] IReadOnlyCollection<ISqlStatement> Statements { get; }
    [Required] IReadOnlyCollection<IStoredProcedureParameter> Parameters { get; }
}
