﻿namespace ModelFramework.Objects;

public static class MetadataNames
{
    /// <summary>
    /// Metadata name for defining a custom using.
    /// </summary>
    public const string CustomUsing = "ModelFramework.Objects.CustomUsing";

    /// <summary>
    /// Metadata name for defining a namespace that can be abbreviated in code generation.
    /// </summary>
    public const string NamespaceToAbbreviate = "ModelFramework.Objects.NamespaceToAbbreviate";

    /// <summary>
    /// Metadata name for defining custom modifiers.
    /// </summary>
    public const string CustomModifiers = "ModelFramework.Objects.CustomModifiers";

    /// <summary>
    /// Metadata name for defining a property getter visibility.
    /// </summary>
    public const string PropertyGetterModifiers = "ModelFramework.Objects.GetterVisibility";

    /// <summary>
    /// Metadata name for defining a property setter visibility.
    /// </summary>
    public const string PropertySetterModifiers = "ModelFramework.Objects.SetterVisibility";

    /// <summary>
    /// Metadata name for defining a property initializer visibility.
    /// </summary>
    public const string PropertyInitializerModifiers = "ModelFramework.Objects.InitVisibility";

    /// <summary>
    /// Metadata name for defining a property getter template name.
    /// </summary>
    public const string PropertyGetterTemplateName = "ModelFramework.Objects.PropertyGetterTemplateName";

    /// <summary>
    /// Metadata name for defining a property setter template name.
    /// </summary>
    public const string PropertySetterTemplateName = "ModelFramework.Objects.PropertySetterTemplateName";

    /// <summary>
    /// Metadata name for defining a property initializer template name.
    /// </summary>
    public const string PropertyInitializerTemplateName = "ModelFramework.Objects.PropertyInitializerTemplateName";

    /// <summary>
    /// Metadata name for defining code template name. (method or c'tor)
    /// </summary>
    public const string CodeTemplateName = "ModelFramework.Objects.CodeTemplateName";

    /// <summary>
    /// Metadata name for defining a custom default value in immutable builder c'tor.
    /// </summary>
    public const string CustomImmutableBuilderDefaultValue = "ModelFramework.Objects.Immutable.Builder.Ctor.CustomDefaultValue";

    /// <summary>
    /// Metadata name for defining a custom default value in immutable c'tor.
    /// </summary>
    public const string CustomImmutableDefaultValue = "ModelFramework.Objects.Immutable.Ctor.CustomDefaultValue";

    /// <summary>
    /// Metadata name for defining a custom type for argument in immutable c'tor.
    /// </summary>
    public const string CustomImmutableArgumentType = "ModelFramework.Objects.Immutable.Ctor.CustomArgumentType";

    /// <summary>
    /// Metadata name for defining a custom type for argument in immutable builder.
    /// </summary>
    public const string CustomBuilderArgumentType = "ModelFramework.Objects.Builder.ArgumentType";

    /// <summary>
    /// Metadata name for defining a custom initialization on a Add method, where {0} is the property name
    /// </summary>
    public const string CustomBuilderAddExpression = "ModelFramework.Objects.Builder.AddExpression";

    /// <summary>
    /// Metadata name for defining a custom initialization on a With method, where {0} is the property name in pascal case, and {1} is the C# corrected property name in pascal case
    /// </summary>
    public const string CustomBuilderWithExpression = "ModelFramework.Objects.Builder.WithExpression";

    /// <summary>
    /// Metadata name for defining a custom parameter expression in the builder.
    /// </summary>
    public const string CustomBuilderMethodParameterExpression = "ModelFramework.Objects.Immutable.Ctor.MethodParameterExpression";

    /// <summary>
    /// Metadata name for defining a custom initialization in the constructor of the builder.
    /// </summary>
    public const string CustomBuilderConstructorInitializeExpression = "ModelFramework.Objects.Builder.ConstructorInitializeExpression";

    /// <summary>
    /// Metadata name for defining an additional parameter (of type IParameter) in the copy constructor of the builder.
    /// </summary>
    public const string AdditionalBuilderCopyConstructorAdditionalParameter = "ModelFramework.Objects.Builder.AdditionalCopyConstructorAdditionalParameter";

    /// <summary>
    /// Metadata name for defining custom typenames for a With/Add overload in a builder. (type: string[])
    /// </summary>
    public const string CustomBuilderWithOverloadArgumentTypes = "ModelFramework.Objects.Builder.WithOverload.ArgumentTypes";

    /// <summary>
    /// Metadata name for defining nullable indicator for custom typenames for a With/Add overload in a builder. (type: bool[])
    /// </summary>
    public const string CustomBuilderWithOverloadArgumentTypeNullables = "ModelFramework.Objects.Builder.WithOverload.ArgumentTypeNullables";

    /// <summary>
    /// Metadata name for defining param array indicator for custom typenames for a With/Add overload in a builder. (type: bool[])
    /// </summary>
    public const string CustomBuilderWithOverloadArgumentTypeParamArrays = "ModelFramework.Objects.Builder.WithOverload.ArgumentTypeParamArrays";

    /// <summary>
    /// Metadata name for defining custom argument names for a With/Add overload in a builder. (type: string[])
    /// </summary>
    public const string CustomBuilderWithOverloadArgumentNames = "ModelFramework.Objects.Builder.WithOverload.ArgumentNames";

    /// <summary>
    /// Metadata name for defining a custom method name for a With/Add overload in a builder.
    /// </summary>
    public const string CustomBuilderWithOverloadMethodName = "ModelFramework.Objects.Builder.WithOverload.MethodName";

    /// <summary>
    /// Metadata name for defining a custom expression for a With/Add overload in a builder.
    /// </summary>
    public const string CustomBuilderWithOverloadInitializeExpression = "ModelFramework.Objects.Builder.WithOverload.InitializeExpression";

    /// <summary>
    /// Metadata name for defining default value on a With method in a builder.
    /// </summary>
    public const string CustomBuilderWithDefaultPropertyValue = "ModelFramework.Objects.Builder.Property.DefaultValue";

    /// <summary>
    /// Metadata name for defining a custom type for argument in observable c'tor.
    /// </summary>
    public const string CustomObservableArgumentType = "ModelFramework.Objects.Observable.Ctor.CustomArgumentType";
}
