using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelFramework.Common.Default
{
    public partial record Metadata : IValidatableObject
    {
        public override string ToString() => Value == null
            ? $"[{Name}] = NULL"
            : $"[{Name}] = [{Value}]";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult("Name cannot be null or whitespace", new[] { nameof(Name) });
            }
        }
    }
}
