﻿using System;
using System.Collections.Generic;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;

namespace ModelFramework.Database.Default
{
    public record UniqueConstraintField : IUniqueConstraintField
    {
        public UniqueConstraintField(string name, IEnumerable<IMetadata> metadata)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            }

            Name = name;
            Metadata = new ValueCollection<IMetadata>(metadata);
        }

        public string Name { get; }
        public ValueCollection<IMetadata> Metadata { get; }
    }
}
