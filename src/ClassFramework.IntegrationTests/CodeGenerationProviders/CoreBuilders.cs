﻿namespace ClassFramework.IntegrationTests.CodeGenerationProviders;

public class CoreBuilders : TestCodeGenerationProviderBase
{
    public CoreBuilders(ICsharpExpressionCreator csharpExpressionCreator, IPipeline<IConcreteTypeBuilder, BuilderContext> builderPipeline, IPipeline<IConcreteTypeBuilder, EntityContext> entityPipeline, IPipeline<TypeBaseBuilder, ReflectionContext> reflectionPipeline, IPipeline<InterfaceBuilder, InterfaceContext> interfacePipeline) : base(csharpExpressionCreator, builderPipeline, entityPipeline, reflectionPipeline, interfacePipeline)
    {
    }

    public override IEnumerable<TypeBase> Model => GetBuilders(GetCoreModels(), "ClassFramework.Domain.Builders", "ClassFramework.Domain");

    public override string Path => "ClassFramework.Domain.POC/Builders";
}
