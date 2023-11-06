namespace ClassFramework.Pipelines.Tests;

public abstract class TestBase
{
    protected IFixture Fixture { get; } = new Fixture().Customize(new AutoNSubstituteCustomization());

    protected virtual void InitializeParser()
    {
        var parser = Fixture.Freeze<IFormattableStringParser>();
        parser.Parse(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>())
              .Returns(x => x.ArgAt<string>(0) == "{Error}"
                ? Result.Error<string>("Kaboom")
                : Result.Success(x.ArgAt<string>(0)
                    .Replace("{Name}", CreateReplacement(x[2], y => y.Name, y => y.Name), StringComparison.Ordinal)
                    .Replace("{NameLower}", CreateReplacement(x[2], y => y.Name.ToLower(x.ArgAt<IFormatProvider>(1).ToCultureInfo()), y => y.Name.ToLower(x.ArgAt<IFormatProvider>(1).ToCultureInfo())), StringComparison.Ordinal)
                    .Replace("{NameUpper}", CreateReplacement(x[2], y => y.Name.ToUpper(x.ArgAt<IFormatProvider>(1).ToCultureInfo()), y => y.Name.ToUpper(x.ArgAt<IFormatProvider>(1).ToCultureInfo())), StringComparison.Ordinal)
                    .Replace("{NamePascal}", CreateReplacement(x[2], y => y.Name.ToPascalCase(x.ArgAt<IFormatProvider>(1).ToCultureInfo()), y => y.Name.ToPascalCase(x.ArgAt<IFormatProvider>(1).ToCultureInfo())), StringComparison.Ordinal)
                    .Replace("{Namespace}", CreateReplacement(x[2], y => y.Namespace, null), StringComparison.Ordinal)
                    .Replace("{Class.Name}", CreateReplacement(x[2], y => y.Name, null), StringComparison.Ordinal)
                    .Replace("{Class.NameLower}", CreateReplacement(x[2], y => y.Name.ToLower(x.ArgAt<IFormatProvider>(1).ToCultureInfo()), null), StringComparison.Ordinal)
                    .Replace("{Class.NameUpper}", CreateReplacement(x[2], y => y.Name.ToUpper(x.ArgAt<IFormatProvider>(1).ToCultureInfo()), null), StringComparison.Ordinal)
                    .Replace("{Class.NamePascal}", CreateReplacement(x[2], y => y.Name.ToPascalCase(x.ArgAt<IFormatProvider>(1).ToCultureInfo()), null), StringComparison.Ordinal)
                    .Replace("{Class.Namespace}", CreateReplacement(x[2], y => y.Namespace, null), StringComparison.Ordinal)
                    .Replace("{Class.FullName}", CreateReplacement(x[2], y => y.GetFullName(), null), StringComparison.Ordinal)
                    .Replace("{NullCheck.Source}", "/* source null check goes here */ ", StringComparison.Ordinal)
                    .Replace("{NullCheck.Source.Argument}", "/* source argument null check goes here */ ", StringComparison.Ordinal)
                    .Replace("{NullCheck.Argument}", "/* argument null check goes here */", StringComparison.Ordinal)
                    .Replace("{EntityNameSuffix}", "/* suffix goes here*/", StringComparison.Ordinal)));
    }

    private static string CreateReplacement(
        object? input,
        Func<TypeBase, string> typeBaseDelegate,
        Func<ClassProperty, string>? classPropertyDelegate)
        => input switch
        {
            PipelineContext<ClassBuilder, BuilderContext> classContext => typeBaseDelegate(classContext.Context.SourceModel),
            PipelineContext<ClassBuilder, EntityContext> classContext => typeBaseDelegate(classContext.Context.SourceModel),
            PipelineContext<ClassPropertyBuilder, BuilderContext> propertyContext => typeBaseDelegate(propertyContext.Context.SourceModel),
            PipelineContext<ClassPropertyBuilder, EntityContext> propertyContext => typeBaseDelegate(propertyContext.Context.SourceModel),
            PipelineContext<ClassProperty, BuilderContext> propertyContext => typeBaseDelegate(propertyContext.Context.SourceModel),
            PipelineContext<ClassProperty, EntityContext> propertyContext => typeBaseDelegate(propertyContext.Context.SourceModel),
            ParentChildContext<BuilderContext, ClassProperty> parentChild => classPropertyDelegate is null
                ? typeBaseDelegate(parentChild.ParentContext.Context.SourceModel)
                : classPropertyDelegate(parentChild.ChildContext),
            ParentChildContext<EntityContext, ClassProperty> parentChild => classPropertyDelegate is null
                ? typeBaseDelegate(parentChild.ParentContext.Context.SourceModel)
                : classPropertyDelegate(parentChild.ChildContext),
            _ => throw new NotSupportedException($"Context of type {input?.GetType()} is not supported")
        };

    protected static TypeBase CreateModel(string baseClass = "", params MetadataBuilder[] propertyMetadataBuilders)
        => new ClassBuilder()
            .WithName("SomeClass")
            .WithNamespace("SomeNamespace")
            .WithBaseClass(baseClass)
            .AddProperties(new ClassPropertyBuilder().WithName("Property1").WithType(typeof(int)).AddMetadata(propertyMetadataBuilders).AddAttributes(new AttributeBuilder().WithName("MyAttribute")))
            .AddProperties(new ClassPropertyBuilder().WithName("Property2").WithType(typeof(string)).AddMetadata(propertyMetadataBuilders).AddAttributes(new AttributeBuilder().WithName("MyAttribute")))
            .AddProperties(new ClassPropertyBuilder().WithName("Property3").WithType(typeof(List<int>)).AddMetadata(propertyMetadataBuilders).AddAttributes(new AttributeBuilder().WithName("MyAttribute")))
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

    protected static Pipelines.Builder.PipelineBuilderSettings CreateBuilderSettings(
        bool enableBuilderInheritance = false,
        bool isAbstract = false,
        bool enableEntityInheritance = false,
        bool addNullChecks = false,
        bool enableNullableReferenceTypes = false,
        bool copyAttributes = false,
        bool addCopyConstructor = false,
        bool setDefaultValues = true,
        string newCollectionTypeName = "System.Collections.Generic.List",
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
        Func<IParentTypeContainer, TypeBase, bool>? inheritanceComparisonDelegate = null)
        => new Pipelines.Builder.PipelineBuilderSettings(
            typeSettings: new Pipelines.Builder.PipelineBuilderTypeSettings(newCollectionTypeName: newCollectionTypeName),
            constructorSettings: new Pipelines.Builder.PipelineBuilderConstructorSettings(addCopyConstructor, setDefaultValues),
            generationSettings: new PipelineBuilderGenerationSettings(addNullChecks: addNullChecks, enableNullableReferenceTypes: enableNullableReferenceTypes, copyAttributes: copyAttributes),
            inheritanceSettings: new Pipelines.Builder.PipelineBuilderInheritanceSettings(enableBuilderInheritance: enableBuilderInheritance, isAbstract: isAbstract, baseClass: baseClass, baseClassBuilderNameSpace: baseClassBuilderNameSpace, inheritanceComparisonDelegate: inheritanceComparisonDelegate),
            classSettings: CreateEntitySettings(enableEntityInheritance, addNullChecks, validateArguments, allowGenerationWithoutProperties),
            nameSettings: new Pipelines.Builder.PipelineBuilderNameSettings(setMethodNameFormatString, addMethodNameFormatString, builderNamespaceFormatString, builderNameFormatString, buildMethodName, buildTypedMethodName)
        );

    protected static Pipelines.Entity.PipelineBuilderSettings CreateEntitySettings(
        bool enableEntityInheritance = false,
        bool addNullChecks = false,
        ArgumentValidationType validateArguments = ArgumentValidationType.None,
        bool allowGenerationWithoutProperties = false,
        bool isAbstract = false,
        Class? baseClass = null,
        string entityNamespaceFormatString = "{Namespace}",
        string entityNameFormatString = "{Class.Name}{EntityNameSuffix}")
        => new Pipelines.Entity.PipelineBuilderSettings(
            allowGenerationWithoutProperties: allowGenerationWithoutProperties,
            inheritanceSettings: new Pipelines.Entity.PipelineBuilderInheritanceSettings(enableInheritance: enableEntityInheritance, isAbstract: isAbstract, baseClass: baseClass),
            constructorSettings: new Pipelines.Entity.PipelineBuilderConstructorSettings(validateArguments: validateArguments, addNullChecks: addNullChecks),
            nameSettings: new Pipelines.Entity.PipelineBuilderNameSettings(entityNamespaceFormatString, entityNameFormatString)
        );
}

public abstract class TestBase<T> : TestBase
{
    protected T CreateSut() => Fixture.Create<T>();
}
