namespace ClassFramework.CodeGeneration.Models.Abstractions;

public interface IAttributesContainer
{
    [Required] IReadOnlyCollection<IAttribute> Attributes { get; }
}
