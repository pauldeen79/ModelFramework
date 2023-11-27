namespace ClassFramework.Pipelines.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPipelines(this IServiceCollection services)
        => services
            .AddScoped<IPipeline<ClassBuilder, BuilderContext>>(services => services.GetRequiredService<IPipelineBuilder<ClassBuilder, BuilderContext>>().Build())
            .AddScoped<IPipeline<ClassBuilder, EntityContext>>(services => services.GetRequiredService<IPipelineBuilder<ClassBuilder, EntityContext>>().Build())
            .AddScoped<IPipelineBuilder<ClassBuilder, BuilderContext>, Builder.PipelineBuilder>()
            .AddScoped<IPipelineBuilder<ClassBuilder, EntityContext>, Entity.PipelineBuilder>()
            .AddScoped<ISharedFeatureBuilder, Shared.Features.PartialFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, Builder.Features.ValidationFeatureBuilder>() // important to register this one first, because validation should be performed first
            .AddScoped<IBuilderFeatureBuilder, Builder.Features.AbstractBuilderFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, Builder.Features.AddAttributesFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, Builder.Features.AddBuildMethodFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, Builder.Features.AddCopyConstructorFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, Builder.Features.AddDefaultConstructorFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, Builder.Features.AddFluentMethodsForCollectionPropertiesFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, Builder.Features.AddFluentMethodsForNonCollectionPropertiesFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, Builder.Features.AddInterfacesFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, Builder.Features.AddMetadataFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, Builder.Features.AddPropertiesFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, Builder.Features.BaseClassFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, Builder.Features.GenericsFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, Builder.Features.SetNameFeatureBuilder>()
            .AddScoped<IBuilderFeatureBuilder, Builder.Features.ValidatableObjectFeatureBuilder>()
            .AddScoped<IEntityFeatureBuilder, Entity.Features.AbstractEntityFeatureBuilder>()
            .AddScoped<IEntityFeatureBuilder, Entity.Features.AddAttributesFeatureBuilder>()
            .AddScoped<IEntityFeatureBuilder, Entity.Features.AddConstructorFeatureBuilder>()
            .AddScoped<IEntityFeatureBuilder, Entity.Features.AddGenericsFeatureBuilder>()
            .AddScoped<IEntityFeatureBuilder, Entity.Features.AddInterfacesFeatureBuilder>()
            .AddScoped<IEntityFeatureBuilder, Entity.Features.AddMetadataFeatureBuilder>()
            .AddScoped<IEntityFeatureBuilder, Entity.Features.AddPropertiesFeatureBuilder>()
            .AddScoped<IEntityFeatureBuilder, Entity.Features.BaseClassFeatureBuilder>()
            .AddScoped<IEntityFeatureBuilder, Entity.Features.ObservableFeatureBuilder>()
            .AddScoped<IEntityFeatureBuilder, Entity.Features.SetNameFeatureBuilder>()
            .AddScoped<IEntityFeatureBuilder, Entity.Features.SetRecordFeatureBuilder>()
            .AddScoped<IEntityFeatureBuilder, Entity.Features.ValidationFeatureBuilder>()
            .AddScoped<IPlaceholderProcessor, BuilderPipelinePlaceholderProcessor>()
            .AddScoped<IPlaceholderProcessor, EntityPipelinePlaceholderProcessor>()
            .AddScoped<IPipelinePlaceholderProcessor, ClassPropertyProcessor>()
            .AddScoped<IPipelinePlaceholderProcessor, TypeBaseProcessor>();
}
