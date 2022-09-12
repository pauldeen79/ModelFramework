namespace ModelFramework.CodeGeneration.CodeGenerationProviders;

public abstract class CSharpClassBase : ClassBase
{
    protected virtual IEnumerable<ClassMethodBuilder> CreateExtraOverloads(IClass c)
        => Enumerable.Empty<ClassMethodBuilder>();
    protected virtual Type BuilderClassCollectionType => typeof(List<>);
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
    protected virtual string BaseClassBuilderNamespace => string.Empty;
    protected virtual bool IsMemberValid(IParentTypeContainer parent, ITypeBase typeBase) => true;
    protected virtual AttributeBuilder AttributeInitializeDelegate(Attribute sourceAttribute)
        => new AttributeBuilder().WithName(sourceAttribute.GetType().FullName);
    protected virtual string[] GetCustomBuilderTypes() => Array.Empty<string>();
    protected virtual string[] GetNonDomainTypes() => Array.Empty<string>();
    protected virtual Dictionary<string, string> GetCustomDefaultValueForBuilderClassConstructorValues() => new();
    protected virtual Dictionary<string, string> GetBuilderNamespaceMappings() => new();
    protected virtual Dictionary<string, string> GetModelMappings() => new();

    protected abstract string GetFullBasePath();
    protected abstract string RootNamespace { get; }

    protected abstract Type RecordCollectionType { get; }
    protected abstract Type RecordConcreteCollectionType { get; }
    protected virtual string FormatInstanceTypeName(ITypeBase instance, bool forCreate) => string.Empty;

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
                constructorSettings: new ImmutableClassConstructorSettings(
                    validateArguments: ValidateArgumentsInConstructor,
                    addNullChecks: AddNullChecks),
                addPrivateSetters: AddPrivateSetters)
            )
            .WithNamespace(@namespace)
            .WithName(type.GetEntityClassName())
            .With(x => FixImmutableClassProperties(x))
            .BuildTyped();

    protected ClassBuilder CreateBuilder(ITypeBase typeBase, string @namespace)
        => typeBase.ToImmutableBuilderClassBuilder(CreateImmutableBuilderClassSettings())
            .WithNamespace(@namespace)
            .WithPartial();

    protected ClassBuilder CreateNonGenericBuilder(ITypeBase typeBase, string @namespace)
        => typeBase.ToNonGenericImmutableBuilderClassBuilder(CreateImmutableBuilderClassSettings())
            .WithNamespace(@namespace)
            .WithPartial();

    protected ClassBuilder CreateBuilderExtensions(ITypeBase typeBase, string @namespace)
        => typeBase.ToBuilderExtensionsClassBuilder(CreateImmutableBuilderClassSettings())
            .WithNamespace(@namespace)
            .WithPartial();

    protected virtual bool IsNotScaffolded(ITypeBase x, string classNameSuffix)
        => !File.Exists(System.IO.Path.Combine(GetFullBasePath(), Path, $"{x.Name}{classNameSuffix}.cs"));

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
            var typeName = property.TypeName.FixTypeName();
            if (!property.IsValueType
                && typeName.StartsWithAny(StringComparison.InvariantCulture, GetBuilderNamespaceMappings().Keys.Select(x => $"{x}."))
                && !GetNonDomainTypes().Contains(property.TypeName))
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
            else if (typeName.StartsWithAny(StringComparison.InvariantCulture, GetBuilderNamespaceMappings().Keys.Select(x => $"{RecordCollectionType.WithoutGenerics()}<{x}.")))
            {
                if (TypeNameNeedsSpecialTreatmentForBuilderInCollection(typeName))
                {
                    property.ConvertCollectionPropertyToBuilderOnBuilder
                    (
                        false,
                        RecordConcreteCollectionType.WithoutGenerics(),
                        GetCustomCollectionArgumentType(typeName),
                        GetCustomBuilderConstructorInitializeExpressionForCollectionProperty(typeName)
                    );
                }
                else
                {
                    property.ConvertCollectionPropertyToBuilderOnBuilder
                    (
                        false,
                        RecordConcreteCollectionType.WithoutGenerics(),
                        GetCustomCollectionArgumentType(typeName)
                    );
                }
            }
            else if (typeName.IsBooleanTypeName() || typeName.IsNullableBooleanTypeName())
            {
                property.SetDefaultArgumentValueForWithMethod(true);
            }
        }
    }

    protected ITypeBase[] MapCodeGenerationModelsToDomain(IEnumerable<Type> types)
        => types
            .Select(x => x.ToClassBuilder(new ClassSettings())
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

    protected ImmutableBuilderClassSettings CreateImmutableBuilderClassSettings()
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
                allowGenerationWithoutProperties: AllowGenerationWithoutProperties),
            inheritanceSettings: new(
                enableEntityInheritance: EnableEntityInheritance,
                enableBuilderInheritance: EnableBuilderInhericance,
                removeDuplicateWithMethods: RemoveDuplicateWithMethods,
                baseClass: BaseClass,
                baseClassBuilderNameSpace: BaseClassBuilderNamespace,
                inheritanceComparisonFunction: IsMemberValid)
        );

    protected ImmutableClassSettings CreateImmutableClassSettings()
        => new
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

    protected ClassSettings CreateClassSettings()
        => new
        (
            createConstructors: true,
            attributeInitializeDelegate: AttributeInitializeDelegate
        );

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
        string classNamespace,
        string className,
        string classTypeName,
        string builderNamespace,
        string builderTypeName)
        => new[] { new ClassBuilder()
        .WithName(className)
        .WithNamespace(classNamespace)
        .WithStatic()
        .AddFields(new ClassFieldBuilder()
            .WithName("registeredTypes")
            .WithStatic()
            .WithTypeName($"Dictionary<Type,Func<{classTypeName},{builderTypeName}>>")
            .WithDefaultValue(GetBuilderFactoryModelDefaultValue(models, builderNamespace,classTypeName, builderTypeName))
        )
        .AddMethods(new ClassMethodBuilder()
            .WithName("Create")
            .WithTypeName($"{classNamespace}.{builderTypeName}")
            .WithStatic()
            .AddParameter("instance", classTypeName)
            .AddLiteralCodeStatements("return registeredTypes.ContainsKey(instance.GetType()) ? registeredTypes[instance.GetType()].Invoke(instance) : throw new ArgumentOutOfRangeException(\"Unknown instance type: \" + instance.GetType().FullName);"),
            new ClassMethodBuilder()
            .WithStatic()
            .WithName("Register")
            .AddParameter("type", typeof(Type))
            .AddParameter("createDelegate", $"Func<{classTypeName},{builderTypeName}>")
            .AddLiteralCodeStatements("registeredTypes.Add(type, createDelegate);")
        )
        .Build() };

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
        => "{0} = source.{0}.Select(x => " + GetBuilderNamespace(typeName) + "." + GetEntityClassName(typeName.GetGenericArguments()) + "BuilderFactory.Create(x)).ToList()";

    private Literal GetDefaultValueForBuilderClassConstructor(string typeName)
    {
        if (GetCustomDefaultValueForBuilderClassConstructorValues().ContainsKey(typeName))
        {
            return new(GetCustomDefaultValueForBuilderClassConstructorValues()[typeName]);
        }

        return new("new " + ReplaceWithBuilderNamespaces(typeName) + "Builder()");
    }

    private static object GetBuilderFactoryModelDefaultValue(
        ITypeBase[] models,
        string builderNamespace,
        string classTypeName,
        string builderTypeName)
    {
        var builder = new StringBuilder();
        builder.AppendLine($"new Dictionary<Type, Func<{classTypeName}, {builderTypeName}>>")
               .AppendLine("{");
        foreach (var name in models.Select(x => x.Name))
        {
            builder.AppendLine("    {typeof(" + name + "),x => new " + builderNamespace + "." + name + "Builder((" + name + ")x)},");
        }
        builder.AppendLine("}");
        return new Literal(builder.ToString());
    }

}
