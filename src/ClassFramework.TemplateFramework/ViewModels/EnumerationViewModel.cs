namespace ClassFramework.TemplateFramework.ViewModels;

public class EnumerationViewModel : AttributeContainerViewModelBase<Enumeration>
{
    public EnumerationViewModel(CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator)
        : base(settings, csharpExpressionCreator)
    {
    }

    public string Name => GetModel().Name.Sanitize().GetCsharpFriendlyName();
}
