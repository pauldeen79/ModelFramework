namespace ClassFramework.Pipelines.Builder.Features;

public class AddDefaultConstructorFeatureBuilder : IBuilderFeatureBuilder
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddDefaultConstructorFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<ClassBuilder, BuilderContext> Build()
        => new AddDefaultConstructorFeature(_formattableStringParser);
}

public class AddDefaultConstructorFeature : IPipelineFeature<ClassBuilder, BuilderContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddDefaultConstructorFeature(IFormattableStringParser formattableStringParser)
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
        }
        else
        {
            context.Model.Constructors.Add(CreateDefaultConstructor(context));
        }
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderContext>> ToBuilder()
        => new AddDefaultConstructorFeatureBuilder(_formattableStringParser);

    private string GetImmutableBuilderClassConstructorInitializer(PipelineContext<ClassBuilder, BuilderContext> context, ClassProperty property)
        => _formattableStringParser
            .Parse(property.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, property.TypeName), context.Context.FormatProvider, new ParentChildContext<ClassProperty>(context, property))
            .GetValueOrThrow()
            .FixCollectionTypeName(context.Context.Settings.TypeSettings.NewCollectionTypeName)
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
                    .Select(x => $"{x.Name} = {GetImmutableBuilderClassConstructorInitializer(context, x)};")
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

    private static string CreateImmutableBuilderClassConstructorChainCall(TypeBase instance, PipelineBuilderSettings settings)
        => instance.GetCustomValueForInheritedClass(settings, _ => "base()");

    private static string GenerateDefaultValueStatement(ClassProperty property, BuilderContext context)
        => $"{property.Name} = {property.GetDefaultValue(context.Settings.GenerationSettings.EnableNullableReferenceTypes, context.FormatProvider.ToCultureInfo())};";

    private static ClassConstructorBuilder CreateInheritanceDefaultConstructor(PipelineContext<ClassBuilder, BuilderContext> context)
        => new ClassConstructorBuilder()
            .WithChainCall("base()")
            .WithProtected(context.Context.IsBuilderForAbstractEntity);
}
