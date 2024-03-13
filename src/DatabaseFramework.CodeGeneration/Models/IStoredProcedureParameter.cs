namespace DatabaseFramework.CodeGeneration.Models;

public interface IStoredProcedureParameter : INameContainer, IMetadataContainer
{
    [Required] string Type { get; }
    [Required(AllowEmptyStrings = true)] string DefaultValue { get; }
}
