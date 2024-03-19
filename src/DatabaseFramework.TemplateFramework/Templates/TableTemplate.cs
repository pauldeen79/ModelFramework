namespace DatabaseFramework.TemplateFramework.Templates;

public sealed class TableTemplate : DatabaseObjectTemplateBase<TableViewModel>
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
SET ANSI_PADDING ON
GO
CREATE TABLE [{Model.Schema}].[{Model.Name}](");

        var fieldsAndPrimaryKeyConstraints = Model.Fields.Cast<object>()
            .Concat(Model.PrimaryKeyConstraints.Cast<object>())
            .Concat(Model.UniqueConstraints.Cast<object>())
            .Concat(Model.CheckConstraints.Cast<object>());

        RenderChildTemplatesByModel(fieldsAndPrimaryKeyConstraints, builder);

        builder.AppendLine(@$") ON [{Model.FileGroupName}]
GO
SET ANSI_PADDING OFF
GO");

        RenderChildTemplatesByModel(Model.Indexes, builder);
        RenderChildTemplatesByModel(Model.DefaultValueConstraints, builder);
    }
}
