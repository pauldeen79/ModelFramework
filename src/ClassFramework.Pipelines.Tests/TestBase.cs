namespace ClassFramework.Pipelines.Tests;

public abstract class TestBase : IDisposable
{
    protected IFixture Fixture { get; } = new Fixture().Customize(new AutoNSubstituteCustomization());

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

    protected static TypeBase CreateModel(string baseClass = "", params MetadataBuilder[] propertyMetadataBuilders)
        => new ClassBuilder()
            .WithName("SomeClass")
            .WithNamespace("SomeNamespace")
            .WithBaseClass(baseClass)
            .AddProperties(new ClassPropertyBuilder().WithName("Property1").WithType(typeof(int)).AddMetadata(propertyMetadataBuilders).AddAttributes(new AttributeBuilder().WithName("MyAttribute")))
            .AddProperties(new ClassPropertyBuilder().WithName("Property2").WithType(typeof(string)).AddMetadata(propertyMetadataBuilders).AddAttributes(new AttributeBuilder().WithName("MyAttribute")))
            .AddProperties(new ClassPropertyBuilder().WithName("Property3").WithType(typeof(List<int>)).AddMetadata(propertyMetadataBuilders).AddAttributes(new AttributeBuilder().WithName("MyAttribute")))
            .AddMetadata(new MetadataBuilder().WithName("MyMetadataName").WithValue("MyMetadataValue"))
            .Build();

    protected static TypeBase CreateGenericModel(bool addProperties)
        => new ClassBuilder()
            .WithName("MyClass")
            .WithNamespace("MyNamespace")
            .AddGenericTypeArguments("T")
            .AddGenericTypeArgumentConstraints("where T : class")
            .AddAttributes(new AttributeBuilder().WithName("MyAttribute"))
            .AddProperties(
                new[]
                {
                    new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)).WithHasSetter(false),
                    new ClassPropertyBuilder().WithName("Property2").WithTypeName(typeof(List<>).ReplaceGenericTypeName(typeof(string))).WithHasSetter(true)
                }.Where(_ => addProperties)
            )
            .Build();

    protected static TypeBase CreateModelWithCustomTypeProperties()
        => new ClassBuilder()
            .WithName("SomeClass")
            .WithNamespace("MySourceNamespace")
            .AddProperties(new ClassPropertyBuilder().WithName("Property1").WithType(typeof(int)))
            .AddProperties(new ClassPropertyBuilder().WithName("Property2").WithType(typeof(int)).WithIsNullable())
            .AddProperties(new ClassPropertyBuilder().WithName("Property3").WithType(typeof(string)))
            .AddProperties(new ClassPropertyBuilder().WithName("Property4").WithType(typeof(string)).WithIsNullable())
            .AddProperties(new ClassPropertyBuilder().WithName("Property5").WithTypeName("MySourceNamespace.MyClass"))
            .AddProperties(new ClassPropertyBuilder().WithName("Property6").WithTypeName("MySourceNamespace.MyClass").WithIsNullable())
            .AddProperties(new ClassPropertyBuilder().WithName("Property7").WithTypeName(typeof(List<>).ReplaceGenericTypeName("MySourceNamespace.MyClass")))
            .AddProperties(new ClassPropertyBuilder().WithName("Property8").WithTypeName(typeof(List<>).ReplaceGenericTypeName("MySourceNamespace.MyClass")).WithIsNullable())
            .Build();

    protected static Pipelines.Builder.PipelineBuilderSettings CreateBuilderSettings(
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
        ArgumentValidationType validateArguments = ArgumentValidationType.None,
        string? baseClassBuilderNameSpace = null,
        bool allowGenerationWithoutProperties = false,
        Class? baseClass = null,
        Func<IParentTypeContainer, TypeBase, bool>? inheritanceComparisonDelegate = null,
        Predicate<Domain.Attribute>? copyAttributePredicate = null,
        Predicate<string>? copyInterfacePredicate = null)
        => new Pipelines.Builder.PipelineBuilderSettings(
            typeSettings: new Pipelines.Builder.PipelineBuilderTypeSettings(newCollectionTypeName: newCollectionTypeName, namespaceMappings, typenameMappings),
            constructorSettings: new Pipelines.Builder.PipelineBuilderConstructorSettings(addCopyConstructor, setDefaultValues),
            generationSettings: new Pipelines.Builder.PipelineBuilderGenerationSettings(addNullChecks: addNullChecks, enableNullableReferenceTypes: enableNullableReferenceTypes, copyAttributes: copyAttributes, copyInterfaces: copyInterfaces, copyAttributePredicate: copyAttributePredicate, copyInterfacePredicate: copyInterfacePredicate, useExceptionThrowIfNull: useExceptionThrowIfNull),
            inheritanceSettings: new Pipelines.Builder.PipelineBuilderInheritanceSettings(enableBuilderInheritance: enableBuilderInheritance, isAbstract: isAbstract, baseClass: baseClass, baseClassBuilderNameSpace: baseClassBuilderNameSpace, inheritanceComparisonDelegate: inheritanceComparisonDelegate),
            entitySettings: CreateEntitySettings(enableEntityInheritance, addNullChecks, enableNullableReferenceTypes, validateArguments, allowGenerationWithoutProperties, copyAttributePredicate: copyAttributePredicate, copyInterfacePredicate: copyInterfacePredicate),
            nameSettings: new Pipelines.Builder.PipelineBuilderNameSettings(setMethodNameFormatString, addMethodNameFormatString, builderNamespaceFormatString, builderNameFormatString, buildMethodName, buildTypedMethodName)
        );

    protected static Pipelines.Entity.PipelineBuilderSettings CreateEntitySettings(
        bool enableEntityInheritance = false,
        bool addNullChecks = false,
        bool enableNullableReferenceTypes = false,
        ArgumentValidationType validateArguments = ArgumentValidationType.None,
        bool allowGenerationWithoutProperties = false,
        bool isAbstract = false,
        Class? baseClass = null,
        string entityNamespaceFormatString = "{Namespace}",
        string entityNameFormatString = "{Class.Name}{EntityNameSuffix}",
        string newCollectionTypeName = "System.Collections.Generic.List",
        bool addSetters = false,
        Visibility? setterVisibility = null,
        IEnumerable<NamespaceMapping>? namespaceMappings = null,
        IEnumerable<TypenameMapping>? typenameMappings = null,
        Predicate<Domain.Attribute>? copyAttributePredicate = null,
        Predicate<string>? copyInterfacePredicate = null)
        => new Pipelines.Entity.PipelineBuilderSettings(
            generationSettings: new Pipelines.Entity.PipelineBuilderGenerationSettings(allowGenerationWithoutProperties: allowGenerationWithoutProperties, addNullChecks: addNullChecks, enableNullableReferenceTypes: enableNullableReferenceTypes, copyAttributePredicate: copyAttributePredicate, copyInterfacePredicate: copyInterfacePredicate, addSetters: addSetters, setterVisibility: setterVisibility),
            inheritanceSettings: new Pipelines.Entity.PipelineBuilderInheritanceSettings(enableInheritance: enableEntityInheritance, isAbstract: isAbstract, baseClass: baseClass),
            constructorSettings: new Pipelines.Entity.PipelineBuilderConstructorSettings(validateArguments: validateArguments),
            nameSettings: new Pipelines.Entity.PipelineBuilderNameSettings(entityNamespaceFormatString, entityNameFormatString),
            typeSettings: new Pipelines.Entity.PipelineBuilderTypeSettings(newCollectionTypeName, namespaceMappings, typenameMappings)
        );

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
