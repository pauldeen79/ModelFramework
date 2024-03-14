namespace DatabaseFramework.CodeGeneration.Models;

internal interface IStoredProcedureParameter : INonViewField
{
    [Required(AllowEmptyStrings = true)] string DefaultValue { get; }
}
