namespace DatabaseFramework.CodeGeneration.Models.Abstractions;

internal interface ISchemaContainer
{
    [Required] [DefaultValue("dbo")] string Schema { get; }
}
