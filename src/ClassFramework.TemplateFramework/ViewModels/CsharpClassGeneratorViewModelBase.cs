namespace ClassFramework.TemplateFramework.ViewModels;

public abstract class CsharpClassGeneratorViewModelBase : ICsharpClassGeneratorSettingsContainer, IViewModel
{
    public CsharpClassGeneratorSettings Settings { get; set; } = default!; // will always be injected in CreateModel (root viewmodel) or OnSetContext (child viewmodels) method
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
    
    public ITemplateContext Context { get; set; } = default!; // will always be injected in OnSetContext method

    public TModel GetModel()
    {
        Guard.IsNotNull(Model);

        return Model;
    }

    protected object? GetParentModel()
    {
        Guard.IsNotNull(Context);
        
        return Context.ParentContext?.Model;
    }

    public string CreateIndentation(int additionalIndents = 0)
    {
        Guard.IsNotNull(Context);

        return new string(' ', 4 * (Context.GetIndentCount() + additionalIndents));
    }
}
