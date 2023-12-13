namespace ClassFramework.Pipelines.Entity.Features;

public class AddInterfacesFeatureBuilder : IEntityFeatureBuilder
{
    public IPipelineFeature<TypeBaseBuilder, EntityContext> Build()
        => new AddInterfacesFeature();
}

public class AddInterfacesFeature : IPipelineFeature<TypeBaseBuilder, EntityContext>
{
    public Result<TypeBaseBuilder> Process(PipelineContext<TypeBaseBuilder, EntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (!context.Context.Settings.CopySettings.CopyInterfaces)
        {
            return Result.Continue<TypeBaseBuilder>();
        }

        context.Model.Interfaces.AddRange(context.Context.SourceModel.Interfaces
            .Where(x => context.Context.Settings.CopySettings.CopyInterfacePredicate?.Invoke(x) ?? true)
            .Select(x => context.Context.MapTypeName(x.FixTypeName())));

        return Result.Continue<TypeBaseBuilder>();
    }

    public IBuilder<IPipelineFeature<TypeBaseBuilder, EntityContext>> ToBuilder()
        => new AddInterfacesFeatureBuilder();
}
