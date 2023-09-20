namespace ClassFramework.CodeGeneration.Models;

public interface IAttributeParameter : IMetadataContainer, INameContainer
{
    object Value { get; }
}
