namespace ClassFramework.CodeGeneration.Models.Abstractions;

public interface IParametersContainer
{
    IReadOnlyCollection<IParameter> Parameters { get; }
}
