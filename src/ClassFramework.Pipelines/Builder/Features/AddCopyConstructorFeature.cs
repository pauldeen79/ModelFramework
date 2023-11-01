namespace ClassFramework.Pipelines.Builder.Features;

public class AddCopyConstructorFeatureBuilder : IBuilderFeatureBuilder
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddCopyConstructorFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<ClassBuilder, BuilderContext> Build()
        => new AddCopyConstructorFeature(_formattableStringParser);
}

public class AddCopyConstructorFeature : IPipelineFeature<ClassBuilder, BuilderContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddCopyConstructorFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public Result<ClassBuilder> Process(PipelineContext<ClassBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (!context.Context.Settings.ConstructorSettings.AddCopyConstructor)
        {
            return Result.Continue<ClassBuilder>();
        }

        if (context.Context.Settings.InheritanceSettings.EnableBuilderInheritance
            && context.Context.IsAbstractBuilder
            && !context.Context.Settings.IsForAbstractBuilder)
        {
            context.Model.Constructors.Add(CreateInheritanceCopyConstructor(context));
        }
        else
        {
            var copyConstructorResult = CreateCopyConstructor(context);
            if (!copyConstructorResult.IsSuccessful())
            {
                return Result.FromExistingResult<ClassBuilder>(copyConstructorResult);
            }

            context.Model.Constructors.Add(copyConstructorResult.Value!);
        }

        return Result.Continue<ClassBuilder>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderContext>> ToBuilder()
        => new AddCopyConstructorFeatureBuilder(_formattableStringParser);

    private Result<ClassConstructorBuilder> CreateCopyConstructor(PipelineContext<ClassBuilder, BuilderContext> context)
    {
        var immutableBuilderInitializationCodeResults = context.Context.SourceModel.Properties
            .Where(x => context.Context.SourceModel.IsMemberValidForImmutableBuilderClass(x, context.Context.Settings))
            .Select(x => CreateImmutableBuilderInitializationCode(x, context))
            .TakeWhileWithFirstNonMatching(x => x.IsSuccessful())
            .ToArray();

        var initializationCodeErrorResult = Array.Find(immutableBuilderInitializationCodeResults, x => !x.IsSuccessful());
        if (initializationCodeErrorResult is not null)
        {
            return Result.FromExistingResult<ClassConstructorBuilder>(initializationCodeErrorResult);
        }

        var immutableBuilderClassConstructorInitializerResults = context.Context.SourceModel.Properties
            .Where(x => context.Context.SourceModel.IsMemberValidForImmutableBuilderClass(x, context.Context.Settings) && x.TypeName.FixTypeName().IsCollectionTypeName())
            .Select(x => new { x.Name, Result = x.GetImmutableBuilderClassConstructorInitializer(context, _formattableStringParser) })
            .TakeWhileWithFirstNonMatching(x => x.Result.IsSuccessful())
            .ToArray();

        var initializerErrorResult = Array.Find(immutableBuilderClassConstructorInitializerResults, x => !x.Result.IsSuccessful());
        if (initializerErrorResult is not null)
        {
            return Result.FromExistingResult<ClassConstructorBuilder>(initializerErrorResult.Result);
        }

        var chainCallResult = CreateImmutableBuilderClassCopyConstructorChainCall(context.Context.SourceModel, context.Context.Settings);
        if (!chainCallResult.IsSuccessful())
        {
            return Result.FromExistingResult<ClassConstructorBuilder>(chainCallResult);
        }

        return Result.Success(new ClassConstructorBuilder()
            .WithChainCall(chainCallResult.Value!)
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
                    .WithTypeName(context.Context.SourceModel.GetFullName() + context.Context.SourceModel.GetGenericTypeArgumentsString())
            )
            .AddStringCodeStatements(immutableBuilderClassConstructorInitializerResults.Select(x => $"{x.Name} = {x.Result.Value};"))
            .AddStringCodeStatements(immutableBuilderInitializationCodeResults.Select(x => $"{x.Value};"))
        );
    }

    private Result<string> CreateImmutableBuilderInitializationCode(ClassProperty property, PipelineContext<ClassBuilder, BuilderContext> context)
        => _formattableStringParser.Parse
        (
            property.Metadata.GetStringValue
            (
                MetadataNames.CustomBuilderConstructorInitializeExpression,
                () => property.TypeName.FixTypeName().IsCollectionTypeName()
                    ? CreateCollectionInitialization(context.Context.Settings)
                    : "{Name} = source.{Name}"
            ),
            context.Context.FormatProvider,
            new ParentChildContext<BuilderContext, ClassProperty>(context, property)
        );

    private static string CreateCollectionInitialization(PipelineBuilderSettings settings)
    {
        if (settings.TypeSettings.NewCollectionTypeName == typeof(IEnumerable<>).WithoutGenerics())
        {
            return "{NullCheck.Source}{Name} = {Name}.Concat(source.{Name})";
        }

        return "{NullCheck.Source}{Name}.AddRange(source.{Name})";
    }

    private static Result<string> CreateImmutableBuilderClassCopyConstructorChainCall(TypeBase instance, PipelineBuilderSettings settings)
        => instance.GetCustomValueForInheritedClass(settings.ClassSettings, _ => Result.Success("base(source)"));

    private static ClassConstructorBuilder CreateInheritanceCopyConstructor(PipelineContext<ClassBuilder, BuilderContext> context)
        => new ClassConstructorBuilder()
            .WithChainCall("base(source)")
            .WithProtected(context.Context.IsBuilderForAbstractEntity)
            .AddParameters
            (
                new ParameterBuilder()
                    .WithName("source")
                    .WithTypeName(context.Context.SourceModel.GetFullName() + context.Context.SourceModel.GetGenericTypeArgumentsString())
            );
}
