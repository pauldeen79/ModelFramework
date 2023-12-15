namespace ClassFramework.Pipelines.Reflection.Features;

public class SetBaseClassFeatureBuilder : IReflectionFeatureBuilder
{
    public IPipelineFeature<TypeBaseBuilder, ReflectionContext> Build()
        => new SetBaseClassFeature();
}

public class SetBaseClassFeature : IPipelineFeature<TypeBaseBuilder, ReflectionContext>
{
    public Result<TypeBaseBuilder> Process(PipelineContext<TypeBaseBuilder, ReflectionContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (context.Model is IBaseClassContainerBuilder baseClassContainerBuilder)
        {
            baseClassContainerBuilder.BaseClass = GetEntityBaseClass(context.Context.SourceModel, context);
        }

        return Result.Continue<TypeBaseBuilder>();
    }

    public IBuilder<IPipelineFeature<TypeBaseBuilder, ReflectionContext>> ToBuilder()
        => new SetBaseClassFeatureBuilder();

    private string GetEntityBaseClass(Type instance, PipelineContext<TypeBaseBuilder, ReflectionContext> context)
    {
        if (context.Context.Settings.InheritanceSettings.EnableInheritance
            && context.Context.Settings.InheritanceSettings.BaseClass is not null)
        {
            return context.Context.Settings.InheritanceSettings.BaseClass.GetFullName();
        }

        if (instance.BaseType is null || instance.BaseType == typeof(object))
        {
            return string.Empty;
        }

        return instance.BaseType.FullName.FixTypeName();
    }
}
