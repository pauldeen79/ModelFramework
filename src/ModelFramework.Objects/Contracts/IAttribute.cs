using CrossCutting.Common;
using ModelFramework.Common.Contracts;

namespace ModelFramework.Objects.Contracts
{
    public interface IAttribute : IMetadataContainer, INameContainer
    {
        ValueCollection<IAttributeParameter> Parameters { get; }
    }
}
