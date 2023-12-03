namespace ClassFramework.TemplateFramework.ViewModels;

public class NewLineViewModel : CsharpClassGeneratorViewModelBase
{
    public NewLineViewModel(CsharpClassGeneratorSettings settings)
        : base(settings)
    {
    }
}

public class NewLineViewModelCreator : IViewModelCreator
{
    public object Create(object model, CsharpClassGeneratorSettings settings)
        => new NewLineViewModel(settings);

    public bool Supports(object model)
        => model is NewLineViewModel;
}
