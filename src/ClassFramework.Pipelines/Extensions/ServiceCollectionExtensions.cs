namespace ClassFramework.Pipelines.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPipelines(this IServiceCollection services)
        => services
            .AddScoped<IPipeline<ClassBuilder, BuilderContext>>(services => services.GetRequiredService<IPipelineBuilder<ClassBuilder, BuilderContext>>().Build())
            .AddScoped<IPipelineBuilder<ClassBuilder, BuilderContext>, PipelineBuilder>()
            .AddScoped<ISharedFeatureBuilder, PartialFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, ValidationFeatureBuilder>() // important to register this one first, because validation should be performed first
            .AddScoped<IBuilderFeatureBuilder, AbstractBuilderFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, AddAttributesFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, AddBuildMethodFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, AddCopyConstructorFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, AddDefaultConstructorFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, AddFluentMethodsForCollectionPropertiesFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, AddFluentMethodsForNonCollectionPropertiesFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, AddPropertiesFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, BaseClassFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, GenericsFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, SetNameFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, ValidatableObjectFeatureBuilder>()
            .AddScoped<IPlaceholderProcessor, ContextSourceModelProcessor>()
            .AddScoped<IPlaceholderProcessor, ParentClassPropertyChildContextProcessor>();
}
