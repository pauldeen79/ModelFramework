namespace ClassFramework.TemplateFramework.ViewModels;

public class EnumerationViewModel : AttributeContainerViewModelBase<Enumeration>
{
    public EnumerationViewModel(ICsharpExpressionCreator csharpExpressionCreator)
        : base(csharpExpressionCreator)
    {
    }

    public string Modifiers
        => GetModel().GetModifiers();

    public string Name
        => GetModel().Name.Sanitize().GetCsharpFriendlyName();

    public IReadOnlyCollection<EnumerationMember> Members
        => GetModel().Members;
}

public class EnumerationViewModelFactoryComponent : IViewModelFactoryComponent
{
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;

    public EnumerationViewModelFactoryComponent(ICsharpExpressionCreator csharpExpressionCreator)
    {
        Guard.IsNotNull(csharpExpressionCreator);

        _csharpExpressionCreator = csharpExpressionCreator;
    }

    public object Create()
        => new EnumerationViewModel(_csharpExpressionCreator);

    public bool Supports(object model)
        => model is Enumeration;
}
