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

    public void Process(PipelineContext<ClassBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (context.Context.Settings.InheritanceSettings.EnableBuilderInheritance && context.Context.Settings.InheritanceSettings.IsAbstract)
        {
            return;
        }

        context.Model.AddMethods(new ClassMethodBuilder()
            .WithName(context.Context.IsBuilderForAbstractEntity || context.Context.IsBuilderForOverrideEntity
                ? context.Context.Settings.NameSettings.BuildTypedMethodName
                : context.Context.Settings.NameSettings.BuildMethodName)
            .WithAbstract(context.Context.IsBuilderForAbstractEntity)
            .WithOverride(context.Context.IsBuilderForOverrideEntity)
            .WithTypeName($"{GetImmutableBuilderBuildMethodReturnType(context.Context)}{context.Context.SourceModel.GetGenericTypeArgumentsString()}")
            .AddStringCodeStatements(context.Context.CreatePragmaWarningDisableStatements())
            .AddStringCodeStatements
            (
                !context.Context.IsBuilderForAbstractEntity
                    ? new[] { $"return {context.CreateEntityInstanciation(_formattableStringParser, string.Empty)};" }
                    : Array.Empty<string>()
            )
            .AddStringCodeStatements(context.Context.CreatePragmaWarningRestoreStatements()));

        if (context.Context.IsBuilderForAbstractEntity)
        {
            context.Model.AddMethods(new ClassMethodBuilder()
                .WithName(context.Context.Settings.NameSettings.BuildMethodName)
                .WithOverride()
                .WithTypeName((context.Context.Settings.InheritanceSettings.BaseClass ?? context.Context.SourceModel).FormatInstanceName(false, context.Context.Settings.TypeSettings.FormatInstanceTypeNameDelegate) + (context.Context.Settings.InheritanceSettings.BaseClass ?? context.Context.SourceModel).GetGenericTypeArgumentsString())
                .AddStringCodeStatements($"return {context.Context.Settings.NameSettings.BuildTypedMethodName}();"));
        }
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderContext>> ToBuilder()
        => new AddBuildMethodFeatureBuilder(_formattableStringParser);

    private static string GetImmutableBuilderBuildMethodReturnType(BuilderContext context)
        => context.IsBuilderForAbstractEntity
            ? "TEntity"
            : context.SourceModel.FormatInstanceName(false, context.Settings.TypeSettings.FormatInstanceTypeNameDelegate);
}
