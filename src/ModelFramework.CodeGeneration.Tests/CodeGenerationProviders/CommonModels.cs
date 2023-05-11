namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class CommonModels : ModelFrameworkCSharpClassBase
{
    public override string Path => "ModelFramework.Common/Models";
    public override string DefaultFileName => "Models.generated.cs";
    public override bool RecurseOnDeleteGeneratedFiles => false;

    protected override string SetMethodNameFormatString => string.Empty;
    protected override string AddMethodNameFormatString => string.Empty;
    protected override string BuilderBuildMethodName => "ToEntity";
    protected override string BuilderNameFormatString => "{0}Model";
    protected override bool UseLazyInitialization => false;

    protected override void FixImmutableBuilderProperties<TBuilder, TEntity>(TypeBaseBuilder<TBuilder, TEntity> typeBaseBuilder)
    {
        // do not do this!
    }

    public override object CreateModel()
        => GetImmutableBuilderClasses(GetCommonModelTypes(),
                                      "ModelFramework.Common",
                                      "ModelFramework.Common.Models");
}
