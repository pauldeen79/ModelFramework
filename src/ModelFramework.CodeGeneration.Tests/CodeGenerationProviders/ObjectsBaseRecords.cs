﻿namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class ObjectsBaseRecords : ModelFrameworkCSharpClassBase
{
    public override string Path => "ModelFramework.Objects";
    public override string DefaultFileName => "Entities.generated.cs";
    public override bool RecurseOnDeleteGeneratedFiles => false;

    protected override bool EnableInheritance => true;

    public override object CreateModel()
        => GetImmutableClasses(GetObjectsModelBaseTypes(), "ModelFramework.Objects");
}