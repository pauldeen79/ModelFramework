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

        foreach (var prop in context.Context.SourceModel.Properties)
        {
            var newProp = new ClassPropertyBuilder(prop);
            newProp.HasSetter = true;
            newProp.HasInitializer = false;
            context.Model.AddProperties(newProp);
        }
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderPipelineBuilderContext>> ToBuilder()
        => new AddPropertiesFeatureBuilder();
}
