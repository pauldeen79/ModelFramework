namespace ClassFramework.Pipelines.Builder.Features;

public class AddPropertiesFeatureBuilder : IBuilder<IPipelineFeature<ClassBuilder, BuilderPipelineBuilderContext>>
{
    public IPipelineFeature<ClassBuilder, BuilderPipelineBuilderContext> Build()
        => new MakePropertiesWritableFeature();
}

public class MakePropertiesWritableFeature : IPipelineFeature<ClassBuilder, BuilderPipelineBuilderContext>
{
    public void Process(PipelineContext<ClassBuilder, BuilderPipelineBuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model.AddProperties(
            context.Context.SourceModel.Properties.Select(
                x => new ClassPropertyBuilder(x).WithHasSetter().WithHasInitializer(false)));
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderPipelineBuilderContext>> ToBuilder()
        => new AddPropertiesFeatureBuilder();
}
