namespace ClassFramework.TemplateFramework.Templates;

public sealed class TypeBaseTemplate : CsharpClassGeneratorBase<TypeBaseViewModel>, IMultipleContentBuilderTemplate
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

            generationEnvironment = new StringBuilderEnvironment(builder.Contents.Last().Builder); // important to take the last contents, in case of sub classes
        }
        else
        {
            var filename = $"{Model.Settings.FilenamePrefix}{Model.Data.Name}{Model.Settings.FilenameSuffix}.cs";
            var contentBuilder = builder.AddContent(filename, Model.Settings.SkipWhenFileExists);
            generationEnvironment = new StringBuilderEnvironment(contentBuilder.Builder);

            Context.Engine.RenderChildTemplateByModel(Model.GetCodeGenerationHeaderModel(), generationEnvironment, Context);
            Context.Engine.RenderChildTemplateByModel(Model.GetUsingsModel(), generationEnvironment, Context);

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

        Context.Engine.RenderChildTemplatesByModel(Model.GetAttributeModels(), generationEnvironment, Context);

        indentedBuilder.AppendLine($"{Model.Data.GetModifiers()}{Model.GetContainerType()} {Model.Name}{Model.GetInheritedClasses()}");
        indentedBuilder.AppendLine("{"); // start class

        // Fields, Properties, Methods, Constructors, Enumerations
        Context.Engine.RenderChildTemplatesByModel(Model.GetMemberModels(), generationEnvironment, Context);

        // Subclasses
        Context.Engine.RenderChildTemplatesByModel(Model.GetSubClassModels(), generationEnvironment, Context);

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
        Guard.IsNotNull(Model);

        for (int i = 0; i < Model.Settings.IndentCount; i++)
        {
            indentedBuilder.IncrementIndent();
        }
    }

    private void PopIndent(IndentedStringBuilder indentedBuilder)
    {
        Guard.IsNotNull(Model);

        for (int i = 0; i < Model.Settings.IndentCount; i++)
        {
            indentedBuilder.DecrementIndent();
        }
    }
}
