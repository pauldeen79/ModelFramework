namespace DatabaseFramework.CodeGeneration.Models;

internal interface IView : IDatabaseObject
{
    [Required(AllowEmptyStrings = true)] string Definition { get; }
}
