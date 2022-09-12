namespace ModelFramework.CodeGeneration.Tests.CodeGenerationProviders;

public class TestCSharpClassBaseModelTransformationOverrideRecords : TestCSharpClassBaseModelTransformationBase
{
    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override IClass? BaseClass => CreateBaseclass(typeof(IMyBaseClass), "MyNamespace.Domain");

    public override object CreateModel()
        => GetImmutableClasses(
            GetOverrideModelTransformationTypes(),
            "MyNamespace.Domain");
}
