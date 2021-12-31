using System;
using System.Collections.Generic;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;

namespace ModelFramework.Database.Default
{
    public record CheckConstraint : ICheckConstraint
    {
        public CheckConstraint(string name,
                               string expression,
                               IEnumerable<IMetadata> metadata)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            }

            if (string.IsNullOrWhiteSpace(expression))
            {
                throw new ArgumentOutOfRangeException(nameof(expression), "Expression cannot be null or whitespace");
            }

            Name = name;
            Expression = expression;
            Metadata = new ValueCollection<IMetadata>(metadata);
        }

        public string Expression { get; }
        public string Name { get; }
        public ValueCollection<IMetadata> Metadata { get; }
    }
}
