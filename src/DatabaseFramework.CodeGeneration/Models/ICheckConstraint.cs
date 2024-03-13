namespace DatabaseFramework.CodeGeneration.Models;

internal interface ICheckConstraint : Abstractions.INameContainer, Abstractions.IMetadataContainer
{
    [Required] string Expression { get; }
}
