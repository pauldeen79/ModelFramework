namespace ClassFramework.Pipelines.Builder.Features;

public class AddInterfacesFeatureBuilder : IBuilderFeatureBuilder
{
    public IPipelineFeature<ClassBuilder, BuilderContext> Build()
        => new AddInterfacesFeature();
}

public class AddInterfacesFeature : IPipelineFeature<ClassBuilder, BuilderContext>
{
    public Result<ClassBuilder> Process(PipelineContext<ClassBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (!context.Context.Settings.GenerationSettings.CopyInterfaces)
        {
            return Result.Continue<ClassBuilder>();
        }

        context.Model.AddInterfaces(context.Context.SourceModel.Interfaces
            .Where(x => context.Context.Settings.GenerationSettings.CopyInterfacePredicate?.Invoke(x) ?? true)
            .Select(context.Context.MapTypeName));

        return Result.Continue<ClassBuilder>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderContext>> ToBuilder()
        => new AddInterfacesFeatureBuilder();
}
