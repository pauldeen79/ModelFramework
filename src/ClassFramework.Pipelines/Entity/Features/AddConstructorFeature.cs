namespace ClassFramework.Pipelines.Entity.Features;

public class AddConstructorFeatureBuilder : IEntityFeatureBuilder
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddConstructorFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<ClassBuilder, EntityContext> Build()
        => new AddConstructorFeature(_formattableStringParser);
}

public class AddConstructorFeature : IPipelineFeature<ClassBuilder, EntityContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddConstructorFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public Result<ClassBuilder> Process(PipelineContext<ClassBuilder, EntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model.AddConstructors(CreateEntityConstructor(context));

        return Result.Continue<ClassBuilder>();
    }


    public IBuilder<IPipelineFeature<ClassBuilder, EntityContext>> ToBuilder()
        => new SetNameFeatureBuilder(_formattableStringParser);

    private ClassConstructorBuilder CreateEntityConstructor(PipelineContext<ClassBuilder, EntityContext> context)
        => new ClassConstructorBuilder()
            .WithProtected(context.Context.Settings.InheritanceSettings.EnableInheritance && context.Context.Settings.InheritanceSettings.IsAbstract)
            .AddParameters(context.CreateImmutableClassCtorParameters())
            .AddStringCodeStatements
            (
                context.Context.Model.Properties
                    .Where(property => context.Context.Model.IsMemberValidForBuilderClass(property, context.Context.Settings))
                    .Where(property => context.Context.Settings.GenerationSettings.AddNullChecks && property.Metadata.GetValue(MetadataNames.EntityNullCheck, () => !property.IsNullable && !property.IsValueType))
                    .Select(property => context.Context.CreateArgumentNullException(property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo()).GetCsharpFriendlyName()))
            )
            .AddStringCodeStatements
            (
                context.Context.Model.Properties
                    .Where(property => context.Context.Model.IsMemberValidForBuilderClass(property, context.Context.Settings))
                    .Select(property => _formattableStringParser.Parse("this.{Name} = {InitializationExpression}{NullableRequiredSuffix};", context.Context.FormatProvider, new ParentChildContext<EntityContext, ClassProperty>(context, property, context.Context.Settings)).GetValueOrThrow())
            )
            .AddStringCodeStatements(CreateValidationCode(context, true))
            .WithChainCall(context.CreateEntityChainCall(false));

    private static IEnumerable<string> CreateValidationCode(PipelineContext<ClassBuilder, EntityContext> context, bool baseClass)

    {
        var needValidation =
            context.Context.Settings.AddValidationCode == ArgumentValidationType.DomainOnly
            || (context.Context.Settings.AddValidationCode == ArgumentValidationType.Shared && baseClass);

        if (!needValidation)
        {
            yield break;
        }

        var customValidationCodeStatements = context.Context.Model.Metadata.GetStringValues(MetadataNames.CustomEntityValidationCode).ToArray();
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
