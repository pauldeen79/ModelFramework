namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class AbstractionsInterfacesModels : CsharpExpressionDumperClassBase
{
    public override string Path => "ModelFramework.CodeGeneration.Tests\\CodeGenerationProviders";

    public override string DefaultFileName => "ModelFrameworkCSharpClassBase.generated.cs";

    public override bool RecurseOnDeleteGeneratedFiles => false;

    protected override Type RecordCollectionType => typeof(ValueCollection<>);
    protected override string Namespace => "ModelFramework.CodeGeneration.Tests.CodeGenerationProviders";
    protected override string ClassName => "ModelFrameworkCSharpClassBase";

    protected override string[] NamespacesToAbbreviate => new[]
    {
        "System.Collections.Generic",
        "ModelFramework.Objects.Builders",
        "ModelFramework.Objects.Contracts"
    };

    protected override Type[] Models => new[]
    {
        typeof(IAttribute),
        typeof(IAttributeParameter),
        typeof(IClass),
        typeof(IClassConstructor),
        typeof(IClassField),
        typeof(IClassMethod),
        typeof(IClassProperty),
        typeof(IEnum),
        typeof(IEnumMember),
        typeof(IInterface),
        typeof(IParameter)
    };
}
