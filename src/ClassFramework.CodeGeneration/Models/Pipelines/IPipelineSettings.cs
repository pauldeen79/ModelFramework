namespace ClassFramework.CodeGeneration.Models.Pipelines;

internal interface IPipelineSettings
{
    bool AddBackingFields { get; }

    bool AddCopyConstructor { get; }

    bool AddFullConstructor { get; }

    string AddMethodNameFormatString { get; }

    bool AddNullChecks { get; }

    bool AddPublicParameterlessConstructor { get; }

    bool AddSetters { get; }

    bool AllowGenerationWithoutProperties { get; }

    Func<Attribute, IAttribute> AttributeInitializeDelegate { get; }

    ITypeBase? BaseClass { get; }

    string BaseClassBuilderNameSpace { get; }

    string BuilderExtensionsCollectionCopyStatementFormatString { get; }
    
    string BuilderExtensionsNameFormatString { get; }

    string BuilderExtensionsNamespaceFormatString { get; }

    string BuilderNameFormatString { get; }

    string BuilderNamespaceFormatString { get; }

    string BuildMethodName { get; }

    string BuildTypedMethodName { get; }

    string CollectionCopyStatementFormatString { get; }

    string CollectionInitializationStatementFormatString { get; }

    string CollectionTypeName { get; }

    Predicate<IAttribute>? CopyAttributePredicate { get; }

    bool CopyAttributes { get; }

    Predicate<string>? CopyInterfacePredicate { get; }

    bool CopyInterfaces { get; }

    bool CreateAsObservable { get; }

    bool CreateConstructors { get; }

    bool CreateRecord { get; }

    bool EnableBuilderInheritance { get; }

    bool EnableInheritance { get; }

    bool EnableNullableReferenceTypes { get; }

    string EntityNameFormatString { get; }

    string EntityNamespaceFormatString { get; }

    Func<IParentTypeContainer, IType, bool>? InheritanceComparisonDelegate { get; }

    Func<IParentTypeContainer, Type, bool>? InheritanceComparisonDelegateForReflection { get; }

    bool IsAbstract { get; }

    bool IsForAbstractBuilder { get; }

    string NameFormatString { get; }

    string NamespaceFormatString { get; }

    IReadOnlyCollection<INamespaceMapping> NamespaceMappings { get; }

    string BuilderNewCollectionTypeName { get; }

    string EntityNewCollectionTypeName { get; }

    string NonCollectionInitializationStatementFormatString { get; }

    bool CreateAsPartial { get; }

    bool SetDefaultValuesInEntityConstructor { get; }

    string SetDefaultValuesMethodName { get; }

    string SetMethodNameFormatString { get; }

    SubVisibility SetterVisibility { get; }

    string ToBuilderFormatString { get; }

    string ToTypedBuilderFormatString { get; }

    IReadOnlyCollection<ITypenameMapping> TypenameMappings { get; }

    bool UseBaseClassFromSourceModel { get; }

    bool UseExceptionThrowIfNull { get; }

    Domains.ArgumentValidationType ValidateArguments { get; }

    Domains.ArgumentValidationType OriginalValidateArguments { get; }
}
