using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;

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
            return new DefaultValueConstraint(FieldName,
                                              DefaultValue,
                                              Name,
                                              Metadata.Select(x => x.Build()));
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
            Metadata = new List<MetadataBuilder>();

            FieldName = source.FieldName;
            DefaultValue = source.DefaultValue;
            Name = source.Name;
            if (source.Metadata != null) Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));

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
        public DefaultValueConstraintBuilder()
        {
            Metadata = new List<MetadataBuilder>();
        }
        public DefaultValueConstraintBuilder(IDefaultValueConstraint source)
        {
            Metadata = new List<MetadataBuilder>();
            FieldName = source.FieldName;
            DefaultValue = source.DefaultValue;
            Name = source.Name;
            if (source.Metadata != null) foreach (var x in source.Metadata) Metadata.Add(new MetadataBuilder(x));
        }
    }
}
