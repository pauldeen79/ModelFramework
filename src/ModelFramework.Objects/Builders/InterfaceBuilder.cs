namespace ModelFramework.Objects.Builders;

public partial class InterfaceBuilder
{
    public InterfaceBuilder AddGeneratedCodeAttribute(string generatorName, string generatorVersion)
        => AddAttributes(new AttributeBuilder().ForCodeGenerator(generatorName, generatorVersion));

    public InterfaceBuilder AddUsings(params string[] usings)
        => AddMetadata(usings.Select(x => new MetadataBuilder().WithName(MetadataNames.CustomUsing).WithValue(x)));

    //TODO: generate overload for this
    public InterfaceBuilder AddInterfaces(params Type[] types)
        => AddInterfaces(types.Select(x => x.FullName));
}
