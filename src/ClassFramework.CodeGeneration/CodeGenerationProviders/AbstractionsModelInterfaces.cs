namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsModelInterfaces : ClassFrameworkModelClassBase
{
    public override string Path => $"{Constants.Namespaces.DomainModels}/Abstractions";

    public override object CreateModel()
        => CreateBuilderInterfaces(
            GetType().Assembly.GetTypes()
                .Where(x => x.Namespace == $"{CodeGenerationRootNamespace}.Models.Abstractions")
                .Select(x => x.ToInterfaceBuilder()
                    .Chain(y => y.Properties.RemoveAll(z => z.ParentTypeFullName != x.FullName)))
            );
}
