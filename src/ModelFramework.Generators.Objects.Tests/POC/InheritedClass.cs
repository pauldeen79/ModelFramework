namespace ModelFramework.Generators.Objects.Tests.POC;

internal class InheritedClass : BaseClass
{
    public string AdditionalProperty { get; }

    public InheritedClass(string baseProperty, string additionalProperty) : base(baseProperty)
    {
        AdditionalProperty = additionalProperty;
    }
}

internal abstract class BaseClass
{
    public string BaseProperty { get; }

    protected BaseClass(string baseProperty)
    {
        BaseProperty = baseProperty;
    }
}

internal class InheritedClassBuilder : BaseClassBuilder<InheritedClassBuilder, InheritedClass>
{
    public string AdditionalProperty { get; set; }

    public InheritedClassBuilder WithAdditionalProperty(string additionalProperty)
    {
        AdditionalProperty = additionalProperty;
        return this;
    }

    public override InheritedClass Build()
    {
        return new InheritedClass(BaseProperty, AdditionalProperty);
    }

    public InheritedClassBuilder() : base()
    {
        AdditionalProperty = string.Empty;
    }
}

internal abstract class BaseClassBuilder<TBuilder, T>
    where T : BaseClass
    where TBuilder : BaseClassBuilder<TBuilder, T>
{
    public string BaseProperty { get; set; }

    public TBuilder WithBaseProperty(string baseProperty)
    {
        BaseProperty = baseProperty;
        return (TBuilder)this;
    }

    public abstract T Build();

    protected BaseClassBuilder()
    {
        BaseProperty = string.Empty;
    }
}
