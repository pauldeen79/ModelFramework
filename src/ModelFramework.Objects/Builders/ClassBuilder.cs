namespace ModelFramework.Objects.Builders;

public partial class ClassBuilder
{
    public ClassBuilder WithBaseClass(Type baseClassType)
        => WithBaseClass(baseClassType.FullName);

    public ClassBuilder AddExcludeFromCodeCoverageAttribute()
        => AddAttributes(new AttributeBuilder().WithName(typeof(ExcludeFromCodeCoverageAttribute).FullName));

    public ClassBuilder AsReadOnly()
        => this.Chain(x => x.Properties.ForEach(x => x.AsReadOnly()));

    public ClassBuilder AddBuilderCopyConstructorAdditionalArguments(params ParameterBuilder[] parameters)
        => AddMetadata(parameters.Select(x => new MetadataBuilder()
            .WithName(MetadataNames.AdditionalBuilderCopyConstructorAdditionalParameter)
            .WithValue(x.Build())));
}
