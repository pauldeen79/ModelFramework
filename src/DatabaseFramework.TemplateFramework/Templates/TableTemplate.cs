namespace DatabaseFramework.TemplateFramework.Templates;

public sealed class TableTemplate : DatabaseSchemaGeneratorBase<TableViewModel>, IMultipleContentBuilderTemplate, IStringBuilderTemplate
{
    public void Render(IMultipleContentBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);
        Guard.IsNotNull(Context);

        StringBuilderEnvironment generationEnvironment;

        if (!Model.Settings.GenerateMultipleFiles)
        {
            if (!builder.Contents.Any())
            {
                builder.AddContent(Context.DefaultFilename, Model.Settings.SkipWhenFileExists);
            }

            generationEnvironment = new StringBuilderEnvironment(builder.Contents.Last().Builder);
        }
        else
        {
            var filename = $"{Model.FilenamePrefix}{Model.Name}{Model.Settings.FilenameSuffix}.cs";
            var contentBuilder = builder.AddContent(filename, Model.Settings.SkipWhenFileExists);
            generationEnvironment = new StringBuilderEnvironment(contentBuilder.Builder);
        }

        RenderTable(generationEnvironment);
    }

    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);

        var generationEnvironment = new StringBuilderEnvironment(builder);
        RenderTable(generationEnvironment);
    }

    private void RenderTable(StringBuilderEnvironment generationEnvironment)
    {
        RenderChildTemplateByModel(Model!.CodeGenerationHeaders, generationEnvironment);
        generationEnvironment.Builder.AppendLine(@$"SET ANSI_NULLS ON
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

        RenderChildTemplatesByModel(fieldsAndPrimaryKeyConstraints, generationEnvironment);

        generationEnvironment.Builder.AppendLine(@$") ON [{Model.FileGroupName}]
GO
SET ANSI_PADDING OFF
GO");

        RenderChildTemplatesByModel(Model.Indexes, generationEnvironment);
        RenderChildTemplatesByModel(Model.DefaultValueConstraints, generationEnvironment);
    }
}
