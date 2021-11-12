﻿using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;

namespace ModelFramework.Database.Default
{
    public record UniqueConstraint : IUniqueConstraint
    {
        public UniqueConstraint(string name,
                                string fileGroupName = null,
                                IEnumerable<IUniqueConstraintField> fields = null,
                                IEnumerable<IMetadata> metadata = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            if (fields?.Any() != true)
            {
                throw new ArgumentException("Fields should contain at least 1 value", nameof(fields));
            }
            Name = name;
            FileGroupName = fileGroupName;
            Fields = new ValueCollection<IUniqueConstraintField>(fields);
            Metadata = new ValueCollection<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        public string Name { get; }
        public ValueCollection<IMetadata> Metadata { get; }
        public string FileGroupName { get; }
        public ValueCollection<IUniqueConstraintField> Fields { get; }
    }
}
