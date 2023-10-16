namespace ClassFramework.Pipelines.Tests;

public abstract class TestBase
{
    protected IFixture Fixture { get; } = new Fixture().Customize(new AutoNSubstituteCustomization());

    protected virtual void InitializeParser()
    {
        var parser = Fixture.Freeze<IFormattableStringParser>();
        parser.Parse(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>())
              .Returns(x => Result<string>.Success(x.ArgAt<string>(0)
                .Replace("{Name}",
                    x[2] switch
                    {
                        PipelineContext<ClassBuilder, BuilderContext> classContext => classContext.Context.SourceModel.Name,
                        PipelineContext<ClassPropertyBuilder, BuilderContext> propertyContext => propertyContext.Context.SourceModel.Name,
                        PipelineContext<ClassProperty, BuilderContext> propertyContext => propertyContext.Context.SourceModel.Name,
                        ParentChildContext<ClassProperty> parentChild => parentChild.ChildContext.Name,
                        _ => throw new NotSupportedException($"Context of type {x[2]?.GetType()} is not supported")
                    }, StringComparison.Ordinal)
                .Replace("{NamePascal}",
                    x[2] switch
                    {
                        PipelineContext<ClassBuilder, BuilderContext> classContext => classContext.Context.SourceModel.Name.ToPascalCase(CultureInfo.InvariantCulture),
                        PipelineContext<ClassPropertyBuilder, BuilderContext> propertyContext => propertyContext.Context.SourceModel.Name.ToPascalCase(CultureInfo.InvariantCulture),
                        PipelineContext<ClassProperty, BuilderContext> propertyContext => propertyContext.Context.SourceModel.Name.ToPascalCase(CultureInfo.InvariantCulture),
                        ParentChildContext<ClassProperty> parentChild => parentChild.ChildContext.Name.ToPascalCase(CultureInfo.InvariantCulture),
                        _ => throw new NotSupportedException($"Context of type {x[2]?.GetType()} is not supported")
                    }, StringComparison.Ordinal)
                .Replace("{Namespace}",
                    x[2] switch
                    {
                        PipelineContext<ClassBuilder, BuilderContext> classContext => classContext.Context.SourceModel.Namespace,
                        PipelineContext<ClassPropertyBuilder, BuilderContext> propertyContext => propertyContext.Context.SourceModel.Namespace,
                        PipelineContext<ClassProperty, BuilderContext> propertyContext => propertyContext.Context.SourceModel.Namespace,
                        ParentChildContext<ClassProperty> parentChild => parentChild.ParentContext.Context.SourceModel.Namespace,
                        _ => throw new NotSupportedException($"Context of type {x[2]?.GetType()} is not supported")
                    }, StringComparison.Ordinal)
                .Replace("{NullCheck.Source}", "/* null check goes here */ ", StringComparison.Ordinal)));
    }

    protected virtual TypeBase CreateModel(string baseClass = "", params MetadataBuilder[] propertyMetadataBuilders)
        => new ClassBuilder()
            .WithName("SomeClass")
            .WithNamespace("SomeNamespace")
            .WithBaseClass(baseClass)
            .AddProperties(new ClassPropertyBuilder().WithName("Property1").WithType(typeof(int)).AddMetadata(propertyMetadataBuilders))
            .AddProperties(new ClassPropertyBuilder().WithName("Property2").WithType(typeof(string)).AddMetadata(propertyMetadataBuilders))
            .AddProperties(new ClassPropertyBuilder().WithName("Property3").WithType(typeof(List<int>)).AddMetadata(propertyMetadataBuilders))
            .Build();
}

public abstract class TestBase<T> : TestBase
{
    protected T CreateSut() => Fixture.Create<T>();
}
