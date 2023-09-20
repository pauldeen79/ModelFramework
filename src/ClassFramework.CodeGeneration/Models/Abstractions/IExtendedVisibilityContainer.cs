namespace ClassFramework.CodeGeneration.Models.Abstractions;

public interface IExtendedVisibilityContainer
{
    bool Static { get; }
    bool Virtual { get; }
    bool Abstract { get; }
    bool Protected { get; }
    bool Override { get; }
}
