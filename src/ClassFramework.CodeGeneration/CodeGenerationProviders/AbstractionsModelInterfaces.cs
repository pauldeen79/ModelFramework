namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class AbstractionsModelInterfaces : ClassFrameworkModelClassBase
{
    public override string Path => $"{Constants.Namespaces.DomainModels}/Abstractions";

    public override object CreateModel()
        => CreateBuilderInterfacesModel();
}
