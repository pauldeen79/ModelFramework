﻿namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class TestInterfacesModels : CSharpExpressionDumperClassBase
{
    public override string Path => "ModelFramework.CodeGeneration.Tests/CodeGenerationProviders";
    public override string DefaultFileName => "ModelFrameworkCSharpClassBase.Test.generated.cs";
    public override bool RecurseOnDeleteGeneratedFiles => false;

    protected override string Namespace => "ModelFramework.CodeGeneration.Tests.CodeGenerationProviders";
    protected override string ClassName => "TestCSharpClassBase";
    protected override string MethodName => "GetTestModels";
    protected override string FileNameSuffix => ".Test.generated";

    protected override string[] NamespacesToAbbreviate => new[]
    {
        "System.Collections.Generic",
        "ModelFramework.Objects.Builders",
        "ModelFramework.Objects.Contracts"
    };

    protected override Type[] Models => new[]
    {
        typeof(Common.Tests.Test.Contracts.IChild),
        typeof(Common.Tests.Test.Contracts.IParent)
    };
}
