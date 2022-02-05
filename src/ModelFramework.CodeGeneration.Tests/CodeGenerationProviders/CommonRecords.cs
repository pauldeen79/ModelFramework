namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class CommonRecords : ModelFrameworkCSharpClassBase
{
    public override string Path => "ModelFramework.Common";

    public override string DefaultFileName => "Entities.generated.cs";

    public override bool RecurseOnDeleteGeneratedFiles => false;

    public override object CreateModel()
        => GetImmutableClasses(GetCommonModelTypes(), "ModelFramework.Common");
}
