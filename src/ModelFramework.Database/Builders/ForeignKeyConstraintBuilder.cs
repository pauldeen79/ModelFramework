using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;

namespace ModelFramework.Database.Builders
{
    public class ForeignKeyConstraintBuilder
    {
        public List<ForeignKeyConstraintFieldBuilder> LocalFields { get; set; }
        public List<ForeignKeyConstraintFieldBuilder> ForeignFields { get; set; }
        public string ForeignTableName { get; set; }
        public CascadeAction CascadeUpdate { get; set; }
        public CascadeAction CascadeDelete { get; set; }
        public string Name { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public IForeignKeyConstraint Build()
        {
            return new ForeignKeyConstraint(Name, ForeignTableName, LocalFields.Select(x => x.Build()), ForeignFields.Select(x => x.Build()), CascadeUpdate, CascadeDelete, Metadata.Select(x => x.Build()));
        }
        public ForeignKeyConstraintBuilder Clear()
        {
            LocalFields.Clear();
            ForeignFields.Clear();
            ForeignTableName = default;
            CascadeUpdate = default;
            CascadeDelete = default;
            Name = default;
            Metadata.Clear();
            return this;
        }
        public ForeignKeyConstraintBuilder Update(IForeignKeyConstraint source)
        {
            LocalFields = new List<ForeignKeyConstraintFieldBuilder>();
            ForeignFields = new List<ForeignKeyConstraintFieldBuilder>();
            ForeignTableName = default;
            CascadeUpdate = default;
            CascadeDelete = default;
            Name = default;
            Metadata = new List<MetadataBuilder>();
            if (source != null)
            {
                if (source.LocalFields != null) LocalFields.AddRange(source.LocalFields.Select(x => new ForeignKeyConstraintFieldBuilder(x)));
                if (source.ForeignFields != null) ForeignFields.AddRange(source.ForeignFields.Select(x => new ForeignKeyConstraintFieldBuilder(x)));
                ForeignTableName = source.ForeignTableName;
                CascadeUpdate = source.CascadeUpdate;
                CascadeDelete = source.CascadeDelete;
                Name = source.Name;
                if (source.Metadata != null) Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public ForeignKeyConstraintBuilder ClearLocalFields()
        {
            LocalFields.Clear();
            return this;
        }
        public ForeignKeyConstraintBuilder AddLocalFields(IEnumerable<ForeignKeyConstraintFieldBuilder> localFields)
        {
            return AddLocalFields(localFields.ToArray());
        }
        public ForeignKeyConstraintBuilder AddLocalFields(params ForeignKeyConstraintFieldBuilder[] localFields)
        {
            if (localFields != null)
            {
                foreach (var itemToAdd in localFields)
                {
                    LocalFields.Add(itemToAdd);
                }
            }
            return this;
        }
        public ForeignKeyConstraintBuilder AddLocalFields(IEnumerable<ForeignKeyConstraintField> localFields)
        {
            return AddLocalFields(localFields.ToArray());
        }
        public ForeignKeyConstraintBuilder AddLocalFields(params ForeignKeyConstraintField[] localFields)
        {
            if (localFields != null)
            {
                foreach (var itemToAdd in localFields)
                {
                    LocalFields.Add(new ForeignKeyConstraintFieldBuilder(itemToAdd));
                }
            }
            return this;
        }
        public ForeignKeyConstraintBuilder ClearForeignFields()
        {
            ForeignFields.Clear();
            return this;
        }
        public ForeignKeyConstraintBuilder AddForeignFields(IEnumerable<ForeignKeyConstraintFieldBuilder> foreignFields)
        {
            return AddForeignFields(foreignFields.ToArray());
        }
        public ForeignKeyConstraintBuilder AddForeignFields(params ForeignKeyConstraintFieldBuilder[] foreignFields)
        {
            if (foreignFields != null)
            {
                foreach (var itemToAdd in foreignFields)
                {
                    ForeignFields.Add(itemToAdd);
                }
            }
            return this;
        }
        public ForeignKeyConstraintBuilder AddForeignFields(IEnumerable<ForeignKeyConstraintField> foreignFields)
        {
            return AddForeignFields(foreignFields.ToArray());
        }
        public ForeignKeyConstraintBuilder AddForeignFields(params ForeignKeyConstraintField[] foreignFields)
        {
            if (foreignFields != null)
            {
                foreach (var itemToAdd in foreignFields)
                {
                    ForeignFields.Add(new ForeignKeyConstraintFieldBuilder(itemToAdd));
                }
            }
            return this;
        }
        public ForeignKeyConstraintBuilder WithForeignTableName(string foreignTableName)
        {
            ForeignTableName = foreignTableName;
            return this;
        }
        public ForeignKeyConstraintBuilder WithCascadeUpdate(CascadeAction cascadeUpdate)
        {
            CascadeUpdate = cascadeUpdate;
            return this;
        }
        public ForeignKeyConstraintBuilder WithCascadeDelete(CascadeAction cascadeDelete)
        {
            CascadeDelete = cascadeDelete;
            return this;
        }
        public ForeignKeyConstraintBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public ForeignKeyConstraintBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public ForeignKeyConstraintBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ForeignKeyConstraintBuilder AddMetadata(params MetadataBuilder[] metadata)
        {
            if (metadata != null)
            {
                foreach (var itemToAdd in metadata)
                {
                    Metadata.Add(itemToAdd);
                }
            }
            return this;
        }
        public ForeignKeyConstraintBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ForeignKeyConstraintBuilder AddMetadata(params IMetadata[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high
        public ForeignKeyConstraintBuilder(IForeignKeyConstraint source = null)
#pragma warning restore S3776 // Cognitive Complexity of methods should not be too high
        {
            LocalFields = new List<ForeignKeyConstraintFieldBuilder>();
            ForeignFields = new List<ForeignKeyConstraintFieldBuilder>();
            Metadata = new List<MetadataBuilder>();
            if (source != null)
            {
                if (source.LocalFields != null) foreach (var x in source.LocalFields) LocalFields.Add(new ForeignKeyConstraintFieldBuilder(x));
                if (source.ForeignFields != null) foreach (var x in source.ForeignFields) ForeignFields.Add(new ForeignKeyConstraintFieldBuilder(x));
                ForeignTableName = source.ForeignTableName;
                CascadeUpdate = source.CascadeUpdate;
                CascadeDelete = source.CascadeDelete;
                Name = source.Name;
                if (source.Metadata != null) foreach (var x in source.Metadata) Metadata.Add(new MetadataBuilder(x));
            }
        }
        public ForeignKeyConstraintBuilder(string name,
                                           string foreignTableName,
                                           IEnumerable<IForeignKeyConstraintField> localFields,
                                           IEnumerable<IForeignKeyConstraintField> foreignFields,
                                           CascadeAction cascadeUpdate = CascadeAction.NoAction,
                                           CascadeAction cascadeDelete = CascadeAction.NoAction,
                                           IEnumerable<IMetadata> metadata = null)
        {
            LocalFields = new List<ForeignKeyConstraintFieldBuilder>();
            ForeignFields = new List<ForeignKeyConstraintFieldBuilder>();
            Metadata = new List<MetadataBuilder>();
            Name = name;
            ForeignTableName = foreignTableName;
            if (localFields != null) LocalFields.AddRange(localFields.Select(x => new ForeignKeyConstraintFieldBuilder(x)));
            if (foreignFields != null) ForeignFields.AddRange(foreignFields.Select(x => new ForeignKeyConstraintFieldBuilder(x)));
            CascadeUpdate = cascadeUpdate;
            CascadeDelete = cascadeDelete;
            if (metadata != null) Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
        }
    }
}
