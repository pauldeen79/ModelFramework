namespace DatabaseFramework.CodeGeneration.Models;

internal interface IDefaultValueConstraint : INameContainer, IMetadataContainer
{
    [Required] string FieldName { get; }
    [Required(AllowEmptyStrings = true)] string DefaultValue { get; }
}
