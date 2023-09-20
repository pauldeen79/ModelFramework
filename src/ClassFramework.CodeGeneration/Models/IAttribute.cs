namespace ClassFramework.CodeGeneration.Models;

public interface IAttribute : IMetadataContainer, INameContainer
{
    IReadOnlyCollection<IAttributeParameter> Parameters { get; }
}
