namespace ClassFramework.Pipelines.Entity.Features;

public class AddInterfacesFeatureBuilder : IEntityFeatureBuilder
{
    public IPipelineFeature<ClassBuilder, EntityContext> Build()
        => new AddInterfacesFeature();
}

public class AddInterfacesFeature : IPipelineFeature<ClassBuilder, EntityContext>
{
    public Result<ClassBuilder> Process(PipelineContext<ClassBuilder, EntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (context.Context.Settings.GenerationSettings.CopyInterfacePredicate is null)
        {
            return Result.Continue<ClassBuilder>();
        }

        context.Model.AddInterfaces(context.Context.Model.Interfaces
            .Where(context.Context.Settings.GenerationSettings.CopyInterfacePredicate.Invoke)
            .Select(context.Context.MapTypeName));

        return Result.Continue<ClassBuilder>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, EntityContext>> ToBuilder()
        => new AddInterfacesFeatureBuilder();
}
