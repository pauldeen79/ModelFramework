namespace ModelFramework.Generators.Objects.Tests.POC;

public record MySharedValidationDomainEntity1
{
    [Required]
    public string Name { get; }
    [Range(1, 100)]
    public int Age { get; }

    public MySharedValidationDomainEntity1(string name, int age, bool validate = true)
    {
        Name = name;
        Age = age;
        if (validate)
        {
            Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
        }
    }
}

public class MySharedValidationDomainEntityBuilder : IValidatableObject
{
    public StringBuilder Name { get; set; }
    public int Age { get; set; }

    public MySharedValidationDomainEntity1 Build() => new(Name.ToString(), Age, true);

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var instance = new MySharedValidationDomainEntity1(Name.ToString(), Age, false);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(instance, new ValidationContext(instance, null, null), results, true);
        return results;
    }

    public MySharedValidationDomainEntityBuilder()
    {
        Name = new StringBuilder();
    }
}
