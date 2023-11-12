namespace ClassFramework.Domain.Tests.TestFixtures;

public record MyCustomEntity
{
    public int Property1 { get; }

    public MyCustomEntity(int property1)
    {
        Property1 = property1;
    }

    public MyCustomEntityBuilder ToBuilder()
    {
        return new MyCustomEntityBuilder(this);
    }
}

public record MyTestEntity
{
    public MyTestEntity(int property1, MyCustomEntity property2, IEnumerable<MyCustomEntity> property3)
    {
        Property1 = property1;
        Property2 = property2.IsNotNull(nameof(property2));
        // When using validation:
        ///Property3 = new ReadOnlyValueCollection<MyCustomEntity>(property3 ?? Enumerable.Empty<MyCustomEntity>());
        // When not using null checks:
        ///Property3 = property3  is null ? null : new ReadOnlyValueCollection<MyCustomEntity>(property3);
        // When using null checks:
        if (property3 is null) throw new ArgumentNullException(nameof(property3));
        Property3 = new ReadOnlyValueCollection<MyCustomEntity>(property3);
    }

    public int Property1 { get; }
    public MyCustomEntity Property2 { get; }
    public IReadOnlyCollection<MyCustomEntity> Property3 { get; }

    public MyTestEntityBuilder ToBuilder()
    {
        return new MyTestEntityBuilder(this);
    }
}

public class MyTestEntityBuilder
{
    private int _property1;
    private MyCustomEntityBuilder _property2;
    private Collection<MyCustomEntityBuilder> _property3;

    public int Property1
    {
        get
        {
            return _property1;
        }
        set
        {
            _property1 = value;
        }
    }

    public MyCustomEntityBuilder Property2
    {
        get
        {
            return _property2;
        }
        set
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            _property2 = value;
        }
    }

    public Collection<MyCustomEntityBuilder> Property3
    {
        get
        {
            return _property3;
        }
        private set
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            _property3 = value;
        }
    }

    public MyTestEntityBuilder()
    {
        _property2 = new MyCustomEntityBuilder();
        _property3 = new Collection<MyCustomEntityBuilder>();
    }

    public MyTestEntityBuilder(MyTestEntity source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        _property1 = source.Property1;
        _property2 = new MyCustomEntityBuilder(source.Property2);
        _property3 = new Collection<MyCustomEntityBuilder>();
    }

    public MyTestEntity Build()
    {
        return new MyTestEntity(Property1, Property2.Build(), Property3.Select(x => x.Build()));
    }
}

public class MyCustomEntityBuilder
{
    private int _property1;

    public int Property1
    {
        get
        {
            return _property1;
        }
        set
        {
            _property1 = value;
        }
    }

    public MyCustomEntityBuilder()
    {
    }

    public MyCustomEntityBuilder(MyCustomEntity source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        _property1 = source.Property1;
    }

    public MyCustomEntity Build()
    {
        return new MyCustomEntity(Property1);
    }
}
