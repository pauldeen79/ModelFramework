namespace ModelFramework.Common.Contracts
{
    public interface IMetadata : INameContainer
    {
        object? Value { get; }
    }
}
