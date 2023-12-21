namespace ClassFramework.Domain.Tests;

public abstract class TestBase
{
    protected IFixture Fixture { get; }

    protected TestBase()
    {
        Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

        // Override creation of our own objects, so we get default values specified by the constructor (i.e. HasGetter=true and HasSetter=true on PropertyBuilder)
        Fixture.Register(() => new AttributeBuilder());
        Fixture.Register(() => new AttributeParameterBuilder());
        Fixture.Register(() => new ConstructorBuilder());
        Fixture.Register(() => new EnumerationBuilder());
        Fixture.Register(() => new FieldBuilder());
        Fixture.Register(() => new LiteralBuilder());
        Fixture.Register(() => new MetadataBuilder());
        Fixture.Register(() => new MethodBuilder());
        Fixture.Register(() => new ParameterBuilder());
        Fixture.Register(() => new PropertyBuilder());
        Fixture.Register(() => new ClassBuilder());
        Fixture.Register(() => new InterfaceBuilder());
        Fixture.Register(() => new StructBuilder());
    }
}

public abstract class TestBase<T> : TestBase
{
    protected T CreateSut() => Fixture.Create<T>();
}
