namespace ModelFramework.Objects.Builders;

public partial class AttributeBuilder
{
    public AttributeBuilder WithName(Type type)
        => WithName(type.FullName);

    public AttributeBuilder AddNameAndParameter(string name, object value)
        => WithName(name).AddParameters(new AttributeParameterBuilder().WithValue(value));

    public AttributeBuilder ForCodeGenerator(string generatorName, string generatorVersion)
        => WithName(typeof(GeneratedCodeAttribute).FullName)
            .AddParameters
            (
                new AttributeParameterBuilder().WithValue(generatorName),
                new AttributeParameterBuilder().WithValue(generatorVersion)
            );
}
