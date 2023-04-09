namespace ModelFramework.Objects.Builders;

public partial class AttributeBuilder
{
    private static List<Func<System.Attribute, AttributeBuilder?>> _customInitializers = CreateDefaultCustomInitializers();

    private static List<Func<System.Attribute, AttributeBuilder?>> CreateDefaultCustomInitializers()
        => new List<Func<System.Attribute, AttributeBuilder?>>
        (
            new[]
            {
                GetStringLengthInitializer(),
                GetRangeInitializer(),
                GetMinLengthInitializer(),
                GetMaxLengthInitializer(),
                GetRegularExpressionInitializer(),
                GetRequiredInitializer(),
                GetUrlInitializer(),
            }
        );

    private static Func<System.Attribute, AttributeBuilder?> GetStringLengthInitializer()
        => new(a => a is StringLengthAttribute x
            ? new AttributeBuilder(x.GetType())
                .AddParameters(new AttributeParameterBuilder().WithValue(x.MaximumLength))
                .AddParameters(CreateConditional(() => x.MinimumLength > 0, new AttributeParameterBuilder().WithValue(x.MinimumLength).WithName(nameof(StringLengthAttribute.MinimumLength))))
                .AddParameters(CreateConditional(() => !string.IsNullOrEmpty(x.ErrorMessage), new AttributeParameterBuilder().WithValue(x.ErrorMessage).WithName(nameof(StringLengthAttribute.ErrorMessage))))
            : null);

    private static Func<System.Attribute, AttributeBuilder?> GetRangeInitializer()
        => new(a => a is RangeAttribute x
            ? new AttributeBuilder(x.GetType())
                .AddParameters(new AttributeParameterBuilder().WithValue(x.Minimum))
                .AddParameters(new AttributeParameterBuilder().WithValue(x.Maximum))
                .AddParameters(CreateConditional(() => !string.IsNullOrEmpty(x.ErrorMessage), new AttributeParameterBuilder().WithValue(x.ErrorMessage).WithName(nameof(RangeAttribute.ErrorMessage))))
            : null);

    private static Func<System.Attribute, AttributeBuilder?> GetMinLengthInitializer()
        => new(a => a is MinLengthAttribute x
            ? new AttributeBuilder(x.GetType())
                .AddParameters(new AttributeParameterBuilder().WithValue(x.Length))
                .AddParameters(CreateConditional(() => !string.IsNullOrEmpty(x.ErrorMessage), new AttributeParameterBuilder().WithValue(x.ErrorMessage).WithName(nameof(MinLengthAttribute.ErrorMessage))))
            : null);

    private static Func<System.Attribute, AttributeBuilder?> GetMaxLengthInitializer()
        => new(a => a is MaxLengthAttribute x
            ? new AttributeBuilder(x.GetType())
                .AddParameters(new AttributeParameterBuilder().WithValue(x.Length))
                .AddParameters(CreateConditional(() => !string.IsNullOrEmpty(x.ErrorMessage), new AttributeParameterBuilder().WithValue(x.ErrorMessage).WithName(nameof(MaxLengthAttribute.ErrorMessage))))
            : null);

    private static Func<System.Attribute, AttributeBuilder?> GetRegularExpressionInitializer()
        => new(a => a is RegularExpressionAttribute x
            ? new AttributeBuilder(x.GetType())
                .AddParameters(new AttributeParameterBuilder().WithValue(x.Pattern))
                .AddParameters(CreateConditional(() => x.MatchTimeoutInMilliseconds != 2000, new AttributeParameterBuilder().WithValue(x.MatchTimeoutInMilliseconds).WithName(nameof(RegularExpressionAttribute.MatchTimeoutInMilliseconds))))
                .AddParameters(CreateConditional(() => !string.IsNullOrEmpty(x.ErrorMessage), new AttributeParameterBuilder().WithValue(x.ErrorMessage).WithName(nameof(RegularExpressionAttribute.ErrorMessage))))
            : null);

    private static Func<System.Attribute, AttributeBuilder?> GetRequiredInitializer()
        => new(a => a is RequiredAttribute x
            ? new AttributeBuilder(x.GetType())
                .AddParameters(CreateConditional(() => x.AllowEmptyStrings, new AttributeParameterBuilder().WithValue(x.AllowEmptyStrings).WithName(nameof(RequiredAttribute.AllowEmptyStrings))))
                .AddParameters(CreateConditional(() => !string.IsNullOrEmpty(x.ErrorMessage), new AttributeParameterBuilder().WithValue(x.ErrorMessage).WithName(nameof(RequiredAttribute.ErrorMessage))))
            : null);

    private static Func<System.Attribute, AttributeBuilder?> GetUrlInitializer()
        => new(a => a is UrlAttribute x
            ? new AttributeBuilder(x.GetType())
                .AddParameters(CreateConditional(() => !string.IsNullOrEmpty(x.ErrorMessage), new AttributeParameterBuilder().WithValue(x.ErrorMessage).WithName(nameof(UrlAttribute.ErrorMessage))))
            : null);

    private static IEnumerable<AttributeParameterBuilder> CreateConditional(Func<bool> condition, AttributeParameterBuilder result)
    {
        if (condition.Invoke())
        {
            yield return result;
        }
    }

    public static void RegisterCustomInitializer(Func<System.Attribute, AttributeBuilder?> initializer)
        => _customInitializers.Add(initializer);

    public AttributeBuilder(Type type) : this()
    {
        Name.Append(type.FullName);
    }

    public AttributeBuilder(System.Attribute source) : this()
    {
        bool found = false;
        foreach (var initializer in _customInitializers)
        {
            var result = initializer.Invoke(source);
            if (result != null)
            {
                _nameDelegate = result._nameDelegate;
                Parameters = result.Parameters;
                Metadata = result.Metadata;
                found = true;
                break;
            }
        }

        if (!found)
        {
            Name.Append(source.GetType().FullName);
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
