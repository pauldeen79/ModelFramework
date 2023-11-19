namespace ClassFramework.Pipelines.Entity.Features;

public class BaseClassFeatureBuilder : IEntityFeatureBuilder
{
    public IPipelineFeature<ClassBuilder, EntityContext> Build()
        => new BaseClassFeature();
}

public class BaseClassFeature : IPipelineFeature<ClassBuilder, EntityContext>
{
    public Result<ClassBuilder> Process(PipelineContext<ClassBuilder, EntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model.BaseClass = GetEntityBaseClass(context.Context.SourceModel, context);

        return Result.Continue<ClassBuilder>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, EntityContext>> ToBuilder()
        => new BaseClassFeatureBuilder();

    private string GetEntityBaseClass(TypeBase instance, PipelineContext<ClassBuilder, EntityContext> context)
        => context.Context.Settings.InheritanceSettings.EnableInheritance && context.Context.Settings.InheritanceSettings.BaseClass is not null
            ? context.Context.Settings.InheritanceSettings.BaseClass.GetFullName()
            : instance.GetCustomValueForInheritedClass(context.Context.Settings, cls => Result.Success(cls.BaseClass!)).Value!; // we're always returning Success here, so we can shortcut the validation of the result by getting .Value
}
