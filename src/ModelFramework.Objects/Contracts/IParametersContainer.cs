using CrossCutting.Common;

namespace ModelFramework.Objects.Contracts
{
    public interface IParametersContainer
    {
        ValueCollection<IParameter> Parameters { get; }
    }
}
