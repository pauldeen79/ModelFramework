namespace ClassFramework.Pipelines.Tests;

public abstract class TestBase : IDisposable
{
    protected IFixture Fixture { get; }

    protected TestBase()
    {
        Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

        Fixture.Customizations.Add(new BuilderOmitter());
    }

    protected ServiceProvider? Provider { get; set; }
    protected IServiceScope? Scope { get; set; }
    private IFormattableStringParser? _formattableStringParser;
    private bool disposedValue;

    private IFormattableStringParser FormattableStringParser
    {
        get
        {
            if (_formattableStringParser is null)
            {
                Provider = new ServiceCollection()
                    .AddParsers()
                    .AddPipelines()
                    .AddCsharpExpressionCreator()
                    .BuildServiceProvider();
                Scope = Provider.CreateScope();
                _formattableStringParser = Scope.ServiceProvider.GetRequiredService<IFormattableStringParser>();
            }

            return _formattableStringParser;
        }
    }

    protected IFormattableStringParser InitializeParser()
    {
        var parser = Fixture.Freeze<IFormattableStringParser>();
        var csharpExpressionCreator = Fixture.Freeze<ICsharpExpressionCreator>();
        csharpExpressionCreator.Create(Arg.Any<object?>()).Returns(x => x.ArgAt<object?>(0).ToStringWithNullCheck());
        
        // Pass through real IFormattableStringParser implementation, with all placeholder processors and stuff in our ClassFramework.Pipelines project.
        // One exception: If we supply "{Error}" as placeholder, then simply return an error with the error message "Kaboom".
        parser.Parse(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>())
              .Returns(x => x.ArgAt<string>(0) == "{Error}"
                ? Result.Error<string>("Kaboom")
                : FormattableStringParser.Parse(x.ArgAt<string>(0), x.ArgAt<IFormatProvider>(1), x.ArgAt<object?>(2))
                    .Transform(x => x.ErrorMessage == "Unknown placeholder in value: Error"
                        ? Result.Error<string>("Kaboom")
                        : x ));

        return parser;
    }

    protected static IConcreteType CreateModel(string baseClass = "", params MetadataBuilder[] propertyMetadataBuilders)
        => new ClassBuilder()
            .WithName("SomeClass")
            .WithNamespace("SomeNamespace")
            .WithBaseClass(baseClass)
            .AddProperties(new PropertyBuilder().WithName("Property1").WithType(typeof(int)).AddMetadata(propertyMetadataBuilders).AddAttributes(new AttributeBuilder().WithName("MyAttribute")))
            .AddProperties(new PropertyBuilder().WithName("Property2").WithType(typeof(string)).AddMetadata(propertyMetadataBuilders).AddAttributes(new AttributeBuilder().WithName("MyAttribute")))
            .AddProperties(new PropertyBuilder().WithName("Property3").WithType(typeof(List<int>)).AddMetadata(propertyMetadataBuilders).AddAttributes(new AttributeBuilder().WithName("MyAttribute")))
            .AddMetadata(new MetadataBuilder().WithName("MyMetadataName").WithValue("MyMetadataValue"))
            .BuildTyped();

    protected static Domain.Types.Interface CreateInterfaceModel(bool addProperties)
        => new InterfaceBuilder()
            .WithName("IMyClass")
            .WithNamespace("MyNamespace")
            .AddAttributes(new AttributeBuilder().WithName("MyAttribute"))
            .AddProperties(
                new[]
                {
                    new PropertyBuilder().WithName("Property1").WithType(typeof(string)).WithHasSetter(false),
                    new PropertyBuilder().WithName("Property2").WithTypeName(typeof(List<>).ReplaceGenericTypeName(typeof(string))).WithHasSetter(true)
                }.Where(_ => addProperties)
            )
            .BuildTyped();

    protected static IConcreteType CreateGenericModel(bool addProperties)
        => new ClassBuilder()
            .WithName("MyClass")
            .WithNamespace("MyNamespace")
            .AddGenericTypeArguments("T")
            .AddGenericTypeArgumentConstraints("where T : class")
            .AddAttributes(new AttributeBuilder().WithName("MyAttribute"))
            .AddProperties(
                new[]
                {
                    new PropertyBuilder().WithName("Property1").WithType(typeof(string)).WithHasSetter(false),
                    new PropertyBuilder().WithName("Property2").WithTypeName(typeof(List<>).ReplaceGenericTypeName(typeof(string))).WithHasSetter(true)
                }.Where(_ => addProperties)
            )
            .BuildTyped();

    protected static IConcreteType CreateModelWithCustomTypeProperties()
        => new ClassBuilder()
            .WithName("MyClass")
            .WithNamespace("MySourceNamespace")
            .AddProperties(new PropertyBuilder().WithName("Property1").WithType(typeof(int)))
            .AddProperties(new PropertyBuilder().WithName("Property2").WithType(typeof(int)).WithIsNullable())
            .AddProperties(new PropertyBuilder().WithName("Property3").WithType(typeof(string)))
            .AddProperties(new PropertyBuilder().WithName("Property4").WithType(typeof(string)).WithIsNullable())
            .AddProperties(new PropertyBuilder().WithName("Property5").WithTypeName("MySourceNamespace.MyClass"))
            .AddProperties(new PropertyBuilder().WithName("Property6").WithTypeName("MySourceNamespace.MyClass").WithIsNullable())
            .AddProperties(new PropertyBuilder().WithName("Property7").WithTypeName(typeof(List<>).ReplaceGenericTypeName("MySourceNamespace.MyClass")))
            .AddProperties(new PropertyBuilder().WithName("Property8").WithTypeName(typeof(List<>).ReplaceGenericTypeName("MySourceNamespace.MyClass")).WithIsNullable())
            .BuildTyped();

    protected static Domain.Types.Interface CreateInterfaceModelWithCustomTypeProperties()
        => new InterfaceBuilder()
            .WithName("IMyClass")
            .WithNamespace("MySourceNamespace")
            .AddProperties(new PropertyBuilder().WithName("Property1").WithType(typeof(int)))
            .AddProperties(new PropertyBuilder().WithName("Property2").WithType(typeof(int)).WithIsNullable())
            .AddProperties(new PropertyBuilder().WithName("Property3").WithType(typeof(string)))
            .AddProperties(new PropertyBuilder().WithName("Property4").WithType(typeof(string)).WithIsNullable())
            .AddProperties(new PropertyBuilder().WithName("Property5").WithTypeName("MySourceNamespace.IMyClass"))
            .AddProperties(new PropertyBuilder().WithName("Property6").WithTypeName("MySourceNamespace.IMyClass").WithIsNullable())
            .AddProperties(new PropertyBuilder().WithName("Property7").WithTypeName(typeof(List<>).ReplaceGenericTypeName("MySourceNamespace.IMyClass")))
            .AddProperties(new PropertyBuilder().WithName("Property8").WithTypeName(typeof(List<>).ReplaceGenericTypeName("MySourceNamespace.IMyClass")).WithIsNullable())
            .BuildTyped();

    protected static IConcreteType CreateModelWithPropertyThatHasAReservedName(Type propertyType)
        => new ClassBuilder()
            .WithName("SomeClass")
            .WithNamespace("SomeNamespace")
            .AddProperties(new PropertyBuilder().WithName("Delegate").WithType(propertyType))
            .BuildTyped();

    protected static Pipelines.Builder.PipelineSettings CreateBuilderSettings(
        bool enableBuilderInheritance = false,
        bool isAbstract = false,
        bool enableEntityInheritance = false,
        bool addNullChecks = false,
        bool enableNullableReferenceTypes = false,
        bool useExceptionThrowIfNull = false,
        bool copyAttributes = false,
        bool copyInterfaces = false,
        bool addCopyConstructor = false,
        bool setDefaultValues = true,
        string newCollectionTypeName = "System.Collections.Generic.List",
        IEnumerable<NamespaceMapping>? namespaceMappings = null,
        IEnumerable<TypenameMapping>? typenameMappings = null,
        string setMethodNameFormatString = "With{Name}",
        string addMethodNameFormatString = "Add{Name}",
        string builderNamespaceFormatString = "{Namespace}.Builders",
        string builderNameFormatString = "{Class.Name}Builder",
        string buildMethodName = "Build",
        string buildTypedMethodName = "BuildTyped",
        string setDefaultValuesMethodName = "SetDefaultValues",
        ArgumentValidationType validateArguments = ArgumentValidationType.None,
        string? baseClassBuilderNameSpace = null,
        bool allowGenerationWithoutProperties = false,
        Class? baseClass = null,
        Func<IParentTypeContainer, IType, bool>? inheritanceComparisonDelegate = null,
        Predicate<Domain.Attribute>? copyAttributePredicate = null,
        Predicate<string>? copyInterfacePredicate = null)
        => new Pipelines.Builder.PipelineSettings(
            typeSettings: new Pipelines.Builder.PipelineTypeSettings(
                newCollectionTypeName: newCollectionTypeName,
                enableNullableReferenceTypes: enableNullableReferenceTypes,
                namespaceMappings: namespaceMappings,
                typenameMappings: typenameMappings),
            constructorSettings: new Pipelines.Builder.PipelineConstructorSettings(addCopyConstructor, setDefaultValues),
            inheritanceSettings: new Pipelines.Builder.PipelineInheritanceSettings(
                enableBuilderInheritance: enableBuilderInheritance,
                isAbstract: isAbstract,
                baseClass: baseClass,
                baseClassBuilderNameSpace: baseClassBuilderNameSpace,
                inheritanceComparisonDelegate: inheritanceComparisonDelegate),
            entitySettings: CreateEntitySettings
            (
                enableEntityInheritance: enableEntityInheritance,
                addNullChecks: addNullChecks,
                useExceptionThrowIfNull: useExceptionThrowIfNull,
                enableNullableReferenceTypes: enableNullableReferenceTypes,
                validateArguments: validateArguments,
                allowGenerationWithoutProperties: allowGenerationWithoutProperties,
                copyAttributes: copyAttributes,
                copyInterfaces: copyInterfaces,
                copyAttributePredicate: copyAttributePredicate,
                copyInterfacePredicate: copyInterfacePredicate
            ),
            nameSettings: new Pipelines.Builder.PipelineNameSettings(setMethodNameFormatString, addMethodNameFormatString, builderNamespaceFormatString, builderNameFormatString, buildMethodName, buildTypedMethodName, setDefaultValuesMethodName)
        );

    protected static Pipelines.Entity.PipelineSettings CreateEntitySettings(
        bool enableEntityInheritance = false,
        bool addNullChecks = false,
        bool useExceptionThrowIfNull = false,
        bool enableNullableReferenceTypes = false,
        bool copyAttributes = false,
        bool copyInterfaces = false,
        ArgumentValidationType validateArguments = ArgumentValidationType.None,
        bool allowGenerationWithoutProperties = false,
        bool isAbstract = false,
        Class? baseClass = null,
        string entityNamespaceFormatString = "{Namespace}",
        string entityNameFormatString = "{Class.Name}",
        string toBuilderFormatString = "ToBuilder",
        string toTypedBuilderFormatString = "ToTypedBuilder",
        string newCollectionTypeName = "System.Collections.Generic.IReadOnlyCollection",
        string collectionTypeName = "",
        bool addSetters = false,
        bool createRecord = false,
        bool addBackingFields = false,
        bool createAsObservable = false,
        SubVisibility setterVisibility = SubVisibility.InheritFromParent,
        IEnumerable<NamespaceMapping>? namespaceMappings = null,
        IEnumerable<TypenameMapping>? typenameMappings = null,
        Predicate<Domain.Attribute>? copyAttributePredicate = null,
        Predicate<string>? copyInterfacePredicate = null)
        => new Pipelines.Entity.PipelineSettings(
            generationSettings: new Pipelines.Entity.PipelineGenerationSettings(
                allowGenerationWithoutProperties: allowGenerationWithoutProperties,
                addSetters: addSetters,
                setterVisibility: setterVisibility,
                createRecord: createRecord,
                addBackingFields: addBackingFields,
                createAsObservable: createAsObservable),
            nullCheckSettings: new Pipelines.Shared.PipelineBuilderNullCheckSettings(
                addNullChecks: addNullChecks,
                useExceptionThrowIfNull: useExceptionThrowIfNull),
            inheritanceSettings: new Pipelines.Entity.PipelineInheritanceSettings(
                enableInheritance: enableEntityInheritance,
                isAbstract: isAbstract,
                baseClass: baseClass),
            constructorSettings: new Pipelines.Entity.PipelineConstructorSettings(validateArguments: validateArguments, collectionTypeName: collectionTypeName),
            nameSettings: new Pipelines.Entity.PipelineNameSettings(entityNamespaceFormatString, entityNameFormatString, toBuilderFormatString, toTypedBuilderFormatString),
            typeSettings: new Pipelines.Entity.PipelineTypeSettings(
                newCollectionTypeName,
                enableNullableReferenceTypes,
                namespaceMappings,
                typenameMappings),
            copySettings: new Pipelines.Shared.PipelineBuilderCopySettings(
                copyAttributes: copyAttributes,
                copyInterfaces: copyInterfaces,
                copyAttributePredicate: copyAttributePredicate,
                copyInterfacePredicate: copyInterfacePredicate)
        );

    protected static Pipelines.OverrideEntity.PipelineSettings CreateOverrideEntitySettings(
        bool enableEntityInheritance = false,
        bool addNullChecks = false,
        bool useExceptionThrowIfNull = false,
        bool enableNullableReferenceTypes = false,
        bool allowGenerationWithoutProperties = false,
        bool isAbstract = false,
        Class? baseClass = null,
        string entityNamespaceFormatString = "{Namespace}",
        string entityNameFormatString = "{Class.Name}",
        string newCollectionTypeName = "System.Collections.Generic.IReadOnlyCollection",
        bool createRecord = false,
        IEnumerable<NamespaceMapping>? namespaceMappings = null,
        IEnumerable<TypenameMapping>? typenameMappings = null)
        => new Pipelines.OverrideEntity.PipelineSettings(
            generationSettings: new Pipelines.OverrideEntity.PipelineGenerationSettings(
                allowGenerationWithoutProperties: allowGenerationWithoutProperties,
                createRecord: createRecord),
            nullCheckSettings: new Pipelines.Shared.PipelineBuilderNullCheckSettings(
                addNullChecks: addNullChecks,
                useExceptionThrowIfNull: useExceptionThrowIfNull),
            inheritanceSettings: new Pipelines.OverrideEntity.PipelineInheritanceSettings(
                enableInheritance: enableEntityInheritance,
                isAbstract: isAbstract,
                baseClass: baseClass),
            nameSettings: new Pipelines.OverrideEntity.PipelineNameSettings(entityNamespaceFormatString, entityNameFormatString),
            typeSettings: new Pipelines.OverrideEntity.PipelineTypeSettings(
                newCollectionTypeName,
                enableNullableReferenceTypes,
                namespaceMappings,
                typenameMappings)
        );

    protected static Pipelines.Reflection.PipelineSettings CreateReflectionSettings(
        bool copyAttributes = false,
        bool copyInterfaces = false,
        bool allowGenerationWithoutProperties = false,
        bool useBaseClassFromSourceModel = true,
        bool partial = true,
        bool createConstructors = true,
        bool enableEntityInheritance = false,
        bool isAbstract = false,
        string namespaceFormatString = "{Namespace}",
        string nameFormatString = "{Class.Name}",
        Class? baseClass = null,
        IEnumerable<NamespaceMapping>? namespaceMappings = null,
        IEnumerable<TypenameMapping>? typenameMappings = null,
        Predicate<Domain.Attribute>? copyAttributePredicate = null,
        Predicate<string>? copyInterfacePredicate = null)
        => new Pipelines.Reflection.PipelineSettings(
            generationSettings: new Pipelines.Reflection.PipelineGenerationSettings(
                allowGenerationWithoutProperties: allowGenerationWithoutProperties,
                useBaseClassFromSourceModel: useBaseClassFromSourceModel,
                partial: partial,
                createConstructors: createConstructors),
            nameSettings: new Pipelines.Reflection.PipelineNameSettings(namespaceFormatString, nameFormatString),
            typeSettings: new Pipelines.Reflection.PipelineTypeSettings(
                namespaceMappings,
                typenameMappings),
            inheritanceSettings: new Pipelines.Reflection.PipelineInheritanceSettings(
                enableInheritance: enableEntityInheritance,
                isAbstract: isAbstract,
                baseClass: baseClass),
            copySettings: new Pipelines.Shared.PipelineBuilderCopySettings(
                copyAttributes: copyAttributes,
                copyInterfaces: copyInterfaces,
                copyAttributePredicate: copyAttributePredicate,
                copyInterfacePredicate: copyInterfacePredicate)
        );

    protected static Pipelines.Interface.PipelineSettings CreateInterfaceSettings(
        bool addSetters = false,
        bool copyAttributes = false,
        bool copyInterfaces = false,
        bool allowGenerationWithoutProperties = false,
        bool enableEntityInheritance = false,
        bool isAbstract = false,
        string namespaceFormatString = "{Namespace}",
        string nameFormatString = "{Class.Name}",
        string newCollectionTypeName = "System.Collections.Generic.IReadOnlyCollection",
        Class? baseClass = null,
        IEnumerable<NamespaceMapping>? namespaceMappings = null,
        IEnumerable<TypenameMapping>? typenameMappings = null,
        Predicate<Domain.Attribute>? copyAttributePredicate = null,
        Predicate<string>? copyInterfacePredicate = null)
        => new Pipelines.Interface.PipelineSettings(
            generationSettings: new Pipelines.Interface.PipelineGenerationSettings(
                addSetters: addSetters,
                allowGenerationWithoutProperties: allowGenerationWithoutProperties),
            nameSettings: new Pipelines.Interface.PipelineNameSettings(
                namespaceFormatString,
                nameFormatString),
            inheritanceSettings: new Pipelines.Interface.PipelineInheritanceSettings(
                enableInheritance: enableEntityInheritance,
                isAbstract: isAbstract,
                baseClass: baseClass),
            typeSettings: new Pipelines.Interface.PipelineTypeSettings(
                newCollectionTypeName,
                namespaceMappings,
                typenameMappings),
            copySettings: new Pipelines.Shared.PipelineBuilderCopySettings(
                copyAttributes: copyAttributes,
                copyInterfaces: copyInterfaces,
                copyAttributePredicate: copyAttributePredicate,
                copyInterfacePredicate: copyInterfacePredicate)
        );
    protected static IEnumerable<NamespaceMapping> CreateNamespaceMappings(string sourceNamespace = "MySourceNamespace")
        => new[]
        {
            new NamespaceMappingBuilder().WithSourceNamespace(sourceNamespace).WithTargetNamespace("MyNamespace")
                .AddMetadata(new MetadataBuilder().WithName(MetadataNames.CustomBuilderNamespace).WithValue("MyNamespace.Builders"))
                .AddMetadata(new MetadataBuilder().WithName(MetadataNames.CustomBuilderName).WithValue("{TypeName.ClassName}Builder"))
                .AddMetadata(new MetadataBuilder().WithName(MetadataNames.CustomEntityNamespace).WithValue("MyNamespace"))
                .AddMetadata(new MetadataBuilder().WithName(MetadataNames.CustomBuilderSourceExpression).WithValue("[Name][NullableSuffix].ToBuilder()"))
                .AddMetadata(new MetadataBuilder().WithName(MetadataNames.CustomBuilderMethodParameterExpression).WithValue("[Name][NullableSuffix].Build()"))
        }.Select(x => x.Build());

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                Scope?.Dispose();
                Provider?.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

public abstract class TestBase<T> : TestBase
{
    protected T CreateSut() => Fixture.Create<T>();
}

internal sealed class BuilderOmitter : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        var propInfo = request as System.Reflection.PropertyInfo;
        if (propInfo is not null && propInfo.DeclaringType?.Namespace?.StartsWith("ClassFramework.Domain.Builders", StringComparison.Ordinal) == true)
        {
            return new OmitSpecimen();
        }

        return new NoSpecimen();
    }
}
