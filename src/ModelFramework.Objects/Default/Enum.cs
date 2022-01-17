﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ModelFramework.Objects.Default
{
    public partial record Enum : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult("Name cannot be null or whitespace", new[] { nameof(Name) });
            }
            if (!Members.Any())
            {
                yield return new ValidationResult("Enum should have at least one member", new[] { nameof(Members) });
            }
        }

        public override string ToString() => Name;
    }
}
