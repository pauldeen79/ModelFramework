namespace ClassFramework.TemplateFramework.Templates;

public class ClassFieldTemplate : CsharpClassGeneratorBase<ClassFieldViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);
        Guard.IsNotNull(Context);

        Context.Engine.RenderChildTemplatesByModel(Model.GetAttributeModels(), new StringBuilderEnvironment(builder), Context);

        builder.Append(Model.CreateIndentation(1));
        builder.Append(Model.GetModel().GetModifiers());
        
        if (Model.Model!.Event)
        {
            builder.Append("event ");
        }
        
        builder.Append($"{Model.TypeName} {Model.Name}");
        
        if (Model.ShouldRenderDefaultValue)
        {
            builder.Append(" = ");
            builder.Append(Model.GetDefaultValueExpression());
        }
        
        builder.AppendLine(";");
    }
}
