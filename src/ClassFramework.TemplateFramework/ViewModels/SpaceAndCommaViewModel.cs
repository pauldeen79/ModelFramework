namespace ClassFramework.TemplateFramework.ViewModels;

public class SpaceAndCommaViewModel : CsharpClassGeneratorViewModelBase
{
    public SpaceAndCommaViewModel(CsharpClassGeneratorSettings settings)
        : base(settings)
    {
    }
}

public class SpaceAndCommaViewModelCreator : IViewModelCreator
{
    public object Create(object model, CsharpClassGeneratorSettings settings)
        => new SpaceAndCommaViewModel(settings);

    public bool Supports(object model)
        => model is SpaceAndCommaViewModel;
}
