namespace ClassFramework.TemplateFramework.ViewModels;

public class ParameterViewModel : AttributeContainerViewModelBase<Parameter>
{
    public ParameterViewModel(ICsharpExpressionCreator csharpExpressionCreator)
        : base(csharpExpressionCreator)
    {
    }

    public string Prefix
    {
        get
        {
            var model = GetModel();

            if (model.IsParamArray)
            {
                return "params ";
            }
            else if (model.IsRef)
            {
                return "ref ";
            }
            else if (model.IsOut)
            {
                return "out ";
            }

            return string.Empty;
        }
    }

    public string TypeName
        => GetModel().TypeName
            .GetCsharpFriendlyTypeName()
            .AppendNullableAnnotation(Model!.IsNullable, Settings.EnableNullableContext)
            .AbbreviateNamespaces(Model.Metadata.GetStringValues(MetadataNames.NamespaceToAbbreviate));

    public string Name
        => GetModel().Name.Sanitize().GetCsharpFriendlyName();

    public bool ShouldRenderDefaultValue
        => GetModel().DefaultValue is not null;

    public string DefaultValueExpression
        => CsharpExpressionCreator.Create(GetModel().DefaultValue);
}

public class ParameterViewModelFactoryComponent : IViewModelFactoryComponent
{
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;

    public ParameterViewModelFactoryComponent(ICsharpExpressionCreator csharpExpressionCreator)
    {
        Guard.IsNotNull(csharpExpressionCreator);

        _csharpExpressionCreator = csharpExpressionCreator;
    }

    public object Create()
        => new ParameterViewModel(_csharpExpressionCreator);

    public bool Supports(object model)
        => model is Parameter;
}
