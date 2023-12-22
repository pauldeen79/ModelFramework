﻿namespace ClassFramework.TemplateFramework.Templates;

public sealed class TypeTemplate : CsharpClassGeneratorBase<TypeViewModel>, IMultipleContentBuilderTemplate, IStringBuilderTemplate
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
            var filename = $"{Model.FilenamePrefix}{Model.Name}{Model.Settings.FilenameSuffix}.cs";
            var contentBuilder = builder.AddContent(filename, Model.Settings.SkipWhenFileExists);
            generationEnvironment = new StringBuilderEnvironment(contentBuilder.Builder);
            RenderChildTemplateByModel(Model!.GetCodeGenerationHeaderModel(), generationEnvironment);
            if (!Model.Settings.EnableGlobalUsings)
            {
                RenderChildTemplateByModel(Model.GetUsingsModel(), generationEnvironment);
            }

            if (Model.ShouldRenderNamespaceScope)
            {
                generationEnvironment.Builder.AppendLine($"namespace {Model.Namespace}");
                generationEnvironment.Builder.AppendLine("{"); // start namespace
            }
        }

        RenderTypeBase(generationEnvironment);

        generationEnvironment.Builder.AppendLineWithCondition("}", Model.ShouldRenderNamespaceScope); // end namespace
    }

    public void Render(StringBuilder builder)
    {
        var generationEnvironment = new StringBuilderEnvironment(builder);
        RenderTypeBase(generationEnvironment);
    }

    private void RenderTypeBase(StringBuilderEnvironment generationEnvironment)
    {
        generationEnvironment.Builder.AppendLineWithCondition("#nullable enable", Model!.ShouldRenderNullablePragmas);

        foreach (var suppression in Model.SuppressWarningCodes)
        {
            generationEnvironment.Builder.AppendLine($"#pragma warning disable {suppression}");
        }

        var indentedBuilder = new IndentedStringBuilder(generationEnvironment.Builder);
        PushIndent(indentedBuilder);

        RenderChildTemplatesByModel(Model.GetAttributeModels(), generationEnvironment);

        indentedBuilder.AppendLine($"{Model.Modifiers}{Model.ContainerType} {Model.Name}{Model.InheritedClasses}");
        indentedBuilder.AppendLine("{"); // start class

        // Fields, Properties, Methods, Constructors, Enumerations
        RenderChildTemplatesByModel(Model.GetMemberModels(), generationEnvironment);

        // Subclasses
        RenderChildTemplatesByModel(Model.GetSubClassModels(), generationEnvironment);

        indentedBuilder.AppendLine("}"); // end class

        PopIndent(indentedBuilder);

        generationEnvironment.Builder.AppendLineWithCondition("#nullable restore", Model.ShouldRenderNullablePragmas);

        foreach (var suppression in Model.SuppressWarningCodes.Reverse())
        {
            generationEnvironment.Builder.AppendLine($"#pragma warning restore {suppression}");
        }
    }

    private void PushIndent(IndentedStringBuilder indentedBuilder)
    {
        Guard.IsNotNull(Model);

        for (int i = 0; i < Context.GetIndentCount(); i++)
        {
            indentedBuilder.IncrementIndent();
        }
    }

    private void PopIndent(IndentedStringBuilder indentedBuilder)
    {
        Guard.IsNotNull(Model);

        for (int i = 0; i < Context.GetIndentCount(); i++)
        {
            indentedBuilder.DecrementIndent();
        }
    }
}
