namespace ClassFramework.TemplateFramework.ViewModels;

public class ClassMethodViewModel : MethodViewModelBase<ClassMethod>
{
    private readonly TypeBase _parent;

    public ClassMethodViewModel(ClassMethod data, CsharpClassGeneratorSettings settings, TypeBase parent, ICsharpExpressionCreator csharpExpressionCreator) : base(data, settings, csharpExpressionCreator)
    {
        Guard.IsNotNull(parent);

        _parent = parent;
    }

    public bool ShouldRenderModifiers
        => string.IsNullOrEmpty(Data.ExplicitInterfaceName) && !(_parent is Interface);

    public string ReturnTypeName => Data.TypeName
        .GetCsharpFriendlyTypeName()
        .AppendNullableAnnotation(Data.IsNullable, Settings.EnableNullableContext)
        .AbbreviateNamespaces(Data.Metadata.GetStringValues(MetadataNames.NamespaceToAbbreviate))
        .WhenNullOrEmpty("void");

    public string ExplicitInterfaceName
        => !string.IsNullOrEmpty(Data.ExplicitInterfaceName) && !(_parent is Interface)
            ? $"{Data.ExplicitInterfaceName}."
            : string.Empty;

    public string Name
    {
        get
        {
            if (Data.Operator)
            {
                return "operator " + Data.Name;
            }
            
            if (Data.IsInterfaceMethod())
            {
                return Data.Name.RemoveInterfacePrefix().Sanitize().GetCsharpFriendlyName();
            }
            
            return Data.Name.Sanitize().GetCsharpFriendlyName();
        }
    }

    public bool OmitCode => _parent is Interface || Data.Abstract || Data.Partial;
}
