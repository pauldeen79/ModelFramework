namespace ClassFramework.Pipelines.BuilderInterface.Features;

public class ValidationFeatureBuilder : IBuilderInterfaceFeatureBuilder
{
    public IPipelineFeature<IConcreteTypeBuilder, BuilderInterfaceContext> Build() => new ValidationFeature();
}

public class ValidationFeature : IPipelineFeature<IConcreteTypeBuilder, BuilderInterfaceContext>
{
    public Result<IConcreteTypeBuilder> Process(PipelineContext<IConcreteTypeBuilder, BuilderInterfaceContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (!context.Context.Settings.AllowGenerationWithoutProperties
            && context.Context.SourceModel.Properties.Count == 0
            && !context.Context.Settings.EnableInheritance)
        {
            return Result.Invalid<IConcreteTypeBuilder>("To create a builder extensions class, there must be at least one property");
        }
        
        return Result.Continue<IConcreteTypeBuilder>();
    }

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, BuilderInterfaceContext>> ToBuilder()
        => new ValidationFeatureBuilder();
}
