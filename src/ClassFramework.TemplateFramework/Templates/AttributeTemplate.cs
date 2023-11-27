namespace ClassFramework.TemplateFramework.Templates;

public sealed class AttributeTemplate : CsharpClassGeneratorBase<CsharpClassGeneratorViewModel<Domain.Attribute>>, IStringBuilderTemplate
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
        Guard.IsNotNull(Context);

        builder.Append(@"    ");
        //builder.Append(Prefix);
        builder.Append(@"[");
        builder.Append(Model.Data.Name);
        if (ShouldRenderParameters)
        {

            builder.Append(@"(");
            builder.Append(GetParametersText());
            builder.Append(@")");
        }

        builder.AppendLine(@"]");
    }

    //private string Prefix
    //    => !(Context.ParentContext != null && Context.ParentContext.Model is TypeBase)
    //        ? "    "
    //        : string.Empty;

    private bool ShouldRenderParameters => Model!.Data.Parameters is not null && Model.Data.Parameters.Count > 0;
    
    private string GetParametersText()
        => string.Join(", ", Model!.Data.Parameters.Select(p =>
            string.IsNullOrEmpty(p.Name)
                ? _csharpExpressionCreator.Create(p.Value)
                : string.Format("{0} = {1}", p.Name, _csharpExpressionCreator.Create(p.Value))
        ));
}
