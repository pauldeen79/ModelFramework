namespace ClassFramework.TemplateFramework.ViewModels;

public class SpaceAndCommaViewModel : CsharpClassGeneratorViewModelBase
{
}

public class SpaceAndCommaViewModelLocatorComponent : IViewModelFactoryComponent
{
    public object Create()
        => new SpaceAndCommaViewModel();

    public bool Supports(object model)
        => model is SpaceAndCommaViewModel;
}
