namespace DatabaseFramework.CodeGeneration.Models;

internal interface IDefaultValueConstraint : INameContainer
{
    [Required] string FieldName { get; }
    [Required] string DefaultValue { get; }
}
