namespace ClassFramework.TemplateFramework.ViewModels;

public class ParameterViewModel : AttributeContainerViewModelBase<Parameter>
{
    public ParameterViewModel(CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator)
        : base(settings, csharpExpressionCreator)
    {
    }

    public string TypeName
        => GetModel().TypeName
            .GetCsharpFriendlyTypeName()
            .AppendNullableAnnotation(Model!.IsNullable, Settings.EnableNullableContext)
            .AbbreviateNamespaces(Model.Metadata.GetStringValues(MetadataNames.NamespaceToAbbreviate));

    public string Name => GetModel().Name.Sanitize().GetCsharpFriendlyName();

    public bool ShouldRenderDefaultValue => GetModel().DefaultValue is not null;

    public string GetDefaultValueExpression() => CsharpExpressionCreator.Create(GetModel().DefaultValue);
}
