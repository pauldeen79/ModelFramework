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

    public Result<ClassBuilder> Process(PipelineContext<ClassBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (context.Context.IsBuilderForAbstractEntity || context.Context.Settings.ClassSettings.ConstructorSettings.OriginalValidateArguments != ArgumentValidationType.Shared)
        {
            return Result.Continue<ClassBuilder>();
        }

        var instanciationResult = context.CreateEntityInstanciation(_formattableStringParser, "Base");
        if (!instanciationResult.IsSuccessful())
        {
            return Result.FromExistingResult<ClassBuilder>(instanciationResult);
        }

        context.Model.AddInterfaces(typeof(IValidatableObject));
        context.Model.AddMethods(new ClassMethodBuilder()
            .WithName(nameof(IValidatableObject.Validate))
            .WithType(typeof(IEnumerable<ValidationResult>))
            .AddParameter("validationContext", typeof(ValidationContext))
            .AddStringCodeStatements(context.Context.CreatePragmaWarningDisableStatements())
            .AddStringCodeStatements($"var instance = {instanciationResult.Value};")
            .AddStringCodeStatements(context.Context.CreatePragmaWarningRestoreStatements())
            .AddStringCodeStatements
            (
                context.Context.SourceModel.Metadata.GetStringValues(MetadataNames.CustomBuilderValidationCode).WhenEmpty(() =>
                new[]
                {
                    $"var results = new {typeof(List<>).ReplaceGenericTypeName(typeof(ValidationResult))}();",
                    $"{typeof(Validator).FullName}.{nameof(Validator.TryValidateObject)}(instance, new {typeof(ValidationContext).FullName}(instance), results, true);",
                    "return results;"
                })
            )
        );

        return Result.Continue<ClassBuilder>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderContext>> ToBuilder()
        => new ValidatableObjectFeatureBuilder(_formattableStringParser);
}
