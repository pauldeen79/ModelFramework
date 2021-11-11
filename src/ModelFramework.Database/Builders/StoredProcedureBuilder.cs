using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Database.Builders
{
    public class StoredProcedureBuilder
    {
        public string Body { get; set; }
        public List<StoredProcedureParameterBuilder> Parameters { get; set; }
        public string Name { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public IStoredProcedure Build()
        {
            return new StoredProcedure(Name, Body, Parameters.Select(x => x.Build()), Metadata.Select(x => x.Build()));
        }
        public StoredProcedureBuilder Clear()
        {
            Body = default;
            Parameters.Clear();
            Name = default;
            Metadata.Clear();
            return this;
        }
        public StoredProcedureBuilder Update(IStoredProcedure source)
        {
            Body = default;
            Parameters = new List<StoredProcedureParameterBuilder>();
            Name = default;
            Metadata = new List<MetadataBuilder>();
            if (source != null)
            {
                Body = source.Body;
                Parameters.AddRange(source.Parameters.Select(x => new StoredProcedureParameterBuilder(x)));
                Name = source.Name;
                Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public StoredProcedureBuilder WithBody(string body)
        {
            Body = body;
            return this;
        }
        public StoredProcedureBuilder ClearParameters()
        {
            Parameters.Clear();
            return this;
        }
        public StoredProcedureBuilder AddParameters(IEnumerable<StoredProcedureParameterBuilder> parameters)
        {
            return AddParameters(parameters.ToArray());
        }
        public StoredProcedureBuilder AddParameters(params StoredProcedureParameterBuilder[] parameters)
        {
            if (parameters != null)
            {
                foreach (var itemToAdd in parameters)
                {
                    Parameters.Add(itemToAdd);
                }
            }
            return this;
        }
        public StoredProcedureBuilder AddParameters(IEnumerable<IStoredProcedureParameter> parameters)
        {
            return AddParameters(parameters.ToArray());
        }
        public StoredProcedureBuilder AddParameters(params IStoredProcedureParameter[] parameters)
        {
            if (parameters != null)
            {
                foreach (var itemToAdd in parameters)
                {
                    Parameters.Add(new StoredProcedureParameterBuilder(itemToAdd));
                }
            }
            return this;
        }
        public StoredProcedureBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public StoredProcedureBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public StoredProcedureBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public StoredProcedureBuilder AddMetadata(params MetadataBuilder[] metadata)
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
        public StoredProcedureBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public StoredProcedureBuilder AddMetadata(params IMetadata[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public StoredProcedureBuilder(IStoredProcedure source = null)
        {
            Parameters = new List<StoredProcedureParameterBuilder>();
            Metadata = new List<MetadataBuilder>();
            if (source != null)
            {
                Body = source.Body;
                foreach (var x in source.Parameters) Parameters.Add(new StoredProcedureParameterBuilder(x));
                Name = source.Name;
                foreach (var x in source.Metadata) Metadata.Add(new MetadataBuilder(x));
            }
        }
        public StoredProcedureBuilder(string name,
                                      string body,
                                      IEnumerable<IStoredProcedureParameter> parameters = null,
                                      IEnumerable<IMetadata> metadata = null)
        {
            Parameters = new List<StoredProcedureParameterBuilder>();
            Metadata = new List<MetadataBuilder>();
            Name = name;
            Body = body;
            if (parameters != null) Parameters.AddRange(parameters.Select(x => new StoredProcedureParameterBuilder(x)));
            if (metadata != null) Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
        }
    }
}
