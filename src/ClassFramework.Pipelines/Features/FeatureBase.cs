namespace ClassFramework.Pipelines.Features;

public abstract class FeatureBase : IPipelineFeature<TypeBuilder, BuilderPipelineBuilderSettings>
{
    protected static string FormatInstanceName(
        TypeBuilder instance,
        bool forCreate,
        Func<TypeBuilder, bool, string>? formatInstanceTypeNameDelegate)
    {
        if (formatInstanceTypeNameDelegate is not null)
        {
            var retVal = formatInstanceTypeNameDelegate(instance, forCreate);
            if (!string.IsNullOrEmpty(retVal))
            {
                return retVal;
            }
        }

        return instance.IsNotNull(nameof(instance)).GetFullName().GetCsharpFriendlyTypeName();
    }

    public abstract void Process(PipelineContext<TypeBuilder, BuilderPipelineBuilderSettings> context);
    public abstract IBuilder<IPipelineFeature<TypeBuilder, BuilderPipelineBuilderSettings>> ToBuilder();
}
