namespace ClassFramework.Pipelines.Builder.Features;

public class AddAttributesFeatureBuilder : IBuilderFeatureBuilder
{
    public IPipelineFeature<ClassBuilder, BuilderContext> Build()
        => new AddAttributesFeature();
}

public class AddAttributesFeature : IPipelineFeature<ClassBuilder, BuilderContext>
{
    public Result<BuilderContext> Process(PipelineContext<ClassBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (!context.Context.Settings.GenerationSettings.CopyAttributes)
        {
            return Result.Continue<BuilderContext>();
        }

        context.Model.AddAttributes(context.Context.SourceModel.Attributes.Select(x => new AttributeBuilder(x)));

        return Result.Continue<BuilderContext>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderContext>> ToBuilder()
        => new AddAttributesFeatureBuilder();
}
