namespace DatabaseFramework.CodeGeneration.Models;

internal interface ICheckConstraint : INameContainer
{
    [Required] string Expression { get; }
}
