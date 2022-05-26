namespace ModelFramework.Objects.Contracts;

public interface IAttributesContainer
{
    IReadOnlyCollection<IAttribute> Attributes { get; }
}
