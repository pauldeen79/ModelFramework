namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractBuilders : ClassFrameworkCSharpClassBase
{
    public override string Path => Constants.Paths.DomainBuilders;

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override bool IsAbstract => true;

    public override object CreateModel()
        => GetImmutableBuilderClasses(
            GetAbstractModels(),
            Constants.Namespaces.Domain,
            Constants.Namespaces.DomainBuilders);
}
