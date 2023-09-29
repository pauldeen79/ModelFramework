namespace ClassFramework.Pipelines.Features;

public class AbstractBuilderFeatureBuilder : IBuilder<IPipelineFeature<TypeBuilder, BuilderPipelineBuilderSettings>>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AbstractBuilderFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<TypeBuilder, BuilderPipelineBuilderSettings> Build()
        => new AbstractBuilderFeature(_formattableStringParser);
}

public class AbstractBuilderFeature : FeatureBase
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AbstractBuilderFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public override void Process(PipelineContext<TypeBuilder, BuilderPipelineBuilderSettings> context)
    {
        context = context.IsNotNull(nameof(context));

        if (context.Model is not ClassBuilder classBuilder)
        {
            return;
        }

        if (context.Context.IsBuilderForAbstractEntity)
        {
            classBuilder
                .AddGenericTypeArguments("TBuilder", "TEntity")
                .AddGenericTypeArgumentConstraints($"where TEntity : {FormatInstanceName(classBuilder, false, context.Context.TypeSettings.FormatInstanceTypeNameDelegate)}")
                .AddGenericTypeArgumentConstraints($"where TBuilder : {_formattableStringParser.Parse(context.Context.NameSettings.BuilderNameFormatString, CultureInfo.CurrentCulture, context).GetValueOrThrow()}<TBuilder, TEntity>")
                .WithAbstract(context.Context.IsBuilderForAbstractEntity);
        }
        else if (context.Context.ClassSettings.ConstructorSettings.OriginalValidateArguments == ArgumentValidationType.Shared)
        {
            classBuilder.AddInterfaces(typeof(IValidatableObject));
        }
    }

    public override IBuilder<IPipelineFeature<TypeBuilder, BuilderPipelineBuilderSettings>> ToBuilder()
        => new AbstractBuilderFeatureBuilder(_formattableStringParser);
}
