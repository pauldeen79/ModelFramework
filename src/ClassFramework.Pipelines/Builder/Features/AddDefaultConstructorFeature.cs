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

    public Result<ClassBuilder> Process(PipelineContext<ClassBuilder, BuilderContext> context)
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
            var defaultConstructorResult = CreateDefaultConstructor(context);
            if (!defaultConstructorResult.IsSuccessful())
            {
                return Result.FromExistingResult<ClassBuilder>(defaultConstructorResult);
            }

            context.Model.Constructors.Add(defaultConstructorResult.Value!);
        }

        return Result.Continue<ClassBuilder>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderContext>> ToBuilder()
        => new AddDefaultConstructorFeatureBuilder(_formattableStringParser);

    private Result<ClassConstructorBuilder> CreateDefaultConstructor(PipelineContext<ClassBuilder, BuilderContext> context)
    {
        var constructorInitializerResults = context.Context.SourceModel.Properties
            .Where(x => context.Context.SourceModel.IsMemberValidForBuilderClass(x, context.Context.Settings) && x.TypeName.FixTypeName().IsCollectionTypeName())
            .Select(x => new { x.Name, Result = x.GetBuilderClassConstructorInitializer(context, _formattableStringParser) })
            .TakeWhileWithFirstNonMatching(x => x.Result.IsSuccessful())
            .ToArray();

        var errorResult = Array.Find(constructorInitializerResults, x => !x.Result.IsSuccessful());
        if (errorResult is not null)
        {
            return Result.FromExistingResult<ClassConstructorBuilder>(errorResult.Result);
        }

        var ctor = new ClassConstructorBuilder()
            .WithChainCall(CreateBuilderClassConstructorChainCall(context.Context.SourceModel, context.Context.Settings))
            .WithProtected(context.Context.IsBuilderForAbstractEntity)
            .AddStringCodeStatements(constructorInitializerResults.Select(x => $"{x.Name} = {x.Result.Value};"));

        if (context.Context.Settings.ConstructorSettings.SetDefaultValues)
        {
            ctor.AddStringCodeStatements
            (
                context.Context.SourceModel.Properties
                    .Where
                    (x =>
                        context.Context.SourceModel.IsMemberValidForBuilderClass(x, context.Context.Settings)
                        && !x.TypeName.FixTypeName().IsCollectionTypeName()
                        && !x.IsNullable
                    )
                    .Select(x => GenerateDefaultValueStatement(x, context.Context))
                    .Where(x => x.IndexOf(" = default(", StringComparison.OrdinalIgnoreCase) == -1)
            );
            ctor.AddStringCodeStatements("SetDefaultValues();");
            context.Model.AddMethods(new ClassMethodBuilder().WithName("SetDefaultValues").WithPartial().WithVisibility(Visibility.Private));
        }

        return Result.Success(ctor);
    }

    private static string CreateBuilderClassConstructorChainCall(TypeBase instance, PipelineBuilderSettings settings)
        => instance.GetCustomValueForInheritedClass(settings.ClassSettings, _ => Result.Success("base()")).GetValueOrThrow(); //note that the delegate always returns success, so we can simply use GetValueOrThrow here

    private static string GenerateDefaultValueStatement(ClassProperty property, BuilderContext context)
        => $"{property.Name} = {property.GetDefaultValue(context.Settings.GenerationSettings.EnableNullableReferenceTypes, context.FormatProvider.ToCultureInfo())};";

    private static ClassConstructorBuilder CreateInheritanceDefaultConstructor(PipelineContext<ClassBuilder, BuilderContext> context)
        => new ClassConstructorBuilder()
            .WithChainCall("base()")
            .WithProtected(context.Context.IsBuilderForAbstractEntity);
}
