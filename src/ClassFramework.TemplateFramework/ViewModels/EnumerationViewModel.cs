namespace ClassFramework.TemplateFramework.ViewModels;

public class EnumerationViewModel : AttributeContainerViewModelBase<Enumeration>
{
    public EnumerationViewModel(CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator)
        : base(settings, csharpExpressionCreator)
    {
    }

    public string Modifiers
        => GetModel().GetModifiers();

    public string Name
        => GetModel().Name.Sanitize().GetCsharpFriendlyName();

    public IReadOnlyCollection<EnumerationMember> Members
        => GetModel().Members;
}

public class EnumerationViewModelCreator : IViewModelCreator
{
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;

    public EnumerationViewModelCreator(ICsharpExpressionCreator csharpExpressionCreator)
    {
        Guard.IsNotNull(csharpExpressionCreator);

        _csharpExpressionCreator = csharpExpressionCreator;
    }

    public object Create(object model, CsharpClassGeneratorSettings settings)
        => new EnumerationViewModel(settings, _csharpExpressionCreator);

    public bool Supports(object model)
        => model is Enumeration;
}
