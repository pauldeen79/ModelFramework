namespace ClassFramework.TemplateFramework.ViewModels;

public abstract class CsharpClassGeneratorViewModelBase : ICsharpClassGeneratorSettingsContainer
{
    public CsharpClassGeneratorSettings Settings { get; set; } = default!;

    public string CreateIndentation(int additionalIndents = 0) => new string(' ', 4 * (Settings.IndentCount + additionalIndents));
}

public abstract class CsharpClassGeneratorViewModelBase<TModel> : CsharpClassGeneratorViewModelBase, IModelContainer<TModel>, ITemplateContextContainer
{
    protected CsharpClassGeneratorViewModelBase(ICsharpExpressionCreator csharpExpressionCreator)
    {
        Guard.IsNotNull(csharpExpressionCreator);

        CsharpExpressionCreator = csharpExpressionCreator;
    }

    public TModel? Model { get; set; }
    public ITemplateContext Context { get; set; } = default!;
    public ICsharpExpressionCreator CsharpExpressionCreator { get; set; }

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
