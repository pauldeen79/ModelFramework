namespace DatabaseFramework.CodeGeneration.Models;

internal interface IStoredProcedureParameter : INameContainer, IMetadataContainer
{
    [Required] string Type { get; }
    [Required(AllowEmptyStrings = true)] string DefaultValue { get; }
}
