using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;

namespace ModelFramework.Database.Builders
{
    public class StoredProcedureBuilder
    {
        public List<StoredProcedureParameterBuilder> Parameters { get; set; }
        public List<ISqlStatementBuilder> Statements { get; set; }
        public string Name { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public IStoredProcedure Build()
        {
            return new StoredProcedure(Name,
                                       Parameters.Select(x => x.Build()),
                                       Statements.Select(x => x.Build()),
                                       Metadata.Select(x => x.Build()));
        }
        public StoredProcedureBuilder Clear()
        {
            Parameters.Clear();
            Statements.Clear();
            Metadata.Clear();
            Name = string.Empty;
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
            foreach (var itemToAdd in parameters)
            {
                Parameters.Add(itemToAdd);
            }
            return this;
        }
        public StoredProcedureBuilder AddParameters(IEnumerable<IStoredProcedureParameter> parameters)
        {
            return AddParameters(parameters.ToArray());
        }
        public StoredProcedureBuilder AddParameters(params IStoredProcedureParameter[] parameters)
        {
            foreach (var itemToAdd in parameters)
            {
                Parameters.Add(new StoredProcedureParameterBuilder(itemToAdd));
            }
            return this;
        }
        public StoredProcedureBuilder AddStatements(IEnumerable<ISqlStatementBuilder> statements)
        {
            return AddStatements(statements.ToArray());
        }
        public StoredProcedureBuilder AddStatements(params ISqlStatementBuilder[] statements)
        {
            foreach (var itemToAdd in statements)
            {
                Statements.Add(itemToAdd);
            }
            return this;
        }
        public StoredProcedureBuilder AddStatements(IEnumerable<ISqlStatement> statements)
        {
            return AddStatements(statements.ToArray());
        }
        public StoredProcedureBuilder AddStatements(params ISqlStatement[] statements)
        {
            foreach (var itemToAdd in statements)
            {
                Statements.Add(itemToAdd.CreateBuilder());
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
            foreach (var itemToAdd in metadata)
            {
                Metadata.Add(itemToAdd);
            }
            return this;
        }
        public StoredProcedureBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public StoredProcedureBuilder AddMetadata(params IMetadata[] metadata)
        {
            Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            return this;
        }
        public StoredProcedureBuilder()
        {
            Name = string.Empty;
            Parameters = new List<StoredProcedureParameterBuilder>();
            Metadata = new List<MetadataBuilder>();
            Statements = new List<ISqlStatementBuilder>();
        }
        public StoredProcedureBuilder(IStoredProcedure source)
        {
            Parameters = new List<StoredProcedureParameterBuilder>();
            Metadata = new List<MetadataBuilder>();
            Statements = new List<ISqlStatementBuilder>();

            foreach (var x in source.Parameters) Parameters.Add(new StoredProcedureParameterBuilder(x));
            foreach (var x in source.Statements) Statements.Add(x.CreateBuilder());
            Name = source.Name;
            foreach (var x in source.Metadata) Metadata.Add(new MetadataBuilder(x));
        }
    }
}
