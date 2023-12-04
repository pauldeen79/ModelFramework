﻿namespace ClassFramework.TemplateFramework.Templates;

public class PropertyCodeBodyTemplate : CsharpClassGeneratorBase<PropertyCodeBodyViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(Model);
        Guard.IsNotNull(builder);

        if (!Model.IsAvailable)
        {
            return;
        }

        builder.Append(Model.CreateIndentation(2));
        builder.Append(Model.Modifiers);
        builder.Append(Model.Verb);
        if (Model.OmitCode)
        {
            builder.AppendLine(";");
        }
        else
        {
            builder.AppendLine();
            builder.Append(Model.CreateIndentation(2));
            builder.AppendLine("{");
            RenderChildTemplatesByModel(Model.CodeStatementModels, builder);
            builder.Append(Model.CreateIndentation(2));
            builder.AppendLine("}");
        }
    }
}
