namespace ClassFramework.TemplateFramework;

public sealed class CsharpClassGenerator : CsharpClassGeneratorBase<CsharpClassGeneratorViewModel<IEnumerable<TypeBase>>>, IMultipleContentBuilderTemplate, IStringBuilderTemplate
{
    public void Render(IMultipleContentBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);
        Guard.IsNotNull(Context);

        StringBuilder? singleStringBuilder = null;
        IGenerationEnvironment generationEnvironment = new MultipleContentBuilderEnvironment(builder);

        if (!Model.Settings.GenerateMultipleFiles)
        {
            // Generate a single generation environment, so we create only a single file in the multiple content builder environment.
            singleStringBuilder = builder.AddContent(Context.DefaultFilename, Model.Settings.SkipWhenFileExists).Builder;
            generationEnvironment = new StringBuilderEnvironment(singleStringBuilder);
            RenderHeader(generationEnvironment);
        }

        RenderNamespaceHierarchy(generationEnvironment, singleStringBuilder);
    }

    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);

        if (Model.Settings.GenerateMultipleFiles)
        {
            throw new NotSupportedException("Can't generate multiple files, because the generation environment has a single StringBuilder instance");
        }

        var generationEnvironment = new StringBuilderEnvironment(builder);
        RenderHeader(generationEnvironment);
        RenderNamespaceHierarchy(generationEnvironment, builder);
    }

    private void RenderHeader(IGenerationEnvironment generationEnvironment)
    {
        Context.Engine.RenderChildTemplate(
            Model!.Settings,
            generationEnvironment,
            Context,
            new TemplateByNameIdentifier("CodeGenerationHeader"));

        if (Context.IsRootContext)
        {
            Context.Engine.RenderChildTemplate(
                Model,
                generationEnvironment,
                Context,
                new TemplateByNameIdentifier("Usings"));
        }
    }

    private void RenderNamespaceHierarchy(IGenerationEnvironment generationEnvironment, StringBuilder? singleStringBuilder)
    {
        foreach (var ns in Model!.Data.GroupBy(x => x.Namespace).OrderBy(x => x.Key))
        {
            if (Context.IsRootContext && singleStringBuilder is not null && !string.IsNullOrEmpty(ns.Key))
            {
                singleStringBuilder.AppendLine(Model.Settings.CultureInfo, $"namespace {ns.Key}");
                singleStringBuilder.AppendLine("{"); // open namespace
            }

            var typeBaseItems = ns
                .OrderBy(typeBase => typeBase.Name)
                .Select(typeBase => new CsharpClassGeneratorViewModel<TypeBase>(typeBase, Model.Settings));

            Context.Engine.RenderCsharpChildTemplates(
                typeBaseItems,
                generationEnvironment,
                Context);

            if (Context.IsRootContext && singleStringBuilder is not null && !string.IsNullOrEmpty(ns.Key))
            {
                singleStringBuilder.AppendLine("}"); // close namespace
            }
        }
    }
}
