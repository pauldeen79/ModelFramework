namespace ClassFramework.TemplateFramework.Templates;

public sealed class AttributeTemplate : CsharpClassGeneratorBase<AttributeViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);
        Guard.IsNotNull(Context);

        var isSingleLineAttributeContainer = Context.ParentContext?.Model is ParameterViewModel;

        if (!isSingleLineAttributeContainer)
        {
            builder.Append(Model.CreateIndentation(GetAdditionalIndents()));
        }

        builder.Append("[");
        builder.Append(Model.Data.Name);
        builder.Append(Model.GetParametersText());
        builder.Append("]");

        if (!isSingleLineAttributeContainer)
        {
            builder.AppendLine();
        }
        else
        {
            builder.Append(" ");
        }
    }

    public int GetAdditionalIndents()
    {
        Guard.IsNotNull(Model);
        Guard.IsNotNull(Context);

        return Context.ParentContext?.Model switch
        {
            ParameterViewModel or TypeBaseViewModel => 0,
            _ => 1
        };
    }
}
