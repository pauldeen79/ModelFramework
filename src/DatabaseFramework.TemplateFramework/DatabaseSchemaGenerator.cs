namespace DatabaseFramework.TemplateFramework;

public sealed class DatabaseSchemaGenerator : DatabaseSchemaGeneratorBase<DatabaseSchemaGeneratorViewModel>/*, IMultipleContentBuilderTemplate*/, IStringBuilderTemplate
{
    //public void Render(IMultipleContentBuilder builder)
    //{
    //    Guard.IsNotNull(builder);
    //    Guard.IsNotNull(Model);
    //    Guard.IsNotNull(Context);

    //    StringBuilder? singleStringBuilder = null;
    //    IGenerationEnvironment generationEnvironment = new MultipleContentBuilderEnvironment(builder);

    //    if (!Model.Settings.GenerateMultipleFiles)
    //    {
    //        // Generate a single generation environment, so we create only a single file in the multiple content builder environment.
    //        singleStringBuilder = builder.AddContent(Context.DefaultFilename, skipWhenFileExists: false).Builder;
    //        generationEnvironment = new StringBuilderEnvironment(singleStringBuilder);
    //    }

    //    RenderSchemaHierarchy(generationEnvironment, singleStringBuilder);
    //}

    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);

        //if (Model.Settings.GenerateMultipleFiles)
        //{
        //    throw new NotSupportedException("Can't generate multiple files, because the generation environment has a single StringBuilder instance");
        //}

        var generationEnvironment = new StringBuilderEnvironment(builder);
        RenderSchemaHierarchy(generationEnvironment, builder);
    }

    private void RenderSchemaHierarchy(IGenerationEnvironment generationEnvironment, StringBuilder? singleStringBuilder)
    {
        foreach (var schema in Model!.Schemas)
        {
            //if (singleStringBuilder is not null && !string.IsNullOrEmpty(schema.Key))
            //{
            //    singleStringBuilder.AppendLine($"namespace {schema.Key}");
            //    singleStringBuilder.AppendLine("{"); // open namespace
            //}

            RenderChildTemplatesByModel(Model.GetDatabaseObjects(schema), generationEnvironment);

            //if (singleStringBuilder is not null && !string.IsNullOrEmpty(schema.Key))
            //{
            //    singleStringBuilder.AppendLine("}"); // close namespace
            //}
        }
    }}
