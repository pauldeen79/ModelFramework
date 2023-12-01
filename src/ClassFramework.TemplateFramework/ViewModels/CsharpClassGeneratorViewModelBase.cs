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

public abstract class CsharpClassGeneratorViewModelBase<TModel> : CsharpClassGeneratorViewModelBase
{
    protected ICsharpExpressionCreator CsharpExpressionCreator { get; }

    protected CsharpClassGeneratorViewModelBase(TModel data, CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator) : base(settings)
    {
        Guard.IsNotNull(csharpExpressionCreator);
        Guard.IsNotNull(data);

        Data = data;
        CsharpExpressionCreator = csharpExpressionCreator;
    }

    public TModel Data { get; }
}
