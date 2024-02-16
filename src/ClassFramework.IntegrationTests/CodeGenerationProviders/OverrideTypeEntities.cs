﻿namespace ClassFramework.IntegrationTests.CodeGenerationProviders;

public class OverrideTypeEntities : TestCodeGenerationProviderBase
{
    public OverrideTypeEntities(ICsharpExpressionCreator csharpExpressionCreator, IPipeline<IConcreteTypeBuilder, BuilderContext> builderPipeline, IPipeline<IConcreteTypeBuilder, BuilderInterfaceContext> builderInterfacePipeline, IPipeline<IConcreteTypeBuilder, EntityContext> entityPipeline, IPipeline<IConcreteTypeBuilder, OverrideEntityContext> overrideEntityPipeline, IPipeline<TypeBaseBuilder, ReflectionContext> reflectionPipeline, IPipeline<InterfaceBuilder, InterfaceContext> interfacePipeline) : base(csharpExpressionCreator, builderPipeline, builderInterfacePipeline, entityPipeline, overrideEntityPipeline, reflectionPipeline, interfacePipeline)
    {
    }

    public override string Path => "ClassFramework.Domain.POC/Types";

    protected override bool EnableEntityInheritance => true;
    protected override Class? BaseClass => CreateBaseclass(typeof(ITypeBase), "ClassFramework.Domain");

    public override IEnumerable<TypeBase> Model
        => GetImmutableClasses(GetOverrideModels(typeof(ITypeBase)), "ClassFramework.Domain.Types");
}
