namespace ClassFramework.TemplateFramework.ViewModels;

public class EnumerationViewModel : AttributeContainerViewModelBase<Enumeration>
{
    public EnumerationViewModel(Enumeration data, CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator)
        : base(data, settings, csharpExpressionCreator)
    {
    }

    public string Name => Data.Name.Sanitize().GetCsharpFriendlyName();
}
