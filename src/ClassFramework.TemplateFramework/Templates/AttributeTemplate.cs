namespace ClassFramework.TemplateFramework.Templates;

public sealed class AttributeTemplate : CsharpClassGeneratorBase<AttributeViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);
        Guard.IsNotNull(Context);

        if (!Model.ParentIsParameter)
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

        if (!Model.ParentIsParameter)
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

        if (Model.ParentIsParameter)
        {
            return string.Empty;
        }

        //TODO: Revief we can simply check the parent on the Model, as it is already injected there
        var model = Context.ParentContext?.GetUnderlyingModel();

        return model is TypeBaseViewModel
            ? string.Empty
            : "    ";
    }
}
