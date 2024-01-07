namespace ClassFramework.IntegrationTests.CodeGenerationProviders;

public class CoreEntities : CsharpClassGeneratorPipelineCodeGenerationProviderBase
{
    public CoreEntities(ICsharpExpressionCreator csharpExpressionCreator, IPipeline<IConcreteTypeBuilder, BuilderContext> builderPipeline, IPipeline<IConcreteTypeBuilder, EntityContext> entityPipeline, IPipeline<TypeBaseBuilder, ReflectionContext> reflectionPipeline) : base(csharpExpressionCreator, builderPipeline, entityPipeline, reflectionPipeline)
    {
    }

    public override IEnumerable<TypeBase> Model => GetImmutableClasses(GetCoreModels(), "ClassFramework.Domain");

    public override string Path => "ClassFramework.Domain";
    public override bool RecurseOnDeleteGeneratedFiles => false;
    public override string LastGeneratedFilesFilename => string.Empty;
    public override Encoding Encoding => Encoding.UTF8;

    protected override string ProjectName => "ClassFramework";
    protected override Type RecordCollectionType => typeof(IReadOnlyCollection<>);
    protected override Type RecordConcreteCollectionType => typeof(ReadOnlyCollection<>);
    protected override string CodeGenerationRootNamespace => "ClassFramework.IntegrationTests";
}
