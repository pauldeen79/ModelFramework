namespace ClassFramework.Pipelines.Builder.Features;

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

        if (context.Context.Settings.InheritanceSettings.EnableBuilderInheritance
            && context.Context.IsAbstractBuilder
            && !context.Context.Settings.IsForAbstractBuilder)
        {
            context.Model.Constructors.Add(CreateInheritanceDefaultConstructor(context));

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
                    .Where(x => context.Context.SourceModel.IsMemberValidForImmutableBuilderClass(x, context.Context.Settings))
                    .Where(x => x.TypeName.FixTypeName().IsCollectionTypeName())
                    .Select(x => $"{x.Name} = {GetImmutableBuilderClassConstructorInitializer(context.Context, x)};")
            )
            .AddStringCodeStatements
            (
                context.Context.SourceModel.Properties
                    .Where(x => context.Context.SourceModel.IsMemberValidForImmutableBuilderClass(x, context.Context.Settings))
                    .Where(x => context.Context.Settings.ConstructorSettings.SetDefaultValues
                        && !x.TypeName.FixTypeName().IsCollectionTypeName()
                        && !x.IsNullable
                        && !x.GetDefaultValue(context.Context.Settings.GenerationSettings.EnableNullableReferenceTypes, context.Context.FormatProvider.ToCultureInfo()).StartsWith("default(", StringComparison.Ordinal))
                    .Select(x => GenerateDefaultValueStatement(x, context.Context))
            );

    private ClassConstructorBuilder CreateCopyConstructor(PipelineContext<ClassBuilder, BuilderContext> context)
        => new ClassConstructorBuilder()
            .WithChainCall(CreateImmutableBuilderClassCopyConstructorChainCall(context.Context.SourceModel, context.Context.Settings))
            .WithProtected(context.Context.IsBuilderForAbstractEntity)
            .AddStringCodeStatements
            (
                new[]
                {
                    $@"if (source is null) throw new {typeof(ArgumentNullException).FullName}(nameof(source));"
                }.Where(_ => context.Context.Settings.GenerationSettings.AddNullChecks)
            )
            .AddParameters
            (
                new ParameterBuilder()
                    .WithName("source")
                    .WithTypeName(context.Context.SourceModel.FormatInstanceName(false, context.Context.Settings.TypeSettings.FormatInstanceTypeNameDelegate) + context.Context.SourceModel.GetGenericTypeArgumentsString())
            )
            .AddStringCodeStatements
            (
                context.Context.SourceModel.Properties
                    .Where(x => context.Context.SourceModel.IsMemberValidForImmutableBuilderClass(x, context.Context.Settings) && x.TypeName.FixTypeName().IsCollectionTypeName())
                    .Select(x => $"{x.Name} = {GetImmutableBuilderClassConstructorInitializer(context.Context, x)};")
            )
            .AddStringCodeStatements
            (
                context.Context.SourceModel.Properties
                    .Where(x => context.Context.SourceModel.IsMemberValidForImmutableBuilderClass(x, context.Context.Settings))
                    .Select(x => $"{CreateImmutableBuilderInitializationCode(x, context)};")
            );

    private string CreateImmutableBuilderInitializationCode(ClassProperty property, PipelineContext<ClassBuilder, BuilderContext> context)
        => _formattableStringParser.Parse
        (
            property.Metadata.GetStringValue
            (
                MetadataNames.CustomBuilderConstructorInitializeExpression,
                () => property.TypeName.FixTypeName().IsCollectionTypeName()
                    ? CreateCollectionInitialization(property, context.Context.Settings)
                    : CreateSingleInitialization(property)
            ),
            context.Context.FormatProvider,
            new ParentChildContext<ClassProperty>(context, property)
        ).GetValueOrThrow();

    private static string CreateSingleInitialization(ClassProperty property)
        => $"{property.Name} = source.{property.Name}";

    private static string CreateCollectionInitialization(ClassProperty property, PipelineBuilderSettings settings)
    {
        if (settings.TypeSettings.NewCollectionTypeName == typeof(IEnumerable<>).WithoutGenerics())
        {
            return $"{{NullCheck.Source}}{property.Name} = {property.Name}.Concat(source.{property.Name})";
        }

        return $"{{NullCheck.Source}}{property.Name}.AddRange(source.{property.Name})";
    }

    private static string CreateImmutableBuilderClassConstructorChainCall(TypeBase instance, PipelineBuilderSettings settings)
        => instance.GetCustomValueForInheritedClass(settings, _ => "base()");

    private static string CreateImmutableBuilderClassCopyConstructorChainCall(TypeBase instance, PipelineBuilderSettings settings)
        => instance.GetCustomValueForInheritedClass(settings, _ => "base(source)");

    private static string GenerateDefaultValueStatement(ClassProperty property, BuilderContext context)
        => $"{property.Name} = {property.GetDefaultValue(context.Settings.GenerationSettings.EnableNullableReferenceTypes, context.FormatProvider.ToCultureInfo())};";

    private static ClassConstructorBuilder CreateInheritanceDefaultConstructor(PipelineContext<ClassBuilder, BuilderContext> context)
        => new ClassConstructorBuilder()
            .WithChainCall("base()")
            .WithProtected(context.Context.IsBuilderForAbstractEntity);

    private static ClassConstructorBuilder CreateInheritanceCopyConstructor(PipelineContext<ClassBuilder, BuilderContext> context)
        => new ClassConstructorBuilder()
            .WithChainCall("base(source)")
            .WithProtected(context.Context.IsBuilderForAbstractEntity)
            .AddParameters
            (
                new ParameterBuilder()
                    .WithName("source")
                    .WithTypeName(context.Context.SourceModel.FormatInstanceName(false, context.Context.Settings.TypeSettings.FormatInstanceTypeNameDelegate) + context.Context.SourceModel.GetGenericTypeArgumentsString())
            );
}
