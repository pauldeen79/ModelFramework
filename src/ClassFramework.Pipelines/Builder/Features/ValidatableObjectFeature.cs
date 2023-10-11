namespace ClassFramework.Pipelines.Builder.Features;

public class ValidatableObjectFeatureBuilder : IBuilderFeatureBuilder
{
    private readonly IFormattableStringParser _formattableStringParser;

    public ValidatableObjectFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<ClassBuilder, BuilderContext> Build()
        => new ValidatableObjectFeature(_formattableStringParser);
}

public class ValidatableObjectFeature : IPipelineFeature<ClassBuilder, BuilderContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public ValidatableObjectFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public void Process(PipelineContext<ClassBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (!context.Context.IsBuilderForAbstractEntity && context.Context.Settings.ClassSettings.ConstructorSettings.OriginalValidateArguments == ArgumentValidationType.Shared)
        {
            context.Model.AddInterfaces(typeof(IValidatableObject));
            context.Model.AddMethods(new ClassMethodBuilder()
                .WithName(nameof(IValidatableObject.Validate))
                .WithType(typeof(IEnumerable<ValidationResult>))
                .AddParameter("validationContext", typeof(ValidationContext))
                .AddStringCodeStatements(CreatePragmaWarningDisableStatements(context.Context))
                .AddStringCodeStatements($"var instance = {CreateEntityInstanciation(context, "Base")};")
                .AddStringCodeStatements(CreatePragmaWarningRestoreStatements(context.Context))
                .AddStringCodeStatements
                (
                    $"var results = new {typeof(List<>).WithoutGenerics()}<{typeof(ValidationResult).FullName}>();",
                    $"{typeof(Validator).FullName}.{nameof(Validator.TryValidateObject)}(instance, new {typeof(ValidationContext).FullName}(instance), results, true);",
                    "return results;"
                )
            );
        }
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderContext>> ToBuilder()
        => new ValidatableObjectFeatureBuilder(_formattableStringParser);

    private string CreateEntityInstanciation(PipelineContext<ClassBuilder, BuilderContext> context, string classNameSuffix)
    {
        var openSign = GetImmutableBuilderPocoOpenSign(context.Context.SourceModel.IsPoco() && context.Context.SourceModel.Properties.Any());
        var closeSign = GetImmutableBuilderPocoCloseSign(context.Context.SourceModel.IsPoco() && context.Context.SourceModel.Properties.Any());
        return $"new {context.Context.SourceModel.FormatInstanceName(true, context.Context.Settings.TypeSettings.FormatInstanceTypeNameDelegate)}{classNameSuffix}{context.Context.SourceModel.GetGenericTypeArgumentsString()}{openSign}{GetConstructionMethodParameters(context.Context.SourceModel, context)}{closeSign}";
    }

    private string GetConstructionMethodParameters(TypeBase instance, PipelineContext<ClassBuilder, BuilderContext> context)
    {
        var poco = instance.IsPoco();
        var properties = instance.GetImmutableBuilderConstructorProperties(context.Context, poco);

        var defaultValueDelegate = poco
            ? new Func<ClassProperty, string>(p => $"{p.Name} = {p.Name}")
            : new Func<ClassProperty, string>(p => $"{p.Name}");

        return string.Join
        (
            ", ",
            properties.Select
            (
                p => _formattableStringParser.Parse
                (
                    p.Metadata.GetStringValue(MetadataNames.CustomBuilderMethodParameterExpression, defaultValueDelegate(p)),
                    context.Context.FormatProvider,
                    new ParentChildContext<ClassProperty>(context, p)
                ).GetValueOrThrow()
            )
        );
    }

    private static string GetImmutableBuilderPocoCloseSign(bool poco)
        => poco
            ? " }"
            : ")";

    private static string GetImmutableBuilderPocoOpenSign(bool poco)
        => poco
            ? " { "
            : "(";
    private static string[] CreatePragmaWarningDisableStatements(BuilderContext context)
        => context.Settings.GenerationSettings.EnableNullableReferenceTypes
        && !context.IsBuilderForAbstractEntity
        && !context.Settings.GenerationSettings.AddNullChecks
            ? new[]
            {
                "#pragma warning disable CS8604 // Possible null reference argument.",
                "#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.",
            }
            : Array.Empty<string>();

    private static string[] CreatePragmaWarningRestoreStatements(BuilderContext context)
        => context.Settings.GenerationSettings.EnableNullableReferenceTypes
        && !context.IsBuilderForAbstractEntity
        && !context.Settings.GenerationSettings.AddNullChecks
            ? new[]
            {
                "#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.",
                "#pragma warning restore CS8604 // Possible null reference argument.",
            }
            : Array.Empty<string>();
}
