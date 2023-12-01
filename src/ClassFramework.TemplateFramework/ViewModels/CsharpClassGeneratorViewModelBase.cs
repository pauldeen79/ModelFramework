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

public abstract class CsharpClassGeneratorViewModelBase<TModel> : CsharpClassGeneratorViewModelBase, IModelContainer<TModel>, ITemplateContextContainer
{
    protected ICsharpExpressionCreator CsharpExpressionCreator { get; }

    protected CsharpClassGeneratorViewModelBase(CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator) : base(settings)
    {
        Guard.IsNotNull(csharpExpressionCreator);

        CsharpExpressionCreator = csharpExpressionCreator;
    }

    public TModel? Model { get; set; }
    public ITemplateContext Context { get; set; } = default!;

    public TModel GetModel()
    {
        Guard.IsNotNull(Model);

        return Model;
    }

    private object? GetParent()
    {
        Guard.IsNotNull(Context);

        return Context.ParentContext?.Model;
    }

    protected object? GetParentModel()
    {
        dynamic? d = GetParent();
        if (d is null)
        {
            throw new InvalidOperationException("Model of parent context is not set");
        }

        return d.Model;
    }
}
