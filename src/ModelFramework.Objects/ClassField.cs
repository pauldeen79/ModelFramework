namespace ModelFramework.Objects;

public partial record ClassField : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            yield return new ValidationResult("Name cannot be null or whitespace", new[] { nameof(Name) });
        }

        if (string.IsNullOrWhiteSpace(TypeName))
        {
            yield return new ValidationResult("TypeName cannot be null or whitespace", new[] { nameof(TypeName) });
        }
    }

    public override string ToString() => Name;
}
