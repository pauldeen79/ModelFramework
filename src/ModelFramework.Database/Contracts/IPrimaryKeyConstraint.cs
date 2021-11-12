using CrossCutting.Common;
using ModelFramework.Common.Contracts;

namespace ModelFramework.Database.Contracts
{
    public interface IPrimaryKeyConstraint : INameContainer, IMetadataContainer, IFileGroupNameContainer
    {
        ValueCollection<IPrimaryKeyConstraintField> Fields { get; }
    }
}
