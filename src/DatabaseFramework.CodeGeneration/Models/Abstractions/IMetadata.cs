namespace DatabaseFramework.CodeGeneration.Models.Abstractions;

internal interface IMetadata : INameContainer
{
    object? Value { get; }
}
