namespace ClassFramework.TemplateFramework.Templates;

public class ClassMethodTemplate : CsharpClassGeneratorBase<ClassMethodViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);
        Guard.IsNotNull(Context);

        Context.Engine.RenderChildTemplatesByModel(Model.GetAttributeModels(), new StringBuilderEnvironment(builder), Context);

        builder.Append(Model.CreateIndentation(1));
        
        if (Model.ShouldRenderModifiers)
        {
            builder.Append(Model.Modifiers);
        }

        builder.Append(Model.ReturnTypeName);
        builder.Append(" ");
        builder.Append(Model.ExplicitInterfaceName);
        builder.Append(Model.Name);
        builder.Append(Model.GenericTypeArguments);
        builder.Append("(");

        if (Model.ExtensionMethod)
        {
            builder.Append("this ");
        }

        Context.Engine.RenderChildTemplatesByModel(Model.GetParameterModels(), new StringBuilderEnvironment(builder), Context);

        builder.Append(")");

        if (Model.OmitCode)
        {
            builder.AppendLine(";");
        }
        else
        {
            builder.AppendLine();
            builder.Append(Model.CreateIndentation(1));
            builder.AppendLine("{");
            Context.Engine.RenderChildTemplatesByModel(Model.GetCodeStatementModels(), new StringBuilderEnvironment(builder), Context);
            builder.Append(Model.CreateIndentation(1));
            builder.AppendLine("}");
        }
    }
}
