namespace ClassFramework.TemplateFramework.ViewModels;

public class ClassMethodViewModel : MethodViewModelBase<ClassMethod>
{
    public ClassMethodViewModel(ICsharpExpressionCreator csharpExpressionCreator)
        : base(csharpExpressionCreator)
    {
    }

    public bool ShouldRenderModifiers
        => string.IsNullOrEmpty(GetModel().ExplicitInterfaceName) && GetParentModel() is not Interface;

    public string ReturnTypeName
        => GetModel().TypeName
            .GetCsharpFriendlyTypeName()
            .AppendNullableAnnotation(Model!.IsNullable, Settings.EnableNullableContext)
            .AbbreviateNamespaces(Model.Metadata.GetStringValues(MetadataNames.NamespaceToAbbreviate))
            .WhenNullOrEmpty("void");

    public string ExplicitInterfaceName
        => !string.IsNullOrEmpty(GetModel().ExplicitInterfaceName) && GetParentModel() is not Interface
            ? $"{Model!.ExplicitInterfaceName}."
            : string.Empty;

    public string GenericTypeArguments
        => GetModel().GetGenericTypeArgumentsString();

    public bool ExtensionMethod
        => GetModel().ExtensionMethod;

    public string Name
    {
        get
        {
            var model = GetModel();
            if (model.Operator)
            {
                return "operator " + model.Name;
            }
            
            if (model.IsInterfaceMethod())
            {
                return model.Name.RemoveInterfacePrefix().Sanitize().GetCsharpFriendlyName();
            }
            
            return model.Name.Sanitize().GetCsharpFriendlyName();
        }
    }

    public bool OmitCode
    {
        get
        {
            var model = GetModel();

            return GetParentModel() is TypeBaseViewModel vm && vm.Model is Interface || model.Abstract || model.Partial;
        }
    }
}
