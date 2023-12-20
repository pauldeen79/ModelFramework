namespace ClassFramework.TemplateFramework.ViewModels;

public class PropertyViewModel : AttributeContainerViewModelBase<Property>
{
    public PropertyViewModel(ICsharpExpressionCreator csharpExpressionCreator)
        : base(csharpExpressionCreator)
    {
    }

    public bool ShouldRenderModifiers
        => GetParentModel() is not Interface;
    
    public string TypeName
        => GetModel().TypeName
            .GetCsharpFriendlyTypeName()
            .AppendNullableAnnotation(Model!.IsNullable, Settings.EnableNullableContext)
            .AbbreviateNamespaces(Model.Metadata.GetStringValues(MetadataNames.NamespaceToAbbreviate));

    public string Name
        => GetModel().Name.Sanitize().GetCsharpFriendlyName();

    public string Modifiers
        => GetModel().GetModifiers(Settings.CultureInfo);

    public string ExplicitInterfaceName
        => !string.IsNullOrEmpty(GetModel().ExplicitInterfaceName) && GetParentModel() is not Interface
            ? $"{Model!.ExplicitInterfaceName}."
            : string.Empty;

    public bool ShouldRenderDefaultValue
        => GetModel().DefaultValue is not null;

    public string DefaultValueExpression
        => CsharpExpressionCreator.Create(GetModel().DefaultValue);

    public IEnumerable<PropertyCodeBodyModel> GetCodeBodyModels()
    {
        var model = GetModel();
        var parentModel = GetParentModel();
        yield return new PropertyCodeBodyModel(model.HasGetter, "get", model.Visibility, model.GetterVisibility, parentModel, model.GetterCodeStatements, Settings.CultureInfo);
        yield return new PropertyCodeBodyModel(model.HasInitializer, "init", model.Visibility, model.InitializerVisibility, parentModel, model.InitializerCodeStatements, Settings.CultureInfo);
        yield return new PropertyCodeBodyModel(model.HasSetter, "set", model.Visibility, model.SetterVisibility, parentModel, model.SetterCodeStatements, Settings.CultureInfo);
    }
}
