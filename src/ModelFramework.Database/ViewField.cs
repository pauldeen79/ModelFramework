﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelFramework.Database
{
    public partial record ViewField : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult("Name cannot be null or whitespace", new[] { nameof(Name) });
            }
        }

        public override string ToString() => Name;
    }
}