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
                .Chain(y => y.Properties.RemoveAll(z => z.ParentTypeFullName != x.FullName))
                .WithAll(y => y.Properties, z => z.TypeName = MapCodeGenerationNamespacesToDomain(z.TypeName))
                .Chain(y =>
                {
                    for (int i = 0; i < y.Interfaces.Count; i++)
                    {
                        y.Interfaces[i] = y.Interfaces[i].Replace("ClassFramework.CodeGeneration.Models.Abstractions.", "ClassFramework.Domain.Abstractions.", StringComparison.Ordinal);
                    }
                })
                .Build()
            )
            .ToArray();
}
