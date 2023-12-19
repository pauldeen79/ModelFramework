namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class CoreBuilders : ClassFrameworkCSharpClassBase
{
    public override string Path => Constants.Paths.DomainBuilders;

    public override object CreateModel()
        => GetImmutableBuilderClasses(
            GetCoreModels(),
            Constants.Namespaces.Domain,
            Constants.Namespaces.DomainBuilders);
}
