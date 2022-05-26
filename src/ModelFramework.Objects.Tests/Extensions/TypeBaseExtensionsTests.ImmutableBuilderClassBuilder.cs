namespace ModelFramework.Objects.Tests.Extensions;

public partial class TypeBaseExtensionsTests
{
    [Fact]
    public void Generating_ImmutableClassBuilder_From_Class_Without_Properties_Throws_Exception()
    {
        // Arrange
        var input = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();

        // Act & Assert
        input.Invoking(x => x.ToImmutableClassBuilder(new ImmutableClassSettings()))
             .Should().Throw<InvalidOperationException>()
             .WithMessage("To create an immutable class, there must be at least one property");
    }

    [Fact]
    public void ToImmutableExtensionClass_Throws_When_Properties_Are_Empty()
    {
        // Arrange
        var input = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();

        // Act & Assert
        input.Invoking(x => x.ToImmutableExtensionClass(new ImmutableClassExtensionsSettings()))
             .Should().Throw<InvalidOperationException>()
             .WithMessage("To create an immutable extensions class, there must be at least one property");
    }

    [Fact]
    public void ToImmutableExtensionClassBuilder_Throws_When_Properties_Are_Empty()
    {
        // Arrange
        var input = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();

        // Act & Assert
        input.Invoking(x => x.ToImmutableExtensionClassBuilder(new ImmutableClassExtensionsSettings()))
             .Should().Throw<InvalidOperationException>()
             .WithMessage("To create an immutable extensions class, there must be at least one property");
    }

    [Fact]
    public void ToBuilderExtensionsClass_Throws_When_Properties_Are_Empty()
    {
        // Arrange
        var input = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();

        // Act & Assert
        input.Invoking(x => x.ToBuilderExtensionsClass(new ImmutableBuilderClassSettings()))
             .Should().Throw<InvalidOperationException>()
             .WithMessage("To create a builder extensions class, there must be at least one property");
    }

    [Fact]
    public void ToBuilderExtensionsClassBuilder_Throws_When_Properties_Are_Empty()
    {
        // Arrange
        var input = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();

        // Act & Assert
        input.Invoking(x => x.ToBuilderExtensionsClassBuilder(new ImmutableBuilderClassSettings()))
             .Should().Throw<InvalidOperationException>()
             .WithMessage("To create a builder extensions class, there must be at least one property");
    }

    [Fact]
    public void Can_Build_ImmutableBuilderClass_From_Immutable_Class()
    {
        // Arrange
        var sut = new ClassBuilder()
            .WithName("TestClass")
            .AddProperties(new ClassPropertyBuilder()
                .WithName("Property1")
                .WithType(typeof(string))
                .AsReadOnly())
            .AddProperties(new ClassPropertyBuilder()
                .WithName("Property2")
                .WithType(typeof(IReadOnlyCollection<string>))
                .AsReadOnly())
            .AddConstructors(new ClassConstructorBuilder()
                .AddParameter("property1", typeof(string))
                .AddParameter("property2", typeof(IEnumerable<string>))
                .AddLiteralCodeStatements
                (
                    "Property1 = property1;",
                    "Property2 = new List<string>(property2);"
                ))
            .Build();

        // Act
        var actual = sut.ToImmutableBuilderClass(new ImmutableBuilderClassSettings());

        // Assert
        actual.Name.Should().Be("TestClassBuilder");
        actual.Properties.Should().HaveCount(2);
        actual.Properties.First().HasSetter.Should().BeTrue();
        actual.Properties.Last().HasSetter.Should().BeTrue();

        // By default, only a public parameterless constructor should be defined
        actual.Constructors.Should().ContainSingle();
        actual.Constructors.First().Parameters.Should().BeEmpty();
        actual.Constructors.First().Visibility.Should().Be(Visibility.Public);
        string.Join(Environment.NewLine, actual.Constructors.First().CodeStatements.Select(x => x.ToString())).Should().Be(@"Property2 = new System.Collections.Generic.List<string>();
Property1 = string.Empty;");

        // Build method
        var buildMethod = actual.Methods.SingleOrDefault(x => x.Name == "Build");
        buildMethod.Should().NotBeNull();
        if (buildMethod != null)
        {
            string.Join(Environment.NewLine, buildMethod.CodeStatements.Select(x => x.ToString())).Should().Be("return new TestClass(Property1, Property2);");
        }
    }

    [Fact]
    public void Can_Build_ImmutableBuilderClass_From_Immutable_Class_With_NullChecks()
    {
        // Arrange
        var sut = new ClassBuilder()
            .WithName("TestClass")
            .AddProperties(new ClassPropertyBuilder()
                .WithName("Property1")
                .WithType(typeof(string))
                .AsReadOnly())
            .AddProperties(new ClassPropertyBuilder()
                .WithName("Property2")
                .WithType(typeof(IReadOnlyCollection<string>))
                .AsReadOnly())
            .AddConstructors(new ClassConstructorBuilder()
                .AddParameter("property1", typeof(string))
                .AddParameter("property2", typeof(IEnumerable<string>))
                .AddLiteralCodeStatements
                (
                    "Property1 = property1;",
                    "Property2 = new List<string>(property2);"
                ))
            .Build();

        // Act
        var actual = sut.ToImmutableBuilderClass(new ImmutableBuilderClassSettings(constructorSettings: new ImmutableBuilderClassConstructorSettings(addNullChecks: true)));

        // Assert
        // By default, only a public parameterless constructor should be defined
        actual.Constructors.Should().ContainSingle();
        actual.Constructors.First().Parameters.Should().BeEmpty();
        actual.Constructors.First().Visibility.Should().Be(Visibility.Public);
        string.Join(Environment.NewLine, actual.Constructors.First().CodeStatements.Select(x => x.ToString())).Should().Be(@"Property2 = new System.Collections.Generic.List<string>();
Property1 = string.Empty;");

        // Build method
        var buildMethod = actual.Methods.SingleOrDefault(x => x.Name == "Build");
        buildMethod.Should().NotBeNull();
        if (buildMethod != null)
        {
            string.Join(Environment.NewLine, buildMethod.CodeStatements.Select(x => x.ToString())).Should().Be("return new TestClass(Property1, Property2);");
        }
    }

    [Fact]
    public void Can_Build_ImmutableBuilderClass_From_Poco_Class()
    {
        // Arrange
        var sut = new ClassBuilder()
            .WithName("TestClass")
            .AddProperties(new ClassPropertyBuilder()
                .WithName("Property1")
                .WithType(typeof(string)))
            .AddProperties(new ClassPropertyBuilder()
                .WithName("Property2")
                .WithType(typeof(List<string>)))
            .Build();

        // Act
        var actual = sut.ToImmutableBuilderClass(new ImmutableBuilderClassSettings());

        // Assert
        actual.Name.Should().Be("TestClassBuilder");
        actual.Properties.Should().HaveCount(2);
        actual.Properties.First().HasSetter.Should().BeTrue();
        actual.Properties.Last().HasSetter.Should().BeTrue();

        // By default, only a public parameterless constructor should be defined
        actual.Constructors.Should().ContainSingle();
        actual.Constructors.First().Parameters.Should().BeEmpty();
        actual.Constructors.First().Visibility.Should().Be(Visibility.Public);
        string.Join(Environment.NewLine, actual.Constructors.First().CodeStatements.Select(x => x.ToString())).Should().Be(@"Property2 = new System.Collections.Generic.List<string>();
Property1 = string.Empty;");

        // Build method
        var buildMethod = actual.Methods.SingleOrDefault(x => x.Name == "Build");
        buildMethod.Should().NotBeNull();
        if (buildMethod != null)
        {
            string.Join(Environment.NewLine, buildMethod.CodeStatements.Select(x => x.ToString())).Should().Be("return new TestClass { Property1 = Property1, Property2 = Property2 };");
        }
    }

    [Fact]
    public void Generating_ImmutableBuilderClass_From_Class_Without_Properties_Throws_Exception()
    {
        // Arrange
        var input = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();

        // Act & Assert
        input.Invoking(x => x.ToImmutableBuilderClass(new ImmutableBuilderClassSettings()))
             .Should().Throw<InvalidOperationException>()
             .WithMessage("To create an immutable builder class, there must be at least one property");
    }

    [Fact]
    public void Can_Build_ImmutableBuilderClass_From_Interface()
    {
        // Arrange
        var sut = new InterfaceBuilder()
            .WithName("ITestClass")
            .AddProperties(new ClassPropertyBuilder()
                .WithName("Property1")
                .WithType(typeof(string))
                .AsReadOnly())
            .AddProperties(new ClassPropertyBuilder()
                .WithName("Property2")
                .WithType(typeof(IReadOnlyCollection<string>))
                .AsReadOnly())
            .Build();

        // Act
        var actual = sut.ToImmutableBuilderClass(new ImmutableBuilderClassSettings(typeSettings: new ImmutableBuilderClassTypeSettings( formatInstanceTypeNameDelegate: (type, forCreate) =>
        {
            if (type.Name == "ITestClass" && forCreate)
            {
                return "TestClass";
            }
            return string.Empty;
        })));

        // Assert
        actual.Name.Should().Be("ITestClassBuilder");
        actual.Properties.Should().HaveCount(2);
        actual.Properties.First().HasSetter.Should().BeTrue();
        actual.Properties.Last().HasSetter.Should().BeTrue();
        // By default, only a public parameterless constructor should be defined
        actual.Constructors.Should().ContainSingle();
        actual.Constructors.First().Parameters.Should().BeEmpty();
        actual.Constructors.First().Visibility.Should().Be(Visibility.Public);
        string.Join(Environment.NewLine, actual.Constructors.First().CodeStatements.Select(x => x.ToString())).Should().Be(@"Property2 = new System.Collections.Generic.List<string>();
Property1 = string.Empty;");

        // Build method
        var buildMethod = actual.Methods.SingleOrDefault(x => x.Name == "Build");
        buildMethod.Should().NotBeNull();
        if (buildMethod != null)
        {
            string.Join(Environment.NewLine, buildMethod.CodeStatements.Select(x => x.ToString())).Should().Be("return new TestClass(Property1, Property2);");
        }
    }

    [Fact]
    public void Can_Add_CopyConstructor_To_ImmutableBuilderClass()
    {
        // Arrange
        var sut = new ClassBuilder()
            .WithName("TestClass")
            .AddProperties(new ClassPropertyBuilder()
                .WithName("Property1")
                .WithType(typeof(string))
                .AsReadOnly())
            .AddProperties(new ClassPropertyBuilder()
                .WithName("Property2")
                .WithType(typeof(IReadOnlyCollection<string>))
                .AsReadOnly())
            .AddConstructors(new ClassConstructorBuilder()
                .AddParameter("property1", typeof(string))
                .AddLiteralCodeStatements("Property1 = property1;"))
            .Build();

        // Act
        var actual = sut.ToImmutableBuilderClass(new ImmutableBuilderClassSettings(constructorSettings: new ImmutableBuilderClassConstructorSettings(addCopyConstructor: true)));

        // Assert
        actual.Name.Should().Be("TestClassBuilder");
        actual.Properties.Should().HaveCount(2);
        actual.Properties.First().HasSetter.Should().BeTrue();
        actual.Properties.Last().HasSetter.Should().BeTrue();
        actual.Constructors.Should().HaveCount(2);

        actual.Constructors.First().Parameters.Should().BeEmpty();
        actual.Constructors.First().Visibility.Should().Be(Visibility.Public);
        string.Join(Environment.NewLine, actual.Constructors.First().CodeStatements.Select(x => x.ToString())).Should().Be(@"Property2 = new System.Collections.Generic.List<string>();
Property1 = string.Empty;");

        actual.Constructors.Last().Parameters.Should().HaveCount(1);
        actual.Constructors.Last().Parameters.First().Name.Should().Be("source");
        actual.Constructors.Last().Parameters.First().TypeName.Should().Be("TestClass");
        actual.Constructors.Last().Visibility.Should().Be(Visibility.Public);
        string.Join(Environment.NewLine, actual.Constructors.Last().CodeStatements.Select(x => x.ToString())).Should().Be(@"Property2 = new System.Collections.Generic.List<string>();
Property1 = source.Property1;
Property2.AddRange(source.Property2);");
    }

    [Fact]
    public void Can_Add_Constructor_With_All_Parameters_To_ImmutableBuilderClass()
    {
        // Arrange
        var sut = new ClassBuilder()
            .WithName("TestClass")
            .AddProperties(new ClassPropertyBuilder()
                .WithName("Property1")
                .WithType(typeof(string))
                .AsReadOnly())
            .AddProperties(new ClassPropertyBuilder()
                .WithName("Property2")
                .WithType(typeof(IReadOnlyCollection<string>))
                .AsReadOnly()
                .ConvertCollectionOnBuilderToEnumerable(true))
            .AddConstructors(new ClassConstructorBuilder()
                .AddParameter("property1", typeof(string))
                .AddParameter("property2", typeof(IEnumerable<string>))
                .AddLiteralCodeStatements("Property1 = property1;")
                .AddLiteralCodeStatements("Property2 = new List<string>(property2);"))
            .Build();

        // Act
        var actual = sut.ToImmutableBuilderClass(new ImmutableBuilderClassSettings(constructorSettings: new ImmutableBuilderClassConstructorSettings(addConstructorWithAllProperties: true)));

        // Assert
        actual.Name.Should().Be("TestClassBuilder");
        actual.Properties.Should().HaveCount(2);
        actual.Properties.First().HasSetter.Should().BeTrue();
        actual.Properties.Last().HasSetter.Should().BeTrue();
        actual.Constructors.Should().HaveCount(2);

        actual.Constructors.First().Parameters.Should().BeEmpty();
        actual.Constructors.First().Visibility.Should().Be(Visibility.Public);

        actual.Constructors.Last().Parameters.Should().HaveCount(2);
        actual.Constructors.Last().Parameters.First().Name.Should().Be("property1");
        actual.Constructors.Last().Parameters.First().TypeName.Should().Be("System.String");
        actual.Constructors.Last().Parameters.Last().Name.Should().Be("property2");
        actual.Constructors.Last().Parameters.Last().TypeName.Should().Be("System.Collections.Generic.IEnumerable<System.String>");
        actual.Constructors.Last().Visibility.Should().Be(Visibility.Public);
    }

    [Fact]
    public void Can_Add_Overload_To_NonCollection_Property()
    {
        // Arrange
        var sut = new ClassBuilder()
            .WithName("TestClass")
            .AddProperties(new ClassPropertyBuilder()
                .WithName("TypeName")
                .WithType(typeof(string))
                .AddBuilderOverload("WithType", typeof(Type), "type", "{2} = type.AssemblyQualifiedName;"))
            .Build();

        // Act
        var actual = sut.ToImmutableBuilderClass(new ImmutableBuilderClassSettings());

        // Assert
        actual.Name.Should().Be("TestClassBuilder");
        actual.Properties.Should().ContainSingle();
        actual.Properties.First().HasSetter.Should().BeTrue();

        // By default, only a public parameterless constructor should be defined
        actual.Constructors.Should().ContainSingle();
        actual.Constructors.First().Parameters.Should().BeEmpty();
        actual.Constructors.First().Visibility.Should().Be(Visibility.Public);
        string.Join(Environment.NewLine, actual.Constructors.First().CodeStatements.Select(x => x.ToString())).Should().Be(@"TypeName = string.Empty;");

        // Build method
        var buildMethod = actual.Methods.SingleOrDefault(x => x.Name == "Build");
        buildMethod.Should().NotBeNull();
        if (buildMethod != null)
        {
            string.Join(Environment.NewLine, buildMethod.CodeStatements.Select(x => x.ToString())).Should().Be("return new TestClass { TypeName = TypeName };");
        }

        // Default 'with' method
        var withTypeNameMethod = actual.Methods.SingleOrDefault(x => x.Name == "WithTypeName");
        withTypeNameMethod.Should().NotBeNull();
        if (withTypeNameMethod != null)
        {
            string.Join(Environment.NewLine, withTypeNameMethod.CodeStatements.Select(x => x.ToString())).Should().Be(@"TypeName = typeName;
return this;");
        }

        // Added overload method
        var withTypeMethod = actual.Methods.SingleOrDefault(x => x.Name == "WithType");
        withTypeMethod.Should().NotBeNull();
        if (withTypeMethod != null)
        {
            string.Join(Environment.NewLine, withTypeMethod.CodeStatements.Select(x => x.ToString())).Should().Be(@"TypeName = type.AssemblyQualifiedName;
return this;");
        }
    }

    [Fact]
    public void Can_Add_Overload_To_Collection_Property()
    {
        // Arrange
        var sut = new ClassBuilder()
            .WithName("TestClass")
            .AddProperties(new ClassPropertyBuilder()
                .WithName("TypeNames")
                .WithType(typeof(IEnumerable<string>))
                .AddBuilderOverload("AddTypes", typeof(IEnumerable<Type>), "types", "{4}.AddRange(types.Select(x => x.AssemblyQualifiedName));"))
            .Build();

        // Act
        var actual = sut.ToImmutableBuilderClass(new ImmutableBuilderClassSettings());

        // Assert
        actual.Name.Should().Be("TestClassBuilder");
        actual.Properties.Should().ContainSingle();
        actual.Properties.First().HasSetter.Should().BeTrue();

        // By default, only a public parameterless constructor should be defined
        actual.Constructors.Should().ContainSingle();
        actual.Constructors.First().Parameters.Should().BeEmpty();
        actual.Constructors.First().Visibility.Should().Be(Visibility.Public);
        string.Join(Environment.NewLine, actual.Constructors.First().CodeStatements.Select(x => x.ToString())).Should().Be(@"TypeNames = new System.Collections.Generic.List<string>();");

        // Build method
        var buildMethod = actual.Methods.SingleOrDefault(x => x.Name == "Build");
        buildMethod.Should().NotBeNull();
        if (buildMethod != null)
        {
            string.Join(Environment.NewLine, buildMethod.CodeStatements.Select(x => x.ToString())).Should().Be("return new TestClass { TypeNames = TypeNames };");
        }

        // Default 'with' method
        var withTypeNameMethod = actual.Methods.SingleOrDefault(x => x.Name == "AddTypeNames" && x.Parameters.First().TypeName == "System.String[]");
        withTypeNameMethod.Should().NotBeNull();
        if (withTypeNameMethod != null)
        {
            string.Join(Environment.NewLine, withTypeNameMethod.CodeStatements.Select(x => x.ToString())).Should().Be(@"TypeNames.AddRange(typeNames);
return this;");
        }

        // Added overload method
        var withTypeMethod = actual.Methods.SingleOrDefault(x => x.Name == "AddTypes" && x.Parameters.First().TypeName == "System.Type[]");
        withTypeMethod.Should().NotBeNull();
        if (withTypeMethod != null)
        {
            string.Join(Environment.NewLine, withTypeMethod.CodeStatements.Select(x => x.ToString())).Should().Be(@"TypeNames.AddRange(types.Select(x => x.AssemblyQualifiedName));
return this;");
        }
    }

    [Fact]
    public void Generating_ImmutableClassBuilder_With_MethodTemplate_Returns_Class_With_Build_And_With_Methods()
    {
        // Arrange
        var properties = CreateProperties();
        var settings = new ImmutableBuilderClassSettings();
        var cls = new ClassBuilder()
            .WithName("MyRecord")
            .WithNamespace("MyNamespace")
            .AddProperties(properties)
            .Build()
            .ToImmutableClass(new ImmutableClassSettings("System.Collections.Generic.IReadOnlyCollection"));

        // Act
        var actual = cls.ToImmutableBuilderClass(settings);

        // Assert
        actual.Methods.Select(x => x.Name).Should().BeEquivalentTo(new[]
        {
            "Build",
            "WithProperty1",
            "AddProperty2",
            "AddProperty2",
            "WithProperty3",
            "AddProperty4",
            "AddProperty4"
        });
        actual.Methods.Where(x => x.Name == "Build").Should().HaveCount(1);
        string.Join(Environment.NewLine, actual.Methods.First(x => x.Name == "Build").CodeStatements.Select(x => x.ToString())).Should().Be(@"return new MyNamespace.MyRecord(Property1, Property2, Property3?.Build(), Property4.Select(x => x.Build()));");
    }

    [Fact]
    public void Generating_ImmutableClassBuilder_Without_MethodTemplate_Returns_Class_With_Build_And_With_Methods()
    {
        // Arrange
        var properties = CreateProperties();
        var settings = new ImmutableBuilderClassSettings(nameSettings: new ImmutableBuilderClassNameSettings(setMethodNameFormatString: string.Empty, addMethodNameFormatString: string.Empty));
        var cls = new ClassBuilder()
            .WithName("MyRecord")
            .WithNamespace("MyNamespace")
            .AddProperties(properties)
            .Build()
            .ToImmutableClass(new ImmutableClassSettings("System.Collections.Generic.IReadOnlyCollection"));

        // Act
        var actual = cls.ToImmutableBuilderClass(settings);

        // Assert
        actual.Methods.Select(x => x.Name).Should().BeEquivalentTo(new[]
        {
            "Build"
        });
        actual.Methods.Where(x => x.Name == "Build").Should().HaveCount(1);
        string.Join(Environment.NewLine, actual.Methods.First(x => x.Name == "Build").CodeStatements.Select(x => x.ToString())).Should().Be(@"return new MyNamespace.MyRecord(Property1, Property2, Property3?.Build(), Property4.Select(x => x.Build()));");
    }

    [Fact]
    public void Can_Create_ImmutableBuilderClass_With_Lazy_Initialization()
    {
        // Arrange
        var sut = CreateImmutableBuilderClass();

        // Act
        var actual = sut.ToImmutableBuilderClass(new ImmutableBuilderClassSettings(useLazyInitialization: true,
                                                                                   typeSettings: new ImmutableBuilderClassTypeSettings(useTargetTypeNewExpressions: false),
                                                                                   enableNullableReferenceTypes: true));

        // Assert
        actual.Fields.Should().HaveCount(2);
        actual.Fields.First().Name.Should().Be("_property1Delegate");
        actual.Fields.First().TypeName.Should().Be("System.Lazy<string>");
        actual.Fields.Last().TypeName.Should().Be("System.Lazy<string?>");
        actual.Properties.Should().HaveCount(3);
        actual.Properties.First().Name.Should().Be("Property1");
        actual.Properties.First().TypeName.Should().Be("System.String");
        string.Join(Environment.NewLine, actual.Properties.First().GetterCodeStatements.Select(y => y.ToString())).Should().Be(@"return _property1Delegate.Value;");
        string.Join(Environment.NewLine, actual.Properties.First().SetterCodeStatements.Select(y => y.ToString())).Should().Be(@"_property1Delegate = new System.Lazy<string>(() => value);");
        actual.Properties.Last().TypeName.Should().Be("System.String"); // note that the nullable annotation will be aded during code generation (in the template)
        string.Join(Environment.NewLine, actual.Properties.Last().GetterCodeStatements.Select(y => y.ToString())).Should().Be(@"return _property3Delegate.Value;");
        string.Join(Environment.NewLine, actual.Properties.Last().SetterCodeStatements.Select(y => y.ToString())).Should().Be(@"_property3Delegate = new System.Lazy<string?>(() => value);");
        actual.Constructors.Should().HaveCount(1);
        string.Join(Environment.NewLine, actual.Constructors.First().CodeStatements.Select(x => x.ToString())).Should().Be(@"Property2 = new System.Collections.Generic.List<string>();
#pragma warning disable CS8603 // Possible null reference return.
_property1Delegate = new System.Lazy<string>(() => string.Empty);
_property3Delegate = new System.Lazy<string?>(() => default);
#pragma warning restore CS8603 // Possible null reference return.");
        actual.Methods.Where(x => x.Name == "Build").Should().HaveCount(1);
        string.Join(Environment.NewLine, actual.Methods.First(x => x.Name == "Build").CodeStatements.Select(x => x.ToString())).Should().Be(@"#pragma warning disable CS8604 // Possible null reference argument.
return new TestClass(Property1, Property2, Property3);
#pragma warning restore CS8604 // Possible null reference argument.");
    }

    [Fact]
    public void Can_Create_ImmutableBuilderClass_With_Lazy_Initialization_And_TargetTypeNewExpressions()
    {
        // Arrange
        var sut = CreateImmutableBuilderClass();

        // Act
        var actual = sut.ToImmutableBuilderClass(new ImmutableBuilderClassSettings(useLazyInitialization: true,
                                                                                   typeSettings: new ImmutableBuilderClassTypeSettings(useTargetTypeNewExpressions: true),
                                                                                   enableNullableReferenceTypes: true));

        // Assert
        string.Join(Environment.NewLine, actual.Properties.First().SetterCodeStatements.Select(y => y.ToString())).Should().Be(@"_property1Delegate = new (() => value);");
        string.Join(Environment.NewLine, actual.Properties.Last().SetterCodeStatements.Select(y => y.ToString())).Should().Be(@"_property3Delegate = new (() => value);");
        string.Join(Environment.NewLine, actual.Constructors.First().CodeStatements.Select(x => x.ToString())).Should().Be(@"Property2 = new System.Collections.Generic.List<string>();
#pragma warning disable CS8603 // Possible null reference return.
_property1Delegate = new (() => string.Empty);
_property3Delegate = new (() => default);
#pragma warning restore CS8603 // Possible null reference return.");
    }

    [Fact]
    public void Can_Create_ImmutableBuilderClass_With_CopyConstructor_With_Lazy_Initilization_And_TargetTypeNewExpressions()
    {
        // Arrange
        var sut = CreateImmutableBuilderClass();

        // Act
        var actual = sut.ToImmutableBuilderClass(new ImmutableBuilderClassSettings(useLazyInitialization: true,
                                                                                   typeSettings: new ImmutableBuilderClassTypeSettings(useTargetTypeNewExpressions: true),
                                                                                   enableNullableReferenceTypes: true,
                                                                                   constructorSettings: new ImmutableBuilderClassConstructorSettings(addCopyConstructor: true)));

        // Assert
        actual.Constructors.Should().HaveCount(2);
        string.Join(Environment.NewLine, actual.Constructors.Last().CodeStatements.Select(x => x.ToString())).Should().Be(@"Property2 = new System.Collections.Generic.List<string>();
_property1Delegate = new (() => source.Property1);
Property2.AddRange(source.Property2);
_property3Delegate = new (() => source.Property3);");
        actual.Methods.Should().HaveCount(7);
        actual.Methods.Select(x => x.Name).Should().BeEquivalentTo(new[]
        {
            "Build",
            "WithProperty1",
            "WithProperty1",
            "AddProperty2",
            "AddProperty2",
            "WithProperty3",
            "WithProperty3"
        });
        string.Join(Environment.NewLine, actual.Methods.ElementAt(1).CodeStatements.Select(x => x.ToString())).Should().Be(@"Property1 = property1;
return this;");
        string.Join(Environment.NewLine, actual.Methods.ElementAt(2).CodeStatements.Select(x => x.ToString())).Should().Be(@"_property1Delegate = new (property1Delegate);
return this;");
        actual.Methods.ElementAt(5).Parameters.First().TypeName.FixTypeName().Should().Be("System.String");
        actual.Methods.ElementAt(5).Parameters.First().IsNullable.Should().BeTrue();
        actual.Methods.ElementAt(6).Parameters.First().TypeName.FixTypeName().Should().Be("System.Func<System.String?>");
        actual.Methods.ElementAt(6).Parameters.First().IsNullable.Should().BeFalse();
    }

    private static IClass CreateImmutableBuilderClass()
        => new ClassBuilder()
            .WithName("TestClass")
            .AddProperties(new ClassPropertyBuilder()
                .WithName("Property1")
                .WithType(typeof(string)))
            .AddProperties(new ClassPropertyBuilder()
                .WithName("Property2")
                .WithType(typeof(IReadOnlyCollection<string>)))
            .AddProperties(new ClassPropertyBuilder()
                .WithName("Property3")
                .WithType(typeof(string))
                .WithIsNullable())
            .AsReadOnly()
            .Build();

    private static ClassPropertyBuilder[] CreateProperties()
        => new[]
        {
            new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)),
            new ClassPropertyBuilder().WithName("Property2").WithType(typeof(ICollection<string>)).ConvertCollectionOnBuilderToEnumerable(true),
            new ClassPropertyBuilder().WithName("Property3").WithTypeName("MyCustomType").ConvertSinglePropertyToBuilderOnBuilder(),
            new ClassPropertyBuilder().WithName("Property4").WithTypeName(typeof(ICollection<string>).FullName!.Replace("System.String","MyCustomType")).ConvertCollectionPropertyToBuilderOnBuilder(true)
        };
}
