namespace ClassFramework.Pipelines.Builder;

public class PipelineBuilder : PipelineBuilder<ClassBuilder, PipelineBuilderContext>
{
    public PipelineBuilder()
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

    public PipelineBuilder(Pipeline<ClassBuilder, PipelineBuilderContext> source) : base(source)
    {
    }
}
