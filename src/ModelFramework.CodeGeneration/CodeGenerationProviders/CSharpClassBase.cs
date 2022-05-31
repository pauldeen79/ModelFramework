﻿namespace ModelFramework.CodeGeneration.CodeGenerationProviders;

public abstract class CSharpClassBase : ClassBase
{
    protected virtual IEnumerable<ClassMethodBuilder> CreateExtraOverloads(IClass c)
        => Enumerable.Empty<ClassMethodBuilder>();
    protected virtual string NewCollectionTypeName => "System.Collections.Generic.List";
    protected virtual string SetMethodNameFormatString => "With{0}";
    protected virtual string AddMethodNameFormatString => "Add{0}";
    protected virtual bool AddNullChecks => false;
    protected virtual bool AddCopyConstructor => true;
    protected virtual bool UseLazyInitialization => true;
    protected virtual bool UseTargetTypeNewExpressions => true;
    protected virtual bool ValidateArgumentsInConstructor => true;
    protected virtual bool InheritFromInterfaces => true;

    protected abstract Type RecordCollectionType { get; }
    protected abstract string FormatInstanceTypeName(ITypeBase instance, bool forCreate);
    protected abstract void FixImmutableBuilderProperties(ClassBuilder classBuilder);
    protected abstract void FixImmutableBuilderProperties(InterfaceBuilder interfaceBuilder);

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

    protected ITypeBase[] GetImmutableClasses(Type[] types, string entitiesNamespace)
        => GetImmutableClasses(types.Select(x => x.IsInterface
            ? (ITypeBase)x.ToInterface()
            : x.ToClass(new ClassSettings())).ToArray(), entitiesNamespace);

    protected ITypeBase[] GetImmutableClasses(ITypeBase[] models, string entitiesNamespace)
        => models.Select
        (
            //x => x is IClass cls
            //    ? CreateClass(cls, entitiesNamespace)
            //    : x is IInterface iinterface
            //        ? CreateInterface(iinterface, entitiesNamespace)
            //        : throw new NotSupportedException("Type of class should be IClass or IInterface")
            x => x switch
            {
                IClass cls => CreateClass(cls, entitiesNamespace),
                IInterface iinterface => CreateInterface(iinterface, entitiesNamespace),
                _ => throw new NotSupportedException("Type of class should be IClass or IInterface")
            }
        ).ToArray();

    private ITypeBase CreateClass(IClass cls, string entitiesNamespace)
        => new ClassBuilder(cls)
            .WithNamespace(entitiesNamespace)
            .Chain(y => FixImmutableBuilderProperties(y))
            .Build()
            .ToImmutableClassBuilder(CreateImmutableClassSettings())
            .WithRecord()
            .WithPartial()
            .Build();

    private ITypeBase CreateInterface(IInterface iinterface, string entitiesNamespace)
        => new InterfaceBuilder(iinterface)
            .WithName(iinterface.Name.StartsWith("I") ? iinterface.Name.Substring(1) : iinterface.Name)
            .WithNamespace(entitiesNamespace)
            .Chain(y => FixImmutableBuilderProperties(y))
            .Build()
            .ToImmutableClassBuilder(CreateImmutableClassSettings())
            .WithRecord()
            .WithPartial()
            .AddInterfaces(new[] { $"{iinterface.Namespace}.{iinterface.Name}" }.Where(_ => InheritFromInterfaces))
            .Build();

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
            .Chain(x => x.Methods.Sort(new Comparison<ClassMethodBuilder>((x, y) => CreateSortString(x).CompareTo(CreateSortString(y)))));

    protected ClassBuilder CreateBuilderExtensions(IClass c, string @namespace)
        => c.ToBuilderExtensionsClassBuilder(CreateImmutableBuilderClassSettings())
            .WithNamespace(@namespace)
            .WithPartial()
            .AddMethods(CreateExtraOverloads(c))
            .Chain(x => x.Methods.Sort(new Comparison<ClassMethodBuilder>((x, y) => CreateSortString(x).CompareTo(CreateSortString(y)))));

    private static string CreateSortString(ClassMethodBuilder x)
        => $"{x.Name}({string.Join(", ", x.Parameters.Select(x => x.TypeName))})";

    protected ImmutableBuilderClassSettings CreateImmutableBuilderClassSettings()
        => new ImmutableBuilderClassSettings(typeSettings: new ImmutableBuilderClassTypeSettings(newCollectionTypeName: NewCollectionTypeName, formatInstanceTypeNameDelegate: FormatInstanceTypeName, useTargetTypeNewExpressions: UseTargetTypeNewExpressions),
                                             constructorSettings: new ImmutableBuilderClassConstructorSettings(addCopyConstructor: AddCopyConstructor, addNullChecks: AddNullChecks),
                                             nameSettings: new ImmutableBuilderClassNameSettings(setMethodNameFormatString: SetMethodNameFormatString, addMethodNameFormatString: AddMethodNameFormatString),
                                             enableNullableReferenceTypes: EnableNullableContext,
                                             useLazyInitialization: UseLazyInitialization);

    protected ImmutableClassSettings CreateImmutableClassSettings()
        => new ImmutableClassSettings(newCollectionTypeName: RecordCollectionType.WithoutGenerics(),
                                      validateArgumentsInConstructor: ValidateArgumentsInConstructor,
                                      addNullChecks: AddNullChecks);

    private IClass CreateImmutableEntities(string entitiesNamespace, ITypeBase x)
        => new ClassBuilder(x.ToClass())
            .WithName(x.Name.Substring(1))
            .WithNamespace(entitiesNamespace)
            .Chain(y => FixImmutableBuilderProperties(y))
            .Build()
            .ToImmutableClass(CreateImmutableClassSettings());
}
