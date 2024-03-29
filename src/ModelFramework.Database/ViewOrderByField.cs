﻿namespace ModelFramework.Database;

public partial record ViewOrderByField : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            yield return new ValidationResult("Name cannot be null or whitespace", new[] { nameof(Name) });
        }
    }

    public override string ToString() => IsDescending
        ? $"{Name} DESC"
        : $"{Name} ASC";
}
