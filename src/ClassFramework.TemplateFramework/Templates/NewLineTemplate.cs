namespace ClassFramework.TemplateFramework.Templates;

public class NewLineTemplate : CsharpClassGeneratorBase<NewLineViewModel>, IStringBuilderTemplate
{
    public NewLineTemplate(IViewModelFactory viewModelFactory) : base(viewModelFactory)
    {
    }

    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);

        builder.AppendLine();
    }
}
