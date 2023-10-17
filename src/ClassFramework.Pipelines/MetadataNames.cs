﻿namespace ClassFramework.Pipelines;

public static class MetadataNames
{
    /// <summary>
    /// Metadata name for defining a custom typename for argument in builder. Note that the value will be converted to string.
    /// </summary>
    public const string CustomBuilderArgumentType = "ClassFramework.Builder.ArgumentType";

    /// <summary>
    /// Metadata name for defining a custom default value in builder c'tor.
    /// </summary>
    public const string CustomBuilderDefaultValue = "ClassFramework.Builder.Ctor.CustomDefaultValue";

    /// <summary>
    /// Metadata name for defining a custom initialization in the constructor of the builder. Note that the value will be converted to string.
    /// </summary>
    public const string CustomBuilderConstructorInitializeExpression = "ClassFramework.Builder.ConstructorInitializeExpression";

    /// <summary>
    /// Metadata name for defining a custom parameter expression in the builder. Note that the value will be converted to string.
    /// </summary>
    public const string CustomBuilderMethodParameterExpression = "ClassFramework.Builder.Ctor.MethodParameterExpression";

    /// <summary>
    /// Metadata name for defining default value for a property on a fluent (With/Set) method in a builder. Note that the value is of type System.Object, and will be rendered using CsharpFormat in code generation.
    /// </summary>
    public const string CustomBuilderWithDefaultPropertyValue = "ClassFramework.Builder.Property.DefaultValue";

    /// <summary>
    /// Metadata name for defining a custom initialization on a fluent (With/Set) method. Note that the value will be converted to string.
    /// </summary>
    public const string CustomBuilderWithExpression = "ClassFramework.Builder.WithExpression";
}
