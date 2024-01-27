using System.Runtime;

namespace ClassFramework.Pipelines.Entity.Features;

public class SetBaseClassFeatureBuilder : IEntityFeatureBuilder
{
    private readonly IFormattableStringParser _formattableStringParser;

    public SetBaseClassFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<IConcreteTypeBuilder, EntityContext> Build()
        => new SetBaseClassFeature(_formattableStringParser);
}

public class SetBaseClassFeature : IPipelineFeature<IConcreteTypeBuilder, EntityContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public SetBaseClassFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public Result<IConcreteTypeBuilder> Process(PipelineContext<IConcreteTypeBuilder, EntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (context.Model is IBaseClassContainerBuilder baseClassContainerBuilder)
        {
            if (context.Context.IsSharedNonBaseClass)
            {
                var nameResult = _formattableStringParser.Parse(context.Context.Settings.NameSettings.EntityNameFormatString, context.Context.FormatProvider, context);
                if (!nameResult.IsSuccessful())
                {
                    return Result.FromExistingResult<IConcreteTypeBuilder>(nameResult);
                }
                baseClassContainerBuilder.WithBaseClass($"{nameResult.Value}Base");
            }
            else
            {
                baseClassContainerBuilder.WithBaseClass(context.Context.SourceModel.GetEntityBaseClass(context.Context.Settings.InheritanceSettings.EnableInheritance, context.Context.Settings.InheritanceSettings.BaseClass));
            }
        }

        return Result.Continue<IConcreteTypeBuilder>();
    }

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, EntityContext>> ToBuilder()
        => new SetBaseClassFeatureBuilder(_formattableStringParser);
}
