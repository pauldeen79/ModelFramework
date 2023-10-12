namespace ClassFramework.CodeGeneration.Models.Abstractions;

internal interface IConstructorsContainer
{
    [Required] IReadOnlyCollection<IClassConstructor> Constructors { get; }
}
