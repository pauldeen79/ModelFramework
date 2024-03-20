namespace DatabaseFramework.TemplateFramework;

public sealed class DatabaseSchemaGenerator : DatabaseSchemaGeneratorBase<DatabaseSchemaGeneratorViewModel>, IMultipleContentBuilderTemplate, IStringBuilderTemplate
{
    public void Render(IMultipleContentBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);
        Guard.IsNotNull(Context);

        IGenerationEnvironment generationEnvironment = new MultipleContentBuilderEnvironment(builder);

        if (!Model.Settings.GenerateMultipleFiles)
        {
            // Generate a single generation environment, so we create only a single file in the multiple content builder environment.
            var singleStringBuilder = builder.AddContent(Context.DefaultFilename, Model.Settings.SkipWhenFileExists).Builder;
            generationEnvironment = new StringBuilderEnvironment(singleStringBuilder);
        }

        RenderSchemaHierarchy(generationEnvironment);
    }

    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);

        if (Model.Settings.GenerateMultipleFiles)
        {
            throw new NotSupportedException("Can't generate multiple files, because the generation environment has a single StringBuilder instance");
        }

        RenderSchemaHierarchy(new StringBuilderEnvironment(builder));
    }

    private void RenderSchemaHierarchy(IGenerationEnvironment generationEnvironment)
    {
        foreach (var schema in Model!.Schemas)
        {
            RenderChildTemplatesByModel(Model.GetDatabaseObjects(schema), generationEnvironment);
        }
    }}
