namespace ClassFramework.Pipelines.Features;

public class MakePropertiesWritableFeatureBuilder : IBuilder<IPipelineFeature<ClassBuilder, BuilderPipelineBuilderSettings>>
{
    public IPipelineFeature<ClassBuilder, BuilderPipelineBuilderSettings> Build()
        => new MakePropertiesWritableFeature();
}

public class MakePropertiesWritableFeature : IPipelineFeature<ClassBuilder, BuilderPipelineBuilderSettings>
{
    public void Process(PipelineContext<ClassBuilder, BuilderPipelineBuilderSettings> context)
    {
        context = context.IsNotNull(nameof(context));

        foreach (var prop in context.Model.Properties)
        {
            prop.WithHasSetter();
        }
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderPipelineBuilderSettings>> ToBuilder()
        => new MakePropertiesWritableFeatureBuilder();
}
