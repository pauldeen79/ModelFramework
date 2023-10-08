namespace ClassFramework.Pipelines;

public static class MetadataNames
{
    /// <summary>
    /// Metadata name for defining a custom type for argument (of type string) in builder.
    /// </summary>
    public const string CustomBuilderArgumentType = "ClassFramework.Builder.ArgumentType";

    /// <summary>
    /// Metadata name for defining an additional parameter (of type ClassParameter) in the copy constructor of the builder.
    /// </summary>
    public const string AdditionalBuilderCopyConstructorAdditionalParameter = "ClassFramework.Builder.AdditionalCopyConstructorAdditionalParameter";

    /// <summary>
    /// Metadata name for defining a custom default value in builder c'tor.
    /// </summary>
    public const string CustomBuilderDefaultValue = "ClassFramework.Builder.Ctor.CustomDefaultValue";

    /// <summary>
    /// Metadata name for defining a custom initialization in the constructor of the builder.
    /// </summary>
    public const string CustomBuilderConstructorInitializeExpression = "ClassFramework.Builder.ConstructorInitializeExpression";
}
