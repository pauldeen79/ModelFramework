namespace ClassFramework.Pipelines.BuilderInterface.Features;

public class SetStaticFeatureBuilder : IBuilderInterfaceFeatureBuilder
{
    public IPipelineFeature<IConcreteTypeBuilder, BuilderInterfaceContext> Build()
        => new SetStaticFeature();
}

public class SetStaticFeature : IPipelineFeature<IConcreteTypeBuilder, BuilderInterfaceContext>
{
    public Result<IConcreteTypeBuilder> Process(PipelineContext<IConcreteTypeBuilder, BuilderInterfaceContext> context)
    {
        context = context.IsNotNull(nameof(context));

        (context.Model as IReferenceTypeBuilder)?.WithStatic();

        return Result.Continue<IConcreteTypeBuilder>();
    }

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, BuilderInterfaceContext>> ToBuilder()
        => new SetStaticFeatureBuilder();
}
