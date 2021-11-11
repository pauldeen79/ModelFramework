using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Database.Builders
{
    public class UniqueConstraintBuilder
    {
        public string Name { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public string FileGroupName { get; set; }
        public List<UniqueConstraintFieldBuilder> Fields { get; set; }
        public IUniqueConstraint Build()
        {
            return new UniqueConstraint(Name, FileGroupName, Fields.Select(x => x.Build()), Metadata.Select(x => x.Build()));
        }
        public UniqueConstraintBuilder Clear()
        {
            Name = default;
            Metadata.Clear();
            FileGroupName = default;
            Fields.Clear();
            return this;
        }
        public UniqueConstraintBuilder Update(IUniqueConstraint source)
        {
            Name = default;
            Metadata = new List<MetadataBuilder>();
            FileGroupName = default;
            Fields = new List<UniqueConstraintFieldBuilder>();
            if (source != null)
            {
                Name = source.Name;
                Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));
                FileGroupName = source.FileGroupName;
                Fields.AddRange(source.Fields.Select(x => new UniqueConstraintFieldBuilder(x)));
            }
            return this;
        }
        public UniqueConstraintBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public UniqueConstraintBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public UniqueConstraintBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public UniqueConstraintBuilder AddMetadata(params MetadataBuilder[] metadata)
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
        public UniqueConstraintBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public UniqueConstraintBuilder AddMetadata(params IMetadata[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public UniqueConstraintBuilder WithFileGroupName(string fileGroupName)
        {
            FileGroupName = fileGroupName;
            return this;
        }
        public UniqueConstraintBuilder ClearFields()
        {
            Fields.Clear();
            return this;
        }
        public UniqueConstraintBuilder AddFields(IEnumerable<UniqueConstraintFieldBuilder> fields)
        {
            return AddFields(fields.ToArray());
        }
        public UniqueConstraintBuilder AddFields(params UniqueConstraintFieldBuilder[] fields)
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
        public UniqueConstraintBuilder AddFields(IEnumerable<IUniqueConstraintField> fields)
        {
            return AddFields(fields.ToArray());
        }
        public UniqueConstraintBuilder AddFields(params IUniqueConstraintField[] fields)
        {
            if (fields != null)
            {
                foreach (var itemToAdd in fields)
                {
                    Fields.Add(new UniqueConstraintFieldBuilder(itemToAdd));
                }
            }
            return this;
        }
        public UniqueConstraintBuilder(IUniqueConstraint source = null)
        {
            Metadata = new List<MetadataBuilder>();
            Fields = new List<UniqueConstraintFieldBuilder>();
            if (source != null)
            {
                Name = source.Name;
                foreach (var x in source.Metadata) Metadata.Add(new MetadataBuilder(x));
                FileGroupName = source.FileGroupName;
                foreach (var x in source.Fields) Fields.Add(new UniqueConstraintFieldBuilder(x));
            }
        }
        public UniqueConstraintBuilder(string name,
                                       string fileGroupName = null,
                                       IEnumerable<IUniqueConstraintField> fields = null,
                                       IReadOnlyCollection<IMetadata> metadata = null)
        {
            Metadata = new List<MetadataBuilder>();
            Fields = new List<UniqueConstraintFieldBuilder>();
            Name = name;
            FileGroupName = fileGroupName;
            if (fields != null) Fields.AddRange(fields.Select(x => new UniqueConstraintFieldBuilder(x)));
            if (metadata != null) Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
        }
    }
}
