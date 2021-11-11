using ModelFramework.Common.Contracts;

namespace ModelFramework.Objects.Contracts
{
    public interface IAttributeParameter : IMetadataContainer, INameContainer
    {
        object Value { get; }
    }
}
