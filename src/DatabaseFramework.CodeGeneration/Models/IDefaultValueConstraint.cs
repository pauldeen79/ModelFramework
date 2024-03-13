namespace DatabaseFramework.CodeGeneration.Models;

internal interface IDefaultValueConstraint : Abstractions.INameContainer, Abstractions.IMetadataContainer
{
    [Required] string FieldName { get; }
    [Required(AllowEmptyStrings = true)] string DefaultValue { get; }
}
