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
            var parentModel = GetParentModel();
            var modelProperty = parentModel?.GetType().GetProperty(nameof(IModelContainer<object>.Model));
            if (modelProperty is not null)
            {
                parentModel = modelProperty.GetValue(parentModel);
            }

            var nameContainer = parentModel as INameContainer;
            if (nameContainer is null)
            {
                throw new InvalidOperationException("Could not get name from parent context");
            }

            return nameContainer.Name.Sanitize().GetCsharpFriendlyName();
        }
    }

    public string ChainCall
        => string.IsNullOrEmpty(GetModel().ChainCall)
            ? string.Empty
            : $" : {Model!.ChainCall}";

    public bool OmitCode
        => GetParentModel() is Interface || GetModel().Abstract;
}
