namespace ClassFramework.TemplateFramework.Templates;

public sealed class TypeBaseTemplate : CsharpClassGeneratorBase<CsharpClassGeneratorViewModel<TypeBase>>, IMultipleContentBuilderTemplate
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

            Context.Engine.RenderChildTemplate(
                Model.Settings,
                generationEnvironment,
                Context,
                new TemplateByNameIdentifier("CodeGenerationHeader")
                );
            Context.Engine.RenderChildTemplate(
                new CsharpClassGeneratorViewModel<IEnumerable<TypeBase>>(new[] { Model.Data }, Model.Settings),
                generationEnvironment,
                Context,
                new TemplateByNameIdentifier("Usings")
                );

            if (!string.IsNullOrEmpty(Model.Data.Namespace))
            {
                contentBuilder.Builder.AppendLine(Model.Settings.CultureInfo, $"namespace {Model.Data.Namespace}");
                contentBuilder.Builder.AppendLine("{"); // start namespace
            }
        }

        generationEnvironment.Builder.AppendLineWithCondition("#nullable enable", Model.Settings.EnableNullableContext && Model.Settings.IndentCount == 1);

        foreach (var suppression in Model.Data.SuppressWarningCodes)
        {
            generationEnvironment.Builder.AppendLine($"#pragma warning disable {suppression}");
        }

        var indentedBuilder = new IndentedStringBuilder(generationEnvironment.Builder);
        PushIndent(indentedBuilder);

        //TODO: Render attributes
        indentedBuilder.AppendLine($"{Model.Data.GetModifiers()}{Model.Data.GetContainerType()} {Model.Data.Name}");
        indentedBuilder.AppendLine("{"); // start class

        //TODO: Render child items (properties, fields, constructors)

        var subClasses = (Model.Data as Class)?.SubClasses;
        if (subClasses is not null && subClasses.Count > 0)
        {
            Context.Engine.RenderChildTemplates(
                subClasses.Select(typeBase => new CsharpClassGeneratorViewModel<TypeBase>(typeBase, Model.Settings.ForSubclasses())),
                new MultipleContentBuilderEnvironment(builder),
                Context,
                model => new TemplateByModelIdentifier(model!.GetType().GetProperty(nameof(CsharpClassGeneratorViewModel<TypeBase>.Data))!.GetValue(model))
                );
        }

        indentedBuilder.AppendLine("}"); // end class

        PopIndent(indentedBuilder);

        generationEnvironment.Builder.AppendLineWithCondition("#nullable restore", Model.Settings.EnableNullableContext && Model.Settings.IndentCount == 1);

        foreach (var suppression in Model.Data.SuppressWarningCodes.Reverse())
        {
            generationEnvironment.Builder.AppendLine($"#pragma warning restore {suppression}");
        }

        generationEnvironment.Builder.AppendLineWithCondition("}", Model.Settings.GenerateMultipleFiles && !string.IsNullOrEmpty(Model.Data.Namespace)); // end namespace
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
