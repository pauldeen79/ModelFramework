namespace DatabaseFramework.CodeGeneration.Models;

public interface ITableField : INameContainer, IMetadataContainer, ICheckConstraintContainer
{
    [Required] string Type { get; }
    bool IsIdentity { get; }
    bool IsRequired { get; }
    byte? NumericPrecision { get; }
    byte? NumericScale { get; }
    int? StringLength { get; }
    [Required(AllowEmptyStrings = true)] string StringCollation { get; }
    bool IsStringMaxLength { get; }
}
