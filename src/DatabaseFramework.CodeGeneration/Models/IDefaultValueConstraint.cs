namespace DatabaseFramework.CodeGeneration.Models;

public interface IDefaultValueConstraint : INameContainer, IMetadataContainer
{
    [Required] string FieldName { get; }
    [Required(AllowEmptyStrings = true)] string DefaultValue { get; }
}
