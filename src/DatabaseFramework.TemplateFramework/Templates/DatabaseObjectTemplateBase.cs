namespace DatabaseFramework.TemplateFramework.Templates;

public abstract class DatabaseObjectTemplateBase<T> : DatabaseSchemaGeneratorBase<T>, IMultipleContentBuilderTemplate, IStringBuilderTemplate
    where T : DatabaseSchemaGeneratorViewModelBase, INameContainer
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

        RenderDatabaseObject(generationEnvironment.Builder);
    }

    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);

        RenderDatabaseObject(builder);
    }

    protected abstract void RenderDatabaseObject(StringBuilder builder);
}
