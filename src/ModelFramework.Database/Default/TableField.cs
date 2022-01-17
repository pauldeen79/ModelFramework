using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelFramework.Database.Default
{
    public partial record TableField : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult("Name cannot be null or whitespace", new[] { nameof(Name) });
            }
            if (string.IsNullOrWhiteSpace(Type))
            {
                yield return new ValidationResult("Type cannot be null or whitespace", new[] { nameof(Type) });
            }
        }

        public override string ToString() => Name;
    }
}
