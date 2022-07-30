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
    protected virtual IClass? BaseClass => null;
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
    protected virtual void PostProcessImmutableBuilderClass(ClassBuilder classBuilder) { }
    protected virtual void PostProcessImmutableEntityClass(ClassBuilder classBuilder) { }

    protected ITypeBase[] GetImmutableBuilderClasses(Type[] types,
                                                     string entitiesNamespace,
                                                     string buildersNamespace,
                                                     params string[] interfacesToAdd)
        => GetImmutableBuilderClasses(types.Select(x => x.IsInterface ? (ITypeBase)x.ToInterface() : x.ToClass(new ClassSettings())).ToArray(),
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
                .Build()
        )
        .ToArray();

    protected ITypeBase[] GetImmutableClasses(Type[] types, string entitiesNamespace)
        => GetImmutableClasses(types.Select(x => x.IsInterface
            ? (ITypeBase)x.ToInterfaceBuilder().With(x => FixImmutableClassProperties(x)).Build()
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
                .Build()
            )
            .ToArray();
    }

    protected ClassBuilder CreateBuilder(IClass cls, string @namespace)
        => cls.ToImmutableBuilderClassBuilder(CreateImmutableBuilderClassSettings())
            .WithNamespace(@namespace)
            .WithPartial()
            .AddMethods(CreateExtraOverloads(cls))
            .With(x => x.Methods.Sort(new Comparison<ClassMethodBuilder>((x, y) => CreateSortString(x).CompareTo(CreateSortString(y)))))
            .With(x => PostProcessImmutableBuilderClass(x));

    protected ClassBuilder CreateBuilderExtensions(IClass cls, string @namespace)
        => cls.ToBuilderExtensionsClassBuilder(CreateImmutableBuilderClassSettings())
            .WithNamespace(@namespace)
            .WithPartial()
            .AddMethods(CreateExtraOverloads(cls))
            .With(x => x.Methods.Sort(new Comparison<ClassMethodBuilder>((x, y) => CreateSortString(x).CompareTo(CreateSortString(y)))));

    protected ImmutableBuilderClassSettings CreateImmutableBuilderClassSettings()
        => new ImmutableBuilderClassSettings
        (
            typeSettings: new ImmutableBuilderClassTypeSettings(
                newCollectionTypeName: NewCollectionTypeName,
                formatInstanceTypeNameDelegate: FormatInstanceTypeName,
                useTargetTypeNewExpressions: UseTargetTypeNewExpressions,
                enableNullableReferenceTypes: EnableNullableContext),
            constructorSettings: new ImmutableBuilderClassConstructorSettings(
                addCopyConstructor: AddCopyConstructor,
                addNullChecks: AddNullChecks),
            nameSettings: new ImmutableBuilderClassNameSettings(
                setMethodNameFormatString: SetMethodNameFormatString,
                addMethodNameFormatString: AddMethodNameFormatString),
            useLazyInitialization: UseLazyInitialization,
            copyPropertyCode: CopyPropertyCode,
            inheritanceSettings: new ImmutableBuilderClassInheritanceSettings(
                enableEntityInheritance: EnableEntityInheritance,
                enableBuilderInheritance: EnableBuilderInhericance,
                baseClass: BaseClass,
                inheritanceComparisonFunction: IsMemberValid)
        );

    protected ImmutableClassSettings CreateImmutableClassSettings()
        => new ImmutableClassSettings
        (
            newCollectionTypeName: RecordCollectionType.WithoutGenerics(),
            constructorSettings: new ImmutableClassConstructorSettings(
                validateArguments: ValidateArgumentsInConstructor && !(EnableEntityInheritance && BaseClass == null),
                addNullChecks: AddNullChecks),
            addPrivateSetters: AddPrivateSetters,
            inheritanceSettings: new ImmutableClassInheritanceSettings(
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
            .With(x => PostProcessImmutableEntityClass(x))
            .Build();

    private ITypeBase CreateImmutableClassFromInterface(IInterface iinterface, string entitiesNamespace)
        => new InterfaceBuilder(iinterface)
            .WithName(iinterface.GetEntityClassName())
            .WithNamespace(entitiesNamespace)
            .With(x => FixImmutableClassProperties(x))
            .Build()
            .ToImmutableClassBuilder(CreateImmutableClassSettings())
            .WithRecord()
            .WithPartial()
            .AddInterfaces(new[] { $"{iinterface.Namespace}.{iinterface.Name}" }.Where(_ => InheritFromInterfaces))
            .With(x => PostProcessImmutableEntityClass(x))
            .Build();

    private ITypeBase CreateImmutableClassFromClass(IClass cls, string entitiesNamespace)
        => new ClassBuilder(cls)
            .WithNamespace(entitiesNamespace)
            .With(x => FixImmutableClassProperties(x))
            .Build()
            .ToImmutableClassBuilder(CreateImmutableClassSettings())
            .WithRecord()
            .WithPartial()
            .With(x => PostProcessImmutableEntityClass(x))
            .Build();

    private static string CreateSortString(ClassMethodBuilder methodBuilder)
        => $"{methodBuilder.Name}({string.Join(", ", methodBuilder.Parameters.Select(x => x.TypeName))})";
}
