using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ModelFramework.Database
{
    public partial record ForeignKeyConstraint : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult("Name cannot be null or whitespace", new[] { nameof(Name) });
            }
            if (string.IsNullOrWhiteSpace(ForeignTableName))
            {
                yield return new ValidationResult("ForeignTableName cannot be null or whitespace", new[] { nameof(ForeignTableName) });
            }

            if (!LocalFields.Any())
            {
                yield return new ValidationResult("LocalFields should contain at least 1 value", new[] { nameof(LocalFields) });
            }
            if (!ForeignFields.Any())
            {
                yield return new ValidationResult("ForeignFields should contain at least 1 value", new[] { nameof(ForeignFields) });
            }
        }

        public override string ToString() => Name;
    }
}
