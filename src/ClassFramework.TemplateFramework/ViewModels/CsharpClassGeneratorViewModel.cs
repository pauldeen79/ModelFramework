namespace ClassFramework.TemplateFramework.ViewModels;

public abstract class CsharpClassGeneratorViewModel
{
    protected CsharpClassGeneratorViewModel(CsharpClassGeneratorSettings settings)
    {
        Guard.IsNotNull(settings);
        Settings = settings;
    }

    public CsharpClassGeneratorSettings Settings { get; }

    public string CreateIndentation(int additionalIndents = 0) => new string(' ', 4 * (Settings.IndentCount + additionalIndents));
}

public class CsharpClassGeneratorViewModel<TModel> : CsharpClassGeneratorViewModel
{
    public CsharpClassGeneratorViewModel(TModel data, CsharpClassGeneratorSettings settings) : base(settings)
    {
        Guard.IsNotNull(data);
        Data = data;
    }

    public TModel Data { get; }
}
