namespace ClassFramework.TemplateFramework;

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
                new TemplateByNameIdentifier("DefaultUsings")
                );
            contentBuilder.Builder.AppendLine(Model.Settings.CultureInfo, $"namespace {Model.Data.Namespace}");
            contentBuilder.Builder.AppendLine("{"); // start namespace
        }

        if (Model.Settings.EnableNullableContext && Model.Settings.IndentCount == 1)
        {
            generationEnvironment.Builder.AppendLine("#nullable enable");
        }

        foreach (var suppression in Model.Data.SuppressWarningCodes)
        {
            generationEnvironment.Builder.AppendLine($"#pragma warning disable {suppression}");
        }

        var indentedBuilder = new IndentedStringBuilder(generationEnvironment.Builder);
        for (int i = 0; i < Model.Settings.IndentCount; i++)
        {
            indentedBuilder.IncrementIndent();
        }

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

        for (int i = 0; i < Model.Settings.IndentCount; i++)
        {
            indentedBuilder.DecrementIndent();
        }

        if (Model.Settings.EnableNullableContext && Model.Settings.IndentCount == 1)
        {
            generationEnvironment.Builder.AppendLine("#nullable restore");
        }

        foreach (var suppression in Model.Data.SuppressWarningCodes.Reverse())
        {
            generationEnvironment.Builder.AppendLine($"#pragma warning restore {suppression}");
        }

        if (Model.Settings.GenerateMultipleFiles)
        {
            generationEnvironment.Builder.AppendLine("}"); // end namespace
        }
    }
}
