namespace ClassFramework.TemplateFramework.ViewModels;

public class CsharpClassGeneratorViewModel
{
    public CsharpClassGeneratorViewModel(CsharpClassGeneratorSettings settings)
    {
        Guard.IsNotNull(settings);
        Settings = settings;
    }

    public CsharpClassGeneratorSettings Settings { get; }
}

public class CsharpClassGeneratorViewModel<TModel> : CsharpClassGeneratorViewModel
{
    public CsharpClassGeneratorViewModel(TModel data, CsharpClassGeneratorSettings settings) : base(settings)
    {
        Data = data;
    }

    public TModel Data { get; }
}
