namespace ClassFramework.TemplateFramework;

public abstract class CsharpClassGeneratorPipelineCodeGenerationProviderBase : CsharpClassGeneratorCodeGenerationProviderBase
{
    protected CsharpClassGeneratorPipelineCodeGenerationProviderBase(
        ICsharpExpressionCreator csharpExpressionCreator,
        IPipeline<IConcreteTypeBuilder, BuilderContext> builderPipeline,
        IPipeline<IConcreteTypeBuilder, EntityContext> entityPipeline,
        IPipeline<TypeBaseBuilder, ReflectionContext> reflectionPipeline) : base(csharpExpressionCreator)
    {
        Guard.IsNotNull(builderPipeline);
        Guard.IsNotNull(entityPipeline);
        Guard.IsNotNull(reflectionPipeline);

        _builderPipeline = builderPipeline;
        _entityPipeline = entityPipeline;
        _reflectionPipeline = reflectionPipeline;
    }

    private readonly IPipeline<IConcreteTypeBuilder, BuilderContext> _builderPipeline;
    private readonly IPipeline<IConcreteTypeBuilder, EntityContext> _entityPipeline;
    private readonly IPipeline<TypeBaseBuilder, ReflectionContext> _reflectionPipeline;

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
            .WithFilenameSuffix(".template.generated")
            .Build();

    protected abstract string ProjectName { get; }
    protected abstract Type RecordCollectionType { get; }
    protected abstract Type RecordConcreteCollectionType { get; }

    protected virtual string RootNamespace => InheritFromInterfaces
        ? $"{ProjectName}.Abstractions"
        : $"{ProjectName}.Domain";
    protected virtual string CodeGenerationRootNamespace => $"{ProjectName}.CodeGeneration";
    protected virtual string CoreNamespace => $"{ProjectName}.Core";
    protected virtual bool InheritFromInterfaces => false;
    protected virtual bool CopyAttributes => false;
    protected virtual bool CopyInterfaces => false;
    protected virtual string? CollectionPropertyGetStatement => null;
    protected virtual ArgumentValidationType ValidateArgumentsInConstructor => ArgumentValidationType.DomainOnly;

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

    protected virtual void FixImmutableClassProperties<TBuilder, TEntity>(TypeBaseBuilder<TBuilder, TEntity> typeBaseBuilder)
        where TEntity : TypeBase
        where TBuilder : TypeBaseBuilder<TBuilder, TEntity>
    {
        Guard.IsNotNull(typeBaseBuilder);

        foreach (var property in typeBaseBuilder.Properties)
        {
            var typeName = property.TypeName.FixTypeName();
            FixImmutableBuilderProperty(property, typeName);

            //TODO: Move to Entity pipeline
            //if (typeName.StartsWith($"{RecordCollectionType.WithoutGenerics()}<", StringComparison.InvariantCulture)
            //    && AddBackingFieldsForCollectionProperties)
            //{
            //    property.AddCollectionBackingFieldOnImmutableClass(RecordConcreteCollectionType, CollectionPropertyGetStatement, forceNullCheck: ValidateArgumentsInConstructor != ArgumentValidationType.None);
            //}
        }
    }

    protected virtual void FixImmutableBuilderProperty(PropertyBuilder property, string typeName)
    {
        Guard.IsNotNull(property);
        Guard.IsNotNull(typeName);

        //TODO: Move to Builder pipeline
        if (typeName.IsBooleanTypeName() || typeName.IsNullableBooleanTypeName())
        {
            property.AddMetadata(new MetadataBuilder().WithName(Pipelines.MetadataNames.CustomBuilderWithDefaultPropertyValue).WithValue(true));
        }
    }

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

        if (ValidateArgumentsInConstructor == ArgumentValidationType.Shared)
        {
            return models.SelectMany
            (
                x => x switch
                {
                    Class cls => new[] { CreateImmutableClassFromClass(cls, entitiesNamespace), CreateImmutableOverrideClassFromClass(cls, entitiesNamespace) },
                    Interface iinterface => new[] { CreateImmutableClassFromInterface(iinterface, entitiesNamespace), CreateImmutableOverrideClassFromInterface(iinterface, entitiesNamespace) },
                    _ => throw new NotSupportedException("Type should be Class or Interface")
                }
            ).ToArray();
        }

        return models.Select
        (
            x => x switch
            {
                Class cls => CreateImmutableClassFromClass(cls, entitiesNamespace),
                Interface iinterface => CreateImmutableClassFromInterface(iinterface, entitiesNamespace),
                _ => throw new NotSupportedException("Type should be Class or Interface")
            }
        ).ToArray();
    }

    protected TypeBase[] GetCoreModels()
        => GetType().Assembly.GetTypes()
            .Where(x => x.IsInterface && x.Namespace == $"{CodeGenerationRootNamespace}.Models" && !GetCustomBuilderTypes().Contains(x.GetEntityClassName()))
            .Select(x => _reflectionPipeline.Process(new InterfaceBuilder(), new ReflectionContext(x, CreateReflectionPipelineSettings(), CultureInfo.InvariantCulture)).GetValueOrThrow().Build())
            .ToArray();

    private Pipelines.Reflection.PipelineSettings CreateReflectionPipelineSettings()
        => new(
            copySettings: new Pipelines.Shared.PipelineBuilderCopySettings(copyAttributes: CopyAttributes, copyInterfaces: CopyInterfaces)
            ); //TODO: Add properties

    private Pipelines.Entity.PipelineSettings CreateEntityPipelineSettings(string entitiesNamespace, ArgumentValidationType? forceValidateArgumentsInConstructor = null, bool? overrideAddNullChecks = null)
        => new(
            copySettings: new Pipelines.Shared.PipelineBuilderCopySettings(copyAttributes: CopyAttributes, copyInterfaces: CopyInterfaces),
            nameSettings: new Pipelines.Entity.PipelineNameSettings(entityNameFormatString: "{Class.NameNoInterfacePrefix}{EntityNameSuffix}"),
            typeSettings: new Pipelines.Entity.PipelineTypeSettings(newCollectionTypeName: RecordConcreteCollectionType.FullName.FixTypeName(), enableNullableReferenceTypes: true, typenameMappings: CreateTypeNameMappings(entitiesNamespace)),
            constructorSettings: new Pipelines.Entity.PipelineConstructorSettings(validateArguments: forceValidateArgumentsInConstructor ?? ValidateArgumentsInConstructor, forceValidateArgumentsInConstructor, RecordConcreteCollectionType.FullName.FixTypeName())
            ); //TODO: Add properties

    private Pipelines.TypenameMapping[] CreateTypeNameMappings(string entitiesNamespace)
        => GetType().Assembly.GetTypes()
            .Where(x => x.IsInterface && x.Namespace == $"{CodeGenerationRootNamespace}.Models" && x.FullName is not null)
            .Select(x => new Pipelines.TypenameMapping(x.FullName!, $"{entitiesNamespace}.{x.GetEntityClassName()}", Enumerable.Empty<Metadata>()))
            .ToArray();

    private Pipelines.Builder.PipelineSettings CreateBuilderPipelineSettings()
        => new(); //TODO: Add properties

    private TypeBase CreateImmutableClassFromInterface(Interface iinterface, string entitiesNamespace)
    {
        var builder = new ClassBuilder();
        _ = _entityPipeline.Process(builder, new EntityContext(new InterfaceBuilder(iinterface)
                .WithName(iinterface.GetEntityClassName())
                .WithNamespace(entitiesNamespace)
                .With(x => FixImmutableClassProperties(x))
                .Build(), CreateEntityPipelineSettings(entitiesNamespace, overrideAddNullChecks: ValidateArgumentsInConstructor == ArgumentValidationType.None ? true : null), CultureInfo.InvariantCulture))
            .GetValueOrThrow();

        return builder.Build();
    }

    //TODO: Generate override entity here, instead of normal entity
    private TypeBase CreateImmutableOverrideClassFromInterface(Interface iinterface, string entitiesNamespace)
    {
        var builder = new ClassBuilder();
        _ = _entityPipeline.Process(builder, new EntityContext(new InterfaceBuilder(iinterface)
            .WithName(iinterface.GetEntityClassName())
            .WithNamespace(entitiesNamespace)
            .With(x => FixImmutableClassProperties(x))
            .Build(), CreateEntityPipelineSettings(entitiesNamespace, overrideAddNullChecks: ValidateArgumentsInConstructor == ArgumentValidationType.None ? true : null), CultureInfo.InvariantCulture))
            .GetValueOrThrow();

        return builder.Build();
    }

    private TypeBase CreateImmutableClassFromClass(Class cls, string entitiesNamespace)
    {
        var builder = new ClassBuilder();
        _ = _entityPipeline.Process(builder, new EntityContext(new ClassBuilder(cls)
            .WithNamespace(entitiesNamespace)
            .With(x => FixImmutableClassProperties(x))
            .Build(), CreateEntityPipelineSettings(entitiesNamespace, overrideAddNullChecks: ValidateArgumentsInConstructor == ArgumentValidationType.None ? true : null), CultureInfo.InvariantCulture))
            .GetValueOrThrow();

        return builder.Build();
    }

    //TODO: Generate override entity here, instead of normal entity
    private TypeBase CreateImmutableOverrideClassFromClass(Class cls, string entitiesNamespace)
    {
        var builder = new ClassBuilder();
        _ = _entityPipeline.Process(builder, new EntityContext(new ClassBuilder(cls)
            .WithNamespace(entitiesNamespace)
            .With(x => FixImmutableClassProperties(x))
            .Build(), CreateEntityPipelineSettings(entitiesNamespace, overrideAddNullChecks: ValidateArgumentsInConstructor == ArgumentValidationType.None ? true : null), CultureInfo.InvariantCulture))
            .GetValueOrThrow();

        return builder.Build();
    }
}
