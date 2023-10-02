namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsBuilderInterfaces : ClassFrameworkCSharpClassBase
{
    public override string Path => $"{Constants.Namespaces.DomainBuilders}/Abstractions";

    public override object CreateModel()
        => GetType().Assembly.GetTypes()
            .Where(x => x.Namespace == "ClassFramework.CodeGeneration.Models.Abstractions")
            .Select(x => x.ToInterfaceBuilder()
                .WithNamespace(CurrentNamespace)
                .WithVisibility(ModelFramework.Objects.Contracts.Visibility.Public)
                .WithName($"{x.Name}Builder")
                .Chain(y => y.Properties.RemoveAll(z => z.ParentTypeFullName != x.FullName))
                .WithAll(y => y.Properties, z =>
                {
                    z.TypeName = MapCodeGenerationNamespacesToDomain(z.TypeName).Replace("System.Collections.Generic.IReadOnlyCollection", "System.Collections.Generic.List", StringComparison.Ordinal);
                    if (!z.TypeName.Contains(".Domains", StringComparison.Ordinal))
                    {
                        foreach (var mapping in GetBuilderNamespaceMappings())
                        {
                            if (z.TypeName.IndexOf($"{mapping.Key}.", StringComparison.Ordinal) > -1)
                            {
                                z.TypeName = z.TypeName.Replace($"{mapping.Key}.", $"{mapping.Value}.", StringComparison.Ordinal);
                                if (z.TypeName.EndsWith(">", StringComparison.Ordinal))
                                {
                                    z.TypeName = z.TypeName.ReplaceSuffix(">", "Builder>", StringComparison.Ordinal);
                                }
                                else
                                {
                                    z.TypeName += "Builder";
                                }
                            }
                        }
                    }
                    z.HasSetter = true;
                    z.SetterVisibility = null; //TODO: Find out why this is set to Private. null should be sufficient (means equal to the property visibility)
                    z.Attributes.Clear();
                })
                .Chain(y =>
                {
                    for (int i = 0; i < y.Interfaces.Count; i++)
                    {
                        y.Interfaces[i] = y.Interfaces[i].Replace("ClassFramework.CodeGeneration.Models.Abstractions.", "ClassFramework.Domain.Builders.Abstractions.", StringComparison.Ordinal) + "Builder";
                    }
                })
                .Build()
            ).ToArray();
}
