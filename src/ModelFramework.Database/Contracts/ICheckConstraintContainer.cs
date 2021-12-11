using CrossCutting.Common;

namespace ModelFramework.Database.Contracts
{
    public interface ICheckConstraintContainer
    {
        ValueCollection<ICheckConstraint> CheckConstraints { get; }
    }
}
