namespace DatabaseFramework.CodeGeneration.Models.Abstractions;

internal interface ISchemaContainer
{
    [Required] string Schema { get; }
}
