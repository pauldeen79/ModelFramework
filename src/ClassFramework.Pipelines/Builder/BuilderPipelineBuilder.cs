namespace ClassFramework.Pipelines.Builder;

public class BuilderPipelineBuilder : PipelineBuilder<ClassBuilder, BuilderPipelineBuilderContext>
{
    public BuilderPipelineBuilder()
    {
        var formattableStringParser = FormattableStringParser.Create
        (
            new ContextSourceModelProcessor(),
            new ClassPropertyProcessor()
        );

        AddFeatures
        (
            new PartialFeatureBuilder(),
            new SetNameFeatureBuilder(formattableStringParser),
            new AbstractBuilderFeatureBuilder(formattableStringParser),
            new AddPropertiesFeatureBuilder(formattableStringParser)
        );
    }

    public BuilderPipelineBuilder(Pipeline<ClassBuilder, BuilderPipelineBuilderContext> source) : base(source)
    {
    }
}
