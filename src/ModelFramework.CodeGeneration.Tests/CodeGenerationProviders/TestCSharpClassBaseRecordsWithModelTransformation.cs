namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class TestCSharpClassBaseRecordsWithModelTransformation : CSharpClassBase
{
    public override string Path => @"NotUsed";
    public override string DefaultFileName => "NotUsed";
    public override bool RecurseOnDeleteGeneratedFiles => false;

    protected override string RootNamespace => "MyNamespace";
    protected override Type RecordCollectionType => typeof(IReadOnlyCollection<>);
    protected override Type RecordConcreteCollectionType => typeof(ReadOnlyCollection<>);
    protected override bool EnableNullableContext => true;
    protected override bool CreateCodeGenerationHeader => false;

    public override object CreateModel()
        => GetImmutableClasses(
            MapCodeGenerationModelsToDomain(new[] { typeof(IMyClass) }),
            "MyNamespace.Domain");

    protected override Dictionary<string, string> GetBuilderNamespaceMappings() => new()
    {
        { "MyNamespace.Domain", "MyNamespace.Domain.Builders" }
    };

    protected override string GetFullBasePath()
    {
        throw new NotImplementedException();
    }

    protected override Dictionary<string, string> GetModelMappings() => new()
    {
        { "ModelFramework.CodeGeneration.Tests.CodeGenerationProviders.I", "MyNamespace.Domain." }
    };
}
