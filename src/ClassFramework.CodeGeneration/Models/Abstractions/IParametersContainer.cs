namespace ClassFramework.CodeGeneration.Models.Abstractions;

public interface IParametersContainer
{
    [Required] IReadOnlyCollection<IParameter> Parameters { get; }
}
