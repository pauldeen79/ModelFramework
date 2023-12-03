namespace ClassFramework.TemplateFramework.ViewModels;

public class ClassFieldViewModel : AttributeContainerViewModelBase<ClassField>
{
    public ClassFieldViewModel(ICsharpExpressionCreator csharpExpressionCreator)
        : base(csharpExpressionCreator)
    {
    }

    public string Modifiers
        => GetModel().GetModifiers();

    public bool Event
        => GetModel().Event;

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

public class ClassFieldViewModelFactoryComponent : IViewModelFactoryComponent
{
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;

    public ClassFieldViewModelFactoryComponent(ICsharpExpressionCreator csharpExpressionCreator)
    {
        Guard.IsNotNull(csharpExpressionCreator);

        _csharpExpressionCreator = csharpExpressionCreator;
    }

    public object Create()
        => new ClassFieldViewModel(_csharpExpressionCreator);

    public bool Supports(object model)
        => model is ClassField;
}
