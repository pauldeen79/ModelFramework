using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Database.Builders
{
    public class StoredProcedureParameterBuilder
    {
        public string Type { get; set; }
        public string DefaultValue { get; set; }
        public string Name { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public IStoredProcedureParameter Build()
        {
            return new StoredProcedureParameter(Name, Type, DefaultValue, Metadata.Select(x => x.Build()));
        }
        public StoredProcedureParameterBuilder WithType(string type)
        {
            Type = type;
            return this;
        }
        public StoredProcedureParameterBuilder WithDefaultValue(string defaultValue)
        {
            DefaultValue = defaultValue;
            return this;
        }
        public StoredProcedureParameterBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public StoredProcedureParameterBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public StoredProcedureParameterBuilder AddMetadata(params MetadataBuilder[] metadata)
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
        public StoredProcedureParameterBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public StoredProcedureParameterBuilder AddMetadata(params IMetadata[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public StoredProcedureParameterBuilder(IStoredProcedureParameter source = null)
        {
            Metadata = new List<MetadataBuilder>();
            if (source != null)
            {
                Type = source.Type;
                DefaultValue = source.DefaultValue;
                Name = source.Name;
                foreach (var x in source.Metadata) Metadata.Add(new MetadataBuilder(x));
            }
        }
        public StoredProcedureParameterBuilder(string name,
                                               string type,
                                               string defaultValue,
                                               IEnumerable<IMetadata> metadata = null)
        {
            Metadata = new List<MetadataBuilder>();
            Name = name;
            Type = type;
            DefaultValue = defaultValue;
            if (metadata != null) Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
        }
    }
}
