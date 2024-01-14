namespace ClassFramework.IntegrationTests.CodeGenerationProviders;

public class CoreEntities : TestCodeGenerationProviderBase
{
    public CoreEntities(ICsharpExpressionCreator csharpExpressionCreator, IPipeline<IConcreteTypeBuilder, BuilderContext> builderPipeline, IPipeline<IConcreteTypeBuilder, EntityContext> entityPipeline, IPipeline<TypeBaseBuilder, ReflectionContext> reflectionPipeline, IPipeline<InterfaceBuilder, InterfaceContext> interfacePipeline) : base(csharpExpressionCreator, builderPipeline, entityPipeline, reflectionPipeline, interfacePipeline)
    {
    }

    public override IEnumerable<TypeBase> Model => GetImmutableClasses(GetCoreModels(), "ClassFramework.Domain");

    public override string Path => "ClassFramework.Domain.POC";
}
