namespace ClassFramework.TemplateFramework;

public sealed class ViewModelTemplateByModelIdentifier : ITemplateIdentifier
{
    public ViewModelTemplateByModelIdentifier(object? model, CsharpClassGeneratorSettings settings)
    {
        Guard.IsNotNull(settings);

        Model = model;
        Settings = settings;
    }

    public object? Model { get; }
    public CsharpClassGeneratorSettings Settings { get; }
}
