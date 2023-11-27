namespace ClassFramework.TemplateFramework.Templates;

public sealed class AttributeTemplate : CsharpClassGeneratorBase<AttributeViewModel>, IStringBuilderTemplate
{
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;

    public AttributeTemplate(ICsharpExpressionCreator csharpExpressionCreator)
    {
        Guard.IsNotNull(csharpExpressionCreator);
        _csharpExpressionCreator = csharpExpressionCreator;
    }

    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);
        //Guard.IsNotNull(Context);

        builder.Append(@"    ");
        //builder.Append(Prefix);
        builder.Append(@"[");
        builder.Append(Model.Data.Name);
        if (Model.ShouldRenderParameters)
        {

            builder.Append(@"(");
            builder.Append(Model.GetParametersText(_csharpExpressionCreator));
            builder.Append(@")");
        }

        builder.AppendLine(@"]");
    }

    //private string Prefix
    //    => !(Context.ParentContext != null && Context.ParentContext.Model is TypeBase)
    //        ? "    "
    //        : string.Empty;
}
