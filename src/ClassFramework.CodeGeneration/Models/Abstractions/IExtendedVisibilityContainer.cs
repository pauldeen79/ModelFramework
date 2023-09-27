namespace ClassFramework.CodeGeneration.Models.Abstractions;

internal interface IExtendedVisibilityContainer
{
    bool Static { get; }
    bool Virtual { get; }
    bool Abstract { get; }
    bool Protected { get; }
    bool Override { get; }
}
