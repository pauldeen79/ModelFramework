namespace ClassFramework.Pipelines.Reflection;

public record PipelineBuilderGenerationSettings
{
    public bool AllowGenerationWithoutProperties { get; }
    public bool UseBaseClassFromSourceModel { get; }
    public bool Partial { get; }
    public bool CreateConstructors { get; }
    public Func<System.Attribute, AttributeBuilder> AttributeInitializeDelegate { get; }

    public PipelineBuilderGenerationSettings(
        bool allowGenerationWithoutProperties = false,
        bool useBaseClassFromSourceModel = true,
        bool partial = true,
        bool createConstructors = true,
        Func<System.Attribute, AttributeBuilder>? attributeInitializeDelegate = null)
    {
        AllowGenerationWithoutProperties = allowGenerationWithoutProperties;
        UseBaseClassFromSourceModel = useBaseClassFromSourceModel;
        Partial = partial;
        CreateConstructors = createConstructors;
        AttributeInitializeDelegate = attributeInitializeDelegate ?? DefaultInitializer;
    }

    private static AttributeBuilder DefaultInitializer(System.Attribute sourceAttribute)
        => sourceAttribute switch
        {
            StringLengthAttribute stringLengthAttribute =>
                new AttributeBuilder().WithName(stringLengthAttribute.GetType())
                    .AddParameters(new AttributeParameterBuilder().WithValue(stringLengthAttribute.MaximumLength))
                    .AddParameters(CreateConditional(() => stringLengthAttribute.MinimumLength > 0, new AttributeParameterBuilder().WithValue(stringLengthAttribute.MinimumLength).WithName(nameof(stringLengthAttribute.MinimumLength))))
                    .AddParameters(ErrorMessage(stringLengthAttribute)),
            RangeAttribute rangeAttribute =>
                new AttributeBuilder().WithName(rangeAttribute.GetType())
                    .AddParameters(new AttributeParameterBuilder().WithValue(rangeAttribute.Minimum))
                    .AddParameters(new AttributeParameterBuilder().WithValue(rangeAttribute.Maximum))
                    .AddParameters(ErrorMessage(rangeAttribute)),
            MinLengthAttribute minLengthAttribute =>
                new AttributeBuilder().WithName(minLengthAttribute.GetType())
                    .AddParameters(new AttributeParameterBuilder().WithValue(minLengthAttribute.Length))
                    .AddParameters(ErrorMessage(minLengthAttribute)),
            MaxLengthAttribute maxLengthAttribute =>
                new AttributeBuilder().WithName(maxLengthAttribute.GetType())
                    .AddParameters(new AttributeParameterBuilder().WithValue(maxLengthAttribute.Length))
                    .AddParameters(ErrorMessage(maxLengthAttribute)),
            RegularExpressionAttribute regularExpressionAttribute =>
                new AttributeBuilder().WithName(regularExpressionAttribute.GetType())
                    .AddParameters(new AttributeParameterBuilder().WithValue(regularExpressionAttribute.Pattern))
                    .AddParameters(CreateConditional(() => regularExpressionAttribute.MatchTimeoutInMilliseconds != 2000, new AttributeParameterBuilder().WithValue(regularExpressionAttribute.MatchTimeoutInMilliseconds).WithName(nameof(RegularExpressionAttribute.MatchTimeoutInMilliseconds))))
                    .AddParameters(ErrorMessage(regularExpressionAttribute)),
            RequiredAttribute requiredAttribute =>
                new AttributeBuilder().WithName(requiredAttribute.GetType())
                    .AddParameters(CreateConditional(() => requiredAttribute.AllowEmptyStrings, new AttributeParameterBuilder().WithValue(requiredAttribute.AllowEmptyStrings).WithName(nameof(RequiredAttribute.AllowEmptyStrings))))
                    .AddParameters(ErrorMessage(requiredAttribute)),
            ValidationAttribute validationAttribute =>
                new AttributeBuilder().WithName(validationAttribute.GetType())
                    .AddParameters(ErrorMessage(validationAttribute)),
            _ => new AttributeBuilder().WithName(sourceAttribute.GetType())
        };

    private static IEnumerable<AttributeParameterBuilder> ErrorMessage(ValidationAttribute validationAttribute)
        => CreateConditional(() => !string.IsNullOrEmpty(validationAttribute.ErrorMessage), new AttributeParameterBuilder().WithValue(validationAttribute.ErrorMessage).WithName(nameof(ValidationAttribute.ErrorMessage)));

    private static IEnumerable<AttributeParameterBuilder> CreateConditional(Func<bool> condition, AttributeParameterBuilder result)
    {
        if (condition.Invoke())
        {
            yield return result;
        }
    }
}
