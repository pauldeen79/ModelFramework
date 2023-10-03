namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsBuilderInterfaces : ClassFrameworkCSharpClassBase
{
    public override string Path => $"{Constants.Namespaces.DomainBuilders}/Abstractions";

    public override object CreateModel()
        => CreateBuilderInterfaces();
}
