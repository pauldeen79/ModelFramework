﻿namespace ModelFramework.Objects
{
    public partial record EnumMember : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult("Name cannot be null or whitespace", new[] { nameof(Name) });
            }
        }

        public override string ToString()
            => Value != null
                ? $"[{Name}] = [{Value}]"
                : Name;
    }
}
