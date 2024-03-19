namespace DatabaseFramework.CodeGeneration.Models.Abstractions;

internal interface IViewField : INameContainer
{
    [Required(AllowEmptyStrings = true)] string SourceSchemaName { get; }
    [Required(AllowEmptyStrings = true)] string SourceObjectName { get; } // when empty, use Name
    [Required(AllowEmptyStrings = true)] string Expression { get; }
    [Required(AllowEmptyStrings = true)] string Alias { get; }
}
