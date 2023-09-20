namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractBuilders : ClassFrameworkCSharpClassBase
{
    public override string Path => Constants.Namespaces.DomainBuilders;

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;

    public override object CreateModel()
        => GetImmutableBuilderClasses(
            GetAbstractModels(),
            //MapCodeGenerationModelsToDomain(new[] { typeof(ICodeStatement), typeof(IType) }),
            Constants.Namespaces.Domain,
            Constants.Namespaces.DomainBuilders);
}
