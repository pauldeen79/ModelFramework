using ModelFramework.Common.Contracts;

namespace ModelFramework.Database.Contracts
{
    public interface ITableField : INameContainer, IMetadataContainer
    {
        string Type { get; }
        bool IsIdentity { get; }
        bool IsRequired { get; }
        byte? NumericPrecision { get; }
        byte? NumericScale { get; }
        int? StringLength { get; }
        string StringCollation { get; }
        bool? IsStringMaxLength { get; }
        ITableFieldCheckConstraint CheckConstraint { get; }
    }
}
