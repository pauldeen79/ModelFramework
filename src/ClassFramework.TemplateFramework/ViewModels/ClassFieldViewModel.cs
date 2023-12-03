namespace ClassFramework.TemplateFramework.ViewModels;

public class ClassFieldViewModel : AttributeContainerViewModelBase<ClassField>
{
    public ClassFieldViewModel(CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator)
        : base(settings, csharpExpressionCreator)
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

public class ClassFieldViewModelCreator : IViewModelCreator
{
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;

    public ClassFieldViewModelCreator(ICsharpExpressionCreator csharpExpressionCreator)
    {
        Guard.IsNotNull(csharpExpressionCreator);

        _csharpExpressionCreator = csharpExpressionCreator;
    }

    public object Create(object model, CsharpClassGeneratorSettings settings)
        => new ClassFieldViewModel(settings, _csharpExpressionCreator);

    public bool Supports(object model)
        => model is ClassField;
}
