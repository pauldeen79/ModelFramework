namespace ClassFramework.IntegrationTests.CodeGenerationProviders;

public class CoreEntities : TestCodeGenerationProviderBase
{
    public CoreEntities(ICsharpExpressionCreator csharpExpressionCreator, IPipeline<IConcreteTypeBuilder, BuilderContext> builderPipeline, IPipeline<IConcreteTypeBuilder, EntityContext> entityPipeline, IPipeline<TypeBaseBuilder, ReflectionContext> reflectionPipeline) : base(csharpExpressionCreator, builderPipeline, entityPipeline, reflectionPipeline)
    {
    }

    public override IEnumerable<TypeBase> Model => GetImmutableClasses(GetCoreModels(), "ClassFramework.Domain");

    public override string Path => "ClassFramework.Domain.POC";
    public override bool RecurseOnDeleteGeneratedFiles => false;
    public override string LastGeneratedFilesFilename => string.Empty;
    public override Encoding Encoding => Encoding.UTF8;
}
