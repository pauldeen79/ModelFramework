﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 7.0.11
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelFramework.Objects.CodeStatements.Builders
{
#nullable enable
    public partial class LiteralCodeStatementBuilder : ModelFramework.Objects.Contracts.ICodeStatementBuilder
    {
        public System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder> Metadata
        {
            get;
            set;
        }

        public System.Text.StringBuilder Statement
        {
            get
            {
                return _statementDelegate.Value;
            }
            set
            {
                _statementDelegate = new (() => value);
            }
        }

        public ModelFramework.Objects.Contracts.ICodeStatement Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new ModelFramework.Objects.CodeStatements.LiteralCodeStatement(Statement?.ToString(), Metadata.Select(x => x.Build()));
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public LiteralCodeStatementBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public LiteralCodeStatementBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public LiteralCodeStatementBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public LiteralCodeStatementBuilder WithStatement(System.Text.StringBuilder statement)
        {
            Statement = statement;
            return this;
        }

        public LiteralCodeStatementBuilder WithStatement(System.Func<System.Text.StringBuilder> statementDelegate)
        {
            _statementDelegate = new (statementDelegate);
            return this;
        }

        public LiteralCodeStatementBuilder WithStatement(string value)
        {
            if (Statement == null)
                Statement = new System.Text.StringBuilder();
            Statement.Clear().Append(value);
            return this;
        }

        public LiteralCodeStatementBuilder AppendToStatement(string value)
        {
            if (Statement == null)
                Statement = new System.Text.StringBuilder();
            Statement.Append(value);
            return this;
        }

        public LiteralCodeStatementBuilder AppendLineToStatement(string value)
        {
            if (Statement == null)
                Statement = new System.Text.StringBuilder();
            Statement.AppendLine(value);
            return this;
        }

        public LiteralCodeStatementBuilder()
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            #pragma warning disable CS8603 // Possible null reference return.
            _statementDelegate = new (() => new System.Text.StringBuilder());
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public LiteralCodeStatementBuilder(ModelFramework.Objects.CodeStatements.LiteralCodeStatement source)
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
            _statementDelegate = new (() => new System.Text.StringBuilder(source.Statement));
        }

        protected System.Lazy<System.Text.StringBuilder> _statementDelegate;
    }
#nullable restore
}

