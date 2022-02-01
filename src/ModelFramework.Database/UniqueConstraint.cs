namespace ModelFramework.Database;

public partial record UniqueConstraint : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            yield return new ValidationResult("Name cannot be null or whitespace", new[] { nameof(Name) });
        }
        if (!Fields.Any())
        {
            yield return new ValidationResult("Fields should contain at least 1 value", new[] { nameof(Fields) });
        }
    }

    public override string ToString() => Name;
}
