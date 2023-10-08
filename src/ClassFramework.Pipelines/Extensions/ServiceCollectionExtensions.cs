using ClassFramework.Pipelines.Builder.Features.Abstractions;

namespace ClassFramework.Pipelines.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPipelines(this IServiceCollection services)
        => services
            .AddScoped<IPipelineBuilder, PipelineBuilder>()
            .AddScoped<IContextBaseBuilder, PartialFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, SetNameFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, AbstractBuilderFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, BaseClassFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, AddPropertiesFeatureBuilder>()
            .AddScoped<IPlaceholderProcessor, ContextSourceModelProcessor>()
            .AddScoped<IPlaceholderProcessor, ClassPropertyProcessor>();
}
