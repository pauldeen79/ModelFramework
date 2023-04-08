namespace ModelFramework.CodeGeneration.CodeGenerationProviders;

public abstract class CSharpClassBase : ClassBase
{
    protected virtual Type BuilderClassCollectionType => typeof(List<>);
    protected virtual string SetMethodNameFormatString => "With{0}";
    protected virtual string AddMethodNameFormatString => "Add{0}";
    protected virtual bool AddNullChecks => false;
    protected virtual bool AddCopyConstructor => true;
    protected virtual bool UseLazyInitialization => true;
    protected virtual bool UseTargetTypeNewExpressions => true;
    protected virtual ArgumentValidationType ValidateArgumentsInConstructor => ArgumentValidationType.Always;
    protected virtual bool InheritFromInterfaces => true;
    protected virtual bool AddPrivateSetters => false;
    protected virtual bool CopyPropertyCode => false;
    protected virtual bool CopyFields => false;
    protected virtual bool EnableEntityInheritance => false;
    protected virtual bool EnableBuilderInhericance => false;
    protected virtual bool RemoveDuplicateWithMethods => true;
    protected virtual bool AllowGenerationWithoutProperties => true;
    protected virtual bool AddBackingFieldsForCollectionProperties => false;
    protected virtual IClass? BaseClass => null;
    protected virtual string BaseClassBuilderNamespace => string.Empty;
    protected virtual bool IsMemberValid(IParentTypeContainer parent, ITypeBase typeBase)
        => parent != null
        && typeBase != null
        && (string.IsNullOrEmpty(parent.ParentTypeFullName)
            || parent.ParentTypeFullName.GetClassName() == $"I{typeBase.Name}"
            || GetModelAbstractBaseTyped().Any(x => x == parent.ParentTypeFullName.GetClassName())
            || (BaseClass != null && $"I{typeBase.Name}" == BaseClass.Name));

    protected virtual AttributeBuilder AttributeInitializeDelegate(Attribute sourceAttribute)
        => new AttributeBuilder().WithName(sourceAttribute.GetType().FullName);
    protected abstract string ProjectName { get; }
    protected virtual string RootNamespace => $"{ProjectName}.Domain";
    protected virtual string CodeGenerationRootNamespace => $"{ProjectName}.CodeGeneration";
    protected abstract Type RecordCollectionType { get; }
    protected abstract Type RecordConcreteCollectionType { get; }

    protected virtual string FormatInstanceTypeName(ITypeBase instance, bool forCreate) => string.Empty;

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
        result.AddRange(GetPureAbstractModels().Select(x => new KeyValuePair<string, string>($"{RootNamespace}.{x.GetEntityClassName()}", "null")));
        return result;
    }

    protected virtual Dictionary<string, string> GetBuilderNamespaceMappings()
    {
        var result = new Dictionary<string, string>();
        result.AddRange(
            GetCustomBuilderTypes()
                .Select(x => new KeyValuePair<string, string>($"{RootNamespace}.{x}s", $"{RootNamespace}.Builders.{x}s"))
                .Concat(new[] { new KeyValuePair<string, string>(RootNamespace, $"{RootNamespace}.Builders") }));
        result.AddRange(GetCustomBuilderNamespaceMapping());
        return result;
    }

    protected virtual Dictionary<string, string> GetModelMappings()
    {
        var result = new Dictionary<string, string>();
        result.Add($"{CodeGenerationRootNamespace}.Models.I", $"{RootNamespace}.");
        result.AddRange(GetPureAbstractModels().Select(x => new KeyValuePair<string, string>($"{CodeGenerationRootNamespace}.Models.{x.GetEntityClassName()}s.I", $"{RootNamespace}.{x.GetEntityClassName()}s.")));
        result.Add($"{CodeGenerationRootNamespace}.Models.Domains.", $"{RootNamespace}.Domains.");
        result.Add($"{CodeGenerationRootNamespace}.I", $"{RootNamespace}.I");
        return result;
    }

    protected virtual string[] GetNonDomainTypes()
        => GetType().Assembly.GetExportedTypes()
            .Where(x => x.IsInterface && x.Namespace == CodeGenerationRootNamespace)
            .Select(x => $"{RootNamespace}.{x.Name}")
            .ToArray();

    protected virtual string[] GetModelAbstractBaseTyped() => Array.Empty<string>();

    protected virtual string[] GetExternalCustomBuilderTypes() => Array.Empty<string>();

    protected virtual IEnumerable<KeyValuePair<string, string>> GetCustomBuilderNamespaceMapping() => Enumerable.Empty<KeyValuePair<string, string>>();

    protected ITypeBase[] GetCoreModels()
        => MapCodeGenerationModelsToDomain(
            GetType().Assembly.GetExportedTypes()
                .Where(x => x.IsInterface && x.Namespace == $"{CodeGenerationRootNamespace}.Models" && !GetCustomBuilderTypes().Contains(x.GetEntityClassName())));

    protected ITypeBase[] GetAbstractModels()
        => MapCodeGenerationModelsToDomain(GetPureAbstractModels());

    protected ITypeBase[] GetOverrideModels(Type abstractType)
        => MapCodeGenerationModelsToDomain(
            GetType().Assembly.GetExportedTypes()
                .Where(x => x.IsInterface && x.GetInterfaces().Any(y => y == abstractType)));

    protected ITypeBase[] GetImmutableBuilderClasses(Type[] types,
                                                     string entitiesNamespace,
                                                     string buildersNamespace,
                                                     params string[] interfacesToAdd)
        => GetImmutableBuilderClasses(types.Select(x => x.ToTypeBase(CreateClassSettings())).ToArray(),
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
        => GetImmutableBuilderExtensionClasses(types.Select(x => x.ToClass()).ToArray(),
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
            : x.ToClassBuilder().With(x => FixImmutableClassProperties(x)).Build()).ToArray(), entitiesNamespace);

    protected ITypeBase[] GetImmutableClasses(ITypeBase[] models, string entitiesNamespace)
    {
        if (ValidateArgumentsInConstructor == ArgumentValidationType.Optional)
        {
            return models.SelectMany
            (
                x => x switch
                {
                    IClass cls => new[] { CreateImmutableClassFromClass(cls, entitiesNamespace), CreateImmutableOverrideClassFromClass(cls, entitiesNamespace)  },
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
                t => t.ToClassBuilder(CreateClassSettings())
                    .WithName(t.Name)
                    .WithNamespace(t.FullName.GetNamespaceWithDefault())
                .With(x => FixImmutableBuilderProperties(x))
                .BuildTyped()
            )
            .ToArray();
    }

    protected IClass CreateBaseclass(Type type, string @namespace)
        => type.ToClass().ToImmutableClassBuilder(new ImmutableClassSettings
            (
                newCollectionTypeName: RecordCollectionType.WithoutGenerics(),
                allowGenerationWithoutProperties: AllowGenerationWithoutProperties,
                constructorSettings: new(
                    validateArguments: ValidateArgumentsInConstructor,
                    addNullChecks: AddNullChecks),
                addPrivateSetters: AddPrivateSetters)
            )
            .WithNamespace(@namespace)
            .WithName(type.GetEntityClassName())
            .With(x => FixImmutableClassProperties(x))
            .BuildTyped();

    protected ClassBuilder CreateBuilder(ITypeBase typeBase, string @namespace)
        => typeBase.ToImmutableBuilderClassBuilder(CreateImmutableBuilderClassSettings(ArgumentValidationType.Never))
            .WithNamespace(@namespace)
            .WithPartial();

    protected ClassBuilder CreateNonGenericBuilder(ITypeBase typeBase, string @namespace)
        => typeBase.ToNonGenericImmutableBuilderClassBuilder(CreateImmutableBuilderClassSettings(ArgumentValidationType.Never))
            .WithNamespace(@namespace)
            .WithPartial();

    protected ClassBuilder CreateBuilderExtensions(ITypeBase typeBase, string @namespace)
        => typeBase.ToBuilderExtensionsClassBuilder(CreateImmutableBuilderClassSettings(ArgumentValidationType.Never))
            .WithNamespace(@namespace)
            .WithPartial();

    protected virtual void FixImmutableClassProperties<TBuilder, TEntity>(TypeBaseBuilder<TBuilder, TEntity> typeBaseBuilder)
        where TEntity : ITypeBase
        where TBuilder : TypeBaseBuilder<TBuilder, TEntity>
        => FixImmutableBuilderProperties(typeBaseBuilder);

    protected virtual void FixImmutableBuilderProperties<TBuilder, TEntity>(TypeBaseBuilder<TBuilder, TEntity> typeBaseBuilder)
        where TEntity : ITypeBase
        where TBuilder : TypeBaseBuilder<TBuilder, TEntity>
    {
        foreach (var property in typeBaseBuilder.Properties)
        {
            var typeName = property.TypeName.ToString().FixTypeName();
            FixImmutableBuilderProperty(property, typeName);

            if (typeName.StartsWith($"{RecordCollectionType.WithoutGenerics()}<", StringComparison.InvariantCulture)
                && AddBackingFieldsForCollectionProperties)
            {
                property.AddCollectionBackingFieldOnImmutableClass(BuilderClassCollectionType);
            }
        }
    }

    protected virtual void FixImmutableBuilderProperty(ClassPropertyBuilder property, string typeName)
    {
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
        else if (typeName.IsStringTypeName())
        {
            property.ConvertStringPropertyToStringBuilderPropertyOnBuilder();
        }
    }

    private void FixCollectionDomainProperty(ClassPropertyBuilder property, string typeName)
    {
        if (TypeNameNeedsSpecialTreatmentForBuilderInCollection(typeName))
        {
            property.ConvertCollectionPropertyToBuilderOnBuilder
            (
                false,
                RecordConcreteCollectionType.WithoutGenerics(),
                GetCustomCollectionArgumentType(typeName),
                GetCustomBuilderConstructorInitializeExpressionForCollectionProperty(typeName),
                builderCollectionTypeName: GetBuilderClassCollectionTypeName()
            );
        }
        else
        {
            property.ConvertCollectionPropertyToBuilderOnBuilder
            (
                false,
                RecordConcreteCollectionType.WithoutGenerics(),
                GetCustomCollectionArgumentType(typeName),
                customBuilderMethodParameterExpression: GetCustomBuilderMethodParameterExpressionForCollectionProperty(typeName),
                builderCollectionTypeName: GetBuilderClassCollectionTypeName()
            );
        }
    }

    private void FixSingleDomainProperty(ClassPropertyBuilder property, string typeName)
    {
        property.ConvertSinglePropertyToBuilderOnBuilder
        (
            $"{GetBuilderNamespace(typeName)}.{typeName.GetClassName()}Builder",
            GetCustomBuilderConstructorInitializeExpressionForSingleProperty(property, typeName),
            GetCustomBuilderMethodParameterExpression(typeName)
        );

        if (!property.IsNullable)
        {
            property.SetDefaultValueForBuilderClassConstructor(GetDefaultValueForBuilderClassConstructor(typeName));
        }
    }

    private string? GetBuilderClassCollectionTypeName()
        => BuilderClassCollectionType == null
            ? null
            : BuilderClassCollectionType.WithoutGenerics();

    private string? GetCustomBuilderMethodParameterExpressionForCollectionProperty(string typeName)
        => !string.IsNullOrEmpty(GetEntityClassName(typeName.GetGenericArguments()))
            ? "{0}.Select(x => x.BuildTyped())"
            : null;

    protected ITypeBase[] MapCodeGenerationModelsToDomain(IEnumerable<Type> types)
        => types
            .Select(x => x.ToClassBuilder(new ClassSettings())
                .AddMetadata("CSharpClassBase.ModelType", x)
                .WithNamespace(RootNamespace)
                .WithName(x.GetEntityClassName())
                .With(y => y.Properties.ForEach(z => GetModelMappings().ToList().ForEach(m => z.TypeName = z.TypeName.Replace(m.Key, m.Value))))
                .Build())
            .ToArray();

    protected string GetBuilderNamespace(string typeName)
        => GetBuilderNamespaceMappings()
            .Where(x => typeName.StartsWith(x.Key + ".", StringComparison.InvariantCulture))
            .Select(x => x.Value)
            .FirstOrDefault() ?? string.Empty;

    protected string ReplaceWithBuilderNamespaces(string typeName)
    {
        var match = GetBuilderNamespaceMappings()
            .Select(x => new { x.Key, x.Value })
            .FirstOrDefault(x => typeName.Contains($"{x.Key}."));

        return match == null
            ? typeName
            : typeName.Replace($"{match.Key}.", $"{match.Value}.");
    }

    /// <summary>
    /// Gets the base typename, based on a derived class.
    /// </summary>
    /// <param name="className">The typename to get the base classname from.</param>
    /// <returns>Base classname when found, otherwise string.Empty</returns>
    protected string GetEntityClassName(string className)
        => GetCustomBuilderTypes().FirstOrDefault(x => className.EndsWith(x, StringComparison.InvariantCulture)) ?? string.Empty;

    protected string GetEntityTypeName(string builderFullName)
    {
        var match = GetBuilderNamespaceMappings()
            .Select(x => new { x.Key, x.Value })
            .FirstOrDefault(x => builderFullName.StartsWith($"{x.Value}.", StringComparison.InvariantCulture));

        return match == null
            ? builderFullName.ReplaceSuffix("Builder", string.Empty, StringComparison.InvariantCulture)
            : builderFullName
                .Replace($"{match.Value}.", $"{match.Key}.")
                .ReplaceSuffix("Builder", string.Empty, StringComparison.InvariantCulture);
    }

    protected string? GetCustomBuilderMethodParameterExpression(string typeName)
        => string.IsNullOrEmpty(GetEntityClassName(typeName)) || GetCustomBuilderTypes().Contains(typeName.GetClassName())
            ? string.Empty
            : "{0}{2}.BuildTyped()";

    protected ImmutableBuilderClassSettings CreateImmutableBuilderClassSettings(ArgumentValidationType? forceValidateArgumentsInConstructor = null)
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
                addMethodNameFormatString: AddMethodNameFormatString),
            generationSettings: new(
                useLazyInitialization: UseLazyInitialization,
                copyPropertyCode: CopyPropertyCode,
                copyFields: CopyFields,
                allowGenerationWithoutProperties: AllowGenerationWithoutProperties),
            inheritanceSettings: new(
                enableEntityInheritance: EnableEntityInheritance,
                enableBuilderInheritance: EnableBuilderInhericance,
                removeDuplicateWithMethods: RemoveDuplicateWithMethods,
                baseClass: BaseClass,
                baseClassBuilderNameSpace: BaseClassBuilderNamespace,
                inheritanceComparisonFunction: EnableBuilderInhericance
                    ? IsMemberValid
                    : (_, _) => true),
            classSettings: CreateImmutableClassSettings(forceValidateArgumentsInConstructor ?? ArgumentValidationType.Never)
        );

    protected ImmutableClassSettings CreateImmutableClassSettings(ArgumentValidationType? forceValidateArgumentsInConstructor = null)
        => new
        (
            newCollectionTypeName: RecordCollectionType.WithoutGenerics(),
            allowGenerationWithoutProperties: AllowGenerationWithoutProperties,
            constructorSettings: new(
                validateArguments: forceValidateArgumentsInConstructor ?? CombineValidateArguments(ValidateArgumentsInConstructor, !(EnableEntityInheritance && BaseClass == null)),
                originalValidateArguments: ValidateArgumentsInConstructor,
                addNullChecks: AddNullChecks),
            addPrivateSetters: AddPrivateSetters,
            inheritanceSettings: new(
                enableInheritance: EnableEntityInheritance,
                baseClass: BaseClass,
                inheritanceComparisonFunction: EnableEntityInheritance
                    ? IsMemberValid
                    : (_, _) => true)
        );

    protected ArgumentValidationType CombineValidateArguments(ArgumentValidationType validateArgumentsInConstructor, bool secondCondition)
        => secondCondition
            ? validateArgumentsInConstructor
            : ArgumentValidationType.Never;

    protected ClassSettings CreateClassSettings()
        => new
        (
            createConstructors: true,
            attributeInitializeDelegate: AttributeInitializeDelegate
        );

    protected string GetModelTypeName(Type modelType)
        => GetCoreModels().Concat(GetAbstractModels())
            .FirstOrDefault(x => x.Metadata.GetValue<Type?>("CSharpClassBase.ModelType", () => null) == modelType)?.GetFullName()
                ?? throw new ArgumentOutOfRangeException(nameof(modelType), $"Unknown model type: {modelType.FullName}");

    protected virtual string CurrentNamespace => Path.Replace("/", ".");

    protected static object CreateServiceCollectionExtensions(
    string @namespace,
        string className,
    string methodName,
    ITypeBase[] types,
    Func<ITypeBase, string> formatDelegate)
        => new[] { new ClassBuilder()
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
                .AddLiteralCodeStatements(types.Select(x => formatDelegate.Invoke(x)))
                .AddLiteralCodeStatements(";")
            )
            .Build() };

    protected static ITypeBase[] CreateBuilderFactoryModels(
    ITypeBase[] models,
        BuilderFactoryNamespaceSettings namespaceSettings,
        string? createLiteralCodeStatement = null)
        => new[]
        {
            new ClassBuilder()
                .WithName(namespaceSettings.ClassName)
                .WithNamespace(namespaceSettings.ClassNamespace)
                .WithStatic()
                .AddFields(new ClassFieldBuilder()
                    .WithName("registeredTypes")
                    .WithStatic()
                    .WithTypeName($"Dictionary<Type,Func<{namespaceSettings.ClassTypeName},{namespaceSettings.BuilderTypeName}>>")
                    .WithDefaultValue(GetBuilderFactoryModelDefaultValue(models, namespaceSettings.BuilderNamespace, namespaceSettings.ClassTypeName, namespaceSettings.BuilderTypeName, namespaceSettings.OverrideClassNamespace))
                )
                .AddMethods(new ClassMethodBuilder()
                    .WithName("Create")
                    .WithTypeName($"{namespaceSettings.ClassNamespace}.{namespaceSettings.BuilderTypeName}")
                    .WithStatic()
                    .AddParameter("instance", namespaceSettings.ClassTypeName)
                    .Chain(x =>
                    {
                        if (createLiteralCodeStatement != null && createLiteralCodeStatement.Length > 0)
                        {
                            x.AddLiteralCodeStatements(createLiteralCodeStatement);
                        }
                    })
                    .AddLiteralCodeStatements("return registeredTypes.ContainsKey(instance.GetType()) ? registeredTypes[instance.GetType()].Invoke(instance) : throw new ArgumentOutOfRangeException(\"Unknown instance type: \" + instance.GetType().FullName);"),
                    new ClassMethodBuilder()
                    .WithStatic()
                    .WithName("Register")
                    .AddParameter("type", typeof(Type))
                    .AddParameter("createDelegate", $"Func<{namespaceSettings.ClassTypeName},{namespaceSettings.BuilderTypeName}>")
                    .AddLiteralCodeStatements("registeredTypes.Add(type, createDelegate);")
                )
                .Build()
        };

    private IClass CreateImmutableEntity(string entitiesNamespace, ITypeBase typeBase)
        => new ClassBuilder(typeBase.ToClass())
            .WithName(typeBase.GetEntityClassName())
            .WithNamespace(entitiesNamespace)
            .With(x => FixImmutableBuilderProperties(x))
            .Build()
            .ToImmutableClassBuilder(CreateImmutableClassSettings(ArgumentValidationType.Never))
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
            .AddInterfaces((new[] { iinterface.GetFullName() }).Where(_ => InheritFromInterfaces))
            .Build();

    private ITypeBase CreateImmutableOverrideClassFromInterface(IInterface iinterface, string entitiesNamespace)
        => new InterfaceBuilder(iinterface)
            .WithName(iinterface.GetEntityClassName())
            .WithNamespace(entitiesNamespace)
            .With(x => FixImmutableClassProperties(x))
            .Build()
            .ToImmutableClassValidateOverrideBuilder(CreateImmutableClassSettings())
            .WithRecord()
            .WithPartial()
            .AddInterfaces((new[] { iinterface.GetFullName() }).Where(_ => InheritFromInterfaces))
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

    private ITypeBase CreateImmutableOverrideClassFromClass(IClass cls, string entitiesNamespace)
        => new ClassBuilder(cls)
            .WithNamespace(entitiesNamespace)
            .With(x => FixImmutableClassProperties(x))
            .Build()
            .ToImmutableClassValidateOverrideBuilder(CreateImmutableClassSettings(ValidateArgumentsInConstructor))
            .WithRecord()
            .WithPartial()
            .Build();

    private bool TypeNameNeedsSpecialTreatmentForBuilderConstructorInitializeExpression(string typeName)
        => GetCustomBuilderTypes().Any(x => GetBuilderNamespaceMappings().Any(y => typeName == $"{y.Key}.{x}"));

    private string GetCustomCollectionArgumentType(string typeName)
        => ReplaceWithBuilderNamespaces(typeName).ReplaceSuffix(">", "Builder>", StringComparison.InvariantCulture);

    private bool TypeNameNeedsSpecialTreatmentForBuilderInCollection(string typeName)
        => GetCustomBuilderTypes().Any(x => GetBuilderNamespaceMappings().Any(y => typeName == $"{RecordCollectionType.WithoutGenerics()}<{y.Key}.{x}>"));

    private string GetCustomBuilderConstructorInitializeExpressionForSingleProperty(ClassPropertyBuilder property, string typeName)
    {
        if (TypeNameNeedsSpecialTreatmentForBuilderConstructorInitializeExpression(typeName))
        {
            return property.IsNullable
                ? "_{1}Delegate = new (() => source.{0} == null ? null : " + GetBuilderNamespace(typeName) + "." + GetEntityClassName(typeName) + "BuilderFactory.Create(source.{0}))"
                : "_{1}Delegate = new (() => " + GetBuilderNamespace(typeName) + "." + GetEntityClassName(typeName) + "BuilderFactory.Create(source.{0}))";
        }

        return property.IsNullable
            ? "_{1}Delegate = new (() => source.{0} == null ? null : new " + ReplaceWithBuilderNamespaces(typeName).GetNamespaceWithDefault() + ".{5}Builder(source.{0}))"
            : "_{1}Delegate = new (() => new " + ReplaceWithBuilderNamespaces(typeName).GetNamespaceWithDefault() + ".{5}Builder(source.{0}))";
    }

    private string GetCustomBuilderConstructorInitializeExpressionForCollectionProperty(string typeName)
        => "{0} = source.{0}.Select(x => " + GetBuilderNamespace(typeName.GetGenericArguments()) + "." + GetEntityClassName(typeName.GetGenericArguments()) + "BuilderFactory.Create(x)).ToList()";

    private Literal GetDefaultValueForBuilderClassConstructor(string typeName)
    {
        if (GetCustomDefaultValueForBuilderClassConstructorValues().ContainsKey(typeName))
        {
            return new(GetCustomDefaultValueForBuilderClassConstructorValues()[typeName]);
        }

        if (TypeNameNeedsSpecialTreatmentForBuilderConstructorInitializeExpression(typeName))
        {
            return new("default");
        }
        
        return new("new " + ReplaceWithBuilderNamespaces(typeName) + "Builder()");
    }

    private IEnumerable<Type> GetPureAbstractModels()
        => GetType().Assembly.GetExportedTypes()
            .Where(x => x.IsInterface && x.Namespace.StartsWith($"{CodeGenerationRootNamespace}.Models.") && x.GetInterfaces().Length == 1)
            .Select(x => x.GetInterfaces()[0])
            .Distinct();

    private static object GetBuilderFactoryModelDefaultValue(
        ITypeBase[] models,
        string builderNamespace,
        string classTypeName,
        string builderTypeName,
        string overrideClassNamespace)
    {
        var builder = new StringBuilder();
        builder.AppendLine($"new Dictionary<Type, Func<{classTypeName}, {builderTypeName}>>")
               .AppendLine("{");
        // note that generic types are skipped here. you need to fill createLiteralCodeStatement to handle these ones
        foreach (var modelName in models.Where(x => !x.GenericTypeArguments.Any()).Select(x => x.Name))
        {
            builder.AppendLine("    {typeof(" + overrideClassNamespace + "." + modelName + "),x => new " + builderNamespace + "." + modelName + "Builder((" + overrideClassNamespace + "." + modelName + ")x)},");
        }
        builder.AppendLine("}");
        return new Literal(builder.ToString());
    }
}
