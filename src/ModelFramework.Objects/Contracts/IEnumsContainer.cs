namespace ModelFramework.Objects.Contracts;

public interface IEnumsContainer
{
    IReadOnlyCollection<IEnum> Enums { get; }
}
