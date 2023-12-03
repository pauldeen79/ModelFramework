namespace ClassFramework.TemplateFramework.ViewModels;

public abstract class CsharpClassGeneratorViewModelBase : ICsharpClassGeneratorSettingsContainer, IViewModel
{
    public CsharpClassGeneratorSettings Settings { get; set; } = default!; // will always be injected in CreateViewModel method

    public string CreateIndentation(int additionalIndents = 0)
        => new string(' ', 4 * (Settings.IndentCount + 1 + additionalIndents));
}

public abstract class CsharpClassGeneratorViewModelBase<TModel> : CsharpClassGeneratorViewModelBase, IModelContainer<TModel>, ITemplateContextContainer
{
    protected CsharpClassGeneratorViewModelBase(ICsharpExpressionCreator csharpExpressionCreator)
    {
        Guard.IsNotNull(csharpExpressionCreator);

        CsharpExpressionCreator = csharpExpressionCreator;
    }

    public TModel? Model { get; set; }
    public ICsharpExpressionCreator CsharpExpressionCreator { get; set; }
    
    public ITemplateContext Context { get; set; } = default!; // will always be injected in CreateViewModel method

    public TModel GetModel()
    {
        Guard.IsNotNull(Model);

        return Model;
    }

    protected object? GetParentModel()
    {
        dynamic? d = Context?.ParentContext?.Model;
        if (d is null)
        {
            throw new InvalidOperationException("Model of parent context is not set");
        }

        return d.Model;
    }
}
