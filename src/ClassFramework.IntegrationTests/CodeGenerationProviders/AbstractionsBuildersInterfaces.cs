namespace ClassFramework.IntegrationTests.CodeGenerationProviders;

public class AbstractionsBuildersInterfaces : TestCodeGenerationProviderBase
{
    public AbstractionsBuildersInterfaces(ICsharpExpressionCreator csharpExpressionCreator, IPipeline<IConcreteTypeBuilder, BuilderContext> builderPipeline, IPipeline<IConcreteTypeBuilder, EntityContext> entityPipeline, IPipeline<TypeBaseBuilder, ReflectionContext> reflectionPipeline, IPipeline<InterfaceBuilder, InterfaceContext> interfacePipeline) : base(csharpExpressionCreator, builderPipeline, entityPipeline, reflectionPipeline, interfacePipeline)
    {
    }

    public override IEnumerable<TypeBase> Model => GetInterfaces(GetBuilders(GetAbstractionsInterfaces(), "ClassFramework.Domain.Builders", "ClassFramework.Domain"), "ClassFramework.Domain.Builders.Abstractions");

    public override string Path => "ClassFramework.Domain.POC/Builders/Abstractions";
}
