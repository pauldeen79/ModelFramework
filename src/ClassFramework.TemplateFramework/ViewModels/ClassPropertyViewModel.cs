using TemplateFramework.Abstractions.Extensions;

namespace ClassFramework.TemplateFramework.ViewModels;

public class ClassPropertyViewModel : AttributeContainerViewModelBase<ClassProperty>
{
    public ClassPropertyViewModel(ICsharpExpressionCreator csharpExpressionCreator)
        : base(csharpExpressionCreator)
    {
    }

    public bool ShouldRenderModifiers
        => GetParentModel() is not Interface;
    
    public bool ShouldRenderExplicitInterfaceName
        => !string.IsNullOrEmpty(GetModel().ExplicitInterfaceName)
        && GetParentModel() is not Interface;

    public string TypeName
        => GetModel().TypeName
            .GetCsharpFriendlyTypeName()
            .AppendNullableAnnotation(Model!.IsNullable, Settings.EnableNullableContext)
            .AbbreviateNamespaces(Model.Metadata.GetStringValues(MetadataNames.NamespaceToAbbreviate));

    public string Name
        => GetModel().Name.Sanitize().GetCsharpFriendlyName();

    public string Modifiers
        => GetModel().GetModifiers();

    public string ExplicitInterfaceName
        => !string.IsNullOrEmpty(GetModel().ExplicitInterfaceName) && GetParentModel() is not Interface
            ? $"{Model!.ExplicitInterfaceName}."
            : string.Empty;

    public bool HasGetter
        => GetModel().HasGetter;

    public bool HasInitializer
        => GetModel().HasInitializer;

    public bool HasSetter
        => GetModel().HasSetter;

    public string GetterModifiers
        => GetSubModifiers(GetModel().GetterVisibility);

    public string SetterModifiers
        => GetSubModifiers(GetModel().SetterVisibility);

    public string InitializerModifiers
        => GetSubModifiers(GetModel().InitializerVisibility);

    public bool OmitGetterCode
        => GetModel().GetterCodeStatements.Count == 0 || GetParentModel() is Interface;

    public bool OmitInitializerCode
        => GetModel().InitializerCodeStatements.Count == 0 || GetParentModel() is Interface;

    public bool OmitSetterCode
        => GetModel().SetterCodeStatements.Count == 0 || GetParentModel() is Interface;

    public IEnumerable GetGetterCodeStatementModels()
        => GetModel().GetterCodeStatements;

    public IEnumerable GetInitializerCodeStatementModels()
        => GetModel().InitializerCodeStatements;

    public IEnumerable GetSetterCodeStatementModels()
        => GetModel().SetterCodeStatements;

    private string GetSubModifiers(Visibility? subVisibility)
    {
        var builder = new StringBuilder();

        if (subVisibility is not null && subVisibility != GetModel().Visibility)
        {
            builder.Append(subVisibility.ToString()!.ToLower(Settings.CultureInfo));
        }

        if (builder.Length > 0)
        {
            builder.Append(" ");
        }

        return builder.ToString();
    }
}
