namespace ClassFramework.Pipelines.Reflection.Features;

public class AddInterfacesFeatureBuilder : IReflectionFeatureBuilder
{
    public IPipelineFeature<TypeBaseBuilder, ReflectionContext> Build()
        => new AddInterfacesFeature();
}

public class AddInterfacesFeature : IPipelineFeature<TypeBaseBuilder, ReflectionContext>
{
    public Result<TypeBaseBuilder> Process(PipelineContext<TypeBaseBuilder, ReflectionContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (!context.Context.Settings.CopySettings.CopyInterfaces)
        {
            return Result.Continue<TypeBaseBuilder>();
        }

        context.Model.Interfaces.AddRange(context.Context.SourceModel.GetInterfaces()
            .Select(x => x.FullName.FixTypeName())
            .Where(x => context.Context.Settings.CopySettings.CopyInterfacePredicate?.Invoke(x) ?? true)
            .Select(x => context.Context.MapTypeName(x)));

        return Result.Continue<TypeBaseBuilder>();
    }

    public IBuilder<IPipelineFeature<TypeBaseBuilder, ReflectionContext>> ToBuilder()
        => new AddInterfacesFeatureBuilder();
}
