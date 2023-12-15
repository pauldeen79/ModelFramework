namespace ClassFramework.Pipelines.Entity.Features;

public class SetBaseClassFeatureBuilder : IEntityFeatureBuilder
{
    public IPipelineFeature<IConcreteTypeBuilder, EntityContext> Build()
        => new SetBaseClassFeature();
}

public class SetBaseClassFeature : IPipelineFeature<IConcreteTypeBuilder, EntityContext>
{
    public Result<IConcreteTypeBuilder> Process(PipelineContext<IConcreteTypeBuilder, EntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (context.Model is IBaseClassContainerBuilder baseClassContainerBuilder)
        {
            baseClassContainerBuilder.BaseClass = GetEntityBaseClass(context.Context.SourceModel, context);
        }

        return Result.Continue<IConcreteTypeBuilder>();
    }

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, EntityContext>> ToBuilder()
        => new SetBaseClassFeatureBuilder();

    private string GetEntityBaseClass(IType instance, PipelineContext<IConcreteTypeBuilder, EntityContext> context)
        => context.Context.Settings.InheritanceSettings.EnableInheritance && context.Context.Settings.InheritanceSettings.BaseClass is not null
            ? context.Context.Settings.InheritanceSettings.BaseClass.GetFullName()
            : instance.GetCustomValueForInheritedClass(context.Context.Settings, cls => Result.Success(cls.BaseClass!)).Value!; // we're always returning Success here, so we can shortcut the validation of the result by getting .Value
}
