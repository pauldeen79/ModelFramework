namespace DatabaseFramework.CodeGeneration.Models.Abstractions;

internal interface INonViewField : INameContainer
{
    SqlFieldType Type { get; }
    byte? NumericPrecision { get; }
    byte? NumericScale { get; }
    int? StringLength { get; }
    [Required(AllowEmptyStrings = true)] string StringCollation { get; }
    bool? IsStringMaxLength { get; }
}
