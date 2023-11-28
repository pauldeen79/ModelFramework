namespace ClassFramework.TemplateFramework.Templates;

public class ClassConstructorTemplate : CsharpClassGeneratorBase<ClassConstructorViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);
        Guard.IsNotNull(Context);

        Context.Engine.RenderCsharpChildTemplates(Model.GetAttributeModels(), new StringBuilderEnvironment(builder), Context);

        builder.Append(Model.CreateIndentation(1));
        builder.Append(Model.Data.GetModifiers());
        builder.Append(Model.Name);
        builder.Append("(");

        Context.Engine.RenderCsharpChildTemplates(Model.GetParameterModels(), new StringBuilderEnvironment(builder), Context);

        builder.Append(")");
        builder.Append(Model.ChainCall);

        if (Model.OmitCode)
        {
            builder.AppendLine(";");
        }
        else
        {
            builder.AppendLine();
            builder.Append(Model.CreateIndentation(1));
            builder.AppendLine("{");
            Context.Engine.RenderCsharpChildTemplates(Model.GetCodeStatementModels(), new StringBuilderEnvironment(builder), Context);
            builder.Append(Model.CreateIndentation(1));
            builder.AppendLine("}");
        }
    }
}
