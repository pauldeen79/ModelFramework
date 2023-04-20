namespace ModelFramework.Objects.Builders;

public partial class ClassBuilder
{
    public ClassBuilder WithBaseClass(Type baseClassType)
        => WithBaseClass(baseClassType.FullName);

    public ClassBuilder AddExcludeFromCodeCoverageAttribute()
        => AddAttributes(new AttributeBuilder().WithName(typeof(ExcludeFromCodeCoverageAttribute).FullName));

    public ClassBuilder AsReadOnly()
        => this.WithAll(x => x.Properties, x => x.AsReadOnly());

    public ClassBuilder AsWritable()
        => this.WithAll(x => x.Properties, x => x.AsWritable());

    public ClassBuilder AddBuilderCopyConstructorAdditionalArguments(params ParameterBuilder[] parameters)
        => AddMetadata(parameters.Select(x => new MetadataBuilder()
            .WithName(MetadataNames.AdditionalBuilderCopyConstructorAdditionalParameter)
            .WithValue(x.Build())));
}
