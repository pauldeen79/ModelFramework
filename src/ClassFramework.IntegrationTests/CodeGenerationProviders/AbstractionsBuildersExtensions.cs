﻿namespace ClassFramework.IntegrationTests.CodeGenerationProviders;

public class AbstractionsBuildersExtensions : TestCodeGenerationProviderBase
{
    public AbstractionsBuildersExtensions(ICsharpExpressionCreator csharpExpressionCreator, IPipeline<IConcreteTypeBuilder, BuilderContext> builderPipeline, IPipeline<IConcreteTypeBuilder, BuilderInterfaceContext> builderInterfacePipeline, IPipeline<IConcreteTypeBuilder, EntityContext> entityPipeline, IPipeline<IConcreteTypeBuilder, OverrideEntityContext> overrideEntityPipeline, IPipeline<TypeBaseBuilder, ReflectionContext> reflectionPipeline, IPipeline<InterfaceBuilder, InterfaceContext> interfacePipeline) : base(csharpExpressionCreator, builderPipeline, builderInterfacePipeline, entityPipeline, overrideEntityPipeline, reflectionPipeline, interfacePipeline)
    {
    }

    public override IEnumerable<TypeBase> Model => GetBuilderExtensions(GetAbstractionsInterfaces(), "ClassFramework.Domain.Builders.Abstractions", "ClassFramework.Domain.Abstractions");

    public override string Path => "ClassFramework.Domain.POC/Builders/Extensions";

    protected override bool EnableEntityInheritance => true;
}