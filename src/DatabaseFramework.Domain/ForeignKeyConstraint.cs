namespace DatabaseFramework.Domain;

public partial record ForeignKeyConstraint : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!LocalFields.Any())
        {
            yield return new ValidationResult($"{nameof(LocalFields)} should contain at least 1 value", new[] { nameof(LocalFields) });
        }
        if (!ForeignFields.Any())
        {
            yield return new ValidationResult($"{nameof(ForeignFields)} should contain at least 1 value", new[] { nameof(ForeignFields) });
        }
    }
}
