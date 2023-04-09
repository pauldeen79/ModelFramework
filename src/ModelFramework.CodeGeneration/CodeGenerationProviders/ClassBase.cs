namespace ModelFramework.CodeGeneration.CodeGenerationProviders;

public abstract class ClassBase : ICodeGenerationProvider
{
    public bool GenerateMultipleFiles { get; private set; }
    public bool SkipWhenFileExists { get; private set; }
    public string BasePath { get; private set; } = string.Empty;

    public abstract string Path { get; }
    public abstract string DefaultFileName { get; }
    public abstract bool RecurseOnDeleteGeneratedFiles { get; }
    public abstract object CreateModel();

    public virtual string LastGeneratedFilesFileName => $"*{FileNameSuffix}.cs";
    public virtual Action? AdditionalActionDelegate => null;

    protected abstract bool EnableNullableContext { get; }
    protected abstract bool CreateCodeGenerationHeader { get; }
    protected virtual string FileNameSuffix => ".generated";
    protected virtual bool UseCustomInitializersOnAttributeBuilder => false;

    protected virtual AttributeBuilder? AttributeInitializeDelegate(Attribute sourceAttribute)
        => sourceAttribute switch
        {
            StringLengthAttribute stringLengthAttribute =>
                new AttributeBuilder(stringLengthAttribute.GetType())
                    .AddParameters(new AttributeParameterBuilder().WithValue(stringLengthAttribute.MaximumLength))
                    .AddParameters(CreateConditional(() => stringLengthAttribute.MinimumLength > 0, new AttributeParameterBuilder().WithValue(stringLengthAttribute.MinimumLength).WithName(nameof(stringLengthAttribute.MinimumLength))))
                    .AddParameters(CreateConditional(() => !string.IsNullOrEmpty(stringLengthAttribute.ErrorMessage), new AttributeParameterBuilder().WithValue(stringLengthAttribute.ErrorMessage).WithName(nameof(stringLengthAttribute.ErrorMessage)))),
            RangeAttribute rangeAttribute =>
                new AttributeBuilder(rangeAttribute.GetType())
                    .AddParameters(new AttributeParameterBuilder().WithValue(rangeAttribute.Minimum))
                    .AddParameters(new AttributeParameterBuilder().WithValue(rangeAttribute.Maximum))
                    .AddParameters(CreateConditional(() => !string.IsNullOrEmpty(rangeAttribute.ErrorMessage), new AttributeParameterBuilder().WithValue(rangeAttribute.ErrorMessage).WithName(nameof(RangeAttribute.ErrorMessage)))),
            MinLengthAttribute minLengthAttribute =>
                new AttributeBuilder(minLengthAttribute.GetType())
                    .AddParameters(new AttributeParameterBuilder().WithValue(minLengthAttribute.Length))
                    .AddParameters(CreateConditional(() => !string.IsNullOrEmpty(minLengthAttribute.ErrorMessage), new AttributeParameterBuilder().WithValue(minLengthAttribute.ErrorMessage).WithName(nameof(MinLengthAttribute.ErrorMessage)))),
            MaxLengthAttribute maxLengthAttribute =>
                new AttributeBuilder(maxLengthAttribute.GetType())
                    .AddParameters(new AttributeParameterBuilder().WithValue(maxLengthAttribute.Length))
                    .AddParameters(CreateConditional(() => !string.IsNullOrEmpty(maxLengthAttribute.ErrorMessage), new AttributeParameterBuilder().WithValue(maxLengthAttribute.ErrorMessage).WithName(nameof(MaxLengthAttribute.ErrorMessage)))),
            RegularExpressionAttribute regularExpressionAttribute =>
                new AttributeBuilder(regularExpressionAttribute.GetType())
                    .AddParameters(new AttributeParameterBuilder().WithValue(regularExpressionAttribute.Pattern))
                    .AddParameters(CreateConditional(() => regularExpressionAttribute.MatchTimeoutInMilliseconds != 2000, new AttributeParameterBuilder().WithValue(regularExpressionAttribute.MatchTimeoutInMilliseconds).WithName(nameof(RegularExpressionAttribute.MatchTimeoutInMilliseconds))))
                    .AddParameters(CreateConditional(() => !string.IsNullOrEmpty(regularExpressionAttribute.ErrorMessage), new AttributeParameterBuilder().WithValue(regularExpressionAttribute.ErrorMessage).WithName(nameof(RegularExpressionAttribute.ErrorMessage)))),
            RequiredAttribute requiredAttribute =>
                new AttributeBuilder(requiredAttribute.GetType())
                    .AddParameters(CreateConditional(() => requiredAttribute.AllowEmptyStrings, new AttributeParameterBuilder().WithValue(requiredAttribute.AllowEmptyStrings).WithName(nameof(RequiredAttribute.AllowEmptyStrings))))
                    .AddParameters(CreateConditional(() => !string.IsNullOrEmpty(requiredAttribute.ErrorMessage), new AttributeParameterBuilder().WithValue(requiredAttribute.ErrorMessage).WithName(nameof(RequiredAttribute.ErrorMessage)))),
            UrlAttribute urlAttribute =>
                new AttributeBuilder(urlAttribute.GetType())
                    .AddParameters(CreateConditional(() => !string.IsNullOrEmpty(urlAttribute.ErrorMessage), new AttributeParameterBuilder().WithValue(urlAttribute.ErrorMessage).WithName(nameof(UrlAttribute.ErrorMessage)))),
            _ => new AttributeBuilder(sourceAttribute, UseCustomInitializersOnAttributeBuilder)
        };

    public void Initialize(bool generateMultipleFiles, bool skipWhenFileExists, string basePath)
    {
        GenerateMultipleFiles = generateMultipleFiles;
        SkipWhenFileExists = skipWhenFileExists;
        BasePath = basePath;
    }

    public object CreateAdditionalParameters()
        => new Dictionary<string, object>
        {
            { nameof(CSharpClassGenerator.EnableNullableContext), EnableNullableContext },
            { nameof(CSharpClassGenerator.CreateCodeGenerationHeader), CreateCodeGenerationHeader },
            { nameof(CSharpClassGenerator.GenerateMultipleFiles), GenerateMultipleFiles },
            { nameof(CSharpClassGenerator.SkipWhenFileExists), SkipWhenFileExists },
            { nameof(CSharpClassGenerator.FileNamePrefix), FileNamePrefix },
            { nameof(CSharpClassGenerator.FileNameSuffix), FileNameSuffix }
        };

    public object CreateGenerator()
        => new CSharpClassGenerator();

    protected static IEnumerable<AttributeParameterBuilder> CreateConditional(Func<bool> condition, AttributeParameterBuilder result)
    {
        if (condition.Invoke())
        {
            yield return result;
        }
    }

    private string FileNamePrefix => string.IsNullOrEmpty(Path)
        ? string.Empty
        : Path + "/";
}
