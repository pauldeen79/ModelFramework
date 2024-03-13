namespace DatabaseFramework.CodeGeneration.Models;

internal interface IViewField : Abstractions.INameContainer, Abstractions.IMetadataContainer
{
    [Required(AllowEmptyStrings = true)] string SourceSchemaName { get; }
    [Required(AllowEmptyStrings = true)] string SourceObjectName { get; } // when empty, use Name
    [Required(AllowEmptyStrings = true)] string Expression { get; }
    [Required(AllowEmptyStrings = true)] string Alias { get; }
}
