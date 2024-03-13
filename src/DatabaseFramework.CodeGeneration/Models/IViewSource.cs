namespace DatabaseFramework.CodeGeneration.Models;

internal interface IViewSource : INameContainer, IMetadataContainer
{
    [Required(AllowEmptyStrings = true)] string Alias { get; }
    [Required(AllowEmptyStrings = true)] string SourceSchemaName { get; }
    [Required(AllowEmptyStrings = true)] string SourceObjectName { get; } // when empty, use Name
}
