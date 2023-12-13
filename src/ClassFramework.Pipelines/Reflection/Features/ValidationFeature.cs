namespace ClassFramework.Pipelines.Reflection.Features;

public class ValidationFeatureBuilder : IReflectionFeatureBuilder
{
    public IPipelineFeature<ClassBuilder, ReflectionContext> Build() => new ValidationFeature();
}

public class ValidationFeature : IPipelineFeature<ClassBuilder, ReflectionContext>
{
    public Result<ClassBuilder> Process(PipelineContext<ClassBuilder, ReflectionContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (!context.Context.Settings.GenerationSettings.AllowGenerationWithoutProperties
            && context.Context.SourceModel.GetProperties().Length == 0)
        {
            return Result.Invalid<ClassBuilder>("To create a class, there must be at least one property");
        }
        
        return Result.Continue<ClassBuilder>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, ReflectionContext>> ToBuilder()
        => new ValidationFeatureBuilder();
}
