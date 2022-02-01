namespace ModelFramework.Database;

public partial record CheckConstraint : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            yield return new ValidationResult("Name cannot be null or whitespace", new[] { nameof(Name) });
        }
        if (string.IsNullOrWhiteSpace(Expression))
        {
            yield return new ValidationResult("Expression cannot be null or whitespace", new[] { nameof(Expression) });
        }
    }

    public override string ToString() => Name;
}
