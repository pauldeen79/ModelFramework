namespace ClassFramework.TemplateFramework.Templates;

public sealed class AttributeTemplate : CsharpClassGeneratorBase<AttributeViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);
        Guard.IsNotNull(Context);

        for (int i = 1; i <= Model.Settings.IndentCount; i++)
        {
            builder.Append(@"    ");
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

        builder.AppendLine(@"]");
    }

    public string GetPrefix()
    {
        // Hacking here... If the parent context has iterations, then the model is wrapped in an anomyoust type (together with the index).
        // In this case, we need to get the Model property of the anonymous model instance.
        var model = Context.ParentContext?.Model;
        if (model is not null && model.GetType().FullName?.StartsWith("<>f__AnonymousType", StringComparison.Ordinal) == true)
        {
            var property = model.GetType().GetProperty("Model");
            if (property is not null)
            {
                model = property.GetValue(model);
            }
        }

        return model is TypeBaseViewModel
                ? string.Empty
                : "    ";
    }
}
