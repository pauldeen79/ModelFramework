namespace ClassFramework.CodeGeneration.Models.Abstractions;

internal interface IBaseClassContainer : ITypeBase
{
    string? BaseClass { get; }
}
