namespace ClassFramework.CodeGeneration.Models;

internal interface IAttributeParameter : IMetadataContainer, INameContainer
{
    object Value { get; }
}
