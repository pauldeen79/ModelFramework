namespace ClassFramework.Pipelines.Features;

public class MakePropertiesWritableFeatureBuilder : IBuilder<IPipelineFeature<TypeBuilder, BuilderPipelineBuilderSettings>>
{
    public IPipelineFeature<TypeBuilder, BuilderPipelineBuilderSettings> Build()
        => new MakePropertiesWritableFeature();
}

public class MakePropertiesWritableFeature : IPipelineFeature<TypeBuilder, BuilderPipelineBuilderSettings>
{
    public void Process(PipelineContext<TypeBuilder, BuilderPipelineBuilderSettings> context)
    {
        context = context.IsNotNull(nameof(context));

        foreach (var prop in context.Model.Properties)
        {
            prop.WithHasSetter();
        }
    }

    public IBuilder<IPipelineFeature<TypeBuilder, BuilderPipelineBuilderSettings>> ToBuilder()
        => new MakePropertiesWritableFeatureBuilder();
}
