namespace ClassFramework.Pipelines.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPipelines(this IServiceCollection services)
        => services
            .AddScoped<IPipelineBuilder<ClassBuilder, BuilderContext>, PipelineBuilder>()
            .AddScoped<ISharedFeatureBuilder, PartialFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, AbstractBuilderFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, AddConstructorsFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, AddPropertiesFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, BaseClassFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, GenericsFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, SetNameFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, ValidatableObjectFeatureBuilder>()
            .AddScoped<IPlaceholderProcessor, ContextSourceModelProcessor>()
            .AddScoped<IPlaceholderProcessor, ClassPropertyProcessor>()
            .AddScoped<IPlaceholderProcessor, ParentClassPropertyChildContextProcessor>();
}
