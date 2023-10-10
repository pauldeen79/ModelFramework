namespace ClassFramework.Pipelines.Builder.Features;

public class ValidationFeatureBuilder : IBuilderFeatureBuilder
{
    public IPipelineFeature<ClassBuilder, BuilderContext> Build()
        => new ValidationFeature();
}
public class ValidationFeature : IPipelineFeature<ClassBuilder, BuilderContext>
{
    public void Process(PipelineContext<ClassBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (!context.Context.Settings.ClassSettings.AllowGenerationWithoutProperties && !context.Context.SourceModel.Properties.Any() && !context.Context.Settings.InheritanceSettings.EnableEntityInheritance)
        {
            throw new InvalidOperationException("To create an immutable builder class, there must be at least one property");
        }
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderContext>> ToBuilder()
        => new ValidationFeatureBuilder();
}
