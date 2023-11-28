namespace ClassFramework.TemplateFramework.Templates;

public sealed class TypeBaseTemplate : CsharpClassGeneratorBase<TypeBaseViewModel>, IMultipleContentBuilderTemplate
{
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;

    public TypeBaseTemplate(ICsharpExpressionCreator csharpExpressionCreator)
    {
        Guard.IsNotNull(csharpExpressionCreator);
        _csharpExpressionCreator = csharpExpressionCreator;
    }
    
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

            generationEnvironment = new StringBuilderEnvironment(builder.Contents.Last().Builder); // important to take the last contents, in case of sub classes
        }
        else
        {
            var filename = $"{Model.Settings.FilenamePrefix}{Model.Data.Name}{Model.Settings.FilenameSuffix}.cs";
            var contentBuilder = builder.AddContent(filename, Model.Settings.SkipWhenFileExists);
            generationEnvironment = new StringBuilderEnvironment(contentBuilder.Builder);

            Context.Engine.RenderChildTemplate(
                new CodeGenerationHeaderViewModel(Model.Settings),
                generationEnvironment,
                Context,
                new TemplateByNameIdentifier(nameof(CodeGenerationHeaderTemplate)));
            Context.Engine.RenderChildTemplate(
                new UsingsViewModel(new[] { Model.Data }, Model.Settings),
                generationEnvironment,
                Context,
                new TemplateByNameIdentifier(nameof(UsingsTemplate)));

            if (Model.ShouldRenderNamespaceScope)
            {
                contentBuilder.Builder.AppendLine($"namespace {Model.Data.Namespace}");
                contentBuilder.Builder.AppendLine("{"); // start namespace
            }
        }

        generationEnvironment.Builder.AppendLineWithCondition("#nullable enable", Model.ShouldRenderNullablePragmas);

        foreach (var suppression in Model.Data.SuppressWarningCodes)
        {
            generationEnvironment.Builder.AppendLine($"#pragma warning disable {suppression}");
        }

        var indentedBuilder = new IndentedStringBuilder(generationEnvironment.Builder);
        PushIndent(indentedBuilder);

        Context.Engine.RenderCsharpChildTemplates(
            Model.Data.Attributes.Select(attribute => new AttributeViewModel(attribute, Model.Settings, _csharpExpressionCreator)),
            generationEnvironment,
            Context);

        indentedBuilder.AppendLine($"{Model.Data.GetModifiers()}{Model.GetContainerType()} {Model.GetName()}{Model.GetInheritedClasses()}");
        indentedBuilder.AppendLine("{"); // start class

        // Fields, Properties, Methods, Constructors, Enumerations
        Context.Engine.RenderCsharpChildTemplates(
            Model.GetMembers(),
            generationEnvironment,
            Context);

        var subClasses = (Model.Data as Class)?.SubClasses;
        if (subClasses is not null)
        {
            Context.Engine.RenderCsharpChildTemplates(
                subClasses.Select(typeBase => new TypeBaseViewModel(typeBase, Model.Settings.ForSubclasses())),
                generationEnvironment,
                Context);
        }

        indentedBuilder.AppendLine("}"); // end class

        PopIndent(indentedBuilder);

        generationEnvironment.Builder.AppendLineWithCondition("#nullable restore", Model.ShouldRenderNullablePragmas);

        foreach (var suppression in Model.Data.SuppressWarningCodes.Reverse())
        {
            generationEnvironment.Builder.AppendLine($"#pragma warning restore {suppression}");
        }

        generationEnvironment.Builder.AppendLineWithCondition("}", Model.ShouldRenderNamespaceScope); // end namespace
    }

    private void PushIndent(IndentedStringBuilder indentedBuilder)
    {
        for (int i = 0; i < Model!.Settings.IndentCount; i++)
        {
            indentedBuilder.IncrementIndent();
        }
    }

    private void PopIndent(IndentedStringBuilder indentedBuilder)
    {
        for (int i = 0; i < Model!.Settings.IndentCount; i++)
        {
            indentedBuilder.DecrementIndent();
        }
    }
}
