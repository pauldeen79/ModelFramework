namespace DatabaseFramework.CodeGeneration.Models;

public interface ICheckConstraint : INameContainer, IMetadataContainer
{
    [Required] string Expression { get; }
}
