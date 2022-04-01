﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.3
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
    public partial class ViewOrderByFieldBuilder
    {
        public bool IsDescending
        {
            get
            {
                return _isDescendingDelegate.Value;
            }
            set
            {
                _isDescendingDelegate = new (() => value);
            }
        }

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

        public ViewOrderByFieldBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public ViewOrderByFieldBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public ViewOrderByFieldBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public ModelFramework.Database.Contracts.IViewOrderByField Build()
        {
            return new ModelFramework.Database.ViewOrderByField(IsDescending, SourceSchemaName, SourceObjectName, Expression, Alias, Name, Metadata.Select(x => x.Build()));
        }

        public ViewOrderByFieldBuilder WithAlias(System.Func<string> aliasDelegate)
        {
            _aliasDelegate = new (aliasDelegate);
            return this;
        }

        public ViewOrderByFieldBuilder WithAlias(string alias)
        {
            Alias = alias;
            return this;
        }

        public ViewOrderByFieldBuilder WithExpression(System.Func<string> expressionDelegate)
        {
            _expressionDelegate = new (expressionDelegate);
            return this;
        }

        public ViewOrderByFieldBuilder WithExpression(string expression)
        {
            Expression = expression;
            return this;
        }

        public ViewOrderByFieldBuilder WithIsDescending(bool isDescending = true)
        {
            IsDescending = isDescending;
            return this;
        }

        public ViewOrderByFieldBuilder WithIsDescending(System.Func<bool> isDescendingDelegate)
        {
            _isDescendingDelegate = new (isDescendingDelegate);
            return this;
        }

        public ViewOrderByFieldBuilder WithName(System.Func<string> nameDelegate)
        {
            _nameDelegate = new (nameDelegate);
            return this;
        }

        public ViewOrderByFieldBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public ViewOrderByFieldBuilder WithSourceObjectName(System.Func<string> sourceObjectNameDelegate)
        {
            _sourceObjectNameDelegate = new (sourceObjectNameDelegate);
            return this;
        }

        public ViewOrderByFieldBuilder WithSourceObjectName(string sourceObjectName)
        {
            SourceObjectName = sourceObjectName;
            return this;
        }

        public ViewOrderByFieldBuilder WithSourceSchemaName(System.Func<string> sourceSchemaNameDelegate)
        {
            _sourceSchemaNameDelegate = new (sourceSchemaNameDelegate);
            return this;
        }

        public ViewOrderByFieldBuilder WithSourceSchemaName(string sourceSchemaName)
        {
            SourceSchemaName = sourceSchemaName;
            return this;
        }

        public ViewOrderByFieldBuilder()
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            _isDescendingDelegate = new (() => default);
            _sourceSchemaNameDelegate = new (() => string.Empty);
            _sourceObjectNameDelegate = new (() => string.Empty);
            _expressionDelegate = new (() => string.Empty);
            _aliasDelegate = new (() => string.Empty);
            _nameDelegate = new (() => string.Empty);
        }

        public ViewOrderByFieldBuilder(ModelFramework.Database.Contracts.IViewOrderByField source)
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            _isDescendingDelegate = new (() => source.IsDescending);
            _sourceSchemaNameDelegate = new (() => source.SourceSchemaName);
            _sourceObjectNameDelegate = new (() => source.SourceObjectName);
            _expressionDelegate = new (() => source.Expression);
            _aliasDelegate = new (() => source.Alias);
            _nameDelegate = new (() => source.Name);
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
        }

        private System.Lazy<bool> _isDescendingDelegate;

        private System.Lazy<string> _sourceSchemaNameDelegate;

        private System.Lazy<string> _sourceObjectNameDelegate;

        private System.Lazy<string> _expressionDelegate;

        private System.Lazy<string> _aliasDelegate;

        private System.Lazy<string> _nameDelegate;
    }
#nullable restore
}

