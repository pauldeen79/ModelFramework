namespace ClassFramework.Pipelines.Builder;

public class PipelineBuilder : PipelineBuilder<ClassBuilder, BuilderContext>
{
    public PipelineBuilder(
        IEnumerable<ISharedFeatureBuilder> sharedFeatureBuilders,
        IEnumerable<IBuilderFeatureBuilder> builderFeatureBuilders)
    {
        AddFeatures(sharedFeatureBuilders);
        AddFeatures(builderFeatureBuilders);
    }

    public PipelineBuilder(Pipeline<ClassBuilder, BuilderContext> source) : base(source)
    {
    }

    protected override void Initialize(ClassBuilder model, PipelineContext<ClassBuilder, BuilderContext> pipelineContext)
    {
        pipelineContext = pipelineContext.IsNotNull(nameof(pipelineContext));

        if (!pipelineContext.Context.Settings.ClassSettings.AllowGenerationWithoutProperties
            && !pipelineContext.Context.SourceModel.Properties.Any()
            && !pipelineContext.Context.Settings.ClassSettings.InheritanceSettings.EnableInheritance)
        {
            throw new InvalidOperationException("To create a builder class, there must be at least one property");
        }
    }
}
