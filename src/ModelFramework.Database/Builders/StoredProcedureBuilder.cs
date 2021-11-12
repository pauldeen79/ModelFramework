﻿using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;

namespace ModelFramework.Database.Builders
{
    public class StoredProcedureBuilder
    {
        public string Body { get; set; }
        public List<StoredProcedureParameterBuilder> Parameters { get; set; }
        public List<ISqlStatementBuilder> Statements { get; set; }
        public string Name { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public IStoredProcedure Build()
        {
            return new StoredProcedure(Name, Body, Parameters.Select(x => x.Build()), Statements.Select(x => x.Build()), Metadata.Select(x => x.Build()));
        }
        public StoredProcedureBuilder Clear()
        {
            Body = default;
            Parameters.Clear();
            Statements.Clear();
            Name = default;
            Metadata.Clear();
            return this;
        }
        public StoredProcedureBuilder Update(IStoredProcedure source)
        {
            Body = default;
            Parameters = new List<StoredProcedureParameterBuilder>();
            Statements = new List<ISqlStatementBuilder>();
            Name = default;
            Metadata = new List<MetadataBuilder>();
            if (source != null)
            {
                Body = source.Body;
                if (source.Parameters != null) Parameters.AddRange(source.Parameters.Select(x => new StoredProcedureParameterBuilder(x)));
                if (source.Statements != null) Statements.AddRange(source.Statements.Select(x => x.CreateBuilder()));
                Name = source.Name;
                if (source.Metadata != null) Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));
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
        public StoredProcedureBuilder ClearStatements()
        {
            Statements.Clear();
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
        public StoredProcedureBuilder AddStatements(IEnumerable<ISqlStatementBuilder> statements)
        {
            return AddStatements(statements.ToArray());
        }
        public StoredProcedureBuilder AddStatements(params ISqlStatementBuilder[] statements)
        {
            if (statements != null)
            {
                foreach (var itemToAdd in statements)
                {
                    Statements.Add(itemToAdd);
                }
            }
            return this;
        }
        public StoredProcedureBuilder AddStatements(IEnumerable<ISqlStatement> statements)
        {
            return AddStatements(statements.ToArray());
        }
        public StoredProcedureBuilder AddStatements(params ISqlStatement[] statements)
        {
            if (statements != null)
            {
                foreach (var itemToAdd in statements)
                {
                    Statements.Add(itemToAdd.CreateBuilder());
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
#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high
        public StoredProcedureBuilder(IStoredProcedure source = null)
#pragma warning restore S3776 // Cognitive Complexity of methods should not be too high
        {
            Parameters = new List<StoredProcedureParameterBuilder>();
            Metadata = new List<MetadataBuilder>();
            if (source != null)
            {
                Body = source.Body;
                if (source.Parameters != null) foreach (var x in source.Parameters) Parameters.Add(new StoredProcedureParameterBuilder(x));
                if (source.Statements != null) foreach (var x in source.Statements) Statements.Add(x.CreateBuilder());
                Name = source.Name;
                if (source.Metadata != null) foreach (var x in source.Metadata) Metadata.Add(new MetadataBuilder(x));
            }
        }
        public StoredProcedureBuilder(string name,
                                      string body,
                                      IEnumerable<IStoredProcedureParameter> parameters = null,
                                      IEnumerable<ISqlStatement> statements = null,
                                      IEnumerable<IMetadata> metadata = null)
        {
            Parameters = new List<StoredProcedureParameterBuilder>();
            Statements = new List<ISqlStatementBuilder>();
            Metadata = new List<MetadataBuilder>();
            Name = name;
            Body = body;
            if (parameters != null) Parameters.AddRange(parameters.Select(x => new StoredProcedureParameterBuilder(x)));
            if (statements != null) Statements.AddRange(statements.Select(x => x.CreateBuilder()));
            if (metadata != null) Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
        }
    }
}
