namespace DatabaseFramework.CodeGeneration.Models.Abstractions;

internal interface IFileGroupNameContainer
{
    [Required] string FileGroupName { get; }
}
