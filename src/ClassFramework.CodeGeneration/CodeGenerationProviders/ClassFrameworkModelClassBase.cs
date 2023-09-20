namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public abstract class ClassFrameworkModelClassBase : ClassFrameworkCSharpClassBase
{
    protected override string AddMethodNameFormatString => string.Empty; // we don't want Add methods for collection properties
    protected override string SetMethodNameFormatString => string.Empty; // we don't want With methods for non-collection properties
    protected override string BuilderNameFormatString => "{0}Model";
    protected override string BuilderBuildMethodName => "ToEntity";
    protected override string BuilderFactoryName => "ModelFactory";
    protected override string BuilderBuildTypedMethodName => "ToTypedEntity";
    protected override string BuilderName => "Model";
    protected override string BuildersName => "Models";
}
