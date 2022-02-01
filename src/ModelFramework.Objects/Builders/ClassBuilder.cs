namespace ModelFramework.Objects.Builders;

public partial class ClassBuilder
{
    public ClassBuilder WithBaseClass(Type baseClassType)
        => WithBaseClass(baseClassType.FullName);

    public ClassBuilder AddUsings(params string[] usings)
        => AddMetadata(usings.Select(x => new MetadataBuilder().WithName(MetadataNames.CustomUsing).WithValue(x)));

    public ClassBuilder AddGeneratedCodeAttribute(string generatorName, string generatorVersion)
        => AddAttributes(new AttributeBuilder().ForCodeGenerator(generatorName, generatorVersion));

    public ClassBuilder AddExcludeFromCodeCoverageAttribute()
        => AddAttributes(new AttributeBuilder().WithName(typeof(ExcludeFromCodeCoverageAttribute).FullName));
}
