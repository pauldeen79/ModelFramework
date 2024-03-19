namespace DatabaseFramework.TemplateFramework.Templates;

public class StoredProcedureTemplate : DatabaseObjectTemplateBase<StoredProcedureViewModel>
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
CREATE PROCEDURE [{Model.Schema}].[{Model.Name}]");

        RenderChildTemplatesByModel(Model.Parameters, builder);

        builder.AppendLine(@"AS
BEGIN");
        
        RenderChildTemplatesByModel(Model.Statements, builder);

        builder.AppendLine(@"END
GO");
    }
}
