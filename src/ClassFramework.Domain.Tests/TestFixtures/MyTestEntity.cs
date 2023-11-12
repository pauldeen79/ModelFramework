namespace ClassFramework.Domain.Tests.TestFixtures;

public partial record MyCustomEntity
{
    public int Property1 { get; }

    public MyCustomEntity(int property1)
    {
        Property1 = property1;

        // When using validation (either DDD or IValidatableObject style):
        Validate();
    }

    // When using validation with IValidatableObject interface:
    protected void Validate()
    {
        System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
    }

    // When using domain driven style validation (just throw argument exceptions), use this:
    ///partial void Validate();

    public MyCustomEntityBuilder ToBuilder()
    {
        return new MyCustomEntityBuilder(this);
    }
}

public partial record MyTestEntity
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

        // When using validation (either DDD or IValidatableObject style):
        Validate();
    }

    // When using validation with IValidatableObject interface:
    protected void Validate()
    {
        System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
    }

    // When using domain driven style validation (just throw argument exceptions), use this:
    ///partial void Validate();

    public int Property1 { get; }
    public MyCustomEntity Property2 { get; }
    public IReadOnlyCollection<MyCustomEntity> Property3 { get; }

    public MyTestEntityBuilder ToBuilder()
    {
        return new MyTestEntityBuilder(this);
    }
}

public partial class MyTestEntityBuilder ///: System.ComponentModel.DataAnnotations.IValidatableObject
{
    private MyCustomEntityBuilder _property2;
    private Collection<MyCustomEntityBuilder> _property3;

    // When not nullable, use auto property and no backing field
    public int Property1 { get; set; }

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
        // Note that if you need to assign a default value to Property1, then you should either use _property1 or Property1, depending on the availability of a backing field.
        _property2 = new MyCustomEntityBuilder();
        _property3 = new Collection<MyCustomEntityBuilder>();
        SetDefaultValues();
    }

    public MyTestEntityBuilder(MyTestEntity source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        // Note that if you need to assign a default value to Property1, then you should either use _property1 or Property1, depending on the availability of a backing field.
        Property1 = source.Property1;
        _property2 = new MyCustomEntityBuilder(source.Property2);
        _property3 = new Collection<MyCustomEntityBuilder>();
    }

    partial void SetDefaultValues();

    public MyTestEntity Build()
    {
        return new MyTestEntity(Property1, Property2.Build(), Property3.Select(x => x.Build()));
    }
}

public partial class MyCustomEntityBuilder ///: System.ComponentModel.DataAnnotations.IValidatableObject
{
    public int Property1 { get; set; }

    public MyCustomEntityBuilder()
    {
    }

    public MyCustomEntityBuilder(MyCustomEntity source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        Property1 = source.Property1;
    }

    public MyCustomEntity Build()
    {
        return new MyCustomEntity(Property1);
    }
}
