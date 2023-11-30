namespace ClassFramework.TemplateFramework.ViewModels;

public class ClassFieldViewModel : AttributeContainerViewModelBase<ClassField>
{
    public ClassFieldViewModel(ClassField data, CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator) : base(data, settings, csharpExpressionCreator)
    {
    }

    public string TypeName
        => Data.TypeName
            .GetCsharpFriendlyTypeName()
            .AppendNullableAnnotation(Data.IsNullable, Settings.EnableNullableContext)
            .AbbreviateNamespaces(Data.Metadata.GetStringValues(MetadataNames.NamespaceToAbbreviate));
    
    public string Name => Data.Name.Sanitize().GetCsharpFriendlyName();

    public bool ShouldRenderDefaultValue => Data.DefaultValue is not null;

    public string GetDefaultValueExpression() => CsharpExpressionCreator.Create(Data.DefaultValue);
}
