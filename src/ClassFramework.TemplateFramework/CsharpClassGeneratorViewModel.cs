namespace ClassFramework.TemplateFramework;

public sealed class CsharpClassGeneratorViewModel<TModel>
{
    public CsharpClassGeneratorViewModel(TModel data, CsharpClassGeneratorSettings settings)
    {
        Data = data;
        Settings = settings;
    }

    public TModel Data { get; }
    public CsharpClassGeneratorSettings Settings { get; }
}
