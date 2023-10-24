namespace ClassFramework.Pipelines.Builder.Features;

public class ValidationFeatureBuilder : IBuilderFeatureBuilder
{
    public IPipelineFeature<ClassBuilder, BuilderContext> Build() => new ValidationFeature();
}

public class ValidationFeature : IPipelineFeature<ClassBuilder, BuilderContext>
{
    public Result<ClassBuilder> Process(PipelineContext<ClassBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (!context.Context.Settings.ClassSettings.AllowGenerationWithoutProperties
            && !context.Context.SourceModel.Properties.Any()
            && !context.Context.Settings.ClassSettings.InheritanceSettings.EnableInheritance)
        {
            return Result.Invalid<ClassBuilder>("To create a builder class, there must be at least one property");
        }
        
        return Result.Continue<ClassBuilder>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderContext>> ToBuilder()
        => new ValidationFeatureBuilder();
}
