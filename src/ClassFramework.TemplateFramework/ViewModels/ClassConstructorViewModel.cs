namespace ClassFramework.TemplateFramework.ViewModels;

public class ClassConstructorViewModel : MethodViewModelBase<ClassConstructor>
{
    public ClassConstructorViewModel(CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator)
        : base(settings, csharpExpressionCreator)
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

public class ClassConstructorViewModelCreator : IViewModelCreator
{
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;

    public ClassConstructorViewModelCreator(ICsharpExpressionCreator csharpExpressionCreator)
    {
        Guard.IsNotNull(csharpExpressionCreator);

        _csharpExpressionCreator = csharpExpressionCreator;
    }

    public object Create(object model, CsharpClassGeneratorSettings settings)
        => new ClassConstructorViewModel(settings, _csharpExpressionCreator);

    public bool Supports(object model)
        => model is ClassConstructor;
}
