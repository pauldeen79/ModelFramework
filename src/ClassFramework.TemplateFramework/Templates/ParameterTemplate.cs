namespace ClassFramework.TemplateFramework.Templates;

public class ParameterTemplate : CsharpClassGeneratorBase<ParameterViewModel>, IStringBuilderTemplate
{
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;

    public ParameterTemplate(ICsharpExpressionCreator csharpExpressionCreator)
    {
        Guard.IsNotNull(csharpExpressionCreator);
        _csharpExpressionCreator = csharpExpressionCreator;
    }

    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);
        Guard.IsNotNull(Context);

        var attributes = Model.Data.Attributes.Select(attribute => new AttributeViewModel(attribute, Model.Settings, _csharpExpressionCreator, Model.Data));
        Context.Engine.RenderChildTemplatesByModel(attributes, new StringBuilderEnvironment(builder), Context);

        builder.AppendWithCondition("params ", Model.Data.IsParamArray);
        builder.AppendWithCondition("ref ", Model.Data.IsRef);
        builder.AppendWithCondition("out ", Model.Data.IsOut);
        builder.Append(Model.TypeName);
        builder.Append(" ");
        builder.Append(Model.Name);

        if (Model.ShouldRenderDefaultValue)
        {
            builder.Append(" = ");
            builder.Append(Model.GetDefaultValueExpression());
        }
    }
}
