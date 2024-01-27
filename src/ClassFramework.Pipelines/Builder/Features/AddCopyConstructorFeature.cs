﻿namespace ClassFramework.Pipelines.Builder.Features;

public class AddCopyConstructorFeatureBuilder : IBuilderFeatureBuilder
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddCopyConstructorFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<IConcreteTypeBuilder, BuilderContext> Build()
        => new AddCopyConstructorFeature(_formattableStringParser);
}

public class AddCopyConstructorFeature : IPipelineFeature<IConcreteTypeBuilder, BuilderContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddCopyConstructorFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public Result<IConcreteTypeBuilder> Process(PipelineContext<IConcreteTypeBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (!context.Context.Settings.ConstructorSettings.AddCopyConstructor)
        {
            return Result.Continue<IConcreteTypeBuilder>();
        }

        if (context.Context.Settings.InheritanceSettings.EnableBuilderInheritance
            && context.Context.IsAbstractBuilder
            && !context.Context.Settings.IsForAbstractBuilder)
        {
            context.Model.AddConstructors(CreateInheritanceCopyConstructor(context));
        }
        else
        {
            var copyConstructorResult = CreateCopyConstructor(context);
            if (!copyConstructorResult.IsSuccessful())
            {
                return Result.FromExistingResult<IConcreteTypeBuilder>(copyConstructorResult);
            }

            context.Model.AddConstructors(copyConstructorResult.Value!);
        }

        return Result.Continue<IConcreteTypeBuilder>();
    }

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, BuilderContext>> ToBuilder()
        => new AddCopyConstructorFeatureBuilder(_formattableStringParser);

    private Result<ConstructorBuilder> CreateCopyConstructor(PipelineContext<IConcreteTypeBuilder, BuilderContext> context)
    {
        var initializationCodeResults = GetInitializationCodeResults(context);
        var initializationCodeErrorResult = Array.Find(initializationCodeResults, x => !x.Item2.IsSuccessful());
        if (initializationCodeErrorResult is not null)
        {
            return Result.FromExistingResult<ConstructorBuilder>(initializationCodeErrorResult.Item2);
        }

        var constructorInitializerResults = GetConstructorInitializerResults(context);
        var initializerErrorResult = Array.Find(constructorInitializerResults, x => !x.Item2.IsSuccessful());
        if (initializerErrorResult is not null)
        {
            return Result.FromExistingResult<ConstructorBuilder>(initializerErrorResult.Item2);
        }

        var results = new[]
        {
            new { Name = "NullCheck.Source", LazyResult = new Lazy<Result<string>>(() =>_formattableStringParser.Parse("{NullCheck.Source}", context.Context.FormatProvider, context)) },
            new { Name = "Name", LazyResult = new Lazy<Result<string>>(() => _formattableStringParser.Parse(context.Context.Settings.EntitySettings.NameSettings.EntityNameFormatString, context.Context.FormatProvider, context)) },
            new { Name = "Namespace", LazyResult = new Lazy<Result<string>>(() => context.Context.SourceModel.Metadata.WithMappingMetadata(context.Context.SourceModel.GetFullName().GetCollectionItemType().WhenNullOrEmpty(context.Context.SourceModel.GetFullName), context.Context.Settings.TypeSettings).GetStringResult(MetadataNames.CustomEntityNamespace, () => _formattableStringParser.Parse(context.Context.Settings.EntitySettings.NameSettings.EntityNamespaceFormatString, context.Context.FormatProvider, context))) }
        }.TakeWhileWithFirstNonMatching(x => x.LazyResult.Value.IsSuccessful()).ToArray();

        var error = Array.Find(results, x => !x.LazyResult.Value.IsSuccessful());
        if (error is not null)
        {
            // Error in formattable string parsing
            return Result.FromExistingResult<ConstructorBuilder>(error.LazyResult.Value);
        }

        return Result.Success(new ConstructorBuilder()
            .WithChainCall(CreateBuilderClassCopyConstructorChainCall(context.Context.SourceModel, context.Context.Settings))
            .WithProtected(context.Context.IsBuilderForAbstractEntity)
            .AddStringCodeStatements
            (
                new[] { results.First(x => x.Name == "NullCheck.Source").LazyResult.Value.Value! }.Where(x => !string.IsNullOrEmpty(x))
            )
            .AddParameters
            (
                new ParameterBuilder()
                    .WithName("source")
                    .WithTypeName($"{results.First(x => x.Name == "Namespace").LazyResult.Value.Value.AppendWhenNotNullOrEmpty(".")}{results.First(x => x.Name == "Name").LazyResult.Value.Value!}{context.Context.SourceModel.GetGenericTypeArgumentsString()}")
            )
            .AddStringCodeStatements(constructorInitializerResults.Select(x => $"{x.Item1} = {x.Item2.Value};"))
            .AddStringCodeStatements(initializationCodeResults.Select(x => $"{GetSourceExpression(x.Item2.Value, x.Item1, context)};"))
        );
    }

    private Tuple<Property, Result<string>>[] GetInitializationCodeResults(PipelineContext<IConcreteTypeBuilder, BuilderContext> context)
        => context.Context.SourceModel.Properties
            .Where(x => context.Context.SourceModel.IsMemberValidForBuilderClass(x, context.Context.Settings))
            .Select(x => new Tuple<Property, Result<string>>
            (
                x,
                CreateBuilderInitializationCode(x, context)
            ))
            .TakeWhileWithFirstNonMatching(x => x.Item2.IsSuccessful())
            .ToArray();

    private Tuple<string, Result<string>>[] GetConstructorInitializerResults(PipelineContext<IConcreteTypeBuilder, BuilderContext> context)
        => context.Context.SourceModel.Properties
            .Where(x => context.Context.SourceModel.IsMemberValidForBuilderClass(x, context.Context.Settings) && x.TypeName.FixTypeName().IsCollectionTypeName())
            .Select(x => new Tuple<string, Result<string>>
            (
                x.GetBuilderMemberName(context.Context.Settings.EntitySettings.NullCheckSettings.AddNullChecks, context.Context.Settings.TypeSettings.EnableNullableReferenceTypes, context.Context.Settings.EntitySettings.ConstructorSettings.OriginalValidateArguments, context.Context.FormatProvider.ToCultureInfo()),
                x.GetBuilderConstructorInitializer(context, _formattableStringParser)
            ))
            .TakeWhileWithFirstNonMatching(x => x.Item2.IsSuccessful())
            .ToArray();

    private Result<string> CreateBuilderInitializationCode(Property property, PipelineContext<IConcreteTypeBuilder, BuilderContext> context)
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
            new ParentChildContext<PipelineContext<IConcreteTypeBuilder, BuilderContext>, Property>(context, property, context.Context.Settings)
        );

    private static string CreateCollectionInitialization(PipelineSettings settings)
    {
        if (settings.TypeSettings.NewCollectionTypeName == typeof(IEnumerable<>).WithoutGenerics())
        {
            return "{NullCheck.Source.Argument}{BuilderMemberName} = {Name}.Concat(source.[SourceExpression])";
        }

        return "{NullCheck.Source.Argument}{BuilderMemberName}.AddRange(source.[SourceExpression])";
    }

    private static string? GetSourceExpression(string? value, Property sourceProperty, PipelineContext<IConcreteTypeBuilder, BuilderContext> context)
    {
        if (value is null || !value.Contains("[SourceExpression]"))
        {
            return value;
        }

        if (value == "[SourceExpression]")
        {
            return sourceProperty.Name;
        }

        var metadata = sourceProperty.Metadata.WithMappingMetadata(sourceProperty.TypeName.GetCollectionItemType().WhenNullOrEmpty(sourceProperty.TypeName), context.Context.Settings.TypeSettings);
        var sourceExpression = metadata.GetStringValue(MetadataNames.CustomBuilderSourceExpression, "[Name]");
        return sourceProperty.TypeName.FixTypeName().IsCollectionTypeName()
            ? value.Replace("[SourceExpression]", $"{sourceProperty.Name}.Select(x => {sourceExpression})").Replace("[Name]", "x").Replace("[NullableSuffix]", string.Empty).Replace(".Select(x => x)", string.Empty)
            : value.Replace("[SourceExpression]", sourceExpression).Replace("[Name]", sourceProperty.Name).Replace("[NullableSuffix]", sourceProperty.GetSuffix(context.Context.Settings.TypeSettings.EnableNullableReferenceTypes));
    }
    private static string CreateBuilderClassCopyConstructorChainCall(IType instance, PipelineSettings settings)
        => instance.GetCustomValueForInheritedClass(settings.EntitySettings.InheritanceSettings.EnableInheritance, _ => Result.Success("base(source)")).Value!; //note that the delegate always returns success, so we can simply use the Value here

    private static ConstructorBuilder CreateInheritanceCopyConstructor(PipelineContext<IConcreteTypeBuilder, BuilderContext> context)
        => new ConstructorBuilder()
            .WithChainCall("base(source)")
            .WithProtected(context.Context.IsBuilderForAbstractEntity)
            .AddParameters
            (
                new ParameterBuilder()
                    .WithName("source")
                    .WithTypeName($"{context.Context.SourceModel.GetFullName()}{context.Context.SourceModel.GetGenericTypeArgumentsString()}")
            );
}
