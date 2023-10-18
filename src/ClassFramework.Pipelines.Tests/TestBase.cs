namespace ClassFramework.Pipelines.Tests;

public abstract class TestBase
{
    protected IFixture Fixture { get; } = new Fixture().Customize(new AutoNSubstituteCustomization());

    protected virtual void InitializeParser()
    {
        var parser = Fixture.Freeze<IFormattableStringParser>();
        parser.Parse(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>())
              .Returns(x => Result<string>.Success(x.ArgAt<string>(0)
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
                .Replace("{NullCheck.Source}", "/* null check goes here */ ", StringComparison.Ordinal)));
    }

    private string CreateReplacement(object? input, Func<TypeBase, string> typeBaseDelegate, Func<ClassProperty, string>? classPropertyDelegate)
        => input switch
        {
            PipelineContext<ClassBuilder, BuilderContext> classContext => typeBaseDelegate(classContext.Context.SourceModel),
            PipelineContext<ClassPropertyBuilder, BuilderContext> propertyContext => typeBaseDelegate(propertyContext.Context.SourceModel),
            PipelineContext<ClassProperty, BuilderContext> propertyContext => typeBaseDelegate(propertyContext.Context.SourceModel),
            ParentChildContext<ClassProperty> parentChild => classPropertyDelegate is null ? typeBaseDelegate(parentChild.ParentContext.Context.SourceModel) : classPropertyDelegate(parentChild.ChildContext),
            _ => throw new NotSupportedException($"Context of type {input?.GetType()} is not supported")
        };

    protected virtual TypeBase CreateModel(string baseClass = "", params MetadataBuilder[] propertyMetadataBuilders)
        => new ClassBuilder()
            .WithName("SomeClass")
            .WithNamespace("SomeNamespace")
            .WithBaseClass(baseClass)
            .AddProperties(new ClassPropertyBuilder().WithName("Property1").WithType(typeof(int)).AddMetadata(propertyMetadataBuilders).AddAttributes(new AttributeBuilder().WithName("MyAttribute")))
            .AddProperties(new ClassPropertyBuilder().WithName("Property2").WithType(typeof(string)).AddMetadata(propertyMetadataBuilders).AddAttributes(new AttributeBuilder().WithName("MyAttribute")))
            .AddProperties(new ClassPropertyBuilder().WithName("Property3").WithType(typeof(List<int>)).AddMetadata(propertyMetadataBuilders).AddAttributes(new AttributeBuilder().WithName("MyAttribute")))
            .Build();
}

public abstract class TestBase<T> : TestBase
{
    protected T CreateSut() => Fixture.Create<T>();
}
