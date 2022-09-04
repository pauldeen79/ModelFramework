namespace ModelFramework.CodeGeneration.CodeGenerationProviders;

public abstract class CSharpClassBase : ClassBase
{
    protected virtual IEnumerable<ClassMethodBuilder> CreateExtraOverloads(IClass c)
        => Enumerable.Empty<ClassMethodBuilder>();
    protected virtual string NewCollectionTypeName => typeof(List<>).WithoutGenerics();
    protected virtual string SetMethodNameFormatString => "With{0}";
    protected virtual string AddMethodNameFormatString => "Add{0}";
    protected virtual bool AddNullChecks => false;
    protected virtual bool AddCopyConstructor => true;
    protected virtual bool UseLazyInitialization => true;
    protected virtual bool UseTargetTypeNewExpressions => true;
    protected virtual bool ValidateArgumentsInConstructor => true;
    protected virtual bool InheritFromInterfaces => true;
    protected virtual bool AddPrivateSetters => false;
    protected virtual bool CopyPropertyCode => true;
    protected virtual bool EnableEntityInheritance => false;
    protected virtual bool EnableBuilderInhericance => false;
    protected virtual bool RemoveDuplicateWithMethods => true;
    protected virtual bool AllowGenerationWithoutProperties => true;
    protected virtual IClass? BaseClass => null;
    protected virtual string BaseClassBuilderNameSpace => string.Empty;
    protected virtual bool IsMemberValid(IParentTypeContainer parent, ITypeBase typeBase) => true;

    protected abstract Type RecordCollectionType { get; }
    protected virtual string FormatInstanceTypeName(ITypeBase instance, bool forCreate) => string.Empty;
    protected virtual void FixImmutableBuilderProperties<TBuilder, TEntity>(TypeBaseBuilder<TBuilder, TEntity> typeBaseBuilder)
        where TEntity : ITypeBase
        where TBuilder : TypeBaseBuilder<TBuilder, TEntity>
    { }
    protected virtual void FixImmutableClassProperties<TBuilder, TEntity>(TypeBaseBuilder<TBuilder, TEntity> typeBaseBuilder)
        where TEntity : ITypeBase
        where TBuilder : TypeBaseBuilder<TBuilder, TEntity>
    { }

    protected ITypeBase[] GetImmutableBuilderClasses(Type[] types,
                                                     string entitiesNamespace,
                                                     string buildersNamespace,
                                                     params string[] interfacesToAdd)
        => GetImmutableBuilderClasses(types.Select(x => x.ToTypeBase()).ToArray(),
                                      entitiesNamespace,
                                      buildersNamespace,
                                      interfacesToAdd);

    protected ITypeBase[] GetImmutableBuilderClasses(ITypeBase[] models,
                                                     string entitiesNamespace,
                                                     string buildersNamespace,
                                                     params string[] interfacesToAdd)
        => models.Select
        (
            x => CreateBuilder(CreateImmutableEntity(entitiesNamespace, x), buildersNamespace)
                .With(x => x.AddInterfaces(interfacesToAdd.Select(y => string.Format(y, x.Name))))
                .Build()
        ).ToArray();

    protected ITypeBase[] GetImmutableNonGenericBuilderClasses(Type[] types,
                                                               string entitiesNamespace,
                                                               string buildersNamespace,
                                                               params string[] interfacesToAdd)
        => GetImmutableNonGenericBuilderClasses(types.Select(x => x.ToTypeBase()).ToArray(),
                                                entitiesNamespace,
                                                buildersNamespace,
                                                interfacesToAdd);

    protected ITypeBase[] GetImmutableNonGenericBuilderClasses(ITypeBase[] models,
                                                               string entitiesNamespace,
                                                               string buildersNamespace,
                                                               params string[] interfacesToAdd)
        => models.Select
        (
            x => CreateNonGenericBuilder(CreateImmutableEntity(entitiesNamespace, x), buildersNamespace)
                .With(x => x.AddInterfaces(interfacesToAdd.Select(y => string.Format(y, x.Name))))
                .Build()
        ).ToArray();

    protected IClass[] GetImmutableBuilderExtensionClasses(Type[] types,
                                                           string entitiesNamespace,
                                                           string buildersNamespace,
                                                           string builderInterfacesNamespace)
        => GetImmutableBuilderExtensionClasses(types.Select(x => x.ToClass(new ClassSettings())).ToArray(),
                                                                           entitiesNamespace,
                                                                           buildersNamespace,
                                                                           builderInterfacesNamespace);

    protected IClass[] GetImmutableBuilderExtensionClasses(ITypeBase[] models,
                                                           string entitiesNamespace,
                                                           string buildersNamespace,
                                                           string builderInterfacesNamespace)
        => models.Select
        (
            x => CreateBuilderExtensions(CreateImmutableEntity(entitiesNamespace, x), buildersNamespace)
                .With
                (
                    x => x.Methods.ForEach(y => y.AddGenericTypeArguments("T")
                                                 .AddGenericTypeArgumentConstraints($"where T : {builderInterfacesNamespace}.I{y.TypeName}"))
                )
                .With
                (
                    x => x.Methods.ForEach(y => y.WithTypeName("T")
                                                 .With(z => z.Parameters.First().WithTypeName(z.TypeName)))
                )
                .BuildTyped()
        )
        .ToArray();

    protected ITypeBase[] GetImmutableClasses(Type[] types, string entitiesNamespace)
        => GetImmutableClasses(types.Select(x => x.IsInterface
            ? x.ToInterfaceBuilder().With(x => FixImmutableClassProperties(x)).Build()
            : x.ToClassBuilder(new ClassSettings()).With(x => FixImmutableClassProperties(x)).Build()).ToArray(), entitiesNamespace);

    protected ITypeBase[] GetImmutableClasses(ITypeBase[] models, string entitiesNamespace)
        => models.Select
        (
            x => x switch
            {
                IClass cls => CreateImmutableClassFromClass(cls, entitiesNamespace),
                IInterface iinterface => CreateImmutableClassFromInterface(iinterface, entitiesNamespace),
                _ => throw new NotSupportedException("Type of class should be IClass or IInterface")
            }
        ).ToArray();

    protected IClass[] GetClassesFromSameNamespace(Type type)
    {
        if (type.FullName == null)
        {
            throw new ArgumentException("Can't get classes from same namespace when the FullName of this type is null. Could not determine namespace.");
        }

        return type.Assembly.GetExportedTypes()
            .Where
            (
                t => t.FullName != null
                    && t.FullName.GetNamespaceWithDefault() == type.FullName.GetNamespaceWithDefault()
                    && t.GetProperties().Any()
            )
            .Select
            (
                t => t.ToClassBuilder(new ClassSettings(createConstructors: true))
                    .WithName(t.Name)
                    .WithNamespace(t.FullName.GetNamespaceWithDefault())
                .With(x => FixImmutableBuilderProperties(x))
                .BuildTyped()
            )
            .ToArray();
    }

    protected IClass CreateBaseclass(Type type, string @namespace)
        => type.ToClass(new ClassSettings()).ToImmutableClassBuilder(new ImmutableClassSettings
            (
                newCollectionTypeName: RecordCollectionType.WithoutGenerics(),
                allowGenerationWithoutProperties: AllowGenerationWithoutProperties,
                constructorSettings: new ImmutableClassConstructorSettings(
                    validateArguments: ValidateArgumentsInConstructor,
                    addNullChecks: AddNullChecks),
                addPrivateSetters: AddPrivateSetters)
            )
            .WithNamespace(@namespace)
            .WithName(type.GetEntityClassName())
            .With(x => FixImmutableClassProperties(x))
            .BuildTyped();

    protected ClassBuilder CreateBuilder(IClass cls, string @namespace)
        => cls.ToImmutableBuilderClassBuilder(CreateImmutableBuilderClassSettings())
            .WithNamespace(@namespace)
            .WithPartial();

    protected ClassBuilder CreateNonGenericBuilder(IClass cls, string @namespace)
        => cls.ToNonGenericImmutableBuilderClassBuilder(CreateImmutableBuilderClassSettings())
            .WithNamespace(@namespace)
            .WithPartial();

    protected ClassBuilder CreateBuilderExtensions(IClass cls, string @namespace)
        => cls.ToBuilderExtensionsClassBuilder(CreateImmutableBuilderClassSettings())
            .WithNamespace(@namespace)
            .WithPartial();

    protected ImmutableBuilderClassSettings CreateImmutableBuilderClassSettings()
        => new ImmutableBuilderClassSettings
        (
            typeSettings: new(
                newCollectionTypeName: NewCollectionTypeName,
                formatInstanceTypeNameDelegate: FormatInstanceTypeName,
                useTargetTypeNewExpressions: UseTargetTypeNewExpressions,
                enableNullableReferenceTypes: EnableNullableContext),
            constructorSettings: new(
                addCopyConstructor: AddCopyConstructor,
                addNullChecks: AddNullChecks),
            nameSettings: new(
                setMethodNameFormatString: SetMethodNameFormatString,
                addMethodNameFormatString: AddMethodNameFormatString),
            generationSettings: new(
                useLazyInitialization: UseLazyInitialization,
                copyPropertyCode: CopyPropertyCode,
                allowGenerationWithoutProperties: AllowGenerationWithoutProperties),
            inheritanceSettings: new(
                enableEntityInheritance: EnableEntityInheritance,
                enableBuilderInheritance: EnableBuilderInhericance,
                removeDuplicateWithMethods: RemoveDuplicateWithMethods,
                baseClass: BaseClass,
                baseClassBuilderNameSpace: BaseClassBuilderNameSpace,
                inheritanceComparisonFunction: IsMemberValid)
        );

    protected ImmutableClassSettings CreateImmutableClassSettings()
        => new ImmutableClassSettings
        (
            newCollectionTypeName: RecordCollectionType.WithoutGenerics(),
            allowGenerationWithoutProperties: AllowGenerationWithoutProperties,
            constructorSettings: new(
                validateArguments: ValidateArgumentsInConstructor && !(EnableEntityInheritance && BaseClass == null),
                addNullChecks: AddNullChecks),
            addPrivateSetters: AddPrivateSetters,
            inheritanceSettings: new(
                enableInheritance: EnableEntityInheritance,
                baseClass: BaseClass,
                inheritanceComparisonFunction: IsMemberValid)
        );

    private IClass CreateImmutableEntity(string entitiesNamespace, ITypeBase typeBase)
        => new ClassBuilder(typeBase.ToClass())
            .WithName(typeBase.GetEntityClassName())
            .WithNamespace(entitiesNamespace)
            .With(x => FixImmutableBuilderProperties(x))
            .Build()
            .ToImmutableClassBuilder(CreateImmutableClassSettings())
            .BuildTyped();

    private ITypeBase CreateImmutableClassFromInterface(IInterface iinterface, string entitiesNamespace)
        => new InterfaceBuilder(iinterface)
            .WithName(iinterface.GetEntityClassName())
            .WithNamespace(entitiesNamespace)
            .With(x => FixImmutableClassProperties(x))
            .Build()
            .ToImmutableClassBuilder(CreateImmutableClassSettings())
            .WithRecord()
            .WithPartial()
            .AddInterfaces((new[] { $"{iinterface.Namespace}.{iinterface.Name}" }).Where(_ => InheritFromInterfaces))
            .Build();

    private ITypeBase CreateImmutableClassFromClass(IClass cls, string entitiesNamespace)
        => new ClassBuilder(cls)
            .WithNamespace(entitiesNamespace)
            .With(x => FixImmutableClassProperties(x))
            .Build()
            .ToImmutableClassBuilder(CreateImmutableClassSettings())
            .WithRecord()
            .WithPartial()
            .Build();
}
