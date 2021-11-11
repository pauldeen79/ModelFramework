using System.Collections.Generic;

namespace ModelFramework.Objects.Contracts
{
    public interface IParametersContainer
    {
        IReadOnlyCollection<IParameter> Parameters { get; }
    }
}
