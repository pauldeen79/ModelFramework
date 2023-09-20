namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class CoreEntities : ClassFrameworkCSharpClassBase
{
    public override string Path => Constants.Namespaces.Domain;

    public override object CreateModel()
        => GetImmutableClasses(GetCoreModels(), Constants.Namespaces.Domain);
}
