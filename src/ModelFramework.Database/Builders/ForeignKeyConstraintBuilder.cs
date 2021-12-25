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
            return new ForeignKeyConstraint(Name,
                                            ForeignTableName,
                                            LocalFields.Select(x => x.Build()),
                                            ForeignFields.Select(x => x.Build()),
                                            CascadeUpdate,
                                            CascadeDelete,
                                            Metadata.Select(x => x.Build()));
        }
        public ForeignKeyConstraintBuilder Clear()
        {
            LocalFields.Clear();
            ForeignFields.Clear();
            ForeignTableName = string.Empty;
            CascadeUpdate = default;
            CascadeDelete = default;
            Name = string.Empty;
            Metadata.Clear();
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
            LocalFields.AddRange(localFields);
            return this;
        }
        public ForeignKeyConstraintBuilder AddLocalFields(IEnumerable<ForeignKeyConstraintField> localFields)
        {
            return AddLocalFields(localFields.ToArray());
        }
        public ForeignKeyConstraintBuilder AddLocalFields(params ForeignKeyConstraintField[] localFields)
        {
            LocalFields.AddRange(localFields.Select(itemToAdd => new ForeignKeyConstraintFieldBuilder(itemToAdd)));
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
            ForeignFields.AddRange(foreignFields);
            return this;
        }
        public ForeignKeyConstraintBuilder AddForeignFields(IEnumerable<ForeignKeyConstraintField> foreignFields)
        {
            return AddForeignFields(foreignFields.ToArray());
        }
        public ForeignKeyConstraintBuilder AddForeignFields(params ForeignKeyConstraintField[] foreignFields)
        {
            ForeignFields.AddRange(foreignFields.Select(itemToAdd => new ForeignKeyConstraintFieldBuilder(itemToAdd)));
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
            Metadata.AddRange(metadata);
            return this;
        }
        public ForeignKeyConstraintBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ForeignKeyConstraintBuilder AddMetadata(params IMetadata[] metadata)
        {
            Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            return this;
        }
        public ForeignKeyConstraintBuilder()
        {
            ForeignTableName = string.Empty;
            Name = string.Empty;
            LocalFields = new List<ForeignKeyConstraintFieldBuilder>();
            ForeignFields = new List<ForeignKeyConstraintFieldBuilder>();
            Metadata = new List<MetadataBuilder>();
        }
        public ForeignKeyConstraintBuilder(IForeignKeyConstraint source)
        {
            LocalFields = new List<ForeignKeyConstraintFieldBuilder>();
            ForeignFields = new List<ForeignKeyConstraintFieldBuilder>();
            Metadata = new List<MetadataBuilder>();
            LocalFields.AddRange(source.LocalFields.Select(x => new ForeignKeyConstraintFieldBuilder(x)));
            ForeignFields.AddRange(source.ForeignFields.Select(x => new ForeignKeyConstraintFieldBuilder(x)));
            ForeignTableName = source.ForeignTableName;
            CascadeUpdate = source.CascadeUpdate;
            CascadeDelete = source.CascadeDelete;
            Name = source.Name;
            Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));
        }
    }
}
