using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;

namespace ModelFramework.Database.Default
{
    public record ForeignKeyConstraint : IForeignKeyConstraint
    {
        public ForeignKeyConstraint(string name,
                                    string foreignTableName,
                                    IEnumerable<IForeignKeyConstraintField> localFields,
                                    IEnumerable<IForeignKeyConstraintField> foreignFields,
                                    CascadeAction cascadeUpdate,
                                    CascadeAction cascadeDelete,
                                    IEnumerable<IMetadata> metadata)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            }

            if (string.IsNullOrWhiteSpace(foreignTableName))
            {
                throw new ArgumentOutOfRangeException(nameof(foreignTableName), "ForeignTableName cannot be null or whitespace");
            }

            if (!localFields.Any())
            {
                throw new ArgumentException("LocalFields should contain at least 1 value", nameof(localFields));
            }
            if (!foreignFields.Any())
            {
                throw new ArgumentException("ForeignFields should contain at least 1 value", nameof(foreignFields));
            }

            Name = name;
            ForeignTableName = foreignTableName;
            LocalFields = new ValueCollection<IForeignKeyConstraintField>(localFields);
            ForeignFields = new ValueCollection<IForeignKeyConstraintField>(foreignFields);
            CascadeUpdate = cascadeUpdate;
            CascadeDelete = cascadeDelete;
            Metadata = new ValueCollection<IMetadata>(metadata);
        }

        public ValueCollection<IForeignKeyConstraintField> LocalFields { get; }
        public ValueCollection<IForeignKeyConstraintField> ForeignFields { get; }
        public string ForeignTableName { get; }
        public CascadeAction CascadeUpdate { get; }
        public CascadeAction CascadeDelete { get; }
        public string Name { get; }
        public ValueCollection<IMetadata> Metadata { get; }
    }
}
