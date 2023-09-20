namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractNonGenericModels : ClassFrameworkModelClassBase
{
    public override string Path => Constants.Namespaces.DomainModels;

    protected override bool EnableEntityInheritance => true;
    protected override bool EnableBuilderInhericance => true;
    protected override string FileNameSuffix => ".nongeneric.template.generated";

    public override object CreateModel()
        => GetImmutableNonGenericBuilderClasses(
            GetAbstractModels(),
            //new[] { typeof(ICodeStatement), typeof(IType) }.Select(x => x.ToClassBuilder().With(x => MapCodeGenerationModelsToDomain().ToArray(),
            //MapCodeGenerationModelsToDomain(new[] { typeof(ICodeStatement), typeof(IType) }),
            Constants.Namespaces.Domain,
            Constants.Namespaces.DomainModels);
}
