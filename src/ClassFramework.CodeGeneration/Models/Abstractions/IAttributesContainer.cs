namespace ClassFramework.CodeGeneration.Models.Abstractions;

public interface IAttributesContainer
{
    IReadOnlyCollection<IAttribute> Attributes { get; }
}
