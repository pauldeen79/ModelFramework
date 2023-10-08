﻿namespace ClassFramework.Pipelines.Builder.Features;

public class AddConstructorsFeatureBuilder : IBuilderFeatureBuilder
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddConstructorsFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<ClassBuilder, BuilderContext> Build()
        => new AddConstructorsFeature(_formattableStringParser);
}

public class AddConstructorsFeature : IPipelineFeature<ClassBuilder, BuilderContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddConstructorsFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public void Process(PipelineContext<ClassBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (!context.Context.NeedsConstructors)
        {
            return;
        }

        if (context.Context.Settings.InheritanceSettings.EnableBuilderInheritance
            && context.Context.IsAbstractBuilder
            && !context.Context.Settings.IsForAbstractBuilder)
        {
            context.Model.Constructors.Add(new ClassConstructorBuilder()
                .WithChainCall("base()")
                .WithProtected(context.Context.IsBuilderForAbstractEntity));

            if (context.Context.Settings.ConstructorSettings.AddCopyConstructor)
            {
                context.Model.Constructors.Add(CreateInheritanceCopyConstructor(context));
            }
        }
        else
        {
            context.Model.Constructors.Add(CreateDefaultConstructor(context));

            if (context.Context.Settings.ConstructorSettings.AddCopyConstructor)
            {
                context.Model.Constructors.Add(CreateCopyConstructor(context));
            }
        }
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderContext>> ToBuilder()
        => new AddConstructorsFeatureBuilder(_formattableStringParser);

    private string GetImmutableBuilderClassConstructorInitializer(BuilderContext context, ClassProperty property)
        => _formattableStringParser
            .Parse(property.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, property.TypeName), context.FormatProvider, property)
            .GetValueOrThrow()
            .FixCollectionTypeName(context.Settings.TypeSettings.NewCollectionTypeName)
            .GetCollectionInitializeStatement()
            .GetCsharpFriendlyTypeName();

    private ClassConstructorBuilder CreateDefaultConstructor(PipelineContext<ClassBuilder, BuilderContext> context)
        => new ClassConstructorBuilder()
            .WithChainCall(CreateImmutableBuilderClassConstructorChainCall(context.Context.SourceModel, context.Context.Settings))
            .WithProtected(context.Context.IsBuilderForAbstractEntity)
            .AddStringCodeStatements
            (
                context.Context.SourceModel.Properties
                    .Where(x => context.Context.SourceModel.IsMemberValidForImmutableBuilderClass(x, context.Context.Settings.InheritanceSettings, isForWithStatement: false))
                    .Where(x => x.TypeName.IsCollectionTypeName())
                    .Select(x => $"{x.Name} = {GetImmutableBuilderClassConstructorInitializer(context.Context, x)};")
            )
            .AddStringCodeStatements
            (
                context.Context.SourceModel.Properties
                    .Where(x => context.Context.SourceModel.IsMemberValidForImmutableBuilderClass(x, context.Context.Settings.InheritanceSettings, isForWithStatement: false))
                    .Where(x => context.Context.Settings.ConstructorSettings.SetDefaultValues
                        && !x.TypeName.IsCollectionTypeName()
                        && !x.IsNullable
                        && !x.GetDefaultValue(context.Context.Settings.GenerationSettings.EnableNullableReferenceTypes).StartsWith("default(", StringComparison.Ordinal))
                    .Select(x => GenerateDefaultValueStatement(x, context.Context.Settings))
            );

    private ClassConstructorBuilder CreateCopyConstructor(PipelineContext<ClassBuilder, BuilderContext> context)
        => new ClassConstructorBuilder()
            .WithChainCall(CreateImmutableBuilderClassCopyConstructorChainCall(context.Context.SourceModel, context.Context.Settings))
            .WithProtected(context.Context.IsBuilderForAbstractEntity)
            .AddStringCodeStatements
            (
                new[]
                {
                    "if (source is null)",
                    "{",
                    $@"    throw new {typeof(ArgumentNullException).FullName}(nameof(source));",
                    "}"
                }.Where(_ => context.Context.Settings.GenerationSettings.AddNullChecks)
            )
            .AddParameters
            (
                new ParameterBuilder()
                    .WithName("source")
                    .WithTypeName(context.Context.SourceModel.FormatInstanceName(false, context.Context.Settings.TypeSettings.FormatInstanceTypeNameDelegate) + context.Context.SourceModel.GetGenericTypeArgumentsString())
            )
            .AddParameters
            (
                context.Context.SourceModel.Metadata
                    .GetValues<Parameter>(MetadataNames.AdditionalBuilderCopyConstructorAdditionalParameter)
                    .Select(x => new ParameterBuilder(x))
            )
            .AddStringCodeStatements
            (
                context.Context.SourceModel.Properties
                    .Where(x => context.Context.SourceModel.IsMemberValidForImmutableBuilderClass(x, context.Context.Settings.InheritanceSettings, isForWithStatement: false) && x.TypeName.IsCollectionTypeName())
                    .Select(x => $"{x.Name} = {GetImmutableBuilderClassConstructorInitializer(context.Context, x)};")
            )
            .AddStringCodeStatements
            (
                context.Context.SourceModel.Properties
                    .Where(x => context.Context.SourceModel.IsMemberValidForImmutableBuilderClass(x, context.Context.Settings.InheritanceSettings, isForWithStatement: false))
                    .Select(x => $"{CreateImmutableBuilderInitializationCode(x, context)};")
            );

    private string CreateImmutableBuilderInitializationCode(ClassProperty property, PipelineContext<ClassBuilder, BuilderContext> context)
        => _formattableStringParser.Parse
        (
            property.Metadata.GetStringValue
            (
                MetadataNames.CustomBuilderConstructorInitializeExpression,
                () => property.TypeName.IsCollectionTypeName()
                    ? CreateCollectionInitialization(property, context.Context.Settings)
                    : CreateSingleInitialization(property)
            ),
            context.Context.FormatProvider,
            context //TODO: Create a new context here, a composition of both the original pipeline context (which holds the settings and the source model) and the property
        ).GetValueOrThrow();

        //=> string.Format
        //(
        //    property.Metadata.GetStringValue
        //    (
        //        MetadataNames.CustomBuilderConstructorInitializeExpression,
        //        () => property.TypeName.IsCollectionTypeName()
        //            ? CreateCollectionInitialization(property, context.Context.Settings)
        //            : CreateSingleInitialization(property, context.Context.Settings)
        //    ),
        //    property.Name,                                                                              // 0
        //    property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo()),                                                               // 1
        //    property.TypeName.GetCsharpFriendlyTypeName(),                                              // 2
        //    property.TypeName.GetGenericArguments().GetCsharpFriendlyTypeName(),                        // 3
        //    context.Context.Settings.GenerationSettings.AddNullChecks && !property.IsValueType && property.IsNullable  // 4
        //        ? $"if (source.{property.Name} != null) "
        //        : "",
        //    context.Context.Settings.TypeSettings.FormatInstanceTypeNameDelegate != null                                // 5
        //        ? context.Context.Settings.TypeSettings.FormatInstanceTypeNameDelegate.Invoke(new ClassBuilder().WithName(property.TypeName.GetClassName()).WithNamespace(property.TypeName.GetNamespaceWithDefault()).Build(), true).GetClassName().WhenNullOrEmpty(property.TypeName.GetClassName)
        //        : property.TypeName.GetClassName(),
        //    property.TypeName.GetGenericArguments().GetClassName(),                                     // 6
        //    context.Context.Settings.NameSettings.BuildersNamespace,                                                    // 7
        //    property.TypeName.WithoutProcessedGenerics().GetCsharpFriendlyTypeName(),                   // 8
        //    string.IsNullOrEmpty(property.TypeName.GetGenericArguments())                               // 9
        //        ? string.Empty
        //        : $"<{property.TypeName.GetGenericArguments().GetCsharpFriendlyTypeName()}>",
        //    context.Context.Settings.TypeSettings.FormatInstanceTypeNameDelegate != null                                // 10
        //        ? context.Context.Settings.TypeSettings.FormatInstanceTypeNameDelegate.Invoke(new ClassBuilder().WithName(property.TypeName.WithoutProcessedGenerics().GetClassName()).WithNamespace(property.TypeName.GetNamespaceWithDefault()).Build(), true).GetClassName().WhenNullOrEmpty(() => property.TypeName.WithoutProcessedGenerics().GetClassName())
        //        : property.TypeName.GetClassName()
        //);

    private static string CreateSingleInitialization(ClassProperty property)
        => $"{property.Name} = source.{property.Name}";

    private static string CreateCollectionInitialization(ClassProperty property, PipelineBuilderSettings settings)
    {
        if (settings.TypeSettings.NewCollectionTypeName == typeof(IEnumerable<>).WithoutGenerics())
        {
            return settings.GenerationSettings.AddNullChecks && !property.IsValueType && property.IsNullable
                ? $"if (source.{property.Name} is not null) {property.Name} = source.{property.Name}"
                : $"{property.Name} = source.{property.Name}";
        }

        return settings.GenerationSettings.AddNullChecks && !property.IsValueType && property.IsNullable
            ? $"if (source.{property.Name} is not null) {property.Name}.AddRange(source.{property.Name})"
            : $"{property.Name}.AddRange(source.{property.Name})";
    }

    private static string CreateImmutableBuilderClassConstructorChainCall(TypeBase instance, PipelineBuilderSettings settings)
        => instance.GetCustomValueForInheritedClass(settings, _ => "base()");

    private static string CreateImmutableBuilderClassCopyConstructorChainCall(TypeBase instance, PipelineBuilderSettings settings)
        => instance.GetCustomValueForInheritedClass(settings, _ => "base(source)");

    private static string GenerateDefaultValueStatement(ClassProperty property, PipelineBuilderSettings settings)
        => $"{property.Name} = {property.GetDefaultValue(settings.GenerationSettings.EnableNullableReferenceTypes)};";

    private static ClassConstructorBuilder CreateInheritanceCopyConstructor(PipelineContext<ClassBuilder, BuilderContext> context)
        => new ClassConstructorBuilder()
            .WithChainCall("base(source)")
            .WithProtected(context.Context.IsBuilderForAbstractEntity)
            .AddParameters
            (
                new ParameterBuilder()
                    .WithName("source")
                    .WithTypeName(context.Context.SourceModel.FormatInstanceName(false, context.Context.Settings.TypeSettings.FormatInstanceTypeNameDelegate) + context.Context.SourceModel.GetGenericTypeArgumentsString())
            )
            .AddParameters
            (
                context.Context.SourceModel.Metadata
                    .GetValues<Parameter>(MetadataNames.AdditionalBuilderCopyConstructorAdditionalParameter)
                    .Select(x => new ParameterBuilder(x))
            );
}

