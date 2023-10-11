namespace ClassFramework.Pipelines;

public static class MetadataNames
{
    /// <summary>
    /// Metadata name for defining a custom typename for argument in builder.
    /// </summary>
    public const string CustomBuilderArgumentType = "ClassFramework.Builder.ArgumentType";

    /// <summary>
    /// Metadata name for defining a custom default value in builder c'tor.
    /// </summary>
    public const string CustomBuilderDefaultValue = "ClassFramework.Builder.Ctor.CustomDefaultValue";

    /// <summary>
    /// Metadata name for defining a custom initialization in the constructor of the builder.
    /// </summary>
    public const string CustomBuilderConstructorInitializeExpression = "ClassFramework.Builder.ConstructorInitializeExpression";

    /// <summary>
    /// Metadata name for defining a custom parameter expression in the builder.
    /// </summary>
    public const string CustomBuilderMethodParameterExpression = "ClassFramework.Builder.Ctor.MethodParameterExpression";

}
