namespace ClassFramework.CodeGeneration.Models.Abstractions;

internal interface IFieldsContainer
{
    [Required] IReadOnlyCollection<IClassField> Fields { get; }
}
