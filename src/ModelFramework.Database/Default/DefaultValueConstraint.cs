using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelFramework.Database.Default
{
    public partial record DefaultValueConstraint : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult("Name cannot be null or whitespace", new[] { nameof(Name) });
            }
            if (string.IsNullOrWhiteSpace(FieldName))
            {
                yield return new ValidationResult("FieldName cannot be null or whitespace", new[] { nameof(FieldName) });
            }
            if (string.IsNullOrWhiteSpace(DefaultValue))
            {
                yield return new ValidationResult("DefaultValue cannot be null or whitespace", new[] { nameof(DefaultValue) });
            }
        }

        public override string ToString() => Name;
    }
}
