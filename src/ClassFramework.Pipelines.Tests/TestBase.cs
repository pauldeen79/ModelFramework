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
                        ClassProperty classProperty => classProperty.Name,
                        ParentChildContext<ClassProperty> parentChild => parentChild.ParentContext.Context.SourceModel.Name,
                        _ => throw new NotSupportedException($"Context of type {x[2]?.GetType()} is not supported")
                    }, StringComparison.Ordinal)
                .Replace("{NullCheck.Source}", "/* null check goes here */ ", StringComparison.Ordinal)));
    }

    protected virtual TypeBase CreateModel(string baseClass = "")
        => new ClassBuilder()
            .WithName("SomeClass")
            .WithNamespace("SomeNamespace")
            .WithBaseClass(baseClass)
            .AddProperties(new ClassPropertyBuilder().WithName("Property1").WithType(typeof(int)))
            .AddProperties(new ClassPropertyBuilder().WithName("Property2").WithType(typeof(string)))
            .AddProperties(new ClassPropertyBuilder().WithName("Property3").WithType(typeof(List<int>)))
            .Build();
}

public abstract class TestBase<T> : TestBase
{
    protected T CreateSut() => Fixture.Create<T>();
}
