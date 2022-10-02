﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.9
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
    public partial class ViewSourceBuilder
    {
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

        public ModelFramework.Database.Contracts.IViewSource Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            return new ModelFramework.Database.ViewSource(Alias?.ToString(), SourceSchemaName?.ToString(), SourceObjectName?.ToString(), Name?.ToString(), Metadata.Select(x => x.Build()));
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public ViewSourceBuilder WithAlias(System.Text.StringBuilder alias)
        {
            Alias = alias;
            return this;
        }

        public ViewSourceBuilder WithAlias(System.Func<System.Text.StringBuilder> aliasDelegate)
        {
            _aliasDelegate = new (aliasDelegate);
            return this;
        }

        public ViewSourceBuilder WithAlias(string value)
        {
            if (Alias == null)
                Alias = new System.Text.StringBuilder();
            Alias.Clear().Append(value);
            return this;
        }

        public ViewSourceBuilder AppendToAlias(string value)
        {
            if (Alias == null)
                Alias = new System.Text.StringBuilder();
            Alias.Append(value);
            return this;
        }

        public ViewSourceBuilder AppendLineToAlias(string value)
        {
            if (Alias == null)
                Alias = new System.Text.StringBuilder();
            Alias.AppendLine(value);
            return this;
        }

        public ViewSourceBuilder WithSourceSchemaName(System.Text.StringBuilder sourceSchemaName)
        {
            SourceSchemaName = sourceSchemaName;
            return this;
        }

        public ViewSourceBuilder WithSourceSchemaName(System.Func<System.Text.StringBuilder> sourceSchemaNameDelegate)
        {
            _sourceSchemaNameDelegate = new (sourceSchemaNameDelegate);
            return this;
        }

        public ViewSourceBuilder WithSourceSchemaName(string value)
        {
            if (SourceSchemaName == null)
                SourceSchemaName = new System.Text.StringBuilder();
            SourceSchemaName.Clear().Append(value);
            return this;
        }

        public ViewSourceBuilder AppendToSourceSchemaName(string value)
        {
            if (SourceSchemaName == null)
                SourceSchemaName = new System.Text.StringBuilder();
            SourceSchemaName.Append(value);
            return this;
        }

        public ViewSourceBuilder AppendLineToSourceSchemaName(string value)
        {
            if (SourceSchemaName == null)
                SourceSchemaName = new System.Text.StringBuilder();
            SourceSchemaName.AppendLine(value);
            return this;
        }

        public ViewSourceBuilder WithSourceObjectName(System.Text.StringBuilder sourceObjectName)
        {
            SourceObjectName = sourceObjectName;
            return this;
        }

        public ViewSourceBuilder WithSourceObjectName(System.Func<System.Text.StringBuilder> sourceObjectNameDelegate)
        {
            _sourceObjectNameDelegate = new (sourceObjectNameDelegate);
            return this;
        }

        public ViewSourceBuilder WithSourceObjectName(string value)
        {
            if (SourceObjectName == null)
                SourceObjectName = new System.Text.StringBuilder();
            SourceObjectName.Clear().Append(value);
            return this;
        }

        public ViewSourceBuilder AppendToSourceObjectName(string value)
        {
            if (SourceObjectName == null)
                SourceObjectName = new System.Text.StringBuilder();
            SourceObjectName.Append(value);
            return this;
        }

        public ViewSourceBuilder AppendLineToSourceObjectName(string value)
        {
            if (SourceObjectName == null)
                SourceObjectName = new System.Text.StringBuilder();
            SourceObjectName.AppendLine(value);
            return this;
        }

        public ViewSourceBuilder WithName(System.Text.StringBuilder name)
        {
            Name = name;
            return this;
        }

        public ViewSourceBuilder WithName(System.Func<System.Text.StringBuilder> nameDelegate)
        {
            _nameDelegate = new (nameDelegate);
            return this;
        }

        public ViewSourceBuilder WithName(string value)
        {
            if (Name == null)
                Name = new System.Text.StringBuilder();
            Name.Clear().Append(value);
            return this;
        }

        public ViewSourceBuilder AppendToName(string value)
        {
            if (Name == null)
                Name = new System.Text.StringBuilder();
            Name.Append(value);
            return this;
        }

        public ViewSourceBuilder AppendLineToName(string value)
        {
            if (Name == null)
                Name = new System.Text.StringBuilder();
            Name.AppendLine(value);
            return this;
        }

        public ViewSourceBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public ViewSourceBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public ViewSourceBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public ViewSourceBuilder()
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            #pragma warning disable CS8603 // Possible null reference return.
            _aliasDelegate = new (() => new System.Text.StringBuilder());
            _sourceSchemaNameDelegate = new (() => new System.Text.StringBuilder());
            _sourceObjectNameDelegate = new (() => new System.Text.StringBuilder());
            _nameDelegate = new (() => new System.Text.StringBuilder());
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public ViewSourceBuilder(ModelFramework.Database.Contracts.IViewSource source)
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            _aliasDelegate = new (() => new System.Text.StringBuilder(source.Alias));
            _sourceSchemaNameDelegate = new (() => new System.Text.StringBuilder(source.SourceSchemaName));
            _sourceObjectNameDelegate = new (() => new System.Text.StringBuilder(source.SourceObjectName));
            _nameDelegate = new (() => new System.Text.StringBuilder(source.Name));
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
        }

        protected System.Lazy<System.Text.StringBuilder> _aliasDelegate;

        protected System.Lazy<System.Text.StringBuilder> _sourceSchemaNameDelegate;

        protected System.Lazy<System.Text.StringBuilder> _sourceObjectNameDelegate;

        protected System.Lazy<System.Text.StringBuilder> _nameDelegate;
    }
#nullable restore
}

