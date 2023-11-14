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
        var initializationCodeResults = context.Context.Model.Properties
            .Where(x => context.Context.Model.IsMemberValidForBuilderClass(x, context.Context.Settings))
            .Select(x => new
            {
                x.Name,
                Source = x,
                Result = CreateBuilderInitializationCode(x, context)
            })
            .TakeWhileWithFirstNonMatching(x => x.Result.IsSuccessful())
            .ToArray();

        var initializationCodeErrorResult = Array.Find(initializationCodeResults, x => !x.Result.IsSuccessful());
        if (initializationCodeErrorResult is not null)
        {
            return Result.FromExistingResult<ClassConstructorBuilder>(initializationCodeErrorResult.Result);
        }

        var constructorInitializerResults = context.Context.Model.Properties
            .Where(x => context.Context.Model.IsMemberValidForBuilderClass(x, context.Context.Settings) && x.TypeName.FixTypeName().IsCollectionTypeName())
            .Select(x => new
            {
                Name = x.GetInitializationName(context.Context.Settings.GenerationSettings.AddNullChecks, context.Context.Settings.GenerationSettings.EnableNullableReferenceTypes, context.Context.Settings.EntitySettings.ConstructorSettings.OriginalValidateArguments, context.Context.FormatProvider.ToCultureInfo()),
                Result = x.GetBuilderClassConstructorInitializer(context, _formattableStringParser, x.TypeName)
            })
            .TakeWhileWithFirstNonMatching(x => x.Result.IsSuccessful())
            .ToArray();

        var initializerErrorResult = Array.Find(constructorInitializerResults, x => !x.Result.IsSuccessful());
        if (initializerErrorResult is not null)
        {
            return Result.FromExistingResult<ClassConstructorBuilder>(initializerErrorResult.Result);
        }

        return Result.Success(new ClassConstructorBuilder()
            .WithChainCall(CreateBuilderClassCopyConstructorChainCall(context.Context.Model, context.Context.Settings))
            .WithProtected(context.Context.IsBuilderForAbstractEntity)
            .AddStringCodeStatements
            (
                new[]
                {
                    _formattableStringParser.Parse("{NullCheck.Source}", context.Context.FormatProvider, context).GetValueOrThrow()
                }.Where(x => !string.IsNullOrEmpty(x))
            )
            .AddParameters
            (
                new ParameterBuilder()
                    .WithName("source")
                    .WithTypeName(context.Context.Model.GetFullName() + context.Context.Model.GetGenericTypeArgumentsString())
            )
            .AddStringCodeStatements(constructorInitializerResults.Select(x => $"{x.Name} = {x.Result.Value};"))
            .AddStringCodeStatements(initializationCodeResults.Select(x => $"{GetSourceExpression(x.Result.Value, x.Source, context.Context.Settings.TypeSettings, context.Context.Settings.GenerationSettings.EnableNullableReferenceTypes)};"))
        );
    }

    private Result<string> CreateBuilderInitializationCode(ClassProperty property, PipelineContext<ClassBuilder, BuilderContext> context)
        => _formattableStringParser.Parse
        (
            property.Metadata
                .WithMappingMetadata(property.TypeName.GetCollectionItemType().WhenNullOrEmpty(property.TypeName), context.Context.Settings.TypeSettings)
                .GetStringValue
                (
                    MetadataNames.CustomBuilderConstructorInitializeExpression,
                    () => property.TypeName.FixTypeName().IsCollectionTypeName()
                        ? CreateCollectionInitialization(context.Context.Settings)
                        : "{BuilderMemberName} = source.[SourceExpression]" // note that we are not prefixing {NullCheck.Source.Argument}, because we can simply always copy the value, regardless if it's null :)
                ),
            context.Context.FormatProvider,
            new ParentChildContext<BuilderContext, ClassProperty>(context, property, context.Context.Settings)
        );

    private static string CreateCollectionInitialization(PipelineBuilderSettings settings)
    {
        if (settings.TypeSettings.NewCollectionTypeName == typeof(IEnumerable<>).WithoutGenerics())
        {
            return "{NullCheck.Source.Argument}{Name} = {Name}.Concat(source.[SourceExpression])";
        }

        return "{NullCheck.Source.Argument}{Name}.AddRange(source.[SourceExpression])";
    }

    private static string? GetSourceExpression(string? value, ClassProperty sourceProperty, PipelineBuilderTypeSettings typeSettings, bool enableNullableReferenceTypes)
    {
        if (value is null || !value.Contains("[SourceExpression]"))
        {
            return value;
        }

        if (value == "[SourceExpression]")
        {
            return sourceProperty.Name;
        }

        var metadata = sourceProperty.Metadata.WithMappingMetadata(sourceProperty.TypeName.GetCollectionItemType().WhenNullOrEmpty(sourceProperty.TypeName), typeSettings);
        var sourceExpression = metadata.GetStringValue(MetadataNames.CustomBuilderSourceExpression, "[Name]");
        return sourceProperty.TypeName.IsCollectionTypeName()
            ? value.Replace("[SourceExpression]", $"{sourceProperty.Name}.Select(x => {sourceExpression})").Replace("[Name]", "x").Replace("[NullableSuffix]", string.Empty)
            : value.Replace("[SourceExpression]", sourceExpression).Replace("[Name]", sourceProperty.Name).Replace("[NullableSuffix]", sourceProperty.GetSuffix(enableNullableReferenceTypes));
    }
    private static string CreateBuilderClassCopyConstructorChainCall(TypeBase instance, PipelineBuilderSettings settings)
        => instance.GetCustomValueForInheritedClass(settings.EntitySettings, _ => Result.Success("base(source)")).GetValueOrThrow(); //note that the delegate always returns success, so we can simply use GetValueOrThrow here

    private static ClassConstructorBuilder CreateInheritanceCopyConstructor(PipelineContext<ClassBuilder, BuilderContext> context)
        => new ClassConstructorBuilder()
            .WithChainCall("base(source)")
            .WithProtected(context.Context.IsBuilderForAbstractEntity)
            .AddParameters
            (
                new ParameterBuilder()
                    .WithName("source")
                    .WithTypeName(context.Context.Model.GetFullName() + context.Context.Model.GetGenericTypeArgumentsString())
            );
}
