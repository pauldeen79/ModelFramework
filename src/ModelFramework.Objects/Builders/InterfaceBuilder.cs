namespace ModelFramework.Objects.Builders;

public partial class InterfaceBuilder
{
    public InterfaceBuilder AddGeneratedCodeAttribute(string generatorName, string generatorVersion)
        => AddAttributes(new AttributeBuilder().ForCodeGenerator(generatorName, generatorVersion));

    public InterfaceBuilder AddUsings(params string[] usings)
        => AddMetadata(usings.Select(x => new MetadataBuilder().WithName(MetadataNames.CustomUsing).WithValue(x)));

    public InterfaceBuilder AddNamespacesToAbbreviate(params string[] namespacesToAbbreviate)
        => AddMetadata(namespacesToAbbreviate.Select(x => new MetadataBuilder().WithName(MetadataNames.NamespaceToAbbreviate).WithValue(x)));
}
