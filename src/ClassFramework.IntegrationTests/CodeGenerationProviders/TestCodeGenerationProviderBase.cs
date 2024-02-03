namespace ClassFramework.IntegrationTests.CodeGenerationProviders;

public abstract class TestCodeGenerationProviderBase : CsharpClassGeneratorPipelineCodeGenerationProviderBase
{
    protected TestCodeGenerationProviderBase(ICsharpExpressionCreator csharpExpressionCreator, IPipeline<IConcreteTypeBuilder, BuilderContext> builderPipeline, IPipeline<IConcreteTypeBuilder, EntityContext> entityPipeline, IPipeline<IConcreteTypeBuilder, OverrideEntityContext> overrideEntityPipeline, IPipeline<TypeBaseBuilder, ReflectionContext> reflectionPipeline, IPipeline<InterfaceBuilder, InterfaceContext> interfacePipeline) : base(csharpExpressionCreator, builderPipeline, entityPipeline, overrideEntityPipeline, reflectionPipeline, interfacePipeline)
    {
    }

    public override bool RecurseOnDeleteGeneratedFiles => false;
    public override string LastGeneratedFilesFilename => string.Empty;
    public override Encoding Encoding => Encoding.UTF8;

    protected override Type RecordCollectionType => typeof(IReadOnlyCollection<>);
    protected override Type RecordConcreteCollectionType => typeof(List<>);
    protected override Type BuilderCollectionType => typeof(List<>);
    //protected override Type RecordCollectionType => typeof(ObservableValueCollection<>);
    //protected override Type RecordConcreteCollectionType => typeof(ObservableValueCollection<>);
    //protected override Type BuilderCollectionType => typeof(ObservableValueCollection<>);

    protected override string ProjectName => "ClassFramework";
    protected override string CodeGenerationRootNamespace => "ClassFramework.IntegrationTests";
    protected override string CoreNamespace => "ClassFramework.Domain";
    protected override bool CopyAttributes => true;
    protected override bool CopyInterfaces => true;
    //protected override ArgumentValidationType ValidateArgumentsInConstructor => ArgumentValidationType.Shared;
    protected override Domain.Domains.SubVisibility SetterVisibility => Domain.Domains.SubVisibility.Private;
    //protected override bool AddBackingFields => true;
    //protected override bool CreateAsObservable => true;
}
