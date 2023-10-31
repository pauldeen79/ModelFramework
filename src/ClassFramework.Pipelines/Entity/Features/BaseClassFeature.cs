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

        var baseClassResult = GetEntityBaseClass(context.Context.SourceModel, context);
        if (!baseClassResult.IsSuccessful())
        {
            return Result.FromExistingResult<ClassBuilder>(baseClassResult);
        }

        context.Model.BaseClass = baseClassResult.Value!;

        return Result.Continue<ClassBuilder>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, EntityContext>> ToBuilder()
        => new BaseClassFeatureBuilder();

    private Result<string> GetEntityBaseClass(TypeBase instance, PipelineContext<ClassBuilder, EntityContext> context)
        => context.Context.Settings.InheritanceSettings.EnableInheritance && context.Context.Settings.InheritanceSettings.BaseClass is not null
            ? Result.Success(context.Context.Settings.InheritanceSettings.BaseClass.GetFullName())
            : instance.GetCustomValueForInheritedClass(context.Context.Settings, cls => Result.Success(cls.BaseClass!));
}
