using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Database.Builders
{
    public class DefaultValueConstraintBuilder
    {
        public string FieldName { get; set; }
        public string DefaultValue { get; set; }
        public string Name { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public DefaultValueConstraint Build()
        {
            return new DefaultValueConstraint(FieldName, DefaultValue, Name, Metadata.Select(x => x.Build()));
        }
        public DefaultValueConstraintBuilder Clear()
        {
            FieldName = default;
            DefaultValue = default;
            Name = default;
            Metadata.Clear();
            return this;
        }
        public DefaultValueConstraintBuilder Update(IDefaultValueConstraint source)
        {
            FieldName = default;
            DefaultValue = default;
            Name = default;
            Metadata = new List<MetadataBuilder>();
            if (source != null)
            {
                FieldName = source.FieldName;
                DefaultValue = source.DefaultValue;
                Name = source.Name;
                Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public DefaultValueConstraintBuilder WithFieldName(string fieldName)
        {
            FieldName = fieldName;
            return this;
        }
        public DefaultValueConstraintBuilder WithDefaultValue(string defaultValue)
        {
            DefaultValue = defaultValue;
            return this;
        }
        public DefaultValueConstraintBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public DefaultValueConstraintBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public DefaultValueConstraintBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public DefaultValueConstraintBuilder AddMetadata(params MetadataBuilder[] metadata)
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
        public DefaultValueConstraintBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public DefaultValueConstraintBuilder AddMetadata(params IMetadata[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public DefaultValueConstraintBuilder(IDefaultValueConstraint source = null)
        {
            Metadata = new List<MetadataBuilder>();
            if (source != null)
            {
                FieldName = source.FieldName;
                DefaultValue = source.DefaultValue;
                Name = source.Name;
                foreach (var x in source.Metadata) Metadata.Add(new MetadataBuilder(x));
            }
        }
        public DefaultValueConstraintBuilder(string fieldName,
                                             string defaultValue,
                                             string name,
                                             IEnumerable<IMetadata> metadata = null)
        {
            Metadata = new List<MetadataBuilder>();
            FieldName = fieldName;
            DefaultValue = defaultValue;
            Name = name;
            if (metadata != null) Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
        }
    }
}
