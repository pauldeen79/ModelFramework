namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class TestCSharpClassBaseBuildersWithModelTransformation : CSharpClassBase
{
    public override string Path => @"NotUsed";
    public override string DefaultFileName => "NotUsed";
    public override bool RecurseOnDeleteGeneratedFiles => false;

    protected override string RootNamespace => "MyNamespace";
    protected override Type RecordCollectionType => typeof(IReadOnlyCollection<>);
    protected override bool EnableNullableContext => true;
    protected override bool CreateCodeGenerationHeader => false;

    public override object CreateModel()
        => GetImmutableBuilderClasses(
            MapCodeGenerationModelsToDomain(new[] { typeof(IMyClass) }),
            "MyNamespace.Domain",
            "MyNamespace.Domain.Builders");

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

public interface IMyClass
{
    IReadOnlyCollection<IMyClass> SubTypes { get; }
    IMyClass? ParentType { get; }
}
