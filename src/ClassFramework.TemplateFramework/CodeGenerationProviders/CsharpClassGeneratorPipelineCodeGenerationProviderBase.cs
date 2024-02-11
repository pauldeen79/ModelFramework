namespace ClassFramework.TemplateFramework.CodeGenerationProviders;

public abstract class CsharpClassGeneratorPipelineCodeGenerationProviderBase : CsharpClassGeneratorCodeGenerationProviderBase
{
    protected CsharpClassGeneratorPipelineCodeGenerationProviderBase(
        ICsharpExpressionCreator csharpExpressionCreator,
        IPipeline<IConcreteTypeBuilder, BuilderContext> builderPipeline,
        IPipeline<IConcreteTypeBuilder, EntityContext> entityPipeline,
        IPipeline<IConcreteTypeBuilder, OverrideEntityContext> overrideEntityPipeline,
        IPipeline<TypeBaseBuilder, ReflectionContext> reflectionPipeline,
        IPipeline<InterfaceBuilder, InterfaceContext> interfacePipeline) : base(csharpExpressionCreator)
    {
        Guard.IsNotNull(builderPipeline);
        Guard.IsNotNull(entityPipeline);
        Guard.IsNotNull(overrideEntityPipeline);
        Guard.IsNotNull(reflectionPipeline);
        Guard.IsNotNull(interfacePipeline);

        _builderPipeline = builderPipeline;
        _entityPipeline = entityPipeline;
        _overrideEntityPipeline = overrideEntityPipeline;
        _reflectionPipeline = reflectionPipeline;
        _interfacePipeline = interfacePipeline;
    }

    private readonly IPipeline<IConcreteTypeBuilder, BuilderContext> _builderPipeline;
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

        return models.Select(x => CreateInterface(x, interfacesNamespace, RecordCollectionType.WithoutGenerics())).ToArray();
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
            .Select(x => CreateInterface(x, interfacesNamespace, RecordConcreteCollectionType.WithoutGenerics(), "I{Class.Name}"))
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
        var entitySettings = new Pipelines.Entity.PipelineSettings(
            generationSettings: new Pipelines.Entity.PipelineGenerationSettings(
                addSetters: AddSetters,
                addBackingFields: AddBackingFields,
                setterVisibility: SetterVisibility,
                createAsObservable: CreateAsObservable,
                createRecord: CreateRecord,
                allowGenerationWithoutProperties: AllowGenerationWithoutProperties),
            copySettings: new Pipelines.Shared.PipelineBuilderCopySettings(
                copyAttributes: CopyAttributes,
                copyInterfaces: CopyInterfaces),
            nameSettings: new Pipelines.Entity.PipelineNameSettings(
                entityNameFormatString: "{Class.NameNoInterfacePrefix}",
                entityNamespaceFormatString: @namespace),
            inheritanceSettings: new Pipelines.Entity.PipelineInheritanceSettings(
                enableInheritance: true,
                isAbstract: true,
                baseClass: null,
                (parentNameContainer, typeBase)
                    => parentNameContainer is not null
                    && typeBase is not null
                    && (string.IsNullOrEmpty(parentNameContainer.ParentTypeFullName)
                        || parentNameContainer.ParentTypeFullName.GetClassName().In(typeBase.Name, $"I{typeBase.Name}")
                        || Array.Exists(GetModelAbstractBaseTyped(), x => x == parentNameContainer.ParentTypeFullName.GetClassName())
                        || (parentNameContainer.ParentTypeFullName.StartsWith($"{RootNamespace}.Abstractions.") && typeBase.Namespace == RootNamespace)
                    )),
            typeSettings: new Pipelines.Entity.PipelineTypeSettings(
                newCollectionTypeName: RecordCollectionType.WithoutGenerics(),
                enableNullableReferenceTypes: true,
                typenameMappings: CreateTypenameMappings(),
                namespaceMappings: CreateNamespaceMappings()),
            constructorSettings: new Pipelines.Entity.PipelineConstructorSettings(ValidateArgumentsInConstructor),
            nullCheckSettings: new Pipelines.Shared.PipelineBuilderNullCheckSettings(AddNullChecks, UseExceptionThrowIfNull)
            );
        _ = _entityPipeline.Process(builder, new EntityContext(typeBase, entitySettings, CultureInfo.InvariantCulture)).GetValueOrThrow();
        return builder.BuildTyped();
    }

    protected ArgumentValidationType CombineValidateArguments(ArgumentValidationType validateArgumentsInConstructor, bool secondCondition)
        => secondCondition
            ? validateArgumentsInConstructor
            : ArgumentValidationType.None;

    private Pipelines.Reflection.PipelineSettings CreateReflectionPipelineSettings()
        => new(
            generationSettings: new Pipelines.Reflection.PipelineGenerationSettings(
                allowGenerationWithoutProperties: AllowGenerationWithoutProperties),
            copySettings: new Pipelines.Shared.PipelineBuilderCopySettings(
                copyAttributes: CopyAttributes,
                copyInterfaces: CopyInterfaces),
            typeSettings: new Pipelines.Reflection.PipelineTypeSettings(
                namespaceMappings: CreateNamespaceMappings(),
                typenameMappings: CreateTypenameMappings())
            );

    private Pipelines.Entity.PipelineSettings CreateEntityPipelineSettings(
        string entitiesNamespace,
        ArgumentValidationType? forceValidateArgumentsInConstructor = null,
        bool? overrideAddNullChecks = null,
        string entityNameFormatString = "{Class.NameNoInterfacePrefix}")
        => new(
            generationSettings: new Pipelines.Entity.PipelineGenerationSettings(
                addSetters: AddSetters,
                addBackingFields: AddBackingFields,
                setterVisibility: SetterVisibility,
                createAsObservable: CreateAsObservable,
                createRecord: CreateRecord,
                allowGenerationWithoutProperties: AllowGenerationWithoutProperties),
            copySettings: new Pipelines.Shared.PipelineBuilderCopySettings(
                copyAttributes: CopyAttributes,
                copyInterfaces: CopyInterfaces),
            nameSettings: new Pipelines.Entity.PipelineNameSettings(
                entityNameFormatString: entityNameFormatString,
                entityNamespaceFormatString: entitiesNamespace,
                toBuilderFormatString: ToBuilderFormatString,
                toTypedBuilderFormatString: ToTypedBuilderFormatString),
            inheritanceSettings: new Pipelines.Entity.PipelineInheritanceSettings(
                EnableEntityInheritance,
                IsAbstract,
                BaseClass,
                InheritanceComparisonDelegate),
            typeSettings: new Pipelines.Entity.PipelineTypeSettings(
                newCollectionTypeName: RecordCollectionType.WithoutGenerics(),
                enableNullableReferenceTypes: true,
                typenameMappings: CreateTypenameMappings(),
                namespaceMappings: CreateNamespaceMappings()),
            constructorSettings: new Pipelines.Entity.PipelineConstructorSettings(
                validateArguments: forceValidateArgumentsInConstructor ?? CombineValidateArguments(ValidateArgumentsInConstructor, !(EnableEntityInheritance && BaseClass is null)),
                originalValidateArguments: ValidateArgumentsInConstructor,
                collectionTypeName: RecordConcreteCollectionType.WithoutGenerics(),
                addFullConstructor: AddFullConstructor,
                addPublicParameterlessConstructor: AddPublicParameterlessConstructor),
            nullCheckSettings: new Pipelines.Shared.PipelineBuilderNullCheckSettings(
                addNullChecks: forceValidateArgumentsInConstructor != ArgumentValidationType.Shared && (overrideAddNullChecks ?? false),
                useExceptionThrowIfNull: UseExceptionThrowIfNull)
            );

    private Pipelines.OverrideEntity.PipelineSettings CreateOverrideEntityPipelineSettings(
        string entitiesNamespace,
        ArgumentValidationType? forceValidateArgumentsInConstructor = null,
        bool? overrideAddNullChecks = null,
        string entityNameFormatString = "{Class.NameNoInterfacePrefix}")
        => new(
        generationSettings: new Pipelines.OverrideEntity.PipelineGenerationSettings(
            createRecord: CreateRecord,
            allowGenerationWithoutProperties: AllowGenerationWithoutProperties),
        nameSettings: new Pipelines.OverrideEntity.PipelineNameSettings(
            entityNameFormatString: entityNameFormatString,
            entityNamespaceFormatString: entitiesNamespace),
        inheritanceSettings: new Pipelines.OverrideEntity.PipelineInheritanceSettings(
            EnableEntityInheritance,
            IsAbstract,
            BaseClass,
            InheritanceComparisonDelegate),
        typeSettings: new Pipelines.OverrideEntity.PipelineTypeSettings(
            newCollectionTypeName: RecordCollectionType.WithoutGenerics(),
            enableNullableReferenceTypes: true,
            typenameMappings: CreateTypenameMappings(),
            namespaceMappings: CreateNamespaceMappings()),
        nullCheckSettings: new Pipelines.Shared.PipelineBuilderNullCheckSettings(
            addNullChecks: forceValidateArgumentsInConstructor != ArgumentValidationType.Shared && (overrideAddNullChecks ?? false),
            useExceptionThrowIfNull: UseExceptionThrowIfNull)
        );

    private Pipelines.Interface.PipelineSettings CreateInterfacePipelineSettings(string interfacesNamespace, string newCollectionTypeName, string nameFormatString = "{Class.Name}")
        => new(
            nameSettings: new Pipelines.Interface.PipelineNameSettings(
                namespaceFormatString: interfacesNamespace,
                nameFormatString: nameFormatString),
            inheritanceSettings: new Pipelines.Interface.PipelineInheritanceSettings(
                EnableEntityInheritance,
                IsAbstract,
                BaseClass,
                InheritanceComparisonDelegate),
            typeSettings: new Pipelines.Interface.PipelineTypeSettings(
                newCollectionTypeName: newCollectionTypeName,
                typenameMappings: CreateTypenameMappings(),
                namespaceMappings: CreateNamespaceMappings()),
            copySettings: new Pipelines.Shared.PipelineBuilderCopySettings(
                copyAttributes: CopyAttributes,
                copyInterfaces: CopyInterfaces),
            generationSettings: new Pipelines.Interface.PipelineGenerationSettings(
                addSetters: AddSetters,
                allowGenerationWithoutProperties: AllowGenerationWithoutProperties)
        );

    private IEnumerable<NamespaceMapping>? CreateNamespaceMappings()
    {
        // From models to domain entities
        yield return new NamespaceMapping($"{CodeGenerationRootNamespace}.Models", CoreNamespace, Enumerable.Empty<Metadata>());
        yield return new NamespaceMapping($"{CodeGenerationRootNamespace}.Models.Domains", $"{CoreNamespace}.Domains", Enumerable.Empty<Metadata>());
        yield return new NamespaceMapping($"{CodeGenerationRootNamespace}.Models.Abstractions", $"{CoreNamespace}.Abstractions", Enumerable.Empty<Metadata>());

        // From domain entities to builders
        yield return new NamespaceMapping(CoreNamespace, CoreNamespace, Enumerable.Empty<Metadata>());
        yield return new NamespaceMapping($"{CoreNamespace}.Abstractions", $"{CoreNamespace}.Abstractions", new[]
        {
            new Metadata($"{CoreNamespace}.Builders.Abstractions", Pipelines.MetadataNames.CustomBuilderInterfaceNamespace),
            new Metadata("{TypeName.ClassName}Builder", Pipelines.MetadataNames.CustomBuilderInterfaceName)
        });

        foreach (var entityClassName in GetPureAbstractModels().Select(x => x.GetEntityClassName().ReplaceSuffix("Base", string.Empty, StringComparison.Ordinal)))
        {
            yield return new NamespaceMapping($"{CodeGenerationRootNamespace}.Models.{entityClassName}s", $"{CoreNamespace}.{entityClassName}s", Enumerable.Empty<Metadata>());
            yield return new NamespaceMapping($"{CoreNamespace}.{entityClassName}s", $"{CoreNamespace}.{entityClassName}s", Enumerable.Empty<Metadata>());
        }
    }

    private TypenameMapping[] CreateTypenameMappings()
        => GetType().Assembly.GetTypes()
            .Where(x => x.IsInterface
                && x.Namespace?.StartsWith($"{CodeGenerationRootNamespace}.Models", StringComparison.Ordinal) == true
                && x.Namespace != $"{CodeGenerationRootNamespace}.Models.Abstractions"
                && x.Namespace != $"{CodeGenerationRootNamespace}.Models.Domains"
                && x.FullName is not null)
            .SelectMany(x =>
                new[]
                {
                    new TypenameMapping(x.FullName!, $"{CoreNamespace}.{ReplaceStart(x.Namespace ?? string.Empty, $"{CodeGenerationRootNamespace}.Models", true)}{x.GetEntityClassName()}", Enumerable.Empty<Metadata>()),
                    new TypenameMapping($"{CoreNamespace}.{ReplaceStart(x.Namespace ?? string.Empty, $"{CodeGenerationRootNamespace}.Models", true)}{x.GetEntityClassName()}", $"{CoreNamespace}.{ReplaceStart(x.Namespace ?? string.Empty, $"{CodeGenerationRootNamespace}.Models", true)}{x.GetEntityClassName()}", new[]
                    {
                        new Metadata($"{CoreNamespace}.Builders{ReplaceStart(x.Namespace ?? string.Empty, $"{CodeGenerationRootNamespace}.Models", false)}", Pipelines.MetadataNames.CustomBuilderNamespace),
                        new Metadata("{TypeName.ClassName}Builder", Pipelines.MetadataNames.CustomBuilderName),
                        new Metadata(x.Namespace != $"{CodeGenerationRootNamespace}.Models" && x.Namespace != $"{CodeGenerationRootNamespace}.Models.Abstractions"
                            ? $"new {CoreNamespace}.Builders{ReplaceStart(x.Namespace ?? string.Empty, $"{CodeGenerationRootNamespace}.Models", false)}.{x.GetEntityClassName()}Builder([Name])"
                            : "[Name][NullableSuffix].ToBuilder()", Pipelines.MetadataNames.CustomBuilderSourceExpression),
                            //: $"builderFactory.Create<{CoreNamespace}.Builders.{ReplaceStart(x.Namespace ?? string.Empty, $"{CodeGenerationRootNamespace}.Models", true)}{x.GetEntityClassName()}Builder>([Name])", Pipelines.MetadataNames.CustomBuilderSourceExpression),
                        new Metadata(x.Namespace != $"{CodeGenerationRootNamespace}.Models" && x.Namespace != $"{CodeGenerationRootNamespace}.Models.Abstractions"
                            ? "[Name].BuildTyped()"
                            : "[Name].Build()", Pipelines.MetadataNames.CustomBuilderMethodParameterExpression),
                        //new Metadata(new ParameterBuilder().WithName("builderFactory").WithTypeName($"{CoreNamespace}.Builders.Abstractions.IBuilderFactory").Build(), Pipelines.MetadataNames.CustomBuilderCopyConstructorParameter),
                    })
                })
            .Concat(new[]
            {
                new TypenameMapping(typeof(bool).FullName!, typeof(bool).FullName!, new[] { new Metadata(true, Pipelines.MetadataNames.CustomBuilderWithDefaultPropertyValue) }),
                new TypenameMapping(typeof(List<>).WithoutGenerics(), typeof(List<>).WithoutGenerics(), new[] { new Metadata("[Expression].ToList()", Pipelines.MetadataNames.CustomCollectionInitialization) }),
                new TypenameMapping(typeof(Collection<>).WithoutGenerics(), typeof(Collection<>).WithoutGenerics(), new[] { new Metadata("new [Type]<[Generics]>([Expression].ToList())", Pipelines.MetadataNames.CustomCollectionInitialization) }),
                new TypenameMapping(typeof(ObservableCollection<>).WithoutGenerics(), typeof(ObservableCollection<>).WithoutGenerics(), new[] { new Metadata("new [Type]<[Generics]>([Expression])", Pipelines.MetadataNames.CustomCollectionInitialization) }),
                new TypenameMapping(typeof(IReadOnlyCollection<>).WithoutGenerics(), typeof(IReadOnlyCollection<>).WithoutGenerics(), new[] { new Metadata("[Expression].ToList().AsReadOnly()", Pipelines.MetadataNames.CustomCollectionInitialization) }),
                new TypenameMapping(typeof(IList<>).WithoutGenerics(), typeof(IList<>).WithoutGenerics(), new[] { new Metadata("[Expression].ToList()", Pipelines.MetadataNames.CustomCollectionInitialization) }),
                new TypenameMapping(typeof(ICollection<>).WithoutGenerics(), typeof(ICollection<>).WithoutGenerics(), new[] { new Metadata("[Expression].ToList()", Pipelines.MetadataNames.CustomCollectionInitialization) }),
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

    private Pipelines.Builder.PipelineSettings CreateBuilderPipelineSettings(string buildersNamespace, string entitiesNamespace)
        => new(
            entitySettings: CreateEntityPipelineSettings(entitiesNamespace, forceValidateArgumentsInConstructor: ArgumentValidationType.None, overrideAddNullChecks: GetOverrideAddNullChecks()),
            typeSettings: new Pipelines.Builder.PipelineTypeSettings(
                newCollectionTypeName: BuilderCollectionType.WithoutGenerics(),
                enableNullableReferenceTypes: true,
                namespaceMappings: CreateNamespaceMappings(),
                typenameMappings: CreateTypenameMappings()),
            nameSettings: new Pipelines.Builder.PipelineNameSettings(
                builderNamespaceFormatString: buildersNamespace,
                setMethodNameFormatString: SetMethodNameFormatString,
                addMethodNameFormatString: AddMethodNameFormatString),
            inheritanceSettings: new Pipelines.Builder.PipelineInheritanceSettings(EnableBuilderInhericance, IsAbstract, BaseClass, BaseClassBuilderNamespace, InheritanceComparisonDelegate),
            constructorSettings: new Pipelines.Builder.PipelineConstructorSettings(addCopyConstructor: AddCopyConstructor, setDefaultValues: SetDefaultValues)
        );

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

    private TypeBase CreateNonGenericBuilderClass(TypeBase typeBase, string buildersNamespace, string entitiesNamespace)
    {
        var builder = new ClassBuilder();
        _ = _builderPipeline
            .Process(builder, new BuilderContext(typeBase, CreateBuilderPipelineSettings(buildersNamespace, entitiesNamespace).ForAbstractBuilder(), CultureInfo.InvariantCulture))
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

    private TypeBase CreateInterface(TypeBase typeBase, string interfacesNamespace, string newCollectionTypeName, string nameFormatString = "{Class.Name}")
    {
        var builder = new InterfaceBuilder();
        _ = _interfacePipeline
            .Process(builder, new InterfaceContext(typeBase, CreateInterfacePipelineSettings(interfacesNamespace, newCollectionTypeName, nameFormatString), CultureInfo.InvariantCulture))
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
