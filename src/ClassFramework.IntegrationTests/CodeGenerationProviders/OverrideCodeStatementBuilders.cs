﻿namespace ClassFramework.IntegrationTests.CodeGenerationProviders;

public class OverrideCodeStatementBuilders : TestCodeGenerationProviderBase
{
    public OverrideCodeStatementBuilders(ICsharpExpressionCreator csharpExpressionCreator, IPipeline<IConcreteTypeBuilder, BuilderContext> builderPipeline, IPipeline<IConcreteTypeBuilder, EntityContext> entityPipeline, IPipeline<IConcreteTypeBuilder, OverrideEntityContext> overrideEntityPipeline, IPipeline<TypeBaseBuilder, ReflectionContext> reflectionPipeline, IPipeline<InterfaceBuilder, InterfaceContext> interfacePipeline) : base(csharpExpressionCreator, builderPipeline, entityPipeline, overrideEntityPipeline, reflectionPipeline, interfacePipeline)
    {
    }

    public override string Path => "ClassFramework.Domain.POC/Builders/CodeStatements";

    protected override bool EnableEntityInheritance => true;
    protected override Class? BaseClass => CreateBaseclass(typeof(ICodeStatementBase), "ClassFramework.Domain");

    public override IEnumerable<TypeBase> Model
        => GetBuilders(GetOverrideModels(typeof(ICodeStatementBase)), "ClassFramework.Domain.Builders.CodeStatements", "ClassFramework.Domain.CodeStatements");
}
