﻿namespace ModelFramework.CodeGeneration.CodeGenerationProviders;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1062 // false positive because I've added null guards but code analysis doesn't understand this
#pragma warning restore IDE0079 // Remove unnecessary suppression
public abstract class CSharpClassBase : ClassBase
{
    protected virtual Type BuilderClassCollectionType => typeof(List<>);
    protected virtual string SetMethodNameFormatString => "With{0}";
    protected virtual string AddMethodNameFormatString => "Add{0}";
    protected virtual string BuilderNameFormatString => "{0}Builder";
    protected virtual string BuilderBuildMethodName => "Build";
    protected virtual string BuilderBuildTypedMethodName => "BuildTyped";
    protected virtual string BuilderName => "Builder";
    protected virtual string BuildersName => "Builders";
    protected virtual string BuilderFactoryName => "BuilderFactory";
    protected virtual bool ConvertStringToStringBuilderOnBuilders => true;
    protected virtual bool AddNullChecks => false;
    protected virtual bool AddCopyConstructor => true;
    protected virtual bool UseLazyInitialization => true;
    protected virtual bool UseTargetTypeNewExpressions => true;
    protected virtual ArgumentValidationType ValidateArgumentsInConstructor => ArgumentValidationType.DomainOnly;
    protected virtual bool InheritFromInterfaces => false;
    protected virtual bool AddPrivateSetters => false;
    protected virtual bool CopyPropertyCode => false;
    protected virtual bool CopyFields => false;
    protected virtual bool CopyAttributes => false;
    protected virtual bool EnableEntityInheritance => false;
    protected virtual bool EnableBuilderInhericance => false;
    protected virtual bool RemoveDuplicateWithMethods => true;
    protected virtual bool AllowGenerationWithoutProperties => true;
    protected virtual bool AddBackingFieldsForCollectionProperties => false;
    protected virtual string? CollectionPropertyGetStatement => null;
    protected virtual IClass? BaseClass => null;
    protected virtual bool IsAbstract => false;
    protected virtual string BaseClassBuilderNamespace => string.Empty;
    protected virtual bool IsMemberValid(IParentTypeContainer parentNameContainer, INameContainer nameContainer, ITypeBase typeBase)
        => parentNameContainer is not null
        && typeBase is not null
        && (string.IsNullOrEmpty(parentNameContainer.ParentTypeFullName)
            || (BaseClass is not null && !BaseClass.Properties.Any(x => x.Name == nameContainer.Name))
            || parentNameContainer.ParentTypeFullName.GetClassName().In(typeBase.Name, $"I{typeBase.Name}")
            || Array.Exists(GetModelAbstractBaseTyped(), x => x == parentNameContainer.ParentTypeFullName.GetClassName())
            || (parentNameContainer.ParentTypeFullName.StartsWith($"{CodeGenerationRootNamespace}.Models.Abstractions.") && typeBase.Namespace == RootNamespace)
        );

    protected abstract string ProjectName { get; }
    protected virtual string RootNamespace => InheritFromInterfaces
        ? $"{ProjectName}.Abstractions"
        : $"{ProjectName}.Domain";
    protected virtual string CodeGenerationRootNamespace => $"{ProjectName}.CodeGeneration";
    protected virtual string CoreNamespace => $"{ProjectName}.Core";
    protected abstract Type RecordCollectionType { get; }
    protected abstract Type RecordConcreteCollectionType { get; }

    protected virtual string FormatInstanceTypeName(ITypeBase instance, bool forCreate)
    {
        Guard.IsNotNull(instance);

        if (InheritFromInterfaces && (instance.Namespace == RootNamespace || instance.Namespace == CoreNamespace))
        {
            return forCreate
                ? $"{CoreNamespace}.{instance.Name}"
                : $"{RootNamespace}.I{instance.Name}";
        }

        return string.Empty;
    }

    protected virtual string GetFullBasePath()
        => Directory.GetCurrentDirectory().EndsWith(ProjectName)
            ? System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"src/")
            : System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"../../../../");

    protected virtual string[] GetCustomBuilderTypes()
        => GetPureAbstractModels()
            .Select(x => x.GetEntityClassName())
            .Concat(GetExternalCustomBuilderTypes())
            .ToArray();

    protected virtual Dictionary<string, string> GetCustomDefaultValueForBuilderClassConstructorValues()
    {
        var result = new Dictionary<string, string>();
        var nullableSuffix = EnableNullableContext
            ? "!"
            : string.Empty;

        result.AddRange(GetPureAbstractModels().Select(x => new KeyValuePair<string, string>($"{RootNamespace}.{x.GetEntityClassName()}", $"default{nullableSuffix}")));
        return result;
    }

    protected virtual Dictionary<string, string> GetBuilderNamespaceMappings()
    {
        var result = new Dictionary<string, string>();
        result.AddRange(
            GetCustomBuilderTypes()
                .Select(x => new KeyValuePair<string, string>($"{RootNamespace}.{x}s", $"{RootNamespace}.{BuildersName}.{x}s"))
                .Concat(new[] { new KeyValuePair<string, string>(RootNamespace, $"{RootNamespace}.{BuildersName}") }));
        result.AddRange(GetCustomBuilderNamespaceMapping());
        return result;
    }

    protected virtual Dictionary<string, string> GetModelMappings()
    {
        var result = new Dictionary<string, string>();
        var suffix = InheritFromInterfaces ? "I" : string.Empty;
        result.Add($"{CodeGenerationRootNamespace}.Models.I", $"{RootNamespace}.{suffix}");
        result.AddRange(GetPureAbstractModels().Select(x => new KeyValuePair<string, string>($"{CodeGenerationRootNamespace}.Models.{x.GetEntityClassName().ReplaceSuffix("Base", string.Empty, StringComparison.Ordinal)}s.I", $"{RootNamespace}.{x.GetEntityClassName().ReplaceSuffix("Base", string.Empty, StringComparison.Ordinal)}s.")));
        result.Add($"{CodeGenerationRootNamespace}.Models.Domains.", $"{RootNamespace}.Domains.");
        result.Add($"{CodeGenerationRootNamespace}.Models.Contracts.", $"{RootNamespace}.Contracts.");
        result.Add($"{CodeGenerationRootNamespace}.Models.Abstractions.", $"{RootNamespace}.Abstractions.");
        result.Add($"{CodeGenerationRootNamespace}.I", $"{RootNamespace}.I");
        return result;
    }

    protected virtual string[] GetNonDomainTypes()
        => GetType().Assembly.GetTypes()
            .Where(x => x.IsInterface && x.Namespace == CodeGenerationRootNamespace)
            .Select(x => $"{RootNamespace}.{x.Name}")
            .ToArray();

    protected virtual string[] GetModelAbstractBaseTyped() => Array.Empty<string>();

    protected virtual string[] GetExternalCustomBuilderTypes() => Array.Empty<string>();

    protected virtual IEnumerable<KeyValuePair<string, string>> GetCustomBuilderNamespaceMapping() => Enumerable.Empty<KeyValuePair<string, string>>();

    protected ITypeBase[] GetCoreModels()
        => MapCodeGenerationModelsToDomain(
            GetType().Assembly.GetTypes()
                .Where(x => x.IsInterface && x.Namespace == $"{CodeGenerationRootNamespace}.Models" && !GetCustomBuilderTypes().Contains(x.GetEntityClassName())));

    protected ITypeBase[] GetAbstractModels()
        => MapCodeGenerationModelsToDomain(GetPureAbstractModels());

    protected ITypeBase[] GetOverrideModels(Type abstractType)
    {
        Guard.IsNotNull(abstractType);

        return MapCodeGenerationModelsToDomain(
            GetType().Assembly.GetTypes()
                .Where(x => x.IsInterface && Array.Exists(x.GetInterfaces(), y => y == abstractType)));
    }

    protected ITypeBase[] GetImmutableBuilderClasses(Type[] types,
                                                     string entitiesNamespace,
                                                     string buildersNamespace,
                                                     params string[] interfacesToAdd)
    {
        Guard.IsNotNull(types);
        Guard.IsNotNull(entitiesNamespace);
        Guard.IsNotNull(buildersNamespace);

        return GetImmutableBuilderClasses(types.Select(x => x.ToTypeBase(CreateClassSettings())).ToArray(),
                                          entitiesNamespace,
                                          buildersNamespace,
                                          interfacesToAdd);
    }

    protected ITypeBase[] GetImmutableBuilderClasses(ITypeBase[] models,
                                                     string entitiesNamespace,
                                                     string buildersNamespace,
                                                     params string[] interfacesToAdd)
    {
        Guard.IsNotNull(models);
        Guard.IsNotNull(entitiesNamespace);
        Guard.IsNotNull(buildersNamespace);

        return models.Select
        (
            x => CreateBuilder(CreateImmutableEntity(entitiesNamespace, x), buildersNamespace)
                .With(x => x.AddInterfaces(interfacesToAdd.Select(y => string.Format(y, x.Name))))
                .With(x => Visit(x))
                .Build()
        ).ToArray();
    }

    protected ITypeBase[] GetImmutableNonGenericBuilderClasses(Type[] types,
                                                               string entitiesNamespace,
                                                               string buildersNamespace,
                                                               params string[] interfacesToAdd)
    {
        Guard.IsNotNull(types);
        Guard.IsNotNull(entitiesNamespace);
        Guard.IsNotNull(buildersNamespace);

        return GetImmutableNonGenericBuilderClasses(types.Select(x => x.ToTypeBase()).ToArray(),
                                                    entitiesNamespace,
                                                    buildersNamespace,
                                                    interfacesToAdd);
    }

    protected ITypeBase[] GetImmutableNonGenericBuilderClasses(ITypeBase[] models,
                                                               string entitiesNamespace,
                                                               string buildersNamespace,
                                                               params string[] interfacesToAdd)
    {
        Guard.IsNotNull(models);
        Guard.IsNotNull(entitiesNamespace);
        Guard.IsNotNull(buildersNamespace);

        return models.Select
        (
            x => CreateNonGenericBuilder(CreateImmutableEntity(entitiesNamespace, x), buildersNamespace)
                .With(x => x.AddInterfaces(interfacesToAdd.Select(y => string.Format(y, x.Name))))
                .With(x => Visit(x))
                .Build()
        ).ToArray();
    }

    protected IClass[] GetImmutableBuilderExtensionClasses(Type[] types,
                                                           string entitiesNamespace,
                                                           string buildersNamespace,
                                                           string builderInterfacesNamespace)
    {
        Guard.IsNotNull(types);
        Guard.IsNotNull(entitiesNamespace);
        Guard.IsNotNull(buildersNamespace);

        return GetImmutableBuilderExtensionClasses(types.Select(x => x.ToClass()).ToArray(),
                                                   entitiesNamespace,
                                                   buildersNamespace,
                                                   builderInterfacesNamespace);
    }

    protected IClass[] GetImmutableBuilderExtensionClasses(ITypeBase[] models,
                                                           string entitiesNamespace,
                                                           string buildersNamespace,
                                                           string builderInterfacesNamespace)
    {
        Guard.IsNotNull(models);
        Guard.IsNotNull(entitiesNamespace);
        Guard.IsNotNull(buildersNamespace);

        return models.Select
        (
            x => CreateBuilderExtensions(CreateImmutableEntity(entitiesNamespace, x), buildersNamespace)
                .With(x => Visit(x))
                .BuildTyped()
        )
        .ToArray();
    }

    protected ITypeBase[] GetImmutableClasses(Type[] types, string entitiesNamespace)
    {
        Guard.IsNotNull(types);
        Guard.IsNotNull(entitiesNamespace);

        return GetImmutableClasses(types.Select(x => x.IsInterface
            ? x.ToInterfaceBuilder().With(x => FixImmutableClassProperties(x)).With(x => Visit(x)).Build()
            : x.ToClassBuilder().With(x => FixImmutableClassProperties(x)).With(x => Visit(x)).Build()).ToArray(), entitiesNamespace);
    }

    protected ITypeBase[] GetImmutableClasses(ITypeBase[] models, string entitiesNamespace)
    {
        Guard.IsNotNull(models);
        Guard.IsNotNull(entitiesNamespace);

        if (ValidateArgumentsInConstructor == ArgumentValidationType.Shared)
        {
            return models.SelectMany
            (
                x => x switch
                {
                    IClass cls => new[] { CreateImmutableClassFromClass(cls, entitiesNamespace), CreateImmutableOverrideClassFromClass(cls, entitiesNamespace) },
                    IInterface iinterface => new[] { CreateImmutableClassFromInterface(iinterface, entitiesNamespace), CreateImmutableOverrideClassFromInterface(iinterface, entitiesNamespace) },
                    _ => throw new NotSupportedException("Type of class should be IClass or IInterface")
                }
            ).ToArray();
        }

        return models.Select
        (
            x => x switch
            {
                IClass cls => CreateImmutableClassFromClass(cls, entitiesNamespace),
                IInterface iinterface => CreateImmutableClassFromInterface(iinterface, entitiesNamespace),
                _ => throw new NotSupportedException("Type of class should be IClass or IInterface")
            }
        ).ToArray();
    }

    protected IClass[] GetClassesFromSameNamespace(Type type)
    {
        Guard.IsNotNull(type);

        if (type.FullName is null)
        {
            throw new ArgumentException("Can't get classes from same namespace when the FullName of this type is null. Could not determine namespace.");
        }

        return type.Assembly.GetTypes()
            .Where
            (
                t => t.FullName is not null
                    && t.FullName.GetNamespaceWithDefault() == type.FullName.GetNamespaceWithDefault()
                    && t.GetProperties().Length > 0
            )
            .Select
            (
                t => t.ToClassBuilder(CreateClassSettings())
                      .WithName(t.Name)
                      .WithNamespace(t.FullName.GetNamespaceWithDefault())
                      .With(x => FixImmutableBuilderProperties(x))
                      .With(x => Visit(x))
                      .BuildTyped()
            )
            .ToArray();
    }

    protected IClass CreateBaseclass(Type type, string @namespace)
    {
        Guard.IsNotNull(type);
        Guard.IsNotNull(@namespace);

        return type.ToClass().ToImmutableClassBuilder(new ImmutableClassSettings
        (
            newCollectionTypeName: RecordCollectionType.WithoutGenerics(),
            allowGenerationWithoutProperties: AllowGenerationWithoutProperties,
            constructorSettings: new(
                validateArguments: ValidateArgumentsInConstructor,
                addNullChecks: AddNullChecks),
            addPrivateSetters: AddPrivateSetters,
            enableNullableReferenceTypes: EnableNullableContext,
            inheritanceSettings: new(isAbstract: true))
        )
        .WithNamespace(@namespace)
        .WithName(type.GetEntityClassName())
        .With(x => x.Properties.ForEach(y => GetModelMappings().Where(x => !x.Key.EndsWith(".Contracts", StringComparison.InvariantCulture)).ToList().ForEach(m => y.TypeName = y.TypeName.Replace(m.Key, m.Value))))
        .With(x => FixImmutableClassProperties(x))
        .With(x => Visit(x))
        .BuildTyped();
    }

    protected ClassBuilder CreateBuilder(ITypeBase typeBase, string @namespace)
    {
        Guard.IsNotNull(typeBase);

        return typeBase.ToImmutableBuilderClassBuilder(CreateImmutableBuilderClassSettings(@namespace, ArgumentValidationType.None));
    }

    protected ClassBuilder CreateNonGenericBuilder(ITypeBase typeBase, string @namespace)
    {
        Guard.IsNotNull(typeBase);

        return typeBase.ToNonGenericImmutableBuilderClassBuilder(CreateImmutableBuilderClassSettings(@namespace, ArgumentValidationType.None));
    }

    protected ClassBuilder CreateBuilderExtensions(ITypeBase typeBase, string @namespace)
    {
        Guard.IsNotNull(typeBase);

        TypeBaseBuilder builder = typeBase switch
        {
            IInterface i => new InterfaceBuilder(i),
            IClass c => new ClassBuilder(c),
            _ => throw new NotSupportedException("Type of class should be IClass or IInterface")
        };

        return 
            builder
            .WithAll(y => y.Properties, z =>
            {
                z.TypeName = MapCodeGenerationNamespacesToDomain(z.TypeName)
                    .Replace(RecordCollectionType.WithoutGenerics(), BuilderClassCollectionType.WithoutGenerics(), StringComparison.Ordinal);

                if (!z.TypeName.Contains(".Domains", StringComparison.Ordinal))
                {
                    FixPropertyInterfacesNamespaces(z);
                }

                z.HasSetter = true;
                z.Attributes.Clear();
            })
            .Chain(FixInterfacesNamespaces)
            .Build()
            .ToBuilderExtensionsClassBuilder(CreateImmutableBuilderClassSettings(@namespace, ArgumentValidationType.None));
    }

    protected virtual void FixImmutableClassProperties<TBuilder, TEntity>(TypeBaseBuilder<TBuilder, TEntity> typeBaseBuilder)
        where TEntity : ITypeBase
        where TBuilder : TypeBaseBuilder<TBuilder, TEntity>
    {
        Guard.IsNotNull(typeBaseBuilder);
        FixImmutableBuilderProperties(typeBaseBuilder);
    }

    protected virtual void FixImmutableBuilderProperties<TBuilder, TEntity>(TypeBaseBuilder<TBuilder, TEntity> typeBaseBuilder)
        where TEntity : ITypeBase
        where TBuilder : TypeBaseBuilder<TBuilder, TEntity>
    {
        Guard.IsNotNull(typeBaseBuilder);

        foreach (var property in typeBaseBuilder.Properties)
        {
            var typeName = property.TypeName.FixTypeName();
            FixImmutableBuilderProperty(property, typeName);

            if (typeName.StartsWith($"{RecordCollectionType.WithoutGenerics()}<", StringComparison.InvariantCulture)
                && AddBackingFieldsForCollectionProperties)
            {
                property.AddCollectionBackingFieldOnImmutableClass(RecordConcreteCollectionType, CollectionPropertyGetStatement, forceNullCheck: ValidateArgumentsInConstructor != ArgumentValidationType.None);
            }
        }
    }

    /// <summary>
    /// Allows to modify a generated class using the Visitor pattern
    /// </summary>
    /// <typeparam name="TBuilder">Builder type</typeparam>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <param name="typeBaseBuilder">Builder instance to visit.</param>
    protected virtual void Visit<TBuilder, TEntity>(TypeBaseBuilder<TBuilder, TEntity> typeBaseBuilder)
        where TEntity : ITypeBase
        where TBuilder : TypeBaseBuilder<TBuilder, TEntity>
    {
    }

    protected virtual void FixImmutableBuilderProperty(ClassPropertyBuilder property, string typeName)
    {
        Guard.IsNotNull(property);
        Guard.IsNotNull(typeName);

        if (!property.IsValueType
            && typeName.StartsWithAny(StringComparison.InvariantCulture, GetBuilderNamespaceMappings().Keys.Select(x => $"{x}."))
            && !GetNonDomainTypes().Contains(typeName))
        {
            FixSingleDomainProperty(property, typeName);
        }
        else if (typeName.StartsWithAny(StringComparison.InvariantCulture, GetBuilderNamespaceMappings().Keys.Select(x => $"{RecordCollectionType.WithoutGenerics()}<{x}.")))
        {
            FixCollectionDomainProperty(property, typeName);
        }
        else if (typeName.IsBooleanTypeName() || typeName.IsNullableBooleanTypeName())
        {
            property.SetDefaultArgumentValueForWithMethod(true);
        }
        else if (ConvertStringToStringBuilderOnBuilders && typeName.IsStringTypeName())
        {
            property.ConvertStringPropertyToStringBuilderPropertyOnBuilder(UseLazyInitialization);
        }
    }

    protected ITypeBase[] MapCodeGenerationModelsToDomain(IEnumerable<Type> types)
    {
        Guard.IsNotNull(types);

        return types
            .Select(x => x.ToClassBuilder(new ClassSettings())
            .AddMetadata("CSharpClassBase.ModelType", x)
            .WithNamespace(RootNamespace)
            .WithName(x.GetEntityClassName())
            .With(y => y.Properties.ForEach(z => z.TypeName = MapCodeGenerationNamespacesToDomain(z.TypeName)))
            .Build())
        .ToArray();
    }

    protected string MapCodeGenerationNamespacesToDomain(string typeName)
    {
        foreach (var mapping in GetModelMappings().Where(x => !x.Key.EndsWith(".Contracts", StringComparison.InvariantCulture)))
        {
            typeName = typeName.Replace(mapping.Key, mapping.Value);
        }

        return typeName;
    }

    protected string GetBuilderNamespace(string typeName)
        => GetBuilderNamespaceMappings()
            .Where(x => typeName.StartsWith(x.Key + ".", StringComparison.InvariantCulture))
            .Select(x => x.Value)
            .FirstOrDefault() ?? string.Empty;

    protected virtual string ReplaceWithBuilderNamespaces(string typeName)
    {
        Guard.IsNotNull(typeName);

        var match = GetBuilderNamespaceMappings()
            .Select(x => new { x.Key, x.Value })
            .FirstOrDefault(x => typeName.Contains($"{x.Key}."));

        return match is null
            ? typeName
            : typeName.Replace($"{match.Key}.", $"{match.Value}.");
    }

    /// <summary>
    /// Gets the base typename, based on a derived class.
    /// </summary>
    /// <param name="className">The typename to get the base classname from.</param>
    /// <returns>Base classname when found, otherwise string.Empty</returns>
    protected string GetEntityClassName(string className)
        => Array.Find(GetCustomBuilderTypes(), x => className.EndsWith(x, StringComparison.InvariantCulture)) ?? string.Empty;

    protected string GetEntityTypeName(string builderFullName)
    {
        Guard.IsNotNull(builderFullName);

        var match = GetBuilderNamespaceMappings()
            .Select(x => new { x.Key, x.Value })
            .FirstOrDefault(x => builderFullName.StartsWith($"{x.Value}.", StringComparison.InvariantCulture));

        return match is null
            ? builderFullName.ReplaceSuffix(BuilderName, string.Empty, StringComparison.InvariantCulture)
            : builderFullName
                .Replace($"{match.Value}.", $"{match.Key}.")
                .ReplaceSuffix(BuilderName, string.Empty, StringComparison.InvariantCulture);
    }

    protected string? GetCustomBuilderMethodParameterExpression(string typeName)
        => (string.IsNullOrEmpty(GetEntityClassName(typeName)) || GetCustomBuilderTypes().Contains(typeName.GetClassName())) && string.IsNullOrEmpty(typeName.GetGenericArguments())
            ? string.Empty
            : "{0}{2}." + BuilderBuildTypedMethodName + "()";

    protected ImmutableBuilderClassSettings CreateImmutableBuilderClassSettings(string @namespace, ArgumentValidationType? forceValidateArgumentsInConstructor = null)
        => new
        (
            typeSettings: new(
                newCollectionTypeName: BuilderClassCollectionType.WithoutGenerics(),
                formatInstanceTypeNameDelegate: FormatInstanceTypeName,
                useTargetTypeNewExpressions: UseTargetTypeNewExpressions,
                enableNullableReferenceTypes: EnableNullableContext),
            constructorSettings: new(
                addCopyConstructor: AddCopyConstructor,
                addNullChecks: AddNullChecks),
            nameSettings: new(
                setMethodNameFormatString: SetMethodNameFormatString,
                addMethodNameFormatString: AddMethodNameFormatString,
                builderNameFormatString: BuilderNameFormatString,
                buildersNamespace: @namespace,
                buildMethodName: BuilderBuildMethodName,
                buildTypedMethodName: BuilderBuildTypedMethodName),
            generationSettings: new(
                useLazyInitialization: UseLazyInitialization,
                copyPropertyCode: CopyPropertyCode,
                copyFields: CopyFields,
                copyAttributes: CopyAttributes,
                allowGenerationWithoutProperties: AllowGenerationWithoutProperties),
            inheritanceSettings: new(
                enableEntityInheritance: EnableEntityInheritance,
                enableBuilderInheritance: EnableBuilderInhericance,
                removeDuplicateWithMethods: RemoveDuplicateWithMethods,
                baseClass: BaseClass,
                baseClassBuilderNameSpace: BaseClassBuilderNamespace,
                inheritanceComparisonDelegate: EnableBuilderInhericance
                    ? IsMemberValid
                    : (_, _, _) => true),
            classSettings: CreateImmutableClassSettings(forceValidateArgumentsInConstructor ?? ArgumentValidationType.None)
        );

    protected ImmutableClassSettings CreateImmutableClassSettings(ArgumentValidationType? forceValidateArgumentsInConstructor = null, bool? overrideAddNullChecks = null)
        => new
        (
            newCollectionTypeName: RecordCollectionType.WithoutGenerics(),
            allowGenerationWithoutProperties: AllowGenerationWithoutProperties,
            constructorSettings: new(
                validateArguments: forceValidateArgumentsInConstructor ?? CombineValidateArguments(ValidateArgumentsInConstructor, !(EnableEntityInheritance && BaseClass is null)),
                originalValidateArguments: ValidateArgumentsInConstructor,
                addNullChecks: forceValidateArgumentsInConstructor != ArgumentValidationType.Shared && (overrideAddNullChecks ?? false)),
            addPrivateSetters: AddPrivateSetters,
            inheritanceSettings: new(
                enableInheritance: EnableEntityInheritance,
                baseClass: BaseClass,
                isAbstract: IsAbstract,
                inheritFromInterfaces: InheritFromInterfaces,
                formatInstanceTypeNameDelegate: FormatInstanceTypeName,
                inheritanceComparisonDelegate: EnableEntityInheritance
                    ? IsMemberValid
                    : (_, _, _) => true),
                enableNullableReferenceTypes : EnableNullableContext
        );

    protected ArgumentValidationType CombineValidateArguments(ArgumentValidationType validateArgumentsInConstructor, bool secondCondition)
        => secondCondition
            ? validateArgumentsInConstructor
            : ArgumentValidationType.None;

    protected ClassSettings CreateClassSettings()
        => new
        (
            createConstructors: true,
            attributeInitializeDelegate: AttributeInitializeDelegate,
            useCustomInitializers: UseCustomInitializersOnAttributeBuilder
        );

    protected string GetModelTypeName(Type modelType)
    {
        Guard.IsNotNull(modelType);

        return GetCoreModels().Concat(GetAbstractModels())
            .FirstOrDefault(x => x.Metadata.GetValue<Type?>("CSharpClassBase.ModelType", () => null) == modelType)?.GetFullName()
                ?? throw new ArgumentOutOfRangeException(nameof(modelType), $"Unknown model type: {modelType.FullName}");
    }

    protected virtual string CurrentNamespace => Path.Replace("/", ".");

    protected static object CreateServiceCollectionExtensions(
        string @namespace,
        string className,
        string methodName,
        ITypeBase[] types,
        Func<ITypeBase, string> formatDelegate)
    {
        Guard.IsNotNull(@namespace);
        Guard.IsNotNull(className);
        Guard.IsNotNull(methodName);
        Guard.IsNotNull(types);
        Guard.IsNotNull(formatDelegate);
        return new[]
        {
            new ClassBuilder()
                .WithNamespace(@namespace)
                .WithName(className)
                .WithStatic()
                .WithPartial()
                .AddMethods(new ClassMethodBuilder()
                    .WithVisibility(Visibility.Private)
                    .WithStatic()
                    .WithName(methodName)
                    .WithExtensionMethod()
                    .WithType(typeof(IServiceCollection))
                    .AddParameter("serviceCollection", typeof(IServiceCollection))
                    .AddLiteralCodeStatements("return serviceCollection")
                    .AddLiteralCodeStatements(types.Select(formatDelegate.Invoke))
                    .AddLiteralCodeStatements(";")
                )
                .Build()
        };
    }

    protected ITypeBase[] CreateBuilderFactories(
        ITypeBase[] models,
        BuilderFactoryNamespaceSettings namespaceSettings,
        string? createLiteralCodeStatement = null)
    {
        Guard.IsNotNull(models);
        Guard.IsNotNull(namespaceSettings);

        return
        [
            new ClassBuilder()
                .WithName(namespaceSettings.ClassName)
                .WithNamespace(namespaceSettings.ClassNamespace)
                .WithStatic()
                .WithPartial()
                .AddFields(new ClassFieldBuilder()
                    .WithName("registeredTypes")
                    .WithStatic()
                    .WithTypeName($"Dictionary<{typeof(Type).FullName},Func<{namespaceSettings.ClassTypeName},{namespaceSettings.BuilderTypeName}>>")
                    .WithDefaultValue(GetBuilderFactoryModelDefaultValue(models, namespaceSettings.BuilderNamespace, namespaceSettings.ClassTypeName, namespaceSettings.BuilderTypeName, namespaceSettings.OverrideClassNamespace))
                )
                .AddMethods(
                    new ClassMethodBuilder()
                        .WithName("Create")
                        .WithTypeName($"{namespaceSettings.ClassNamespace}.{namespaceSettings.BuilderTypeName}")
                        .WithStatic()
                        .AddParameter("instance", namespaceSettings.ClassTypeName)
                        .With(x =>
                        {
                            if (!string.IsNullOrEmpty(createLiteralCodeStatement))
                            {
                                x.AddLiteralCodeStatements(createLiteralCodeStatement!);
                            }
                        })
                        .AddLiteralCodeStatements("return registeredTypes.ContainsKey(instance.GetType()) ? registeredTypes[instance.GetType()].Invoke(instance) : throw new " + typeof(ArgumentOutOfRangeException).FullName + "(\"Unknown instance type: \" + instance.GetType().FullName);"),
                    new ClassMethodBuilder()
                        .WithStatic()
                        .WithName("Register")
                        .AddParameter("type", typeof(Type))
                        .AddParameter("createDelegate", $"Func<{namespaceSettings.ClassTypeName},{namespaceSettings.BuilderTypeName}>")
                        .AddLiteralCodeStatements("registeredTypes.Add(type, createDelegate);")
                )
                .Build()
        ];
    }

    protected ITypeBase CreateBuilderInterface(InterfaceBuilder interfaceBuilder)
    {
        Guard.IsNotNull(interfaceBuilder);

        return interfaceBuilder
            .WithNamespace(CurrentNamespace)
            .WithVisibility(Visibility.Public)
            .WithName($"{interfaceBuilder.Name}{BuilderName}")
            .WithAll(y => y.Properties, z =>
            {
                z.TypeName = MapCodeGenerationNamespacesToDomain(z.TypeName)
                    .Replace(RecordCollectionType.WithoutGenerics(), BuilderClassCollectionType.WithoutGenerics(), StringComparison.Ordinal);

                if (!z.TypeName.Contains(".Domains", StringComparison.Ordinal))
                {
                    FixPropertyInterfacesNamespaces(z);
                }

                z.HasSetter = true;
                z.Attributes.Clear();
            })
            .Chain(FixInterfacesNamespaces)
            .Build();
    }

    private void FixPropertyInterfacesNamespaces(ClassPropertyBuilder classPropertyBuilder)
    {
        foreach (var mapping in GetBuilderNamespaceMappings())
        {
            if (classPropertyBuilder.TypeName.IndexOf($"{mapping.Key}.", StringComparison.Ordinal) > -1)
            {
                classPropertyBuilder.TypeName = classPropertyBuilder.TypeName.Replace($"{mapping.Key}.", $"{mapping.Value}.", StringComparison.Ordinal);
                if (classPropertyBuilder.TypeName.EndsWith('>'))
                {
                    classPropertyBuilder.TypeName = classPropertyBuilder.TypeName.ReplaceSuffix(">", $"{BuilderName}>", StringComparison.Ordinal);
                }
                else
                {
                    classPropertyBuilder.TypeName += BuilderName;
                }
            }
        }
    }

    private void FixInterfacesNamespaces(TypeBaseBuilder interfaceBuilder)
    {
        for (int i = 0; i < interfaceBuilder.Interfaces.Count; i++)
        {
            interfaceBuilder.Interfaces[i] = interfaceBuilder.Interfaces[i].Replace($"{CodeGenerationRootNamespace}.Models.Abstractions.", $"{RootNamespace}.{BuildersName}.Abstractions.", StringComparison.Ordinal) + BuilderName;
        }
    }

    protected ITypeBase CreateInterface(InterfaceBuilder interfaceBuilder)
    {
        Guard.IsNotNull(interfaceBuilder);

        return interfaceBuilder
            .WithNamespace(CurrentNamespace)
            .WithVisibility(Visibility.Public)
            .WithAll(y => y.Properties, z => z.TypeName = MapCodeGenerationNamespacesToDomain(z.TypeName))
            .Chain(y =>
            {
                y.Interfaces.RemoveAll(x => x.StartsWith($"{CodeGenerationRootNamespace}.Abstractions."));
                for (int i = 0; i < y.Interfaces.Count; i++)
                {
                    y.Interfaces[i] = MapCodeGenerationNamespacesToDomain(y.Interfaces[i]);
                }
            })
            .Build();
    }

    private void FixCollectionDomainProperty(ClassPropertyBuilder property, string typeName)
    {
        if (typeName.GetGenericArguments().GetNamespaceWithDefault() == $"{RootNamespace}.Contracts")
        {
            // Contracts need to be skipped, do not touch these!
            return;
        }

        property.ConvertCollectionOnBuilderToEnumerable(AddNullChecks, ValidateArgumentsInConstructor, RecordConcreteCollectionType.WithoutGenerics());
        if (TypeNameNeedsSpecialTreatmentForBuilderInCollection(typeName))
        {
            property.ConvertCollectionPropertyToBuilderOnBuilder
            (
                GetCustomCollectionArgumentType(typeName),
                GetCustomBuilderConstructorInitializeExpressionForCollectionProperty(typeName),
                builderCollectionTypeName: GetBuilderClassCollectionTypeName(),
                builderName: BuilderName,
                buildMethodName: BuilderBuildMethodName
            );
        }
        else
        {
            property.ConvertCollectionPropertyToBuilderOnBuilder
            (
                GetCustomCollectionArgumentType(typeName),
                customBuilderMethodParameterExpression: GetCustomBuilderMethodParameterExpressionForCollectionProperty(typeName),
                builderCollectionTypeName: GetBuilderClassCollectionTypeName(),
                builderName: BuilderName,
                buildMethodName: BuilderBuildMethodName
            );
        }
    }

    private void FixSingleDomainProperty(ClassPropertyBuilder property, string typeName)
    {
        if (typeName.WithoutProcessedGenerics().GetNamespaceWithDefault() == $"{RootNamespace}.Contracts")
        {
            // Contracts need to be skipped, do not touch these!
            return;
        }

        var argumentType = !string.IsNullOrEmpty(typeName.GetGenericArguments())
            ? $"{GetBuilderNamespace(typeName.WithoutProcessedGenerics())}.{typeName.WithoutProcessedGenerics().GetClassName()}{BuilderName}<{typeName.GetGenericArguments()}>"
            : $"{GetBuilderNamespace(typeName)}.{typeName.GetClassName()}{BuilderName}";

        property.WithCustomBuilderConstructorInitializeExpressionSingleProperty(argumentType, GetCustomBuilderConstructorInitializeExpressionForSingleProperty(property, typeName));
        property.WithCustomBuilderArgumentTypeSingleProperty(argumentType, BuilderName);
        property.WithCustomBuilderMethodParameterExpression(GetCustomBuilderMethodParameterExpression(typeName), BuilderBuildMethodName);

        if (!property.IsNullable)
        {
            property.SetDefaultValueForBuilderClassConstructor(GetDefaultValueForBuilderClassConstructor(typeName));
        }
    }

    private string? GetBuilderClassCollectionTypeName()
        => BuilderClassCollectionType is null
            ? null
            : BuilderClassCollectionType.WithoutGenerics();

    private string? GetCustomBuilderMethodParameterExpressionForCollectionProperty(string typeName)
    {
        if (AddNullChecks && ValidateArgumentsInConstructor == ArgumentValidationType.Shared)
        {
            return !string.IsNullOrEmpty(GetEntityClassName(typeName.GetGenericArguments())) || typeName.GetGenericArguments().GetNamespaceWithDefault().StartsWith($"{ProjectName}.Domain.")
                ? "{0}?.Select(x => x." + BuilderBuildTypedMethodName + "()) ?? " + typeof(Enumerable).FullName + ".Empty<" + typeName.GetGenericArguments() + ">()"
                : "{0}?.Select(x => x." + BuilderBuildMethodName + "()) ?? " + typeof(Enumerable).FullName + ".Empty<" + typeName.GetGenericArguments() + ">()";
        }

        return !string.IsNullOrEmpty(GetEntityClassName(typeName.GetGenericArguments())) || typeName.GetGenericArguments().GetNamespaceWithDefault().StartsWith($"{ProjectName}.Domain.")
            ? "{0}.Select(x => x." + BuilderBuildTypedMethodName + "())"
            : null;
    }

    private IClass CreateImmutableEntity(string entitiesNamespace, ITypeBase typeBase)
        => new ClassBuilder(typeBase.ToClass())
            .WithName(typeBase.GetEntityClassName())
            .WithNamespace(entitiesNamespace)
            .With(x => FixImmutableBuilderProperties(x))
            .With(x => Visit(x))
            .Build()
            .ToImmutableClassBuilder(CreateImmutableClassSettings(ArgumentValidationType.None))
            .With(x => Visit(x))
            .BuildTyped();

    private ITypeBase CreateImmutableClassFromInterface(IInterface iinterface, string entitiesNamespace)
        => new InterfaceBuilder(iinterface)
            .WithName(iinterface.GetEntityClassName())
            .WithNamespace(entitiesNamespace)
            .With(x => FixImmutableClassProperties(x))
            .With(x => Visit(x))
            .Build()
            .ToImmutableClassBuilder(CreateImmutableClassSettings(overrideAddNullChecks: AddNullChecks && ValidateArgumentsInConstructor == ArgumentValidationType.None ? true : null))
            .WithRecord()
            .WithPartial()
            .With(x => Visit(x))
            .Build();

    private ITypeBase CreateImmutableOverrideClassFromInterface(IInterface iinterface, string entitiesNamespace)
        => new InterfaceBuilder(iinterface)
            .WithName(iinterface.GetEntityClassName())
            .WithNamespace(entitiesNamespace)
            .With(x => FixImmutableClassProperties(x))
            .With(x => Visit(x))
            .Build()
            .ToImmutableClassValidateOverrideBuilder(CreateImmutableClassSettings(overrideAddNullChecks: true))
            .WithRecord()
            .WithPartial()
            .With(x => Visit(x))
            .Build();

    private ITypeBase CreateImmutableClassFromClass(IClass cls, string entitiesNamespace)
        => new ClassBuilder(cls)
            .WithNamespace(entitiesNamespace)
            .With(x => FixImmutableClassProperties(x))
            .With(x => Visit(x))
            .Build()
            .ToImmutableClassBuilder(CreateImmutableClassSettings(overrideAddNullChecks: AddNullChecks && ValidateArgumentsInConstructor == ArgumentValidationType.None ? true : null))
            .WithRecord()
            .WithPartial()
            .With(x => Visit(x))
            .Build();

    private ITypeBase CreateImmutableOverrideClassFromClass(IClass cls, string entitiesNamespace)
        => new ClassBuilder(cls)
            .WithNamespace(entitiesNamespace)
            .With(x => FixImmutableClassProperties(x))
            .With(x => Visit(x))
            .Build()
            .ToImmutableClassValidateOverrideBuilder(CreateImmutableClassSettings(ValidateArgumentsInConstructor, true))
            .WithRecord()
            .WithPartial()
            .With(x => Visit(x))
            .Build();

    private bool TypeNameNeedsSpecialTreatmentForBuilderConstructorInitializeExpression(string typeName)
        => Array.Exists(GetCustomBuilderTypes(), x => GetBuilderNamespaceMappings().Any(y => typeName == $"{y.Key}.{x}"));

    private string GetCustomCollectionArgumentType(string typeName)
        => ReplaceWithBuilderNamespaces(typeName).ReplaceSuffix(">", $"{BuilderName}> ", StringComparison.InvariantCulture);

    private bool TypeNameNeedsSpecialTreatmentForBuilderInCollection(string typeName)
        => Array.Exists(GetCustomBuilderTypes(), x => GetBuilderNamespaceMappings().Any(y => typeName == $"{RecordCollectionType.WithoutGenerics()}<{y.Key}.{x}>"));

    private string GetCustomBuilderConstructorInitializeExpressionForSingleProperty(ClassPropertyBuilder property, string typeName)
    {
        var placeholder = AddNullChecks
            ? "_{1}"
            : "{0}";

        if (TypeNameNeedsSpecialTreatmentForBuilderConstructorInitializeExpression(typeName))
        {
            if (UseLazyInitialization)
            {
                return property.IsNullable
                    ? "_{1}Delegate = new (() => source.{0} == null ? null : " + GetBuilderNamespace(typeName) + "." + GetEntityClassName(typeName) + BuilderFactoryName + ".Create(source.{0}))"
                    : "_{1}Delegate = new (() => " + GetBuilderNamespace(typeName) + "." + GetEntityClassName(typeName) + BuilderFactoryName + ".Create(source.{0}))";
            }

            return property.IsNullable
                ? placeholder + " = source.{0} == null ? null : " + GetBuilderNamespace(typeName) + "." + GetEntityClassName(typeName) + BuilderFactoryName + ".Create(source.{0})"
                : placeholder + " = " + GetBuilderNamespace(typeName) + "." + GetEntityClassName(typeName) + BuilderFactoryName + ".Create(source.{0})";
        }

        if (UseLazyInitialization)
        {
            return property.IsNullable
                ? "_{1}Delegate = new (() => source.{0} == null ? null : new " + ReplaceWithBuilderNamespaces(typeName.WithoutProcessedGenerics()).GetNamespaceWithDefault() + ".{10}" + BuilderName + "{9}(source.{0}))"
                : "_{1}Delegate = new (() => new " + ReplaceWithBuilderNamespaces(typeName.WithoutProcessedGenerics()).GetNamespaceWithDefault() + ".{10}" + BuilderName + "{9}(source.{0}))";
        }

        return property.IsNullable
            ? placeholder + " = source.{0} == null ? null : new " + ReplaceWithBuilderNamespaces(typeName.WithoutProcessedGenerics()).GetNamespaceWithDefault() + ".{10}" + BuilderName + "{9}(source.{0})"
            : placeholder + " = new " + ReplaceWithBuilderNamespaces(typeName.WithoutProcessedGenerics()).GetNamespaceWithDefault() + ".{10}" + BuilderName + "{9}(source.{0})";
    }

    private string GetCustomBuilderConstructorInitializeExpressionForCollectionProperty(string typeName)
        => "{0} = source.{0}.Select(x => " + GetBuilderNamespace(typeName.GetGenericArguments()) + "." + GetEntityClassName(typeName.GetGenericArguments()) + BuilderFactoryName + ".Create(x)).ToList()";

    private Literal GetDefaultValueForBuilderClassConstructor(string typeName)
    {
        if (GetCustomDefaultValueForBuilderClassConstructorValues().TryGetValue(typeName, out var customDefaultValue))
        {
            return new(customDefaultValue);
        }

        if (TypeNameNeedsSpecialTreatmentForBuilderConstructorInitializeExpression(typeName))
        {
            var nullableSuffix = EnableNullableContext
                ? "!"
                : string.Empty;

            return new($"default{nullableSuffix}");
        }

        if (!string.IsNullOrEmpty(typeName.GetGenericArguments()))
        {
            return new($"new {ReplaceWithBuilderNamespaces(typeName.WithoutProcessedGenerics())}{BuilderName}<{typeName.GetGenericArguments()}>()");
        }

        return new("new " + ReplaceWithBuilderNamespaces(typeName) + $"{BuilderName}()");
    }

    protected virtual IEnumerable<Type> GetPureAbstractModels()
        => GetType().Assembly.GetTypes()
            .Where(x => x.IsInterface && x.Namespace?.StartsWith($"{CodeGenerationRootNamespace}.Models.") == true && x.GetInterfaces().Count(x => x.Namespace == $"{CodeGenerationRootNamespace}.Models") == 1)
            .Select(x => x.GetInterfaces().First(x => x.Namespace == $"{CodeGenerationRootNamespace}.Models"))
            .Distinct();

    private Literal GetBuilderFactoryModelDefaultValue(
        ITypeBase[] models,
        string builderNamespace,
        string classTypeName,
        string builderTypeName,
        string overrideClassNamespace)
    {
        var builder = new StringBuilder();
        builder.AppendLine($"new Dictionary<{typeof(Type).FullName}, Func<{classTypeName}, {builderTypeName}>>")
               .AppendLine("        {");
        // note that generic types are skipped here. you need to fill createLiteralCodeStatement to handle these ones
        foreach (var modelName in models.Where(x => x.GenericTypeArguments.Count == 0).Select(x => x.Name))
        {
            builder.AppendLine("            { typeof(" + overrideClassNamespace + "." + modelName + "),x => new " + builderNamespace + "." + modelName + BuilderName + "((" + overrideClassNamespace + "." + modelName + ")x) },");
        }
        builder.Append("        }");
        return new Literal(builder.ToString());
    }
}
