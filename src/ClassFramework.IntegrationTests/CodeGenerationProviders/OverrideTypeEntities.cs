﻿namespace ClassFramework.IntegrationTests.CodeGenerationProviders;

public class OverrideTypeEntities : TestCodeGenerationProviderBase
{
    public OverrideTypeEntities(ICsharpExpressionCreator csharpExpressionCreator, IPipeline<IConcreteTypeBuilder, BuilderContext> builderPipeline, IPipeline<IConcreteTypeBuilder, EntityContext> entityPipeline, IPipeline<TypeBaseBuilder, ReflectionContext> reflectionPipeline, IPipeline<InterfaceBuilder, InterfaceContext> interfacePipeline) : base(csharpExpressionCreator, builderPipeline, entityPipeline, reflectionPipeline, interfacePipeline)
    {
    }

    public override string Path => "ClassFramework.Domain.POC/Types";

    protected override Class? BaseClass => CreateBaseclass(typeof(ITypeBase), "ClassFramework.Domain");

    public override IEnumerable<TypeBase> Model
        => GetImmutableClasses(GetOverrideModels(typeof(ITypeBase)), "ClassFramework.Domain.Types");
}
