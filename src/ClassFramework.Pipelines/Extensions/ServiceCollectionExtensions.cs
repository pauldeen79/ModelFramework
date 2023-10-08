namespace ClassFramework.Pipelines.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPipelines(this IServiceCollection services)
        => services
            .AddScoped<IPipelineBuilder<ClassBuilder, PipelineBuilderContext>, PipelineBuilder>()
            .AddScoped<IContextBaseBuilder, PartialFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, SetNameFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, AbstractBuilderFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, BaseClassFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, AddPropertiesFeatureBuilder>()
            .AddScoped<IPlaceholderProcessor, ContextSourceModelProcessor>()
            .AddScoped<IPlaceholderProcessor, ClassPropertyProcessor>();
}
