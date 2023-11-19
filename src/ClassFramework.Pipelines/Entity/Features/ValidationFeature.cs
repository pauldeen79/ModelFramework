namespace ClassFramework.Pipelines.Entity.Features;

public class ValidationFeatureBuilder : IEntityFeatureBuilder
{
    public IPipelineFeature<ClassBuilder, EntityContext> Build() => new ValidationFeature();
}

public class ValidationFeature : IPipelineFeature<ClassBuilder, EntityContext>
{
    public Result<ClassBuilder> Process(PipelineContext<ClassBuilder, EntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (!context.Context.Settings.GenerationSettings.AllowGenerationWithoutProperties
            && context.Context.SourceModel.Properties.Count == 0
            && !context.Context.Settings.InheritanceSettings.EnableInheritance)
        {
            return Result.Invalid<ClassBuilder>("To create an entity class, there must be at least one property");
        }
        
        return Result.Continue<ClassBuilder>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, EntityContext>> ToBuilder()
        => new ValidationFeatureBuilder();
}
