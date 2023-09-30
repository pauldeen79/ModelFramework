namespace ClassFramework.Pipelines.Features;

public class AbstractBuilderFeatureBuilder : IBuilder<IPipelineFeature<ClassBuilder, BuilderPipelineBuilderSettings>>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AbstractBuilderFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<ClassBuilder, BuilderPipelineBuilderSettings> Build()
        => new AbstractBuilderFeature(_formattableStringParser);
}

public class AbstractBuilderFeature : IPipelineFeature<ClassBuilder, BuilderPipelineBuilderSettings>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AbstractBuilderFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public void Process(PipelineContext<ClassBuilder, BuilderPipelineBuilderSettings> context)
    {
        context = context.IsNotNull(nameof(context));

        if (context.Context.IsBuilderForAbstractEntity)
        {
            context.Model
                .AddGenericTypeArguments("TBuilder", "TEntity")
                .AddGenericTypeArgumentConstraints($"where TEntity : {context.Model.FormatInstanceName(false, context.Context.TypeSettings.FormatInstanceTypeNameDelegate)}")
                .AddGenericTypeArgumentConstraints($"where TBuilder : {_formattableStringParser.Parse(context.Context.NameSettings.BuilderNameFormatString, CultureInfo.CurrentCulture, context).GetValueOrThrow()}<TBuilder, TEntity>")
                .WithAbstract(context.Context.IsBuilderForAbstractEntity);
        }
        else if (context.Context.ClassSettings.ConstructorSettings.OriginalValidateArguments == ArgumentValidationType.Shared)
        {
            context.Model.AddInterfaces(typeof(IValidatableObject));
        }
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderPipelineBuilderSettings>> ToBuilder()
        => new AbstractBuilderFeatureBuilder(_formattableStringParser);
}
