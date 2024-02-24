namespace ClassFramework.Pipelines.Entity.Features;

public class AddAttributesFeatureBuilder : IEntityFeatureBuilder
{
    public IPipelineFeature<IConcreteTypeBuilder, EntityContext> Build()
        => new AddAttributesFeature();
}

public class AddAttributesFeature : IPipelineFeature<IConcreteTypeBuilder, EntityContext>
{
    public Result<IConcreteTypeBuilder> Process(PipelineContext<IConcreteTypeBuilder, EntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (!context.Context.Settings.CopyAttributes)
        {
            return Result.Continue<IConcreteTypeBuilder>();
        }

        context.Model.AddAttributes(context.Context.SourceModel.Attributes
            .Where(x => context.Context.Settings.CopyAttributePredicate?.Invoke(x) ?? true)
            .Select(x => context.Context.MapAttribute(x).ToBuilder()));

        return Result.Continue<IConcreteTypeBuilder>();
    }

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, EntityContext>> ToBuilder()
        => new AddAttributesFeatureBuilder();
}
