namespace ClassFramework.TemplateFramework.Templates;

public class ClassConstructorTemplate : CsharpClassGeneratorBase<ClassConstructorViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);
        Guard.IsNotNull(Context);

        Context.Engine.RenderCsharpChildTemplates(Model.GetAttributeModels(), new StringBuilderEnvironment(builder), Context);

        builder.Append($"        {Model.Data.GetModifiers()}{Model.Name}(");

        Context.Engine.RenderCsharpChildTemplates(Model.GetParameterModels(), new StringBuilderEnvironment(builder), Context);

        builder.Append($"){Model.ChainCall}");

        if (Model.OmitCode)
        {
            builder.AppendLine(";");
        }
        else
        {
            builder.AppendLine();
            builder.AppendLine("        {");
            Context.Engine.RenderCsharpChildTemplates(Model.GetCodeStatementModels(), new StringBuilderEnvironment(builder), Context);
            builder.AppendLine("        }");
        }
    }
}
