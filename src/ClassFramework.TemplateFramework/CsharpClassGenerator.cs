namespace ClassFramework.TemplateFramework;

public sealed class CsharpClassGenerator : CsharpClassGeneratorBase<CsharpClassGeneratorViewModel<IEnumerable<TypeBase>>>, IMultipleContentBuilderTemplate, IStringBuilderTemplate
{
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;

    public CsharpClassGenerator(ICsharpExpressionCreator csharpExpressionCreator)
    {
        Guard.IsNotNull(csharpExpressionCreator);
        _csharpExpressionCreator = csharpExpressionCreator;
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

        Context.Engine.RenderChildTemplateByModel(new CodeGenerationHeaderViewModel(Model.Settings), generationEnvironment, Context);

        if (Context.IsRootContext)
        {
            Context.Engine.RenderChildTemplateByModel(new UsingsViewModel(Model.Data, Model.Settings, _csharpExpressionCreator), generationEnvironment, Context);
        }
    }

    private void RenderNamespaceHierarchy(IGenerationEnvironment generationEnvironment, StringBuilder? singleStringBuilder)
    {
        Guard.IsNotNull(Model);

        foreach (var ns in Model.Data.GroupBy(x => x.Namespace).OrderBy(x => x.Key))
        {
            if (Context.IsRootContext && singleStringBuilder is not null && !string.IsNullOrEmpty(ns.Key))
            {
                singleStringBuilder.AppendLine($"namespace {ns.Key}");
                singleStringBuilder.AppendLine("{"); // open namespace
            }

            var typeBaseItems = ns
                .OrderBy(typeBase => typeBase.Name)
                .Select(typeBase => new TypeBaseViewModel(typeBase, Model.Settings, _csharpExpressionCreator));

            Context.Engine.RenderChildTemplatesByModel(typeBaseItems, generationEnvironment, Context);

            if (Context.IsRootContext && singleStringBuilder is not null && !string.IsNullOrEmpty(ns.Key))
            {
                singleStringBuilder.AppendLine("}"); // close namespace
            }
        }
    }
}
