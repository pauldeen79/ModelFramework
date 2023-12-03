namespace ClassFramework.TemplateFramework.ViewModels;

public class ClassPropertyViewModel : CsharpClassGeneratorViewModelBase<ClassProperty>
{
    public ClassPropertyViewModel(ICsharpExpressionCreator csharpExpressionCreator)
        : base(csharpExpressionCreator)
    {
    }
}

public class ClassPropertyViewModelFactoryComponent : IViewModelFactoryComponent
{
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;

    public ClassPropertyViewModelFactoryComponent(ICsharpExpressionCreator csharpExpressionCreator)
    {
        Guard.IsNotNull(csharpExpressionCreator);

        _csharpExpressionCreator = csharpExpressionCreator;
    }

    public object Create()
        => new ClassPropertyViewModel(_csharpExpressionCreator);

    public bool Supports(object model)
        => model is ClassProperty;
}
