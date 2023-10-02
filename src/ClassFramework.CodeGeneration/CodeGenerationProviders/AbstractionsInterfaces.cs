namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsInterfaces : ClassFrameworkCSharpClassBase
{
    public override string Path => $"{Constants.Namespaces.Domain}/Abstractions";

    public override object CreateModel()
        => GetType().Assembly.GetTypes()
            .Where(x => x.Namespace == "ClassFramework.CodeGeneration.Models.Abstractions")
            .Select(x => x.ToInterfaceBuilder()
                .WithNamespace(CurrentNamespace)
                .WithVisibility(ModelFramework.Objects.Contracts.Visibility.Public)
                .WithAll(y => y.Properties, z => z.TypeName = MapCodeGenerationNamespacesToDomain(z.TypeName))
                .Build()
            )
            .ToArray();
}
