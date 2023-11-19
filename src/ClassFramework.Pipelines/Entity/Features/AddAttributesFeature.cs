namespace ClassFramework.Pipelines.Entity.Features;

public class AddAttributesFeatureBuilder : IEntityFeatureBuilder
{
    public IPipelineFeature<ClassBuilder, EntityContext> Build()
        => new AddAttributesFeature();
}

public class AddAttributesFeature : IPipelineFeature<ClassBuilder, EntityContext>
{
    public Result<ClassBuilder> Process(PipelineContext<ClassBuilder, EntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (!context.Context.Settings.CopySettings.CopyAttributes)
        {
            return Result.Continue<ClassBuilder>();
        }

        context.Model.AddAttributes(context.Context.SourceModel.Attributes
            .Where(x => context.Context.Settings.CopySettings.CopyAttributePredicate?.Invoke(x) ?? true)
            .Select(x => new AttributeBuilder(context.Context.MapAttribute(x))));

        return Result.Continue<ClassBuilder>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, EntityContext>> ToBuilder()
        => new AddAttributesFeatureBuilder();
}
