﻿namespace ClassFramework.Pipelines.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPipelines(this IServiceCollection services)
        => services
            .AddBuilderPipeline()
            .AddEntityPipeline()
            .AddReflectionPipeline()
            .AddSharedPipelineComponents()
            .AddParserComponents();

    private static IServiceCollection AddSharedPipelineComponents(this IServiceCollection services)
        => services
            .AddScoped<ISharedFeatureBuilder<ClassBuilder>, Shared.Features.PartialFeatureBuilder<ClassBuilder>>()
            .AddScoped<ISharedFeatureBuilder<IConcreteTypeBuilder>, Shared.Features.PartialFeatureBuilder<IConcreteTypeBuilder>>()
            .AddScoped<IPipelinePlaceholderProcessor, ClassPropertyProcessor>()
            .AddScoped<IPipelinePlaceholderProcessor, TypeBaseProcessor>();

    private static IServiceCollection AddBuilderPipeline(this IServiceCollection services)
        => services
            .AddScoped<IPipeline<ClassBuilder, BuilderContext>>(services => services.GetRequiredService<IPipelineBuilder<ClassBuilder, BuilderContext>>().Build())
            .AddScoped<IPipelineBuilder<ClassBuilder, BuilderContext>, Builder.PipelineBuilder>()
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
            .AddScoped<IBuilderFeatureBuilder, Builder.Features.ValidatableObjectFeatureBuilder>();

    private static IServiceCollection AddEntityPipeline(this IServiceCollection services)
        => services
            .AddScoped<IPipeline<IConcreteTypeBuilder, EntityContext>>(services => services.GetRequiredService<IPipelineBuilder<IConcreteTypeBuilder, EntityContext>>().Build())
            .AddScoped<IPipelineBuilder<IConcreteTypeBuilder, EntityContext>, Entity.PipelineBuilder>()
            .AddScoped<IEntityFeatureBuilder, Entity.Features.ValidationFeatureBuilder>() // important to register this one first, because validation should be performed first
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
            .AddScoped<IEntityFeatureBuilder, Entity.Features.SetRecordFeatureBuilder>();

    private static IServiceCollection AddReflectionPipeline(this IServiceCollection services)
        => services
            .AddScoped<IPipeline<TypeBaseBuilder, ReflectionContext>>(services => services.GetRequiredService<IPipelineBuilder<TypeBaseBuilder, ReflectionContext>>().Build())
            .AddScoped<IPipelineBuilder<TypeBaseBuilder, ReflectionContext>, Reflection.PipelineBuilder>()
            .AddScoped<IReflectionFeatureBuilder, Reflection.Features.ValidationFeatureBuilder>() // important to register this one first, because validation should be performed first
            .AddScoped<IReflectionFeatureBuilder, Reflection.Features.AddAttributesFeatureBuilder>()
            .AddScoped<IReflectionFeatureBuilder, Reflection.Features.AddInterfacesFeatureBuilder>();

    private static IServiceCollection AddParserComponents(this IServiceCollection services)
        => services
            .AddScoped<IPlaceholderProcessor, BuilderPipelinePlaceholderProcessor>()
            .AddScoped<IPlaceholderProcessor, EntityPipelinePlaceholderProcessor>();
}
