namespace ClassFramework.TemplateFramework.ViewModels;

public class CsharpClassGeneratorViewModel<TModel> : CsharpClassGeneratorViewModelBase
{
    protected ICsharpExpressionCreator CsharpExpressionCreator { get; }

    public CsharpClassGeneratorViewModel(TModel data, CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator) : base(settings)
    {
        Guard.IsNotNull(csharpExpressionCreator);
        Guard.IsNotNull(data);

        Data = data;
        CsharpExpressionCreator = csharpExpressionCreator;
    }

    public TModel Data { get; }
}
