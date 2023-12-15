namespace ClassFramework.Pipelines.Builder.Features;

public class AddDefaultConstructorFeatureBuilder : IBuilderFeatureBuilder
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddDefaultConstructorFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<IConcreteTypeBuilder, BuilderContext> Build()
        => new AddDefaultConstructorFeature(_formattableStringParser);
}

public class AddDefaultConstructorFeature : IPipelineFeature<IConcreteTypeBuilder, BuilderContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddDefaultConstructorFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public Result<IConcreteTypeBuilder> Process(PipelineContext<IConcreteTypeBuilder, BuilderContext> context)
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
                return Result.FromExistingResult<IConcreteTypeBuilder>(defaultConstructorResult);
            }

            context.Model.Constructors.Add(defaultConstructorResult.Value!);
        }

        return Result.Continue<IConcreteTypeBuilder>();
    }

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, BuilderContext>> ToBuilder()
        => new AddDefaultConstructorFeatureBuilder(_formattableStringParser);

    private Result<ClassConstructorBuilder> CreateDefaultConstructor(PipelineContext<IConcreteTypeBuilder, BuilderContext> context)
    {
        var constructorInitializerResults = context.Context.SourceModel.Properties
            .Where(x => context.Context.SourceModel.IsMemberValidForBuilderClass(x, context.Context.Settings) && x.TypeName.FixTypeName().IsCollectionTypeName())
            .Select(x => new
            {
                Name = x.GetBuilderMemberName(context.Context.Settings.EntitySettings.NullCheckSettings.AddNullChecks, context.Context.Settings.TypeSettings.EnableNullableReferenceTypes, context.Context.Settings.EntitySettings.ConstructorSettings.OriginalValidateArguments, context.Context.FormatProvider.ToCultureInfo()),
                Result = x.GetBuilderClassConstructorInitializer(context, _formattableStringParser, x.TypeName)
            })
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
            var defaultValueResults = context.Context.SourceModel.Properties
                .Where
                (x =>
                    context.Context.SourceModel.IsMemberValidForBuilderClass(x, context.Context.Settings)
                    && !x.TypeName.FixTypeName().IsCollectionTypeName()
                    && !x.IsNullable
                    && x.HasNonDefaultDefaultValue()
                )
                .Select(x => GenerateDefaultValueStatement(x, context))
                .TakeWhileWithFirstNonMatching(x => x.IsSuccessful())
                .ToArray();
            
            var defaultValueErrorResult = Array.Find(defaultValueResults, x => !x.IsSuccessful());
            if (defaultValueErrorResult is not null)
            {
                return Result.FromExistingResult<ClassConstructorBuilder>(defaultValueErrorResult);
            }

            ctor.AddStringCodeStatements(defaultValueResults.Select(x => x.Value!));
            ctor.AddStringCodeStatements("SetDefaultValues();");
            context.Model.Methods.Add(new ClassMethodBuilder().WithName("SetDefaultValues").WithPartial().WithVisibility(Visibility.Private));
        }

        return Result.Success(ctor);
    }

    private static string CreateBuilderClassConstructorChainCall(IType instance, PipelineBuilderSettings settings)
        => instance.GetCustomValueForInheritedClass(settings.EntitySettings, _ => Result.Success("base()")).Value!; //note that the delegate always returns success, so we can simply use the Value here

    private Result<string> GenerateDefaultValueStatement(ClassProperty property, PipelineContext<IConcreteTypeBuilder, BuilderContext> context)
        => _formattableStringParser.Parse
        (
            "{BuilderMemberName} = {DefaultValue};",
            context.Context.FormatProvider,
            new ParentChildContext<PipelineContext<IConcreteTypeBuilder, BuilderContext>, ClassProperty>(context, property, context.Context.Settings)
        );

    private static ClassConstructorBuilder CreateInheritanceDefaultConstructor(PipelineContext<IConcreteTypeBuilder, BuilderContext> context)
        => new ClassConstructorBuilder()
            .WithChainCall("base()")
            .WithProtected(context.Context.IsBuilderForAbstractEntity);
}
