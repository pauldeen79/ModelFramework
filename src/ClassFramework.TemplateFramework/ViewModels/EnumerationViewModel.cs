namespace ClassFramework.TemplateFramework.ViewModels;

public class EnumerationViewModel : AttributeContainerViewModelBase<Enumeration>
{
    public EnumerationViewModel(ICsharpExpressionCreator csharpExpressionCreator)
        : base(csharpExpressionCreator)
    {
    }

    public string Modifiers
        => GetModel().GetModifiers(Settings.CultureInfo);

    public string Name
        => GetModel().Name.Sanitize().GetCsharpFriendlyName();

    public IEnumerable<EnumerationMemberViewModel> GetMemberModels()
        => GetModel().Members.Select(x => new EnumerationMemberViewModel(CsharpExpressionCreator) { Model = x });
}
