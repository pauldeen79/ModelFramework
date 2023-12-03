namespace ClassFramework.TemplateFramework.ViewModels;

public class ClassConstructorViewModel : MethodViewModelBase<ClassConstructor>
{
    public ClassConstructorViewModel(ICsharpExpressionCreator csharpExpressionCreator)
        : base(csharpExpressionCreator)
    {
    }

    public string Name
    {
        get
        {
            var nameContainer = GetParentModel() as INameContainer;
            if (nameContainer is null)
            {
                throw new InvalidOperationException("Could not get name from parent context");
            }

            return nameContainer.Name.Sanitize().GetCsharpFriendlyName();
        }
    }

    public string ChainCall => string.IsNullOrEmpty(Model?.ChainCall)
        ? string.Empty
        : $" : {Model.ChainCall}";

    public bool OmitCode
        => GetParentModel() is Interface || GetModel().Abstract;
}

public class ClassConstructorViewModelFactoryComponent : IViewModelFactoryComponent
{
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;

    public ClassConstructorViewModelFactoryComponent(ICsharpExpressionCreator csharpExpressionCreator)
    {
        Guard.IsNotNull(csharpExpressionCreator);

        _csharpExpressionCreator = csharpExpressionCreator;
    }

    public object Create()
        => new ClassConstructorViewModel(_csharpExpressionCreator);

    public bool Supports(object model)
        => model is ClassConstructor;
}
