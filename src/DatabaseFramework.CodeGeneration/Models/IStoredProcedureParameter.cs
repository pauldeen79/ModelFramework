namespace DatabaseFramework.CodeGeneration.Models;

internal interface IStoredProcedureParameter : Abstractions.INameContainer, Abstractions.IMetadataContainer
{
    [Required] string Type { get; }
    [Required(AllowEmptyStrings = true)] string DefaultValue { get; }
}
