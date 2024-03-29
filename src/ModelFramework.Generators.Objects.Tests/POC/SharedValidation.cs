﻿namespace ModelFramework.Generators.Objects.Tests.POC;

public record MySharedValidationDomainEntity1Base
{
    [Required]
    [StringLength(10, MinimumLength = 1)]
    [MinLength(1, ErrorMessage = "boohoo1")]
    [MaxLength(10, ErrorMessage = "boohoo2")]
    [RegularExpression("dog$", ErrorMessage = "boohoo3")]
    [Url(ErrorMessage = "boohoo4")]
    public string Name { get; }
    [Range(1, 100, ErrorMessage = "The Age field must be between 1 and 100.")]
    public int Age { get; }
    [Phone]
    public string PhoneNumber { get; }
    [EmailAddress]
    public string EmailAddress { get; }

    public MySharedValidationDomainEntity1Base(string name, string phoneNumber, string emailAddress, int age)
    {
        Name = name;
        PhoneNumber = phoneNumber;
        EmailAddress = emailAddress;
        Age = age;
    }
}

public record MySharedValidationDomainEntity1 : MySharedValidationDomainEntity1Base
{
    public MySharedValidationDomainEntity1(MySharedValidationDomainEntity1 original) : base(original)
    {
    }

    public MySharedValidationDomainEntity1(string name, string phoneNumber, string emailAddress, int age) : base(name, phoneNumber, emailAddress, age)
    {
        Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
    }
}

public class MySharedValidationDomainEntityBuilder : IValidatableObject
{
    public StringBuilder Name { get; set; }
    public StringBuilder PhoneNumber { get; set; }
    public StringBuilder EmailAddress { get; set; }
    public int Age { get; set; }

    public MySharedValidationDomainEntity1 Build() => new(Name.ToString(), PhoneNumber.ToString(), EmailAddress.ToString(), Age);

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var instance = new MySharedValidationDomainEntity1Base(Name.ToString(), PhoneNumber.ToString(), EmailAddress.ToString(), Age);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(instance, new ValidationContext(instance, null, null), results, true);
        return results;
    }

    public MySharedValidationDomainEntityBuilder()
    {
        Name = new StringBuilder();
        PhoneNumber = new StringBuilder();
        EmailAddress = new StringBuilder();
    }
}
