namespace DatabaseFramework.TemplateFramework.Templates;

public class ViewTemplate : DatabaseObjectTemplateBase<ViewViewModel>
{
    protected override void RenderDatabaseObject(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);

        RenderChildTemplateByModel(Model.CodeGenerationHeaders, builder);

        builder.AppendLine(@$"SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [{Model.Schema}].[{Model.Name}]
AS");

        builder.AppendLine(Model.Definition);
        builder.AppendLine("GO");
    }
}
