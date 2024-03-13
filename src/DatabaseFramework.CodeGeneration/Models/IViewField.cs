namespace DatabaseFramework.CodeGeneration.Models;

public interface IViewField : INameContainer, IMetadataContainer
{
    [Required(AllowEmptyStrings = true)] string SourceSchemaName { get; }
    [Required(AllowEmptyStrings = true)] string SourceObjectName { get; } // when empty, use Name
    [Required(AllowEmptyStrings = true)] string Expression { get; }
    [Required(AllowEmptyStrings = true)] string Alias { get; }
}
