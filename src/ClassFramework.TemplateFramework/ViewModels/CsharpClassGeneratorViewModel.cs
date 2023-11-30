namespace ClassFramework.TemplateFramework.ViewModels;

public class CsharpClassGeneratorViewModel<TModel> : CsharpClassGeneratorViewModelBase
{
    public CsharpClassGeneratorViewModel(TModel data, CsharpClassGeneratorSettings settings) : base(settings)
    {
        Guard.IsNotNull(data);
        Data = data;
    }

    public TModel Data { get; }
}
