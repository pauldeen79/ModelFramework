namespace ModelFramework.Objects.Contracts;

public interface IParametersContainer
{
    IReadOnlyCollection<IParameter> Parameters { get; }
}
