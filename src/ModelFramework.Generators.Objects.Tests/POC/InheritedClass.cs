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

    public override BaseClass Build() => BuildTyped();

    public override InheritedClass BuildTyped()
    {
        return new InheritedClass(BaseProperty, AdditionalProperty);
    }

    public InheritedClassBuilder() : base()
    {
        AdditionalProperty = string.Empty;
    }
}

internal abstract class BaseClassBuilder
{
    public string BaseProperty { get; set; }

    protected BaseClassBuilder()
    {
        BaseProperty = string.Empty;
    }

    public abstract BaseClass Build();
}

internal abstract class BaseClassBuilder<TBuilder, T> : BaseClassBuilder
    where T : BaseClass
    where TBuilder : BaseClassBuilder<TBuilder, T>
{
    public TBuilder WithBaseProperty(string baseProperty)
    {
        BaseProperty = baseProperty;
        return (TBuilder)this;
    }

    public abstract T BuildTyped();
}

internal abstract class MiddleClass : BaseClass
{
    protected MiddleClass(string middleProperty, string baseProperty) : base(baseProperty)
    {
        MiddleProperty = middleProperty;
    }

    public string MiddleProperty { get; }
}

internal abstract class MiddleClassBuilder : BaseClassBuilder
{
    public string MiddleProperty { get; set; }

    protected MiddleClassBuilder() : base()
    {
        MiddleProperty = string.Empty;
    }
}

internal abstract class MiddleClassBuilder<TBuilder, T> : MiddleClassBuilder // note that we can't inherit from the generic base class...
    where T : MiddleClass
    where TBuilder : MiddleClassBuilder<TBuilder, T>
{
    public TBuilder WithBaseProperty(string baseProperty)
    {
        BaseProperty = baseProperty;
        return (TBuilder)this;
    } // note that this is duplicated, because we can't inherit from two base classes...

    public TBuilder WithMiddleProperty(string middleProperty)
    {
        MiddleProperty = middleProperty;
        return (TBuilder)this;
    }
}
