namespace ModelFramework.CodeGeneration.CodeGenerationProviders;

public abstract class CSharpClassBase : ClassBase
{
    protected virtual IEnumerable<ClassMethodBuilder> CreateExtraOverloads(IClass c)
        => Enumerable.Empty<ClassMethodBuilder>();
    protected virtual string NewCollectionTypeName => "System.Collections.Generic.List";
    protected virtual string SetMethodNameFormatString => "With{0}";
    protected virtual bool Poco => false;
    protected virtual bool AddNullChecks => false;
    protected virtual bool AddCopyConstructor => true;

    protected abstract Type RecordCollectionType { get; }

    protected abstract string FormatInstanceTypeName(ITypeBase instance, bool forCreate);
    protected abstract void FixImmutableBuilderProperties(ClassBuilder classBuilder);

    protected IClass[] GetImmutableBuilderClasses(Type[] types,
                                                  string entitiesNamespace,
                                                  string buildersNamespace,
                                                  params string[] interfacesToAdd)
        => GetImmutableBuilderClasses(types.Select(x => x.ToClass(new ClassSettings())).ToArray(),
                                                                  entitiesNamespace,
                                                                  buildersNamespace,
                                                                  interfacesToAdd);

    protected IClass[] GetImmutableBuilderClasses(ITypeBase[] models,
                                                  string entitiesNamespace,
                                                  string buildersNamespace,
                                                  params string[] interfacesToAdd)
        => models.Select
        (
            x => CreateBuilder(CreateImmutableEntities(entitiesNamespace, x), buildersNamespace)
                .Chain(x => x.AddInterfaces(interfacesToAdd.Select(y => string.Format(y, x.Name))))
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
            x => CreateBuilderExtensions(CreateImmutableEntities(entitiesNamespace, x), buildersNamespace)
                .Chain
                (
                    x => x.Methods.ForEach(y => y.AddGenericTypeArguments("T")
                                                 .AddGenericTypeArgumentConstraints($"where T : {builderInterfacesNamespace}.I{y.TypeName}"))
                )
                .Chain
                (
                    x => x.Methods.ForEach(y => y.WithTypeName("T")
                                                 .Chain(z => z.Parameters.First().WithTypeName(z.TypeName)))
                )
                .Build()
        )
        .ToArray();

    protected IClass[] GetImmutableClasses(Type[] types, string entitiesNamespace)
        => GetImmutableClasses(types.Select(x => x.ToClass(new ClassSettings())).ToArray(), entitiesNamespace);

    protected IClass[] GetImmutableClasses(ITypeBase[] models, string entitiesNamespace)
        => models.Select
        (
            x => new ClassBuilder(x.ToClass())
                  .WithName(x.Name.Substring(1))
                  .WithNamespace(entitiesNamespace)
                  .Chain(y => FixImmutableBuilderProperties(y))
                  .Build()
                  .ToImmutableClassBuilder(CreateImmutableClassSettings())
                  .WithRecord()
                  .WithPartial()
                  .AddInterfaces($"{x.Namespace}.{x.Name}")
                  .Build()
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
                t => t.ToClassBuilder(new ClassSettings(createConstructors: true)).WithName(t.Name)
                                                                                  .WithNamespace(t.FullName?.GetNamespaceWithDefault() ?? string.Empty)
                .Chain(x => FixImmutableBuilderProperties(x))
                .Build()
            )
            .ToArray();
    }

    protected ClassBuilder CreateBuilder(IClass c, string @namespace)
        => c.ToImmutableBuilderClassBuilder(CreateImmutableBuilderClassSettings())
            .WithNamespace(@namespace)
            .WithPartial()
            .AddMethods(CreateExtraOverloads(c))
            .Chain(x => x.Methods.Sort(new Comparison<ClassMethodBuilder>((x,y) => x.Name.CompareTo(y.Name))));

    protected ClassBuilder CreateBuilderExtensions(IClass c, string @namespace)
        => c.ToBuilderExtensionsClassBuilder(CreateImmutableBuilderClassSettings())
            .WithNamespace(@namespace)
            .WithPartial()
            .AddMethods(CreateExtraOverloads(c));

    protected ImmutableBuilderClassSettings CreateImmutableBuilderClassSettings()
        => new ImmutableBuilderClassSettings(newCollectionTypeName: NewCollectionTypeName,
                                             constructorSettings: new ImmutableBuilderClassConstructorSettings(addCopyConstructor: AddCopyConstructor, addNullChecks: AddNullChecks),
                                             poco: Poco,
                                             setMethodNameFormatString: SetMethodNameFormatString,
                                             formatInstanceTypeNameDelegate: FormatInstanceTypeName);

    protected ImmutableClassSettings CreateImmutableClassSettings()
        => new ImmutableClassSettings(newCollectionTypeName: RecordCollectionType.WithoutGenerics(),
                                      validateArgumentsInConstructor: true);

    private IClass CreateImmutableEntities(string entitiesNamespace, ITypeBase x)
        => new ClassBuilder(x.ToClass())
            .WithName(x.Name.Substring(1))
            .WithNamespace(entitiesNamespace)
            .Chain(y => FixImmutableBuilderProperties(y))
            .Build()
            .ToImmutableClass(CreateImmutableClassSettings());
}
