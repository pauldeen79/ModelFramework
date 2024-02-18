namespace ClassFramework.Pipelines;

public class PipelineSettings
{
    public PipelineSettings(
        bool addBackingFields,
        bool addCopyConstructor,
        bool addFullConstructor,
        string addMethodNameFormatString,
        bool addNullChecks,
        bool addPublicParameterlessConstructor,
        bool addSetters,
        bool allowGenerationWithoutProperties,
        Func<System.Attribute, AttributeBuilder> attributeInitializeDelegate,
        Class baseClass,
        string baseClassBuilderNameSpace,
        string builderExtensionsNameFormatString,
        string builderExtensionsNamespaceFormatString,
        string builderNameFormatString,
        string builderNamespaceFormatString,
        string buildMethodName,
        string buildTypedMethodName,
        string collectionCopyStatementFormatString,
        string collectionInitializationStatementFormatString,
        string collectionTypeName,
        Predicate<Domain.Attribute> copyAttributePredicate,
        bool copyAttributes,
        Predicate<string> copyInterfacePredicate,
        bool copyInterfaces,
        bool createAsObservable,
        bool createConstructors,
        bool createRecord,
        bool enableBuilderInheritance,
        bool enableInheritance,
        bool enableNullableReferenceTypes,
        string entityNameFormatString,
        string entityNamespaceFormatString,
        Func<IParentTypeContainer, IType, bool> inheritanceComparisonDelegate,
        Func<IParentTypeContainer, Type, bool> inheritanceComparisonDelegateForReflection,
        bool isAbstract,
        bool isForAbstractBuilder,
        string nameFormatString,
        string namespaceFormatString,
        IReadOnlyCollection<NamespaceMapping> namespaceMappings,
        string newCollectionTypeName,
        string nonCollectionInitializationStatementFormatString,
        bool partial,
        bool setDefaultValues,
        string setDefaultValuesMethodName,
        string setMethodNameFormatString,
        SubVisibility setterVisibility,
        string toBuilderFormatString,
        string toTypedBuilderFormatString,
        IReadOnlyCollection<TypenameMapping> typenameMappings,
        bool useBaseClassFromSourceModel,
        bool useExceptionThrowIfNull)
    {
        AddBackingFields = addBackingFields;
        AddCopyConstructor = addCopyConstructor;
        AddFullConstructor = addFullConstructor;
        AddMethodNameFormatString = addMethodNameFormatString;
        AddNullChecks = addNullChecks;
        AddPublicParameterlessConstructor = addPublicParameterlessConstructor;
        AddSetters = addSetters;
        AllowGenerationWithoutProperties = allowGenerationWithoutProperties;
        AttributeInitializeDelegate = attributeInitializeDelegate;
        BaseClass = baseClass;
        BaseClassBuilderNameSpace = baseClassBuilderNameSpace;
        BuilderExtensionsNameFormatString = builderExtensionsNameFormatString;
        BuilderExtensionsNamespaceFormatString = builderExtensionsNamespaceFormatString;
        BuilderNameFormatString = builderNameFormatString;
        BuilderNamespaceFormatString = builderNamespaceFormatString;
        BuildMethodName = buildMethodName;
        BuildTypedMethodName = buildTypedMethodName;
        CollectionCopyStatementFormatString = collectionCopyStatementFormatString;
        CollectionInitializationStatementFormatString = collectionInitializationStatementFormatString;
        CollectionTypeName = collectionTypeName;
        CopyAttributePredicate = copyAttributePredicate;
        CopyAttributes = copyAttributes;
        CopyInterfacePredicate = copyInterfacePredicate;
        CopyInterfaces = copyInterfaces;
        CreateAsObservable = createAsObservable;
        CreateConstructors = createConstructors;
        CreateRecord = createRecord;
        EnableBuilderInheritance = enableBuilderInheritance;
        EnableInheritance = enableInheritance;
        EnableNullableReferenceTypes = enableNullableReferenceTypes;
        EntityNameFormatString = entityNameFormatString;
        EntityNamespaceFormatString = entityNamespaceFormatString;
        InheritanceComparisonDelegate = inheritanceComparisonDelegate;
        InheritanceComparisonDelegateForReflection = inheritanceComparisonDelegateForReflection;
        IsAbstract = isAbstract;
        IsForAbstractBuilder = isForAbstractBuilder;
        NameFormatString = nameFormatString;
        NamespaceFormatString = namespaceFormatString;
        NamespaceMappings = namespaceMappings;
        NewCollectionTypeName = newCollectionTypeName;
        NonCollectionInitializationStatementFormatString = nonCollectionInitializationStatementFormatString;
        Partial = partial;
        SetDefaultValues = setDefaultValues;
        SetDefaultValuesMethodName = setDefaultValuesMethodName;
        SetMethodNameFormatString = setMethodNameFormatString;
        SetterVisibility = setterVisibility;
        ToBuilderFormatString = toBuilderFormatString;
        ToTypedBuilderFormatString = toTypedBuilderFormatString;
        TypenameMappings = typenameMappings;
        UseBaseClassFromSourceModel = useBaseClassFromSourceModel;
        UseExceptionThrowIfNull = useExceptionThrowIfNull;
    }

    public bool AddBackingFields
    {
        get;
    }

    public bool AddCopyConstructor
    {
        get;
    }

    public bool AddFullConstructor
    {
        get;
    }

    public string AddMethodNameFormatString
    {
        get;
    }

    public bool AddNullChecks
    {
        get;
    }

    public bool AddPublicParameterlessConstructor
    {
        get;
    }

    public bool AddSetters
    {
        get;
    }

    public bool AllowGenerationWithoutProperties
    {
        get;
    }

    public Func<System.Attribute, AttributeBuilder> AttributeInitializeDelegate
    {
        get;
    }

    public Class BaseClass
    {
        get;
    }

    public string BaseClassBuilderNameSpace
    {
        get;
    }

    public string BuilderExtensionsNameFormatString
    {
        get;
    }

    public string BuilderExtensionsNamespaceFormatString
    {
        get;
    }

    public string BuilderNameFormatString
    {
        get;
    }

    public string BuilderNamespaceFormatString
    {
        get;
    }

    public string BuildMethodName
    {
        get;
    }

    public string BuildTypedMethodName
    {
        get;
    }

    public string CollectionCopyStatementFormatString
    {
        get;
    }

    public string CollectionInitializationStatementFormatString
    {
        get;
    }

    public string CollectionTypeName
    {
        get;
    }

    public Predicate<Domain.Attribute> CopyAttributePredicate
    {
        get;
    }

    public bool CopyAttributes
    {
        get;
    }

    public Predicate<string> CopyInterfacePredicate
    {
        get;
    }

    public bool CopyInterfaces
    {
        get;
    }

    public bool CreateAsObservable
    {
        get;
    }

    public bool CreateConstructors
    {
        get;
    }

    public bool CreateRecord
    {
        get;
    }

    public bool EnableBuilderInheritance
    {
        get;
    }

    public bool EnableInheritance
    {
        get;
    }

    public bool EnableNullableReferenceTypes
    {
        get;
    }

    public string EntityNameFormatString
    {
        get;
    }

    public string EntityNamespaceFormatString
    {
        get;
    }

    public Func<IParentTypeContainer, IType, bool> InheritanceComparisonDelegate
    {
        get;
    }

    public Func<IParentTypeContainer, Type, bool> InheritanceComparisonDelegateForReflection
    {
        get;
    }

    public bool IsAbstract
    {
        get;
    }

    public bool IsForAbstractBuilder
    {
        get;
    }

    public string NameFormatString
    {
        get;
    }

    public string NamespaceFormatString
    {
        get;
    }

    public IReadOnlyCollection<NamespaceMapping> NamespaceMappings
    {
        get;
    }

    public string NewCollectionTypeName
    {
        get;
    }

    public string NonCollectionInitializationStatementFormatString
    {
        get;
    }

    public bool Partial
    {
        get;
    }

    public bool SetDefaultValues
    {
        get;
    }

    public string SetDefaultValuesMethodName
    {
        get;
    }

    public string SetMethodNameFormatString
    {
        get;
    }

    public SubVisibility SetterVisibility
    {
        get;
    }

    public string ToBuilderFormatString
    {
        get;
    }

    public string ToTypedBuilderFormatString
    {
        get;
    }

    public IReadOnlyCollection<TypenameMapping> TypenameMappings
    {
        get;
    }

    public bool UseBaseClassFromSourceModel
    {
        get;
    }

    public bool UseExceptionThrowIfNull
    {
        get;
    }
}
