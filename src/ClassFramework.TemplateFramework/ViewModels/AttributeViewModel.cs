namespace ClassFramework.TemplateFramework.ViewModels;

public class AttributeViewModel : CsharpClassGeneratorViewModel<Domain.Attribute>
{
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;
    private readonly IAttributesContainer _parent;

    public AttributeViewModel(Domain.Attribute data, CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator, IAttributesContainer parent) : base(data, settings)
    {
        Guard.IsNotNull(csharpExpressionCreator);
        Guard.IsNotNull(parent);
        _csharpExpressionCreator = csharpExpressionCreator;
        _parent = parent;
    }

    public bool ShouldRenderParameters => Data.Parameters is not null && Data.Parameters.Count > 0;

    public bool ParentIsParameter => _parent is Parameter;

    public string GetParametersText()
        => string.Join(", ", Data.Parameters.Select(p =>
            string.IsNullOrEmpty(p.Name)
                ? _csharpExpressionCreator.Create(p.Value)
                : string.Format("{0} = {1}", p.Name, _csharpExpressionCreator.Create(p.Value))
        ));
}
