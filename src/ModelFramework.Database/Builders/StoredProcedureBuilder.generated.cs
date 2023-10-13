﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 7.0.12
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelFramework.Database.Builders
{
#nullable enable
    public partial class StoredProcedureBuilder
    {
        public System.Collections.Generic.List<ModelFramework.Database.Contracts.ISqlStatementBuilder> Statements
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Database.Builders.StoredProcedureParameterBuilder> Parameters
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder> Metadata
        {
            get;
            set;
        }

        public ModelFramework.Database.Contracts.IStoredProcedure Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new ModelFramework.Database.StoredProcedure(Statements.Select(x => x.Build()), Parameters.Select(x => x.Build()), Name, Metadata.Select(x => x.Build()));
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public StoredProcedureBuilder AddStatements(System.Collections.Generic.IEnumerable<ModelFramework.Database.Contracts.ISqlStatementBuilder> statements)
        {
            return AddStatements(statements.ToArray());
        }

        public StoredProcedureBuilder AddStatements(params ModelFramework.Database.Contracts.ISqlStatementBuilder[] statements)
        {
            Statements.AddRange(statements);
            return this;
        }

        public StoredProcedureBuilder AddParameters(System.Collections.Generic.IEnumerable<ModelFramework.Database.Builders.StoredProcedureParameterBuilder> parameters)
        {
            return AddParameters(parameters.ToArray());
        }

        public StoredProcedureBuilder AddParameters(params ModelFramework.Database.Builders.StoredProcedureParameterBuilder[] parameters)
        {
            Parameters.AddRange(parameters);
            return this;
        }

        public StoredProcedureBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public StoredProcedureBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public StoredProcedureBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public StoredProcedureBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public StoredProcedureBuilder()
        {
            Statements = new System.Collections.Generic.List<ModelFramework.Database.Contracts.ISqlStatementBuilder>();
            Parameters = new System.Collections.Generic.List<ModelFramework.Database.Builders.StoredProcedureParameterBuilder>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            Name = string.Empty;
        }

        public StoredProcedureBuilder(ModelFramework.Database.Contracts.IStoredProcedure source)
        {
            Statements = new System.Collections.Generic.List<ModelFramework.Database.Contracts.ISqlStatementBuilder>();
            Parameters = new System.Collections.Generic.List<ModelFramework.Database.Builders.StoredProcedureParameterBuilder>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            Statements.AddRange(source.Statements.Select(x => x.CreateBuilder()));
            Parameters.AddRange(source.Parameters.Select(x => new ModelFramework.Database.Builders.StoredProcedureParameterBuilder(x)));
            Name = source.Name;
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
        }
    }
#nullable restore
}

