namespace ClassFramework.TemplateFramework;

public sealed class CsharpClassGenerator : CsharpClassGeneratorBase<CsharpClassGeneratorViewModel>, IMultipleContentBuilderTemplate, IStringBuilderTemplate
{
    public CsharpClassGenerator(IViewModelFactory viewModelFactory) : base(viewModelFactory)
    {
    }

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
        Guard.IsNotNull(Model);

        RenderChildTemplateByModel(Model.GetCodeGenerationHeaderModel(), generationEnvironment);

        if (Context.IsRootContext)
        {
            RenderChildTemplateByModel(Model.GetUsingsModel(), generationEnvironment);
        }
    }

    private void RenderNamespaceHierarchy(IGenerationEnvironment generationEnvironment, StringBuilder? singleStringBuilder)
    {
        Guard.IsNotNull(Model);

        foreach (var @namespace in Model.Namespaces)
        {
            if (Context.IsRootContext && singleStringBuilder is not null && !string.IsNullOrEmpty(@namespace.Key))
            {
                singleStringBuilder.AppendLine($"namespace {@namespace.Key}");
                singleStringBuilder.AppendLine("{"); // open namespace
            }

            RenderChildTemplatesByModel(Model.GetTypeBaseModels(@namespace), generationEnvironment);

            if (Context.IsRootContext && singleStringBuilder is not null && !string.IsNullOrEmpty(@namespace.Key))
            {
                singleStringBuilder.AppendLine("}"); // close namespace
            }
        }
    }}
