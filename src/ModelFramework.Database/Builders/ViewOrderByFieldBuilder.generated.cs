﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 7.0.5
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

        public System.Text.StringBuilder SourceSchemaName
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

        public System.Text.StringBuilder SourceObjectName
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

        public System.Text.StringBuilder Expression
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

        public System.Text.StringBuilder Alias
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

        public System.Text.StringBuilder Name
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

        public ModelFramework.Database.Contracts.IViewOrderByField Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new ModelFramework.Database.ViewOrderByField(IsDescending, SourceSchemaName?.ToString(), SourceObjectName?.ToString(), Expression?.ToString(), Alias?.ToString(), Name?.ToString(), Metadata.Select(x => x.Build()));
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
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

        public ViewOrderByFieldBuilder WithSourceSchemaName(System.Text.StringBuilder sourceSchemaName)
        {
            SourceSchemaName = sourceSchemaName;
            return this;
        }

        public ViewOrderByFieldBuilder WithSourceSchemaName(System.Func<System.Text.StringBuilder> sourceSchemaNameDelegate)
        {
            _sourceSchemaNameDelegate = new (sourceSchemaNameDelegate);
            return this;
        }

        public ViewOrderByFieldBuilder WithSourceSchemaName(string value)
        {
            if (SourceSchemaName == null)
                SourceSchemaName = new System.Text.StringBuilder();
            SourceSchemaName.Clear().Append(value);
            return this;
        }

        public ViewOrderByFieldBuilder AppendToSourceSchemaName(string value)
        {
            if (SourceSchemaName == null)
                SourceSchemaName = new System.Text.StringBuilder();
            SourceSchemaName.Append(value);
            return this;
        }

        public ViewOrderByFieldBuilder AppendLineToSourceSchemaName(string value)
        {
            if (SourceSchemaName == null)
                SourceSchemaName = new System.Text.StringBuilder();
            SourceSchemaName.AppendLine(value);
            return this;
        }

        public ViewOrderByFieldBuilder WithSourceObjectName(System.Text.StringBuilder sourceObjectName)
        {
            SourceObjectName = sourceObjectName;
            return this;
        }

        public ViewOrderByFieldBuilder WithSourceObjectName(System.Func<System.Text.StringBuilder> sourceObjectNameDelegate)
        {
            _sourceObjectNameDelegate = new (sourceObjectNameDelegate);
            return this;
        }

        public ViewOrderByFieldBuilder WithSourceObjectName(string value)
        {
            if (SourceObjectName == null)
                SourceObjectName = new System.Text.StringBuilder();
            SourceObjectName.Clear().Append(value);
            return this;
        }

        public ViewOrderByFieldBuilder AppendToSourceObjectName(string value)
        {
            if (SourceObjectName == null)
                SourceObjectName = new System.Text.StringBuilder();
            SourceObjectName.Append(value);
            return this;
        }

        public ViewOrderByFieldBuilder AppendLineToSourceObjectName(string value)
        {
            if (SourceObjectName == null)
                SourceObjectName = new System.Text.StringBuilder();
            SourceObjectName.AppendLine(value);
            return this;
        }

        public ViewOrderByFieldBuilder WithExpression(System.Text.StringBuilder expression)
        {
            Expression = expression;
            return this;
        }

        public ViewOrderByFieldBuilder WithExpression(System.Func<System.Text.StringBuilder> expressionDelegate)
        {
            _expressionDelegate = new (expressionDelegate);
            return this;
        }

        public ViewOrderByFieldBuilder WithExpression(string value)
        {
            if (Expression == null)
                Expression = new System.Text.StringBuilder();
            Expression.Clear().Append(value);
            return this;
        }

        public ViewOrderByFieldBuilder AppendToExpression(string value)
        {
            if (Expression == null)
                Expression = new System.Text.StringBuilder();
            Expression.Append(value);
            return this;
        }

        public ViewOrderByFieldBuilder AppendLineToExpression(string value)
        {
            if (Expression == null)
                Expression = new System.Text.StringBuilder();
            Expression.AppendLine(value);
            return this;
        }

        public ViewOrderByFieldBuilder WithAlias(System.Text.StringBuilder alias)
        {
            Alias = alias;
            return this;
        }

        public ViewOrderByFieldBuilder WithAlias(System.Func<System.Text.StringBuilder> aliasDelegate)
        {
            _aliasDelegate = new (aliasDelegate);
            return this;
        }

        public ViewOrderByFieldBuilder WithAlias(string value)
        {
            if (Alias == null)
                Alias = new System.Text.StringBuilder();
            Alias.Clear().Append(value);
            return this;
        }

        public ViewOrderByFieldBuilder AppendToAlias(string value)
        {
            if (Alias == null)
                Alias = new System.Text.StringBuilder();
            Alias.Append(value);
            return this;
        }

        public ViewOrderByFieldBuilder AppendLineToAlias(string value)
        {
            if (Alias == null)
                Alias = new System.Text.StringBuilder();
            Alias.AppendLine(value);
            return this;
        }

        public ViewOrderByFieldBuilder WithName(System.Text.StringBuilder name)
        {
            Name = name;
            return this;
        }

        public ViewOrderByFieldBuilder WithName(System.Func<System.Text.StringBuilder> nameDelegate)
        {
            _nameDelegate = new (nameDelegate);
            return this;
        }

        public ViewOrderByFieldBuilder WithName(string value)
        {
            if (Name == null)
                Name = new System.Text.StringBuilder();
            Name.Clear().Append(value);
            return this;
        }

        public ViewOrderByFieldBuilder AppendToName(string value)
        {
            if (Name == null)
                Name = new System.Text.StringBuilder();
            Name.Append(value);
            return this;
        }

        public ViewOrderByFieldBuilder AppendLineToName(string value)
        {
            if (Name == null)
                Name = new System.Text.StringBuilder();
            Name.AppendLine(value);
            return this;
        }

        public ViewOrderByFieldBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public ViewOrderByFieldBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public ViewOrderByFieldBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public ViewOrderByFieldBuilder()
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            #pragma warning disable CS8603 // Possible null reference return.
            _isDescendingDelegate = new (() => default(bool));
            _sourceSchemaNameDelegate = new (() => new System.Text.StringBuilder());
            _sourceObjectNameDelegate = new (() => new System.Text.StringBuilder());
            _expressionDelegate = new (() => new System.Text.StringBuilder());
            _aliasDelegate = new (() => new System.Text.StringBuilder());
            _nameDelegate = new (() => new System.Text.StringBuilder());
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public ViewOrderByFieldBuilder(ModelFramework.Database.Contracts.IViewOrderByField source)
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            _isDescendingDelegate = new (() => source.IsDescending);
            _sourceSchemaNameDelegate = new (() => new System.Text.StringBuilder(source.SourceSchemaName));
            _sourceObjectNameDelegate = new (() => new System.Text.StringBuilder(source.SourceObjectName));
            _expressionDelegate = new (() => new System.Text.StringBuilder(source.Expression));
            _aliasDelegate = new (() => new System.Text.StringBuilder(source.Alias));
            _nameDelegate = new (() => new System.Text.StringBuilder(source.Name));
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
        }

        protected System.Lazy<bool> _isDescendingDelegate;

        protected System.Lazy<System.Text.StringBuilder> _sourceSchemaNameDelegate;

        protected System.Lazy<System.Text.StringBuilder> _sourceObjectNameDelegate;

        protected System.Lazy<System.Text.StringBuilder> _expressionDelegate;

        protected System.Lazy<System.Text.StringBuilder> _aliasDelegate;

        protected System.Lazy<System.Text.StringBuilder> _nameDelegate;
    }
#nullable restore
}

