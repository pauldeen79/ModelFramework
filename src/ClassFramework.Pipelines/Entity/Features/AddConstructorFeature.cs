namespace ClassFramework.Pipelines.Entity.Features;

public class AddConstructorFeatureBuilder : IEntityFeatureBuilder
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddConstructorFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<IConcreteTypeBuilder, EntityContext> Build()
        => new AddConstructorFeature(_formattableStringParser);
}

public class AddConstructorFeature : IPipelineFeature<IConcreteTypeBuilder, EntityContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddConstructorFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public Result<IConcreteTypeBuilder> Process(PipelineContext<IConcreteTypeBuilder, EntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        var ctorResult = CreateEntityConstructor(context);
        if (!ctorResult.IsSuccessful())
        {
            return Result.FromExistingResult<IConcreteTypeBuilder>(ctorResult);
        }

        context.Model.AddConstructors(ctorResult.Value!);

        return Result.Continue<IConcreteTypeBuilder>();
    }

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, EntityContext>> ToBuilder()
        => new AddConstructorFeatureBuilder(_formattableStringParser);

    private Result<ConstructorBuilder> CreateEntityConstructor(PipelineContext<IConcreteTypeBuilder, EntityContext> context)
    {
        var initializationResults = context.Context.SourceModel.Properties
            .Where(property => context.Context.SourceModel.IsMemberValidForBuilderClass(property, context.Context.Settings))
            .Select(property => _formattableStringParser.Parse("this.{EntityMemberName} = {InitializationExpression}{NullableRequiredSuffix};", context.Context.FormatProvider, new ParentChildContext<PipelineContext<IConcreteTypeBuilder, EntityContext>, Property>(context, property, context.Context.Settings)))
            .TakeWhileWithFirstNonMatching(x => x.IsSuccessful())
            .ToArray();

        var error = Array.Find(initializationResults, x => !x.IsSuccessful());
        if (error is not null)
        {
            return Result.FromExistingResult<ConstructorBuilder>(error);
        }

        return Result.Success(new ConstructorBuilder()
            .WithProtected(context.Context.Settings.InheritanceSettings.EnableInheritance && context.Context.Settings.InheritanceSettings.IsAbstract)
            .AddParameters(context.Context.SourceModel.Properties.CreateImmutableClassCtorParameters(context.Context.FormatProvider, context.Context.Settings.TypeSettings, context.Context.MapTypeName))
            .AddStringCodeStatements
            (
                context.Context.SourceModel.Properties
                    .Where(property => context.Context.SourceModel.IsMemberValidForBuilderClass(property, context.Context.Settings))
                    .Where(property => context.Context.Settings.NullCheckSettings.AddNullChecks && context.Context.Settings.AddValidationCode == ArgumentValidationType.None && property.Metadata.GetValue(MetadataNames.EntityNullCheck, () => !property.IsNullable && !property.IsValueType))
                    .Select(property => context.Context.CreateArgumentNullException(property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo()).GetCsharpFriendlyName()))
            )
            .AddStringCodeStatements(initializationResults.Select(x => x.Value!))
            .AddStringCodeStatements(CreateValidationCode(context, true))
            .WithChainCall(context.CreateEntityChainCall(false)));
    }

    private static IEnumerable<string> CreateValidationCode(PipelineContext<IConcreteTypeBuilder, EntityContext> context, bool baseClass)

    {
        var needValidation =
            context.Context.Settings.AddValidationCode == ArgumentValidationType.DomainOnly
            || (context.Context.Settings.AddValidationCode == ArgumentValidationType.Shared && baseClass);

        if (!needValidation)
        {
            yield break;
        }

        var customValidationCodeStatements = context.Context.SourceModel.Metadata.GetStringValues(MetadataNames.CustomEntityValidationCode).ToArray();
        if (customValidationCodeStatements.Length > 0)
        {
            foreach (var statement in customValidationCodeStatements)
            {
                yield return statement;
            }
        }
        else
        {
            yield return $"{typeof(Validator).FullName}.{nameof(Validator.ValidateObject)}(this, new {typeof(ValidationContext).FullName}(this, null, null), true);";
        }
    }
}
