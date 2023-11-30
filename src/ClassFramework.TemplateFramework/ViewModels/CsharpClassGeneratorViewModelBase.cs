namespace ClassFramework.TemplateFramework.ViewModels;

public abstract class CsharpClassGeneratorViewModelBase
{
    protected CsharpClassGeneratorViewModelBase(CsharpClassGeneratorSettings settings)
    {
        Guard.IsNotNull(settings);
        Settings = settings;
    }

    public CsharpClassGeneratorSettings Settings { get; }

    public string CreateIndentation(int additionalIndents = 0) => new string(' ', 4 * (Settings.IndentCount + additionalIndents));
}
