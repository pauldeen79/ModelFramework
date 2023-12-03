namespace ClassFramework.TemplateFramework.ViewModels;

public class ClassPropertyViewModel : CsharpClassGeneratorViewModelBase<ClassProperty>
{
    public ClassPropertyViewModel(CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator)
        : base(settings, csharpExpressionCreator)
    {
    }
}

public class ClassPropertyViewModelCreator : IViewModelCreator
{
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;

    public ClassPropertyViewModelCreator(ICsharpExpressionCreator csharpExpressionCreator)
    {
        Guard.IsNotNull(csharpExpressionCreator);

        _csharpExpressionCreator = csharpExpressionCreator;
    }

    public object Create(object model, CsharpClassGeneratorSettings settings)
        => new ClassPropertyViewModel(settings, _csharpExpressionCreator);

    public bool Supports(object model)
        => model is ClassProperty;
}
