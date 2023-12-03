namespace ClassFramework.TemplateFramework.ViewModels;

public class NewLineViewModel : CsharpClassGeneratorViewModelBase
{
}

public class NewLineViewModelFactoryComponent : IViewModelFactoryComponent
{
    public object Create()
        => new NewLineViewModel();

    public bool Supports(object model)
        => model is NewLineViewModel;
}
