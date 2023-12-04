namespace ClassFramework.TemplateFramework.Templates;

public class ClassPropertyTemplate : CsharpClassGeneratorBase<ClassPropertyViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);

        RenderChildTemplatesByModel(Model.GetAttributeModels(), builder);

        builder.Append(Model.CreateIndentation(1));

        if (Model.ShouldRenderModifiers)
        {
            builder.Append(Model.Modifiers);
        }

        builder.Append(Model.ExplicitInterfaceName);
        builder.Append(Model.TypeName);
        builder.Append(" ");
        builder.AppendLine(Model.Name);

        builder.Append(Model.CreateIndentation(1));
        builder.AppendLine("{");

        RenderCodeStatements(builder, Model.HasGetter, "get", Model.GetterModifiers, Model.OmitGetterCode, Model.GetGetterCodeStatementModels());
        RenderCodeStatements(builder, Model.HasInitializer, "init", Model.InitializerModifiers, Model.OmitInitializerCode, Model.GetInitializerCodeStatementModels());
        RenderCodeStatements(builder, Model.HasSetter, "set", Model.SetterModifiers, Model.OmitSetterCode, Model.GetSetterCodeStatementModels());

        builder.Append(Model.CreateIndentation(1));
        builder.AppendLine("}");
    }

    private void RenderCodeStatements(StringBuilder builder, bool isAvailable, string verb, string modifiers, bool omitCode, IEnumerable codeStatementModels)
    {
        if (!isAvailable)
        {
            return;
        }

        builder.Append(Model!.CreateIndentation(2));
        builder.Append(modifiers);
        builder.Append(verb);
        if (omitCode)
        {
            builder.AppendLine(";");
        }
        else
        {
            builder.AppendLine();
            builder.Append(Model.CreateIndentation(2));
            builder.AppendLine("{");
            RenderChildTemplatesByModel(codeStatementModels, builder);
            builder.Append(Model.CreateIndentation(2));
            builder.AppendLine("}");
        }
    }
}
