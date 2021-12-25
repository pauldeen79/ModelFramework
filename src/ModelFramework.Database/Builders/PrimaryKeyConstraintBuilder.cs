using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;

namespace ModelFramework.Database.Builders
{
    public class PrimaryKeyConstraintBuilder
    {
        public string Name { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public string FileGroupName { get; set; }
        public List<PrimaryKeyConstraintFieldBuilder> Fields { get; set; }
        public IPrimaryKeyConstraint Build()
        {
            return new PrimaryKeyConstraint(Name,
                                            FileGroupName,
                                            Fields.Select(x => x.Build()),
                                            Metadata.Select(x => x.Build()));
        }
        public PrimaryKeyConstraintBuilder Clear()
        {
            Name = string.Empty;
            Metadata.Clear();
            FileGroupName = string.Empty;
            Fields.Clear();
            return this;
        }
        public PrimaryKeyConstraintBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public PrimaryKeyConstraintBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public PrimaryKeyConstraintBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public PrimaryKeyConstraintBuilder AddMetadata(params MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }
        public PrimaryKeyConstraintBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public PrimaryKeyConstraintBuilder AddMetadata(params IMetadata[] metadata)
        {
            Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            return this;
        }
        public PrimaryKeyConstraintBuilder WithFileGroupName(string fileGroupName)
        {
            FileGroupName = fileGroupName;
            return this;
        }
        public PrimaryKeyConstraintBuilder ClearFields()
        {
            Fields.Clear();
            return this;
        }
        public PrimaryKeyConstraintBuilder AddFields(IEnumerable<PrimaryKeyConstraintFieldBuilder> fields)
        {
            return AddFields(fields.ToArray());
        }
        public PrimaryKeyConstraintBuilder AddFields(params PrimaryKeyConstraintFieldBuilder[] fields)
        {
            Fields.AddRange(fields);
            return this;
        }
        public PrimaryKeyConstraintBuilder AddFields(IEnumerable<PrimaryKeyConstraintField> fields)
        {
            return AddFields(fields.ToArray());
        }
        public PrimaryKeyConstraintBuilder AddFields(params PrimaryKeyConstraintField[] fields)
        {
            Fields.AddRange(fields.Select(itemToAdd => new PrimaryKeyConstraintFieldBuilder(itemToAdd)));
            return this;
        }
        public PrimaryKeyConstraintBuilder()
        {
            Name = string.Empty;
            FileGroupName = string.Empty;
            Metadata = new List<MetadataBuilder>();
            Fields = new List<PrimaryKeyConstraintFieldBuilder>();
        }
        public PrimaryKeyConstraintBuilder(IPrimaryKeyConstraint source)
        {
            Metadata = new List<MetadataBuilder>();
            Fields = new List<PrimaryKeyConstraintFieldBuilder>();

            Name = source.Name;
            Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));
            FileGroupName = source.FileGroupName;
            Fields.AddRange(source.Fields.Select(x => new PrimaryKeyConstraintFieldBuilder(x)));
        }
    }
}
