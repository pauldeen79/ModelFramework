﻿namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class ObjectsOverrideBuilders : ModelFrameworkCSharpClassBase
{
    public override string Path => "ModelFramework.Objects/Builders";
    public override string DefaultFileName => "Builders.generated.cs";
    public override bool RecurseOnDeleteGeneratedFiles => false;

    protected override bool EnableInheritance => true;
    protected override IClass? BaseClass => GetTypeBaseModel();

    public override object CreateModel()
        => GetImmutableBuilderClasses(GetObjectsModelOverrideTypes(),
                                      "ModelFramework.Objects",
                                      "ModelFramework.Objects.Builders");
}