using CrossCutting.Common;

namespace ModelFramework.Objects.Contracts
{
    public interface IAttributesContainer
    {
        ValueCollection<IAttribute> Attributes { get; }
    }
}
