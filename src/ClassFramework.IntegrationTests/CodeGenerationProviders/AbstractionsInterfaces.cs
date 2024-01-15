﻿namespace ClassFramework.IntegrationTests.CodeGenerationProviders;

public class AbstractionsInterfaces : TestCodeGenerationProviderBase
{
    public AbstractionsInterfaces(ICsharpExpressionCreator csharpExpressionCreator, IPipeline<IConcreteTypeBuilder, BuilderContext> builderPipeline, IPipeline<IConcreteTypeBuilder, EntityContext> entityPipeline, IPipeline<TypeBaseBuilder, ReflectionContext> reflectionPipeline, IPipeline<InterfaceBuilder, InterfaceContext> interfacePipeline) : base(csharpExpressionCreator, builderPipeline, entityPipeline, reflectionPipeline, interfacePipeline)
    {
    }

    public override IEnumerable<TypeBase> Model => GetInterfaces(GetAbstractionsInterfaces(), "ClassFramework.Domain.Abstractions");

    public override string Path => "ClassFramework.Domain.POC/Abstractions";

    protected override bool EnableEntityInheritance => true;
    protected override bool IsAbstract => true;
}
