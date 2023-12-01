﻿namespace ClassFramework.TemplateFramework.ViewModels;

public class ClassMethodViewModel : MethodViewModelBase<ClassMethod>
{
    public ClassMethodViewModel(CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator)
        : base(settings, csharpExpressionCreator)
    {
    }

    public bool ShouldRenderModifiers
        => string.IsNullOrEmpty(GetModel().ExplicitInterfaceName) && !(GetParentModel() is Interface);

    public string ReturnTypeName
        => GetModel().TypeName
            .GetCsharpFriendlyTypeName()
            .AppendNullableAnnotation(Model!.IsNullable, Settings.EnableNullableContext)
            .AbbreviateNamespaces(Model.Metadata.GetStringValues(MetadataNames.NamespaceToAbbreviate))
            .WhenNullOrEmpty("void");

    public string ExplicitInterfaceName
        => Model is not null && !string.IsNullOrEmpty(Model.ExplicitInterfaceName) && !(GetParentModel() is Interface)
            ? $"{Model!.ExplicitInterfaceName}."
            : string.Empty;

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

            return GetParentModel() is Interface || model.Abstract || model.Partial;
        }
    }
}
