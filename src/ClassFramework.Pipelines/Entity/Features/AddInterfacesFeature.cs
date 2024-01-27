namespace ClassFramework.Pipelines.Entity.Features;

public class AddInterfacesFeatureBuilder : IEntityFeatureBuilder
{
    public IPipelineFeature<IConcreteTypeBuilder, EntityContext> Build()
        => new AddInterfacesFeature();
}

public class AddInterfacesFeature : IPipelineFeature<IConcreteTypeBuilder, EntityContext>
{
    public Result<IConcreteTypeBuilder> Process(PipelineContext<IConcreteTypeBuilder, EntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (!context.Context.Settings.CopySettings.CopyInterfaces
            || (context.Context.IsSharedBaseClass))
        {
            return Result.Continue<IConcreteTypeBuilder>();
        }

        var baseClass = context.Context.SourceModel.GetEntityBaseClass(context.Context.Settings.InheritanceSettings.EnableInheritance, context.Context.Settings.InheritanceSettings.BaseClass);

        context.Model.AddInterfaces(context.Context.SourceModel.Interfaces
            .Where(x => context.Context.Settings.CopySettings.CopyInterfacePredicate?.Invoke(x) ?? true)
            .Where(x => x != baseClass)
            .Select(x => context.Context.MapTypeName(x.FixTypeName())));

        return Result.Continue<IConcreteTypeBuilder>();
    }

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, EntityContext>> ToBuilder()
        => new AddInterfacesFeatureBuilder();
}
