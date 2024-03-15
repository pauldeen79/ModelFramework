namespace DatabaseFramework.CodeGeneration.Models.Abstractions;

internal interface IFileGroupNameContainer
{
    [Required(AllowEmptyStrings = true)] string FileGroupName { get; }
}
