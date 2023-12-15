namespace ClassFramework.Pipelines.Reflection.Features;

public class SetVisibilityFeatureBuilder : IReflectionFeatureBuilder
{
    public IPipelineFeature<TypeBaseBuilder, ReflectionContext> Build()
        => new SetVisibilityFeature();
}

public class SetVisibilityFeature : IPipelineFeature<TypeBaseBuilder, ReflectionContext>
{
    public Result<TypeBaseBuilder> Process(PipelineContext<TypeBaseBuilder, ReflectionContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (!context.Context.Settings.CopySettings.CopyAttributes)
        {
            return Result.Continue<TypeBaseBuilder>();
        }

        if (context.Context.SourceModel.IsPublic)
        {
            context.Model.Visibility = Visibility.Public;
        }
        else
        {
            context.Model.Visibility = context.Context.SourceModel.IsNotPublic
                ? Visibility.Internal
                : Visibility.Private;
        }

        return Result.Continue<TypeBaseBuilder>();
    }

    public IBuilder<IPipelineFeature<TypeBaseBuilder, ReflectionContext>> ToBuilder()
        => new SetVisibilityFeatureBuilder();
}
