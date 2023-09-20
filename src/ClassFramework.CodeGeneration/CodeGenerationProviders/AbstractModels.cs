namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractModels : ClassFrameworkModelClassBase
{
    public override string Path => Constants.Namespaces.DomainModels;

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;

    public override object CreateModel()
        => GetImmutableBuilderClasses(
            GetAbstractModels(),
            //MapCodeGenerationModelsToDomain(new[] { typeof(ICodeStatement), typeof(IType) }),
            Constants.Namespaces.Domain,
            Constants.Namespaces.DomainModels);
}
