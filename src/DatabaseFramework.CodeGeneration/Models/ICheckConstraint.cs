namespace DatabaseFramework.CodeGeneration.Models;

internal interface ICheckConstraint : INameContainer, IMetadataContainer
{
    [Required] string Expression { get; }
}
