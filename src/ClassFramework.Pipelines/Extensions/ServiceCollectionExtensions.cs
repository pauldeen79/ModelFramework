namespace ClassFramework.Pipelines.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPipelines(this IServiceCollection services)
        => services
            .AddScoped<IPipelineBuilder<ClassBuilder, BuilderContext>, PipelineBuilder>()
            .AddScoped<ISharedFeatureBuilder, PartialFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, ValidationFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, SetNameFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, AbstractBuilderFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, BaseClassFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, AddPropertiesFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, AddConstructorsFeatureBuilder>()
            .AddScoped<IPlaceholderProcessor, ContextSourceModelProcessor>()
            .AddScoped<IPlaceholderProcessor, ClassPropertyProcessor>()
            .AddScoped<IPlaceholderProcessor, ParentClassPropertyChildContextProcessor>();
}
