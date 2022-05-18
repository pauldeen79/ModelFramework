﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.5
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
            get
            {
                return _nameDelegate.Value;
            }
            set
            {
                _nameDelegate = new (() => value);
            }
        }

        public System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder> Metadata
        {
            get;
            set;
        }

        public StoredProcedureBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public StoredProcedureBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public StoredProcedureBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public StoredProcedureBuilder AddParameters(params ModelFramework.Database.Builders.StoredProcedureParameterBuilder[] parameters)
        {
            Parameters.AddRange(parameters);
            return this;
        }

        public StoredProcedureBuilder AddParameters(System.Collections.Generic.IEnumerable<ModelFramework.Database.Builders.StoredProcedureParameterBuilder> parameters)
        {
            return AddParameters(parameters.ToArray());
        }

        public StoredProcedureBuilder AddStatements(params ModelFramework.Database.Contracts.ISqlStatementBuilder[] statements)
        {
            Statements.AddRange(statements);
            return this;
        }

        public StoredProcedureBuilder AddStatements(System.Collections.Generic.IEnumerable<ModelFramework.Database.Contracts.ISqlStatementBuilder> statements)
        {
            return AddStatements(statements.ToArray());
        }

        public ModelFramework.Database.Contracts.IStoredProcedure Build()
        {
            return new ModelFramework.Database.StoredProcedure(Statements.Select(x => x.Build()), Parameters.Select(x => x.Build()), Name, Metadata.Select(x => x.Build()));
        }

        public StoredProcedureBuilder WithName(System.Func<string> nameDelegate)
        {
            _nameDelegate = new (nameDelegate);
            return this;
        }

        public StoredProcedureBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public StoredProcedureBuilder()
        {
            Statements = new System.Collections.Generic.List<ModelFramework.Database.Contracts.ISqlStatementBuilder>();
            Parameters = new System.Collections.Generic.List<ModelFramework.Database.Builders.StoredProcedureParameterBuilder>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            _nameDelegate = new (() => string.Empty);
        }

        public StoredProcedureBuilder(ModelFramework.Database.Contracts.IStoredProcedure source)
        {
            Statements = new System.Collections.Generic.List<ModelFramework.Database.Contracts.ISqlStatementBuilder>();
            Parameters = new System.Collections.Generic.List<ModelFramework.Database.Builders.StoredProcedureParameterBuilder>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            Statements.AddRange(source.Statements.Select(x => x.CreateBuilder()));
            Parameters.AddRange(source.Parameters.Select(x => new ModelFramework.Database.Builders.StoredProcedureParameterBuilder(x)));
            _nameDelegate = new (() => source.Name);
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
        }

        private System.Lazy<string> _nameDelegate;
    }
#nullable restore
}

