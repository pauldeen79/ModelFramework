namespace ClassFramework.Pipelines.Builder.Features;

public class AddAttributesFeatureBuilder : IBuilderFeatureBuilder
{
    public IPipelineFeature<ClassBuilder, BuilderContext> Build()
        => new AddAttributesFeature();
}

public class AddAttributesFeature : IPipelineFeature<ClassBuilder, BuilderContext>
{
    public Result<ClassBuilder> Process(PipelineContext<ClassBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (!context.Context.Settings.EntitySettings.CopySettings.CopyAttributes)
        {
            return Result.Continue<ClassBuilder>();
        }

        context.Model.AddAttributes(context.Context.Model.Attributes
            .Where(x => context.Context.Settings.EntitySettings.CopySettings.CopyAttributePredicate?.Invoke(x) ?? true)
            .Select(x => new AttributeBuilder(context.Context.MapAttribute(x))));

        return Result.Continue<ClassBuilder>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderContext>> ToBuilder()
        => new AddAttributesFeatureBuilder();
}
