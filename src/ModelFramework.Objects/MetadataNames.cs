namespace ModelFramework.Objects
{
    public static class MetadataNames
    {
        /// <summary>
        /// Metadata name for defining a custom using.
        /// </summary>
        public const string CustomUsing = "ModelFramework.Objects.CustomUsing";

        /// <summary>
        /// Metadata name for defining custom modifiers.
        /// </summary>
        public const string CustomModifiers = "ModelFramework.Objects.CustomModifiers";

        /// <summary>
        /// Metadata name for defining a property getter visibility.
        /// </summary>
        public const string PropertyGetterVisibility = "ModelFramework.Objects.GetterVisibility";

        /// <summary>
        /// Metadata name for defining a property setter visibility.
        /// </summary>
        public const string PropertySetterVisibility = "ModelFramework.Objects.SetterVisibility";

        /// <summary>
        /// Metadata name for defining a property initializer visibility.
        /// </summary>
        public const string PropertyInitVisibility = "ModelFramework.Objects.InitVisibility";

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
        public const string PropertyInitTemplateName = "ModelFramework.Objects.PropertyInitTemplateName";

        /// <summary>
        /// Metadata name for defining a property getter body.
        /// </summary>
        public const string PropertyGetterBody = "ModelFramework.Objects.PropertyGetterBody";

        /// <summary>
        /// Metadata name for defining a property setter body.
        /// </summary>
        public const string PropertySetterBody = "ModelFramework.Objects.PropertySetterBody";

        /// <summary>
        /// Metadata name for defining a property initializer body.
        /// </summary>
        public const string PropertyInitBody = "ModelFramework.Objects.PropertyInitBody";

        /// <summary>
        /// Metadata name for defining code template name. (method or c'tor)
        /// </summary>
        public const string CodeTemplateName = "ModelFramework.Objects.CodeTemplateName";

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
        public const string CustomImmutableBuilderArgumentType = "ModelFramework.Objects.Immutable.Builder.ArgumentType";

        /// <summary>
        /// Metadata name for defining a custom initialization on a Add method, where {0} is the property name
        /// </summary>
        public const string CustomImmutableBuilderAddExpression = "ModelFramework.Objects.Immutable.Builder.AddExpression";

        /// <summary>
        /// Metadata name for defining a custom initialization on a With method, where {0} is the property name in pascal case, and {1} is the C# corrected property name in pascal case
        /// </summary>
        public const string CustomImmutableBuilderWithExpression = "ModelFramework.Objects.Immutable.Builder.WithExpression";

        /// <summary>
        /// Metadata name for defining a custom parameter expression in the builder.
        /// </summary>
        public const string CustomImmutableBuilderMethodParameterExpression = "ModelFramework.Objects.Immutable.Ctor.Builder.MethodParameterExpression";

        /// <summary>
        /// Metadata name for defining a custom initialization in the constructor of the builder.
        /// </summary>
        public const string CustomImmutableBuilderConstructorInitializeExpression = "ModelFramework.Objects.Immutable.Builder.ConstructorInitializeExpression";

        /// <summary>
        /// Metadata name for defining a custom typename for a With/Add overload in a builder.
        /// </summary>
        public const string CustomImmutableBuilderWithOverloadArgumentType = "ModelFramework.Objects.Immutable.Builder.WithOverload.ArgumentType";

        /// <summary>
        /// Metadata name for defining a custom expression for a With/Add overload in a builder.
        /// </summary>
        public const string CustomImmutableBuilderWithOverloadInitializeExpression = "ModelFramework.Objects.Immutable.Builder.WithOverload.InitializeExpression";

        /// <summary>
        /// Metadata name for defining a custom type for argument in builder c'tor.
        /// </summary>
        public const string CustomBuilderArgumentType = "ModelFramework.Objects.Builder.Ctor.CustomArgumentType";

        /// <summary>
        /// Metadata name for defining a custom type for argument in observable c'tor.
        /// </summary>
        public const string CustomObservableArgumentType = "ModelFramework.Objects.Observable.Ctor.CustomArgumentType";

        /// <summary>
        /// Metadata name for skipping methods on auto-generated interfaces.
        /// </summary>
        public const string SkipMethodOnAutoGenerateInterface = "ModelFramework.Objects.SkipMethodOnAutoGenerateInterface";
    }
}
