namespace ModelFramework.Objects.Contracts;

public interface IOverload : IParametersContainer
{
    string MethodName { get; }
    string InitializeExpression { get; }
}
