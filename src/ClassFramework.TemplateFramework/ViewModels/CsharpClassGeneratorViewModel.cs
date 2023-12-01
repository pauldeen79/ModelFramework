namespace ClassFramework.TemplateFramework.ViewModels;

public class CsharpClassGeneratorViewModel : CsharpClassGeneratorViewModelBase<IEnumerable<TypeBase>>
{
    public CsharpClassGeneratorViewModel(IEnumerable<TypeBase> data, CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator)
        : base(data, settings, csharpExpressionCreator)
    {
    }
}
