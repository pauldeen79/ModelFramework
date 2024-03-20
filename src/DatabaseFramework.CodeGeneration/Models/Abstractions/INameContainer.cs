namespace DatabaseFramework.CodeGeneration.Models.Abstractions;

internal interface INameContainer
{
    [Required] string Name { get; }
}
