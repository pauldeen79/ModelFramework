using CrossCutting.Common;

namespace ModelFramework.Objects.Contracts
{
    public interface IEnumsContainer
    {
        ValueCollection<IEnum> Enums { get; }
    }
}