namespace ClassFramework.CodeGeneration.Models;

public interface IAttribute : IMetadataContainer, INameContainer
{
    [Required] IReadOnlyCollection<IAttributeParameter> Parameters { get; }
}
