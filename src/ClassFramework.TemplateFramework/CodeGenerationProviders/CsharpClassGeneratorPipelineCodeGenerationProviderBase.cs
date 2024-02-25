﻿namespace ClassFramework.TemplateFramework.CodeGenerationProviders;

public abstract class CsharpClassGeneratorPipelineCodeGenerationProviderBase : CsharpClassGeneratorCodeGenerationProviderBase
{
    protected CsharpClassGeneratorPipelineCodeGenerationProviderBase(
        ICsharpExpressionCreator csharpExpressionCreator,
        IPipeline<IConcreteTypeBuilder, BuilderContext> builderPipeline,
        IPipeline<IConcreteTypeBuilder, BuilderInterfaceContext> builderInterfacePipeline,
        IPipeline<IConcreteTypeBuilder, EntityContext> entityPipeline,
        IPipeline<IConcreteTypeBuilder, OverrideEntityContext> overrideEntityPipeline,
        IPipeline<TypeBaseBuilder, ReflectionContext> reflectionPipeline,
        IPipeline<InterfaceBuilder, InterfaceContext> interfacePipeline) : base(csharpExpressionCreator)
    {
        Guard.IsNotNull(builderPipeline);
        Guard.IsNotNull(builderInterfacePipeline);
        Guard.IsNotNull(entityPipeline);
        Guard.IsNotNull(overrideEntityPipeline);
        Guard.IsNotNull(reflectionPipeline);
        Guard.IsNotNull(interfacePipeline);

        _builderPipeline = builderPipeline;
        _builderInterfacePipeline = builderInterfacePipeline;
        _entityPipeline = entityPipeline;
        _overrideEntityPipeline = overrideEntityPipeline;
        _reflectionPipeline = reflectionPipeline;
        _interfacePipeline = interfacePipeline;
    }

    private readonly IPipeline<IConcreteTypeBuilder, BuilderContext> _builderPipeline;
    private readonly IPipeline<IConcreteTypeBuilder, BuilderInterfaceContext> _builderInterfacePipeline;
    private readonly IPipeline<IConcreteTypeBuilder, EntityContext> _entityPipeline;
    private readonly IPipeline<IConcreteTypeBuilder, OverrideEntityContext> _overrideEntityPipeline;
    private readonly IPipeline<TypeBaseBuilder, ReflectionContext> _reflectionPipeline;
    private readonly IPipeline<InterfaceBuilder, InterfaceContext> _interfacePipeline;

    public override CsharpClassGeneratorSettings Settings
        => new CsharpClassGeneratorSettingsBuilder()
            .WithPath(Path)
            .WithRecurseOnDeleteGeneratedFiles(RecurseOnDeleteGeneratedFiles)
            .WithLastGeneratedFilesFilename(LastGeneratedFilesFilename)
            .WithEncoding(Encoding)
            .WithCultureInfo(CultureInfo.InvariantCulture)
            .WithGenerateMultipleFiles()
            .WithCreateCodeGenerationHeader()
            .WithEnableNullableContext()
            .WithFilenameSuffix(FilenameSuffix)
            .WithEnvironmentVersion(EnvironmentVersion)
            .Build();

    protected abstract string ProjectName { get; }
    protected abstract Type RecordCollectionType { get; }
    protected abstract Type RecordConcreteCollectionType { get; }
    protected abstract Type BuilderCollectionType { get; }

    protected virtual string EnvironmentVersion => string.Empty;
    protected virtual string RootNamespace => $"{ProjectName}.Domain";
    protected virtual string CodeGenerationRootNamespace => $"{ProjectName}.CodeGeneration";
    protected virtual string CoreNamespace => $"{ProjectName}.Core";
    protected virtual bool CopyAttributes => false;
    protected virtual bool CopyInterfaces => false;
    protected virtual bool AddNullChecks => true;
    protected virtual bool UseExceptionThrowIfNull => false;
    protected virtual bool CreateRecord => false;
    protected virtual bool AddBackingFields => false;
    protected virtual SubVisibility SetterVisibility => SubVisibility.InheritFromParent;
    protected virtual bool AddSetters => false;
    protected virtual bool CreateAsObservable => false;
    protected virtual string? CollectionPropertyGetStatement => null;
    protected virtual ArgumentValidationType ValidateArgumentsInConstructor => ArgumentValidationType.DomainOnly;
    protected virtual bool EnableEntityInheritance => false;
    protected virtual bool EnableBuilderInhericance => false;
    protected virtual Class? BaseClass => null;
    protected virtual bool IsAbstract => false;
    protected virtual string BaseClassBuilderNamespace => string.Empty;
    protected virtual bool AllowGenerationWithoutProperties => true;
    protected virtual string SetMethodNameFormatString => "With{Name}";
    protected virtual string AddMethodNameFormatString => "Add{Name}";
    protected virtual string ToBuilderFormatString => "ToBuilder";
    protected virtual string ToTypedBuilderFormatString => "ToTypedBuilder";
    protected virtual bool AddFullConstructor => true;
    protected virtual bool AddPublicParameterlessConstructor => false;
    protected virtual bool AddCopyConstructor => true;
    protected virtual bool SetDefaultValues => true;
    protected virtual string FilenameSuffix => ".template.generated";
    protected virtual Func<IParentTypeContainer, IType, bool>? InheritanceComparisonDelegate => new Func<IParentTypeContainer, IType, bool>((parentNameContainer, typeBase)
        => parentNameContainer is not null
        && typeBase is not null
        && (string.IsNullOrEmpty(parentNameContainer.ParentTypeFullName)
            || (BaseClass is not null && !BaseClass.Properties.Any(x => x.Name == (parentNameContainer as INameContainer)?.Name))
            || parentNameContainer.ParentTypeFullName.GetClassName().In(typeBase.Name, $"I{typeBase.Name}")
            || Array.Exists(GetModelAbstractBaseTyped(), x => x == parentNameContainer.ParentTypeFullName.GetClassName())
            || (parentNameContainer.ParentTypeFullName.StartsWith($"{RootNamespace}.Abstractions.") && typeBase.Namespace == RootNamespace)
        ));

    protected virtual string[] GetModelAbstractBaseTyped() => Array.Empty<string>();

    protected virtual string[] GetExternalCustomBuilderTypes() => Array.Empty<string>();

    protected virtual string[] GetCustomBuilderTypes()
        => GetPureAbstractModels()
            .Select(x => x.GetEntityClassName())
            .Concat(GetExternalCustomBuilderTypes())
            .ToArray();

    protected virtual IEnumerable<Type> GetPureAbstractModels()
        => GetType().Assembly.GetTypes()
            .Where(x => x.IsInterface && x.Namespace?.StartsWith($"{CodeGenerationRootNamespace}.Models.") == true && x.GetInterfaces().Count(x => x.Namespace == $"{CodeGenerationRootNamespace}.Models") == 1)
            .Select(x => x.GetInterfaces().First(x => x.Namespace == $"{CodeGenerationRootNamespace}.Models"))
            .Distinct();

    /// <summary>
    /// Gets the base typename, based on a derived class.
    /// </summary>
    /// <param name="className">The typename to get the base classname from.</param>
    /// <returns>Base classname when found, otherwise string.Empty</returns>
    protected string GetEntityClassName(string className)
        => Array.Find(GetCustomBuilderTypes(), x => className.EndsWith(x, StringComparison.InvariantCulture)) ?? string.Empty;

    protected TypeBase[] GetImmutableClasses(TypeBase[] models, string entitiesNamespace)
    {
        Guard.IsNotNull(models);
        Guard.IsNotNull(entitiesNamespace);

        if (ValidateArgumentsInConstructor == ArgumentValidationType.Shared && !(EnableEntityInheritance && BaseClass is null))
        {
            return models.SelectMany
            (
                x => new[]
                {
                    CreateImmutableClass(x, entitiesNamespace),
                    CreateImmutableOverrideClass(x, entitiesNamespace)
                }
            ).ToArray();
        }

        return models.Select
        (
            x => CreateImmutableClass(x, entitiesNamespace)
        ).ToArray();
    }

    protected TypeBase[] GetInterfaces(TypeBase[] models, string interfacesNamespace)
    {
        Guard.IsNotNull(models);
        Guard.IsNotNull(interfacesNamespace);

        return models.Select(x => CreateInterface(x, interfacesNamespace, RecordCollectionType.WithoutGenerics(), AddSetters)).ToArray();
    }

    protected TypeBase[] GetBuilders(TypeBase[] models, string buildersNamespace, string entitiesNamespace)
    {
        Guard.IsNotNull(models);
        Guard.IsNotNull(buildersNamespace);
        Guard.IsNotNull(entitiesNamespace);

        return models.Select(x =>
        {
            var entityBuilder = new ClassBuilder();
            _ = _entityPipeline
                .Process(entityBuilder, new EntityContext(x, CreateEntityPipelineSettings(entitiesNamespace), CultureInfo.InvariantCulture))
                .GetValueOrThrow();

            return CreateBuilderClass(entityBuilder.Build(), buildersNamespace, entitiesNamespace);
        }).ToArray();
    }

    protected TypeBase[] GetBuilderExtensions(TypeBase[] models, string buildersNamespace, string entitiesNamespace)
    {
        Guard.IsNotNull(models);
        Guard.IsNotNull(buildersNamespace);
        Guard.IsNotNull(entitiesNamespace);

        return models.Select(x =>
        {
            var interfaceBuilder = new InterfaceBuilder();
            _ = _interfacePipeline
                .Process(interfaceBuilder, new InterfaceContext(x, CreateInterfacePipelineSettings(entitiesNamespace, string.Empty, InheritanceComparisonDelegate, true), CultureInfo.InvariantCulture))
                .GetValueOrThrow();

            return CreateBuilderExtensionsClass(interfaceBuilder.Build(), buildersNamespace, entitiesNamespace);
        }).ToArray();
    }

    protected TypeBase[] GetNonGenericBuilders(TypeBase[] models, string buildersNamespace, string entitiesNamespace)
    {
        Guard.IsNotNull(models);
        Guard.IsNotNull(buildersNamespace);
        Guard.IsNotNull(entitiesNamespace);

        return models.Select(x =>
        {
            var entityBuilder = new ClassBuilder();
            _ = _entityPipeline
                .Process(entityBuilder, new EntityContext(x, CreateEntityPipelineSettings(entitiesNamespace, overrideAddNullChecks: GetOverrideAddNullChecks()), CultureInfo.InvariantCulture))
                .GetValueOrThrow();

            return CreateNonGenericBuilderClass(entityBuilder.Build(), buildersNamespace, entitiesNamespace);
        }).ToArray();
    }

    protected TypeBase[] GetBuilderInterfaces(TypeBase[] models, string buildersNamespace, string entitiesNamespace, string interfacesNamespace)
    {
        Guard.IsNotNull(models);
        Guard.IsNotNull(buildersNamespace);
        Guard.IsNotNull(entitiesNamespace);
        Guard.IsNotNull(interfacesNamespace);

        return GetBuilders(models, buildersNamespace, entitiesNamespace)
            .Select(x => CreateInterface(x, interfacesNamespace, RecordConcreteCollectionType.WithoutGenerics(), true, "I{Class.Name}"))
            .ToArray();
    }

    protected TypeBase[] GetCoreModels()
        => GetType().Assembly.GetTypes()
            .Where(x => x.IsInterface && x.Namespace == $"{CodeGenerationRootNamespace}.Models" && !GetCustomBuilderTypes().Contains(x.GetEntityClassName()))
            .Select(x => _reflectionPipeline.Process(new InterfaceBuilder(), new ReflectionContext(x, CreateReflectionPipelineSettings(), CultureInfo.InvariantCulture)).GetValueOrThrow().Build())
            .ToArray();

    protected TypeBase[] GetAbstractionsInterfaces()
        => GetType().Assembly.GetTypes()
            .Where(x => x.IsInterface && x.Namespace == $"{CodeGenerationRootNamespace}.Models.Abstractions")
            .Select(x => _reflectionPipeline.Process(new InterfaceBuilder(), new ReflectionContext(x, CreateReflectionPipelineSettings(), CultureInfo.InvariantCulture)).GetValueOrThrow().Build())
            .ToArray();

    protected TypeBase[] GetAbstractModels()
        => GetPureAbstractModels()
            .Select(x => _reflectionPipeline.Process(new InterfaceBuilder(), new ReflectionContext(x, CreateReflectionPipelineSettings(), CultureInfo.InvariantCulture)).GetValueOrThrow().Build())
            .ToArray();

    protected TypeBase[] GetOverrideModels(Type abstractType)
    {
        Guard.IsNotNull(abstractType);

        return GetType().Assembly.GetTypes()
            .Where(x => x.IsInterface && Array.Exists(x.GetInterfaces(), y => y == abstractType))
            .Select(x => _reflectionPipeline.Process(new InterfaceBuilder(), new ReflectionContext(x, CreateReflectionPipelineSettings(), CultureInfo.InvariantCulture)).GetValueOrThrow().Build())
            .ToArray();
    }

    protected Class CreateBaseclass(Type type, string @namespace)
    {
        Guard.IsNotNull(type);
        Guard.IsNotNull(@namespace);

        var reflectionSettings = CreateReflectionPipelineSettings();
        var typeBase = _reflectionPipeline.Process(new InterfaceBuilder(), new ReflectionContext(type, reflectionSettings, CultureInfo.InvariantCulture)).GetValueOrThrow().Build();

        var builder = new ClassBuilder();
        var entitySettings = new PipelineSettingsBuilder()
            .WithAddSetters(AddSetters)
            .WithAddBackingFields(AddBackingFields)
            .WithSetterVisibility(SetterVisibility)
            .WithCreateAsObservable(CreateAsObservable)
            .WithCreateRecord(CreateRecord)
            .WithAllowGenerationWithoutProperties(AllowGenerationWithoutProperties)
            .WithCopyAttributes(CopyAttributes)
            .WithCopyInterfaces(CopyInterfaces)
            .WithEntityNameFormatString("{Class.NameNoInterfacePrefix}")
            .WithEntityNamespaceFormatString(@namespace)
            .WithEnableInheritance()
            .WithIsAbstract()
            .WithBaseClass(null)
            .WithInheritanceComparisonDelegate(
                (parentNameContainer, typeBase)
                    => parentNameContainer is not null
                    && typeBase is not null
                    && (string.IsNullOrEmpty(parentNameContainer.ParentTypeFullName)
                        || parentNameContainer.ParentTypeFullName.GetClassName().In(typeBase.Name, $"I{typeBase.Name}")
                        || Array.Exists(GetModelAbstractBaseTyped(), x => x == parentNameContainer.ParentTypeFullName.GetClassName())
                        || (parentNameContainer.ParentTypeFullName.StartsWith($"{RootNamespace}.Abstractions.") && typeBase.Namespace == RootNamespace)
                    ))
            .WithEntityNewCollectionTypeName(RecordCollectionType.WithoutGenerics())
            .WithEnableNullableReferenceTypes()
            .AddTypenameMappings(CreateTypenameMappings())
            .AddNamespaceMappings(CreateNamespaceMappings())
            .WithValidateArguments(ValidateArgumentsInConstructor)
            .WithAddNullChecks(AddNullChecks)
            .WithUseExceptionThrowIfNull(UseExceptionThrowIfNull)
            .Build();
        _ = _entityPipeline.Process(builder, new EntityContext(typeBase, entitySettings, CultureInfo.InvariantCulture)).GetValueOrThrow();
        return builder.BuildTyped();
    }

    protected ArgumentValidationType CombineValidateArguments(ArgumentValidationType validateArgumentsInConstructor, bool secondCondition)
        => secondCondition
            ? validateArgumentsInConstructor
            : ArgumentValidationType.None;

    private PipelineSettings CreateReflectionPipelineSettings()
        => new PipelineSettingsBuilder()
            .WithAllowGenerationWithoutProperties(AllowGenerationWithoutProperties)
            .WithCopyAttributes(CopyAttributes)
            .WithCopyInterfaces(CopyInterfaces)
            .AddNamespaceMappings(CreateNamespaceMappings())
            .AddTypenameMappings(CreateTypenameMappings())
            .Build();

    private PipelineSettings CreateEntityPipelineSettings(
        string entitiesNamespace,
        ArgumentValidationType? forceValidateArgumentsInConstructor = null,
        bool? overrideAddNullChecks = null,
        string entityNameFormatString = "{Class.NameNoInterfacePrefix}")
        => new PipelineSettingsBuilder()
                .WithAddSetters(AddSetters)
                .WithAddBackingFields(AddBackingFields)
                .WithSetterVisibility(SetterVisibility)
                .WithCreateAsObservable(CreateAsObservable)
                .WithCreateRecord(CreateRecord)
                .WithAllowGenerationWithoutProperties(AllowGenerationWithoutProperties)
                .WithCopyAttributes(CopyAttributes)
                .WithCopyInterfaces(CopyInterfaces)
                .WithEntityNameFormatString(entityNameFormatString)
                .WithEntityNamespaceFormatString(entitiesNamespace)
                .WithToBuilderFormatString(ToBuilderFormatString)
                .WithToTypedBuilderFormatString(ToTypedBuilderFormatString)
                .WithEnableInheritance(EnableEntityInheritance)
                .WithIsAbstract(IsAbstract)
                .WithBaseClass(BaseClass?.ToBuilder())
                .WithInheritanceComparisonDelegate(InheritanceComparisonDelegate)
                .WithEntityNewCollectionTypeName(RecordCollectionType.WithoutGenerics())
                .WithEnableNullableReferenceTypes()
                .AddTypenameMappings(CreateTypenameMappings())
                .AddNamespaceMappings(CreateNamespaceMappings())
                .WithValidateArguments(forceValidateArgumentsInConstructor ?? CombineValidateArguments(ValidateArgumentsInConstructor, !(EnableEntityInheritance && BaseClass is null)))
                .WithOriginalValidateArguments(ValidateArgumentsInConstructor)
                .WithCollectionTypeName(RecordConcreteCollectionType.WithoutGenerics())
                .WithAddFullConstructor(AddFullConstructor)
                .WithAddPublicParameterlessConstructor(AddPublicParameterlessConstructor)
                .WithAddNullChecks(forceValidateArgumentsInConstructor != ArgumentValidationType.Shared && (overrideAddNullChecks ?? false))
                .WithUseExceptionThrowIfNull(UseExceptionThrowIfNull)
                .Build();

    private PipelineSettings CreateOverrideEntityPipelineSettings(
        string entitiesNamespace,
        ArgumentValidationType? forceValidateArgumentsInConstructor = null,
        bool? overrideAddNullChecks = null,
        string entityNameFormatString = "{Class.NameNoInterfacePrefix}")
        => new PipelineSettingsBuilder()
            .WithCreateRecord(CreateRecord)
            .WithAllowGenerationWithoutProperties(AllowGenerationWithoutProperties)
            .WithEntityNameFormatString(entityNameFormatString)
            .WithEntityNamespaceFormatString(entitiesNamespace)
            .WithEnableInheritance(EnableEntityInheritance)
            .WithIsAbstract(IsAbstract)
            .WithBaseClass(BaseClass?.ToBuilder())
            .WithInheritanceComparisonDelegate(InheritanceComparisonDelegate)
            .WithEntityNewCollectionTypeName(RecordCollectionType.WithoutGenerics())
            .WithEnableNullableReferenceTypes()
            .AddTypenameMappings(CreateTypenameMappings())
            .AddNamespaceMappings(CreateNamespaceMappings())
            .WithAddNullChecks(forceValidateArgumentsInConstructor != ArgumentValidationType.Shared && (overrideAddNullChecks ?? false))
            .WithUseExceptionThrowIfNull(UseExceptionThrowIfNull)
            .Build();

    private PipelineSettings CreateInterfacePipelineSettings(
        string interfacesNamespace,
        string newCollectionTypeName,
        Func<IParentTypeContainer, IType, bool>? inheritanceComparisonDelegate,
        bool addSetters,
        string nameFormatString = "{Class.Name}")
        => new PipelineSettingsBuilder()
            .WithNamespaceFormatString(interfacesNamespace)
            .WithNameFormatString(nameFormatString)
            .WithEnableInheritance(EnableEntityInheritance)
            .WithIsAbstract(IsAbstract)
            .WithBaseClass(BaseClass?.ToBuilder())
            .WithInheritanceComparisonDelegate(inheritanceComparisonDelegate)
            .WithEntityNewCollectionTypeName(newCollectionTypeName)
            .AddTypenameMappings(CreateTypenameMappings())
            .AddNamespaceMappings(CreateNamespaceMappings())
            .WithCopyAttributes(CopyAttributes)
            .WithCopyInterfaces(CopyInterfaces)
            .WithAddSetters(addSetters)
            .WithAllowGenerationWithoutProperties(AllowGenerationWithoutProperties)
            .Build();

    private IEnumerable<NamespaceMappingBuilder> CreateNamespaceMappings()
    {
        // From models to domain entities
        yield return new NamespaceMappingBuilder().WithSourceNamespace($"{CodeGenerationRootNamespace}.Models").WithTargetNamespace(CoreNamespace);
        yield return new NamespaceMappingBuilder().WithSourceNamespace($"{CodeGenerationRootNamespace}.Models.Domains").WithTargetNamespace($"{CoreNamespace}.Domains");
        yield return new NamespaceMappingBuilder().WithSourceNamespace($"{CodeGenerationRootNamespace}.Models.Abstractions").WithTargetNamespace($"{CoreNamespace}.Abstractions");

        // From domain entities to builders
        yield return new NamespaceMappingBuilder().WithSourceNamespace(CoreNamespace).WithTargetNamespace(CoreNamespace);
        yield return new NamespaceMappingBuilder().WithSourceNamespace($"{CoreNamespace}.Abstractions").WithTargetNamespace($"{CoreNamespace}.Abstractions")
            .AddMetadata
            (
                new MetadataBuilder().WithValue($"{CoreNamespace}.Builders.Abstractions").WithName(Pipelines.MetadataNames.CustomBuilderInterfaceNamespace),
                new MetadataBuilder().WithValue("{TypeName.ClassName}Builder").WithName(Pipelines.MetadataNames.CustomBuilderInterfaceName),
                new MetadataBuilder().WithValue($"{CoreNamespace}.Builders.Abstractions").WithName(Pipelines.MetadataNames.CustomBuilderParentTypeNamespace),
                new MetadataBuilder().WithValue("{ParentTypeName.ClassName}Builder").WithName(Pipelines.MetadataNames.CustomBuilderParentTypeName)
            );

        foreach (var entityClassName in GetPureAbstractModels().Select(x => x.GetEntityClassName().ReplaceSuffix("Base", string.Empty, StringComparison.Ordinal)))
        {
            yield return new NamespaceMappingBuilder().WithSourceNamespace($"{CodeGenerationRootNamespace}.Models.{entityClassName}s").WithTargetNamespace($"{CoreNamespace}.{entityClassName}s");
            yield return new NamespaceMappingBuilder().WithSourceNamespace($"{CoreNamespace}.{entityClassName}s").WithTargetNamespace($"{CoreNamespace}.{entityClassName}s");
        }
    }

    private TypenameMappingBuilder[] CreateTypenameMappings()
        => GetType().Assembly.GetTypes()
            .Where(x => x.IsInterface
                && x.Namespace?.StartsWith($"{CodeGenerationRootNamespace}.Models", StringComparison.Ordinal) == true
                && x.Namespace != $"{CodeGenerationRootNamespace}.Models.Abstractions"
                && x.Namespace != $"{CodeGenerationRootNamespace}.Models.Domains"
                && x.FullName is not null)
            .SelectMany(x =>
                new[]
                {
                    new TypenameMappingBuilder().WithSourceTypeName(x.FullName!).WithTargetTypeName($"{CoreNamespace}.{ReplaceStart(x.Namespace ?? string.Empty, $"{CodeGenerationRootNamespace}.Models", true)}{x.GetEntityClassName()}"),
                    new TypenameMappingBuilder().WithSourceTypeName($"{CoreNamespace}.{ReplaceStart(x.Namespace ?? string.Empty, $"{CodeGenerationRootNamespace}.Models", true)}{x.GetEntityClassName()}").WithTargetTypeName($"{CoreNamespace}.{ReplaceStart(x.Namespace ?? string.Empty, $"{CodeGenerationRootNamespace}.Models", true)}{x.GetEntityClassName()}")
                        .AddMetadata
                        (
                            new MetadataBuilder().WithValue($"{CoreNamespace}.Builders{ReplaceStart(x.Namespace ?? string.Empty, $"{CodeGenerationRootNamespace}.Models", false)}").WithName(Pipelines.MetadataNames.CustomBuilderNamespace),
                            new MetadataBuilder().WithValue("{TypeName.ClassName}Builder").WithName(Pipelines.MetadataNames.CustomBuilderName),
                            new MetadataBuilder().WithValue(x.Namespace != $"{CodeGenerationRootNamespace}.Models" && x.Namespace != $"{CodeGenerationRootNamespace}.Models.Abstractions"
                                ? $"new {CoreNamespace}.Builders{ReplaceStart(x.Namespace ?? string.Empty, $"{CodeGenerationRootNamespace}.Models", false)}.{x.GetEntityClassName()}Builder([Name])"
                                : "[Name][NullableSuffix].ToBuilder()").WithName(Pipelines.MetadataNames.CustomBuilderSourceExpression),
                                //: $"builderFactory.Create<{CoreNamespace}.Builders.{ReplaceStart(x.Namespace ?? string.Empty, $"{CodeGenerationRootNamespace}.Models", true)}{x.GetEntityClassName()}Builder>([Name])", Pipelines.MetadataNames.CustomBuilderSourceExpression),
                            new MetadataBuilder().WithValue(x.Namespace != $"{CodeGenerationRootNamespace}.Models" && x.Namespace != $"{CodeGenerationRootNamespace}.Models.Abstractions"
                                ? "[Name].BuildTyped()"
                                : "[Name].Build()").WithName(Pipelines.MetadataNames.CustomBuilderMethodParameterExpression)
                            //new MetadataBuilder().WithValue(new ParameterBuilder().WithName("builderFactory").WithTypeName($"{CoreNamespace}.Builders.Abstractions.IBuilderFactory").Build()).WithName(Pipelines.MetadataNames.CustomBuilderCopyConstructorParameter),
                        )
                })
            .Concat(new[]
            {
                new TypenameMappingBuilder().WithSourceTypeName(typeof(bool).FullName!).WithTargetTypeName(typeof(bool).FullName!),
                new TypenameMappingBuilder().WithSourceTypeName(typeof(List<>).WithoutGenerics()).WithTargetTypeName(typeof(List<>).WithoutGenerics()).AddMetadata(new MetadataBuilder().WithValue("[Expression].ToList()").WithName(Pipelines.MetadataNames.CustomCollectionInitialization)),
                new TypenameMappingBuilder().WithSourceTypeName(typeof(Collection<>).WithoutGenerics()).WithTargetTypeName(typeof(Collection<>).WithoutGenerics()).AddMetadata(new MetadataBuilder().WithValue("new [Type]<[Generics]>([Expression].ToList())").WithName(Pipelines.MetadataNames.CustomCollectionInitialization)),
                new TypenameMappingBuilder().WithSourceTypeName(typeof(ObservableCollection<>).WithoutGenerics()).WithTargetTypeName(typeof(ObservableCollection<>).WithoutGenerics()).AddMetadata(new MetadataBuilder().WithValue("new [Type]<[Generics]>([Expression])").WithName(Pipelines.MetadataNames.CustomCollectionInitialization)),
                new TypenameMappingBuilder().WithSourceTypeName(typeof(IReadOnlyCollection<>).WithoutGenerics()).WithTargetTypeName(typeof(IReadOnlyCollection<>).WithoutGenerics()).AddMetadata(new MetadataBuilder().WithValue("[Expression].ToList().AsReadOnly()").WithName(Pipelines.MetadataNames.CustomCollectionInitialization)),
                new TypenameMappingBuilder().WithSourceTypeName(typeof(IList<>).WithoutGenerics()).WithTargetTypeName(typeof(IList<>).WithoutGenerics()).AddMetadata(new MetadataBuilder().WithValue("[Expression].ToList()").WithName(Pipelines.MetadataNames.CustomCollectionInitialization)),
                new TypenameMappingBuilder().WithSourceTypeName(typeof(ICollection<>).WithoutGenerics()).WithTargetTypeName(typeof(ICollection<>).WithoutGenerics()).AddMetadata(new MetadataBuilder().WithValue("[Expression].ToList()").WithName(Pipelines.MetadataNames.CustomCollectionInitialization)),
            })
            .ToArray();

    private string ReplaceStart(string fullNamespace, string baseNamespace, bool appendDot)
    {
        if (fullNamespace.Length == 0)
        {
            return fullNamespace;
        }

        if (fullNamespace.StartsWith($"{baseNamespace}."))
        {
            return appendDot
                ? string.Concat(fullNamespace.AsSpan(baseNamespace.Length + 1), ".")
                : string.Concat(".", fullNamespace.AsSpan(baseNamespace.Length + 1));
        }

        return string.Empty;
    }

    private PipelineSettings CreateBuilderPipelineSettings(string buildersNamespace, string entitiesNamespace)
        => new PipelineSettingsBuilder(CreateEntityPipelineSettings(entitiesNamespace, forceValidateArgumentsInConstructor: ArgumentValidationType.None, overrideAddNullChecks: GetOverrideAddNullChecks()))
            .WithBuilderNewCollectionTypeName(BuilderCollectionType.WithoutGenerics())
            //.WithEnableNullableReferenceTypes()
            //.AddNamespaceMappings(CreateNamespaceMappings())
            //.AddTypenameMappings(CreateTypenameMappings())
            .WithBuilderNamespaceFormatString(buildersNamespace)
            .WithSetMethodNameFormatString(SetMethodNameFormatString)
            .WithAddMethodNameFormatString(AddMethodNameFormatString)
            .WithEnableBuilderInheritance(EnableBuilderInhericance)
            //.WithIsAbstract(IsAbstract)
            //.WithBaseClass(BaseClass?.ToBuilder())
            .WithBaseClassBuilderNameSpace(BaseClassBuilderNamespace)
            //.WithInheritanceComparisonDelegate(InheritanceComparisonDelegate)
            .WithAddCopyConstructor(AddCopyConstructor)
            .WithSetDefaultValuesInEntityConstructor(SetDefaultValues)
            .Build();

    private PipelineSettings CreateBuilderInterfacePipelineSettings(string buildersNamespace, string entitiesNamespace)
        => new PipelineSettingsBuilder(CreateEntityPipelineSettings(entitiesNamespace, forceValidateArgumentsInConstructor: ArgumentValidationType.None, overrideAddNullChecks: GetOverrideAddNullChecks()))
            .WithBuilderNewCollectionTypeName(BuilderCollectionType.WithoutGenerics())
            //.WithEnableNullableReferenceTypes()
            //.AddNamespaceMappings(CreateNamespaceMappings())
            //.AddTypenameMappings(CreateTypenameMappings())
            .WithBuilderNamespaceFormatString(buildersNamespace)
            .WithSetMethodNameFormatString(SetMethodNameFormatString)
            .WithAddMethodNameFormatString(AddMethodNameFormatString)
            .WithEnableBuilderInheritance(EnableBuilderInhericance)
            //.WithIsAbstract(IsAbstract)
            //.WithBaseClass(BaseClass?.ToBuilder())
            //.WithInheritanceComparisonDelegate(InheritanceComparisonDelegate)
            .Build();

    private TypeBase CreateImmutableClass(TypeBase typeBase, string entitiesNamespace)
    {
        var builder = new ClassBuilder();
        _ = _entityPipeline
            .Process(builder, new EntityContext(typeBase, CreateEntityPipelineSettings(entitiesNamespace, overrideAddNullChecks: GetOverrideAddNullChecks(), entityNameFormatString: "{Class.NameNoInterfacePrefix}{EntityNameSuffix}"), CultureInfo.InvariantCulture))
            .GetValueOrThrow();

        return builder.Build();
    }

    private TypeBase CreateBuilderClass(TypeBase typeBase, string buildersNamespace, string entitiesNamespace)
    {
        var builder = new ClassBuilder();
        _ = _builderPipeline
            .Process(builder, new BuilderContext(typeBase, CreateBuilderPipelineSettings(buildersNamespace, entitiesNamespace), CultureInfo.InvariantCulture))
            .GetValueOrThrow();

        return builder.Build();
    }

    private TypeBase CreateBuilderExtensionsClass(TypeBase typeBase, string buildersNamespace, string entitiesNamespace)
    {
        var builder = new ClassBuilder();
        _ = _builderInterfacePipeline
            .Process(builder, new BuilderInterfaceContext(typeBase, CreateBuilderInterfacePipelineSettings(buildersNamespace, entitiesNamespace), CultureInfo.InvariantCulture))
            .GetValueOrThrow();

        return builder.Build();
    }

    private TypeBase CreateNonGenericBuilderClass(TypeBase typeBase, string buildersNamespace, string entitiesNamespace)
    {
        var builder = new ClassBuilder();
        _ = _builderPipeline
            .Process(builder, new BuilderContext(typeBase, CreateBuilderPipelineSettings(buildersNamespace, entitiesNamespace).ToBuilder().WithIsForAbstractBuilder().Build(), CultureInfo.InvariantCulture))
            .GetValueOrThrow();

        return builder.Build();
    }

    private bool? GetOverrideAddNullChecks()
    {
        if (AddNullChecks || ValidateArgumentsInConstructor == ArgumentValidationType.None)
        {
            return true;
        }

        return null;
    }

    private TypeBase CreateInterface(
        TypeBase typeBase,
        string interfacesNamespace,
        string newCollectionTypeName,
        bool addSetters,
        string nameFormatString = "{Class.Name}")
    {
        var builder = new InterfaceBuilder();
        _ = _interfacePipeline
            .Process(builder, new InterfaceContext(typeBase, CreateInterfacePipelineSettings(interfacesNamespace, newCollectionTypeName, InheritanceComparisonDelegate, addSetters, nameFormatString), CultureInfo.InvariantCulture))
            .GetValueOrThrow();

        return builder.Build();
    }

    private TypeBase CreateImmutableOverrideClass(TypeBase typeBase, string entitiesNamespace)
    {
        var builder = new ClassBuilder();
        _ = _overrideEntityPipeline
            .Process(builder, new OverrideEntityContext(typeBase, CreateOverrideEntityPipelineSettings(entitiesNamespace, overrideAddNullChecks: true), CultureInfo.InvariantCulture))
            .GetValueOrThrow();

        return builder.Build();
    }
}
