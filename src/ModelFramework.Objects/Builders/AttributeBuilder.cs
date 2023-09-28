namespace ModelFramework.Objects.Builders;

public partial class AttributeBuilder
{
    public static AttributeBuilder? DefaultInitializer(System.Attribute sourceAttribute)
        => sourceAttribute switch
        {
            StringLengthAttribute stringLengthAttribute =>
                new AttributeBuilder(stringLengthAttribute)
                    .AddParameters(new AttributeParameterBuilder().WithValue(stringLengthAttribute.MaximumLength))
                    .AddParameters(CreateConditional(() => stringLengthAttribute.MinimumLength > 0, new AttributeParameterBuilder().WithValue(stringLengthAttribute.MinimumLength).WithName(nameof(stringLengthAttribute.MinimumLength))))
                    .AddParameters(CreateConditional(() => !string.IsNullOrEmpty(stringLengthAttribute.ErrorMessage), new AttributeParameterBuilder().WithValue(stringLengthAttribute.ErrorMessage).WithName(nameof(stringLengthAttribute.ErrorMessage)))),
            RangeAttribute rangeAttribute =>
                new AttributeBuilder(rangeAttribute)
                    .AddParameters(new AttributeParameterBuilder().WithValue(rangeAttribute.Minimum))
                    .AddParameters(new AttributeParameterBuilder().WithValue(rangeAttribute.Maximum))
                    .AddParameters(CreateConditional(() => !string.IsNullOrEmpty(rangeAttribute.ErrorMessage), new AttributeParameterBuilder().WithValue(rangeAttribute.ErrorMessage).WithName(nameof(RangeAttribute.ErrorMessage)))),
            MinLengthAttribute minLengthAttribute =>
                new AttributeBuilder(minLengthAttribute)
                    .AddParameters(new AttributeParameterBuilder().WithValue(minLengthAttribute.Length))
                    .AddParameters(CreateConditional(() => !string.IsNullOrEmpty(minLengthAttribute.ErrorMessage), new AttributeParameterBuilder().WithValue(minLengthAttribute.ErrorMessage).WithName(nameof(MinLengthAttribute.ErrorMessage)))),
            MaxLengthAttribute maxLengthAttribute =>
                new AttributeBuilder(maxLengthAttribute)
                    .AddParameters(new AttributeParameterBuilder().WithValue(maxLengthAttribute.Length))
                    .AddParameters(CreateConditional(() => !string.IsNullOrEmpty(maxLengthAttribute.ErrorMessage), new AttributeParameterBuilder().WithValue(maxLengthAttribute.ErrorMessage).WithName(nameof(MaxLengthAttribute.ErrorMessage)))),
            RegularExpressionAttribute regularExpressionAttribute =>
                new AttributeBuilder(regularExpressionAttribute)
                    .AddParameters(new AttributeParameterBuilder().WithValue(regularExpressionAttribute.Pattern))
                    .AddParameters(CreateConditional(() => regularExpressionAttribute.MatchTimeoutInMilliseconds != 2000, new AttributeParameterBuilder().WithValue(regularExpressionAttribute.MatchTimeoutInMilliseconds).WithName(nameof(RegularExpressionAttribute.MatchTimeoutInMilliseconds))))
                    .AddParameters(CreateConditional(() => !string.IsNullOrEmpty(regularExpressionAttribute.ErrorMessage), new AttributeParameterBuilder().WithValue(regularExpressionAttribute.ErrorMessage).WithName(nameof(RegularExpressionAttribute.ErrorMessage)))),
            RequiredAttribute requiredAttribute =>
                new AttributeBuilder(requiredAttribute)
                    .AddParameters(CreateConditional(() => requiredAttribute.AllowEmptyStrings, new AttributeParameterBuilder().WithValue(requiredAttribute.AllowEmptyStrings).WithName(nameof(RequiredAttribute.AllowEmptyStrings))))
                    .AddParameters(CreateConditional(() => !string.IsNullOrEmpty(requiredAttribute.ErrorMessage), new AttributeParameterBuilder().WithValue(requiredAttribute.ErrorMessage).WithName(nameof(RequiredAttribute.ErrorMessage)))),
            UrlAttribute urlAttribute =>
                new AttributeBuilder(urlAttribute)
                    .AddParameters(CreateConditional(() => !string.IsNullOrEmpty(urlAttribute.ErrorMessage), new AttributeParameterBuilder().WithValue(urlAttribute.ErrorMessage).WithName(nameof(UrlAttribute.ErrorMessage)))),
            _ => new AttributeBuilder().WithName(sourceAttribute.GetType().FullName)
        };

    private static IEnumerable<AttributeParameterBuilder> CreateConditional(Func<bool> condition, AttributeParameterBuilder result)
    {
        if (condition.Invoke())
        {
            yield return result;
        }
    }

    public AttributeBuilder(System.Attribute source, Func<System.Attribute, AttributeBuilder?>? initializeDelegate = null) : this()
    {
        var prefilled = initializeDelegate != null
            ? initializeDelegate.Invoke(source)
            : null;
        if (prefilled != null)
        {
            Name = prefilled.Name;
            Parameters = prefilled.Parameters;
            Metadata = prefilled.Metadata;
        }
        else
        {
            Name = source.GetType().FullName;
        }
    }

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
