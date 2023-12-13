namespace ClassFramework.Pipelines.Entity.Features;

public class AddAttributesFeatureBuilder : IEntityFeatureBuilder
{
    public IPipelineFeature<TypeBaseBuilder, EntityContext> Build()
        => new AddAttributesFeature();
}

public class AddAttributesFeature : IPipelineFeature<TypeBaseBuilder, EntityContext>
{
    public Result<TypeBaseBuilder> Process(PipelineContext<TypeBaseBuilder, EntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (!context.Context.Settings.CopySettings.CopyAttributes)
        {
            return Result.Continue<TypeBaseBuilder>();
        }

        context.Model.Attributes.AddRange(context.Context.SourceModel.Attributes
            .Where(x => context.Context.Settings.CopySettings.CopyAttributePredicate?.Invoke(x) ?? true)
            .Select(x => new AttributeBuilder(context.Context.MapAttribute(x))));

        return Result.Continue<TypeBaseBuilder>();
    }

    public IBuilder<IPipelineFeature<TypeBaseBuilder, EntityContext>> ToBuilder()
        => new AddAttributesFeatureBuilder();
}
