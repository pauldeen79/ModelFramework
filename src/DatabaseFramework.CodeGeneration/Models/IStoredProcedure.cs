namespace DatabaseFramework.CodeGeneration.Models;

internal interface IStoredProcedure : Abstractions.INameContainer, Abstractions.IMetadataContainer
{
    [Required] IReadOnlyCollection<ISqlStatement> Statements { get; }
    [Required] IReadOnlyCollection<IStoredProcedureParameter> Parameters { get; }
}
