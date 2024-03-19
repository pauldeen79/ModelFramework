namespace DatabaseFramework.CodeGeneration.Models.Abstractions;

internal interface IViewCondition : IFileGroupNameContainer
{
    [Required] string Expression { get; }
    [Required(AllowEmptyStrings = true)] string Combination { get; } // required on first condition!
}
