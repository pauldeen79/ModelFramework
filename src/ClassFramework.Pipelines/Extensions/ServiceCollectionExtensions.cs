namespace ClassFramework.Pipelines.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPipelines(this IServiceCollection services)
        => services
            .AddScoped<IPipelineBuilder, PipelineBuilder>()
            .AddScoped<IPartialFeatureBuilder, PartialFeatureBuilder>()
            .AddScoped<ISetNameFeatureBuilder, SetNameFeatureBuilder>()
            .AddScoped<IAbstractBuilderFeatureBuilder, AbstractBuilderFeatureBuilder>()
            .AddScoped<IBaseClassFeatureBuilder, BaseClassFeatureBuilder>()
            .AddScoped<IAddPropertiesFeatureBuilder, AddPropertiesFeatureBuilder>()
            .AddScoped<IPlaceholderProcessor, ContextSourceModelProcessor>()
            .AddScoped<IPlaceholderProcessor, ClassPropertyProcessor>();
}
