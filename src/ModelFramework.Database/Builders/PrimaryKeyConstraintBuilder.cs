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
            Name = default;
            Metadata.Clear();
            FileGroupName = default;
            Fields.Clear();
            return this;
        }
        public PrimaryKeyConstraintBuilder Update(IPrimaryKeyConstraint source)
        {
            Metadata = new List<MetadataBuilder>();
            Fields = new List<PrimaryKeyConstraintFieldBuilder>();

            Name = source.Name;
            if (source.Metadata != null) Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));
            FileGroupName = source.FileGroupName;
            Fields.AddRange(source.Fields.Select(x => new PrimaryKeyConstraintFieldBuilder(x)));

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
            if (metadata != null)
            {
                foreach (var itemToAdd in metadata)
                {
                    Metadata.Add(itemToAdd);
                }
            }
            return this;
        }
        public PrimaryKeyConstraintBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public PrimaryKeyConstraintBuilder AddMetadata(params IMetadata[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            }
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
            if (fields != null)
            {
                foreach (var itemToAdd in fields)
                {
                    Fields.Add(itemToAdd);
                }
            }
            return this;
        }
        public PrimaryKeyConstraintBuilder AddFields(IEnumerable<PrimaryKeyConstraintField> fields)
        {
            return AddFields(fields.ToArray());
        }
        public PrimaryKeyConstraintBuilder AddFields(params PrimaryKeyConstraintField[] fields)
        {
            if (fields != null)
            {
                foreach (var itemToAdd in fields)
                {
                    Fields.Add(new PrimaryKeyConstraintFieldBuilder(itemToAdd));
                }
            }
            return this;
        }
        public PrimaryKeyConstraintBuilder()
        {
            Metadata = new List<MetadataBuilder>();
            Fields = new List<PrimaryKeyConstraintFieldBuilder>();
        }
        public PrimaryKeyConstraintBuilder(IPrimaryKeyConstraint source)
        {
            Metadata = new List<MetadataBuilder>();
            Fields = new List<PrimaryKeyConstraintFieldBuilder>();

            Name = source.Name;
            if (source.Metadata != null) foreach (var x in source.Metadata) Metadata.Add(new MetadataBuilder(x));
            FileGroupName = source.FileGroupName;
            if (source.Fields != null) foreach (var x in source.Fields) Fields.Add(new PrimaryKeyConstraintFieldBuilder(x));
        }
    }
}
