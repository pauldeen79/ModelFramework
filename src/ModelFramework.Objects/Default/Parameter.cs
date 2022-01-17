using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelFramework.Objects.Default
{
    public partial record Parameter : IValidatableObject
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
}
