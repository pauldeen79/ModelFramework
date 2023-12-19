namespace ClassFramework.TemplateFramework.Templates;

public class EnumerationTemplate : CsharpClassGeneratorBase<EnumerationViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);
        Guard.IsNotNull(Context);

        RenderChildTemplatesByModel(Model.GetAttributeModels(), builder);

        builder.Append(Model.CreateIndentation(1));
        builder.Append(Model.Modifiers);
        builder.Append("enum ");
        builder.AppendLine(Model.Name);
        builder.Append(Model.CreateIndentation(1));
        builder.AppendLine("{");

        RenderChildTemplatesByModel(Model.GetMemberModels(), builder);

        builder.Append(Model.CreateIndentation(1));
        builder.AppendLine("}");
    }
}
