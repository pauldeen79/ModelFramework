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
            for (int i = 0; i < Model.Settings.IndentCount; i++)
            {
                builder.Append(@"    ");
            }
        }

        builder.Append(GetPrefix());
        builder.Append(@"[");
        builder.Append(Model.Data.Name);

        if (Model.ShouldRenderParameters)
        {

            builder.Append(@"(");
            builder.Append(Model.GetParametersText());
            builder.Append(@")");
        }

        builder.Append(@"]");

        if (!Model.IsSingleLineAttributeContainer)
        {
            builder.AppendLine();
        }
        else
        {
            builder.Append(" ");
        }
    }

    public string GetPrefix()
    {
        Guard.IsNotNull(Model);
        Guard.IsNotNull(Context);

        if (Model.IsSingleLineAttributeContainer)
        {
            return string.Empty;
        }

        return Model.Parent is TypeBase
            ? string.Empty
            : "    ";
    }
}
