namespace ClassFramework.Pipelines.Entity.Features;

public class ValidationFeatureBuilder : IEntityFeatureBuilder
{
    public IPipelineFeature<TypeBaseBuilder, EntityContext> Build() => new ValidationFeature();
}

public class ValidationFeature : IPipelineFeature<TypeBaseBuilder, EntityContext>
{
    public Result<TypeBaseBuilder> Process(PipelineContext<TypeBaseBuilder, EntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (!context.Context.Settings.GenerationSettings.AllowGenerationWithoutProperties
            && context.Context.SourceModel.Properties.Count == 0
            && !context.Context.Settings.InheritanceSettings.EnableInheritance)
        {
            return Result.Invalid<TypeBaseBuilder>("To create an entity class, there must be at least one property");
        }
        
        return Result.Continue<TypeBaseBuilder>();
    }

    public IBuilder<IPipelineFeature<TypeBaseBuilder, EntityContext>> ToBuilder()
        => new ValidationFeatureBuilder();
}
