using System.Collections.Generic;
using ModelFramework.Common.Contracts;

namespace ModelFramework.Objects.Contracts
{
    public interface IAttribute : IMetadataContainer, INameContainer
    {
        IReadOnlyCollection<IAttributeParameter> Parameters { get; }
    }
}
