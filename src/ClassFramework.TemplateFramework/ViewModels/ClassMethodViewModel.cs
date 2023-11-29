namespace ClassFramework.TemplateFramework.ViewModels;

public class ClassMethodViewModel : CsharpClassGeneratorViewModel<ClassMethod>
{
    private readonly TypeBase _parent;
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;

    public ClassMethodViewModel(ClassMethod data, CsharpClassGeneratorSettings settings, TypeBase parent, ICsharpExpressionCreator csharpExpressionCreator) : base(data, settings)
    {
        Guard.IsNotNull(parent);
        Guard.IsNotNull(csharpExpressionCreator);

        _parent = parent;
        _csharpExpressionCreator = csharpExpressionCreator;
    }

    public bool ShouldRenderModifiers => string.IsNullOrEmpty(Data.ExplicitInterfaceName) && !(_parent is Interface);

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

    public IEnumerable<CsharpClassGeneratorViewModel> GetCodeStatementModels()
        => Data.CodeStatements
            .Select(codeStatement => new CodeStatementViewModel(codeStatement, Settings))
            .SelectMany((item, index) => index + 1 < Data.Parameters.Count ? [item, new NewLineViewModel(Settings)] : new CsharpClassGeneratorViewModel[] { item });

    public IEnumerable<CsharpClassGeneratorViewModel> GetParameterModels()
        => Data.Parameters
            .Select(parameter => new ParameterViewModel(parameter, Settings, _csharpExpressionCreator))
            .SelectMany((item, index) => index + 1 < Data.Parameters.Count ? [item, new SpaceAndCommaViewModel(Settings)] : new CsharpClassGeneratorViewModel[] { item });

    public IEnumerable<CsharpClassGeneratorViewModel> GetAttributeModels()
        => Data.Attributes.Select(attribute => new AttributeViewModel(attribute, Settings, _csharpExpressionCreator, Data));
}
