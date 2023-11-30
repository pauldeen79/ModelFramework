namespace ClassFramework.TemplateFramework.Templates;

public sealed class AttributeTemplate : CsharpClassGeneratorBase<AttributeViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);
        Guard.IsNotNull(Context);

        if (!Model.IsSingleLineAttributeContainer)
        {
            builder.Append(Model.CreateIndentation(Model.GetAdditionalIndents()));
        }

        builder.Append("[");
        builder.Append(Model.Data.Name);
        builder.Append(Model.GetParametersText());
        builder.Append("]");

        if (!Model.IsSingleLineAttributeContainer)
        {
            builder.AppendLine();
        }
        else
        {
            builder.Append(" ");
        }
    }
}
