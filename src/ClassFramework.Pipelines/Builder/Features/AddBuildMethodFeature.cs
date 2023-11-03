namespace ClassFramework.Pipelines.Builder.Features;

public class AddBuildMethodFeatureBuilder : IBuilderFeatureBuilder
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddBuildMethodFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<ClassBuilder, BuilderContext> Build()
        => new AddBuildMethodFeature(_formattableStringParser);
}

public class AddBuildMethodFeature : IPipelineFeature<ClassBuilder, BuilderContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddBuildMethodFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public Result<ClassBuilder> Process(PipelineContext<ClassBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (context.Context.Settings.InheritanceSettings.EnableBuilderInheritance && context.Context.Settings.InheritanceSettings.IsAbstract)
        {
            return Result.Continue<ClassBuilder>();
        }

        var instanciationResult = context.CreateEntityInstanciation(_formattableStringParser, string.Empty);
        if (!instanciationResult.IsSuccessful())
        {
            return Result.FromExistingResult<ClassBuilder>(instanciationResult);
        }

        context.Model.AddMethods(new ClassMethodBuilder()
            .WithName(GetName(context))
            .WithAbstract(context.Context.IsBuilderForAbstractEntity)
            .WithOverride(context.Context.IsBuilderForOverrideEntity)
            .WithTypeName($"{GetImmutableBuilderBuildMethodReturnType(context.Context)}{context.Context.SourceModel.GetGenericTypeArgumentsString()}")
            .AddStringCodeStatements(context.Context.CreatePragmaWarningDisableStatements())
            .AddStringCodeStatements
            (
                !context.Context.IsBuilderForAbstractEntity
                    ? new[] { $"return {instanciationResult.Value};" }
                    : Array.Empty<string>()
            )
            .AddStringCodeStatements(context.Context.CreatePragmaWarningRestoreStatements()));

        if (context.Context.IsBuilderForAbstractEntity)
        {
            var baseClass = context.Context.Settings.InheritanceSettings.BaseClass ?? context.Context.SourceModel;
            context.Model.AddMethods(new ClassMethodBuilder()
                .WithName(context.Context.Settings.NameSettings.BuildMethodName)
                .WithOverride()
                .WithTypeName($"{baseClass.GetFullName()}{baseClass.GetGenericTypeArgumentsString()}")
                .AddStringCodeStatements($"return {context.Context.Settings.NameSettings.BuildTypedMethodName}();"));
        }

        return Result.Continue<ClassBuilder>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderContext>> ToBuilder()
        => new AddBuildMethodFeatureBuilder(_formattableStringParser);

    private static string GetName(PipelineContext<ClassBuilder, BuilderContext> context)
        => context.Context.IsBuilderForAbstractEntity || context.Context.IsBuilderForOverrideEntity
            ? context.Context.Settings.NameSettings.BuildTypedMethodName
            : context.Context.Settings.NameSettings.BuildMethodName;

    private static string GetImmutableBuilderBuildMethodReturnType(BuilderContext context)
        => context.IsBuilderForAbstractEntity
            ? "TEntity"
            : context.SourceModel.GetFullName();
}
