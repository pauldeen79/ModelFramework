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
                .AddStringCodeStatements(context.Context.CreatePragmaWarningDisableStatements())
                .AddStringCodeStatements($"var instance = {context.CreateEntityInstanciation(_formattableStringParser, "Base")};")
                .AddStringCodeStatements(context.Context.CreatePragmaWarningRestoreStatements())
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
}
