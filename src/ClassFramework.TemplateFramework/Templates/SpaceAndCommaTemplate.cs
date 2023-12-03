namespace ClassFramework.TemplateFramework.Templates;

public class SpaceAndCommaTemplate : CsharpClassGeneratorBase<NewLineViewModel>, IStringBuilderTemplate
{
    public SpaceAndCommaTemplate(IViewModelFactory viewModelFactory) : base(viewModelFactory)
    {
    }

    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);

        builder.Append(", ");
    }
}
