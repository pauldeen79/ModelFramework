namespace ClassFramework.Pipelines.Builder.Features;

public class AddFieldsFeatureBuilder : IBuilderFeatureBuilder
{
    public IPipelineFeature<ClassBuilder, BuilderContext> Build()
        => new AddFieldsFeature();
}

public class AddFieldsFeature : IPipelineFeature<ClassBuilder, BuilderContext>
{
    public void Process(PipelineContext<ClassBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (context.Context.IsAbstractBuilder
            || !context.Context.Settings.GenerationSettings.CopyFields
            || context.Context.SourceModel is not Class cls)
        {
            return;
        }

        context.Model.AddFields
        (
            cls.Fields
                .Where(x => context.Context.SourceModel.IsMemberValidForImmutableBuilderClass(x, context.Context.Settings))
                .Select(x => new ClassFieldBuilder(x).WithProtected())
        );
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderContext>> ToBuilder()
        => new AddFieldsFeatureBuilder();
}
