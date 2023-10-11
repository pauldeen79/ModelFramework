namespace ClassFramework.Pipelines.Builder.Features;

public class ValidatableObjectFeatureBuilder : IBuilderFeatureBuilder
{
    public IPipelineFeature<ClassBuilder, BuilderContext> Build()
        => new ValidatableObjectFeature();
}

public class ValidatableObjectFeature : IPipelineFeature<ClassBuilder, BuilderContext>
{
    public void Process(PipelineContext<ClassBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (!context.Context.IsBuilderForAbstractEntity && context.Context.Settings.ClassSettings.ConstructorSettings.OriginalValidateArguments == ArgumentValidationType.Shared)
        {
            context.Model.AddInterfaces(typeof(IValidatableObject));
        }
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderContext>> ToBuilder()
        => new ValidatableObjectFeatureBuilder();
}
