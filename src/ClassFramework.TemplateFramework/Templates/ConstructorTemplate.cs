﻿namespace ClassFramework.TemplateFramework.Templates;

public class ConstructorTemplate : CsharpClassGeneratorBase<ConstructorViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);
        Guard.IsNotNull(Context);

        RenderChildTemplatesByModel(Model.GetAttributeModels(), builder);

        builder.Append(Model.CreateIndentation(1));
        builder.Append(Model.Modifiers);
        builder.Append(Model.Name);
        builder.Append("(");

        RenderChildTemplatesByModel(Model.GetParameterModels(), builder);

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
            RenderChildTemplatesByModel(Model.GetCodeStatementModels(), builder);
            builder.Append(Model.CreateIndentation(1));
            builder.AppendLine("}");
        }
    }
}
