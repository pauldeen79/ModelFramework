namespace DatabaseFramework.CodeGeneration.Models;

internal interface IDefaultValueConstraint : INameContainer, IMetadataContainer
{
    [Required] string FieldName { get; }
    [Required] string DefaultValue { get; }
}
