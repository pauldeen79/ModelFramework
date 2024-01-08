using ClassFramework.Pipelines.Domains;

namespace ClassFramework.IntegrationTests.CodeGenerationProviders;

public abstract class TestCodeGenerationProviderBase : CsharpClassGeneratorPipelineCodeGenerationProviderBase
{
    protected TestCodeGenerationProviderBase(ICsharpExpressionCreator csharpExpressionCreator, IPipeline<IConcreteTypeBuilder, BuilderContext> builderPipeline, IPipeline<IConcreteTypeBuilder, EntityContext> entityPipeline, IPipeline<TypeBaseBuilder, ReflectionContext> reflectionPipeline) : base(csharpExpressionCreator, builderPipeline, entityPipeline, reflectionPipeline)
    {
    }

    protected override string ProjectName => "ClassFramework";
    protected override Type RecordCollectionType => typeof(IReadOnlyCollection<>);
    protected override Type RecordConcreteCollectionType => typeof(ReadOnlyCollection<>);
    protected override string CodeGenerationRootNamespace => "ClassFramework.IntegrationTests";
    protected override string CoreNamespace => "ClassFramework.Domain";
    protected override bool CopyAttributes => true;
}
