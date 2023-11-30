namespace ClassFramework.TemplateFramework.Templates;

public class ParameterTemplate : CsharpClassGeneratorBase<ParameterViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);
        Guard.IsNotNull(Context);

        Context.Engine.RenderChildTemplatesByModel(Model.GetAttributeModels(), new StringBuilderEnvironment(builder), Context);

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
