namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsInterfaces : ClassFrameworkCSharpClassBase
{
    public override string Path => $"{Constants.Namespaces.Domain}/Abstractions";

    public override object CreateModel()
        => GetType().Assembly.GetTypes()
            .Where(x => x.Namespace == $"{CodeGenerationRootNamespace}.Models.Abstractions")
            .Select(x => x.ToInterfaceBuilder()
                .Chain(y => y.Properties.RemoveAll(z => z.ParentTypeFullName != x.FullName))
                .Chain(y => y.Interfaces.RemoveAll(z => z.StartsWith($"{CodeGenerationRootNamespace}.Models.I", StringComparison.Ordinal)))
            )
            .Select(CreateInterface)
            .ToArray();
}
