namespace DatabaseFramework.Domain;

public partial record UniqueConstraint : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!Fields.Any())
        {
            yield return new ValidationResult($"{nameof(Fields)} should contain at least 1 value", new[] { nameof(Fields) });
        }
    }
}
