﻿namespace ClassFramework.IntegrationTests.CodeGenerationProviders;

public class AbstractNonGenericBuilders : TestCodeGenerationProviderBase
{
    public AbstractNonGenericBuilders(ICsharpExpressionCreator csharpExpressionCreator, IPipeline<IConcreteTypeBuilder, BuilderContext> builderPipeline, IPipeline<IConcreteTypeBuilder, EntityContext> entityPipeline, IPipeline<TypeBaseBuilder, ReflectionContext> reflectionPipeline, IPipeline<InterfaceBuilder, InterfaceContext> interfacePipeline) : base(csharpExpressionCreator, builderPipeline, entityPipeline, reflectionPipeline, interfacePipeline)
    {
    }

    public override IEnumerable<TypeBase> Model => GetNonGenericBuilders(GetAbstractModels(), "ClassFramework.Domain.Builders", "ClassFramework.Domain");

    public override string Path => "ClassFramework.Domain.POC/Builders";

    protected override bool AddNullChecks => false; // not needed for abstract builders, because each derived class will do its own validation

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override bool IsAbstract => true;

    protected override string FilenameSuffix => ".nongeneric.template.generated";

    // Do nog generate 'With' methods. Do this on the interfaces instead.
    protected override string SetMethodNameFormatString => string.Empty;
    protected override string AddMethodNameFormatString => string.Empty;
}