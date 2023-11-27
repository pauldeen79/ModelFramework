namespace ClassFramework.TemplateFramework.ViewModels;

public class AttributeViewModel : CsharpClassGeneratorViewModel<Domain.Attribute>
{
    public AttributeViewModel(Domain.Attribute data, CsharpClassGeneratorSettings settings) : base(data, settings)
    {
    }

    public bool ShouldRenderParameters => Data.Parameters is not null && Data.Parameters.Count > 0;

    public string GetParametersText(ICsharpExpressionCreator csharpExpressionCreator)
        => string.Join(", ", Data.Parameters.Select(p =>
            string.IsNullOrEmpty(p.Name)
                ? csharpExpressionCreator.Create(p.Value)
                : string.Format("{0} = {1}", p.Name, csharpExpressionCreator.Create(p.Value))
        ));
}
