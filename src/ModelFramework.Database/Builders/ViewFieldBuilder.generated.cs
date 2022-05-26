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
    public partial class ViewFieldBuilder
    {
        public string SourceSchemaName
        {
            get
            {
                return _sourceSchemaNameDelegate.Value;
            }
            set
            {
                _sourceSchemaNameDelegate = new (() => value);
            }
        }

        public string SourceObjectName
        {
            get
            {
                return _sourceObjectNameDelegate.Value;
            }
            set
            {
                _sourceObjectNameDelegate = new (() => value);
            }
        }

        public string Expression
        {
            get
            {
                return _expressionDelegate.Value;
            }
            set
            {
                _expressionDelegate = new (() => value);
            }
        }

        public string Alias
        {
            get
            {
                return _aliasDelegate.Value;
            }
            set
            {
                _aliasDelegate = new (() => value);
            }
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

        public ViewFieldBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public ViewFieldBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public ViewFieldBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public ModelFramework.Database.Contracts.IViewField Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            return new ModelFramework.Database.ViewField(SourceSchemaName, SourceObjectName, Expression, Alias, Name, Metadata.Select(x => x.Build()));
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public ViewFieldBuilder WithAlias(System.Func<string> aliasDelegate)
        {
            _aliasDelegate = new (aliasDelegate);
            return this;
        }

        public ViewFieldBuilder WithAlias(string alias)
        {
            Alias = alias;
            return this;
        }

        public ViewFieldBuilder WithExpression(System.Func<string> expressionDelegate)
        {
            _expressionDelegate = new (expressionDelegate);
            return this;
        }

        public ViewFieldBuilder WithExpression(string expression)
        {
            Expression = expression;
            return this;
        }

        public ViewFieldBuilder WithName(System.Func<string> nameDelegate)
        {
            _nameDelegate = new (nameDelegate);
            return this;
        }

        public ViewFieldBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public ViewFieldBuilder WithSourceObjectName(System.Func<string> sourceObjectNameDelegate)
        {
            _sourceObjectNameDelegate = new (sourceObjectNameDelegate);
            return this;
        }

        public ViewFieldBuilder WithSourceObjectName(string sourceObjectName)
        {
            SourceObjectName = sourceObjectName;
            return this;
        }

        public ViewFieldBuilder WithSourceSchemaName(System.Func<string> sourceSchemaNameDelegate)
        {
            _sourceSchemaNameDelegate = new (sourceSchemaNameDelegate);
            return this;
        }

        public ViewFieldBuilder WithSourceSchemaName(string sourceSchemaName)
        {
            SourceSchemaName = sourceSchemaName;
            return this;
        }

        public ViewFieldBuilder()
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            #pragma warning disable CS8603 // Possible null reference return.
            _sourceSchemaNameDelegate = new (() => string.Empty);
            _sourceObjectNameDelegate = new (() => string.Empty);
            _expressionDelegate = new (() => string.Empty);
            _aliasDelegate = new (() => string.Empty);
            _nameDelegate = new (() => string.Empty);
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public ViewFieldBuilder(ModelFramework.Database.Contracts.IViewField source)
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            _sourceSchemaNameDelegate = new (() => source.SourceSchemaName);
            _sourceObjectNameDelegate = new (() => source.SourceObjectName);
            _expressionDelegate = new (() => source.Expression);
            _aliasDelegate = new (() => source.Alias);
            _nameDelegate = new (() => source.Name);
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
        }

        private System.Lazy<string> _sourceSchemaNameDelegate;

        private System.Lazy<string> _sourceObjectNameDelegate;

        private System.Lazy<string> _expressionDelegate;

        private System.Lazy<string> _aliasDelegate;

        private System.Lazy<string> _nameDelegate;
    }
#nullable restore
}

