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
    public partial class IndexBuilder
    {
        public System.Collections.Generic.List<ModelFramework.Database.Builders.IndexFieldBuilder> Fields
        {
            get;
            set;
        }

        public bool Unique
        {
            get
            {
                return _uniqueDelegate.Value;
            }
            set
            {
                _uniqueDelegate = new (() => value);
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

        public System.Text.StringBuilder FileGroupName
        {
            get
            {
                return _fileGroupNameDelegate.Value;
            }
            set
            {
                _fileGroupNameDelegate = new (() => value);
            }
        }

        public ModelFramework.Database.Contracts.IIndex Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            return new ModelFramework.Database.Index(Fields.Select(x => x.Build()), Unique, Name?.ToString(), Metadata.Select(x => x.Build()), FileGroupName?.ToString());
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public IndexBuilder AddFields(System.Collections.Generic.IEnumerable<ModelFramework.Database.Builders.IndexFieldBuilder> fields)
        {
            return AddFields(fields.ToArray());
        }

        public IndexBuilder AddFields(params ModelFramework.Database.Builders.IndexFieldBuilder[] fields)
        {
            Fields.AddRange(fields);
            return this;
        }

        public IndexBuilder WithUnique(bool unique = true)
        {
            Unique = unique;
            return this;
        }

        public IndexBuilder WithUnique(System.Func<bool> uniqueDelegate)
        {
            _uniqueDelegate = new (uniqueDelegate);
            return this;
        }

        public IndexBuilder WithName(System.Text.StringBuilder name)
        {
            Name = name;
            return this;
        }

        public IndexBuilder WithName(System.Func<System.Text.StringBuilder> nameDelegate)
        {
            _nameDelegate = new (nameDelegate);
            return this;
        }

        public IndexBuilder WithName(string value)
        {
            if (Name == null)
                Name = new System.Text.StringBuilder();
            Name.Clear().Append(value);
            return this;
        }

        public IndexBuilder AppendToName(string value)
        {
            if (Name == null)
                Name = new System.Text.StringBuilder();
            Name.Append(value);
            return this;
        }

        public IndexBuilder AppendLineToName(string value)
        {
            if (Name == null)
                Name = new System.Text.StringBuilder();
            Name.AppendLine(value);
            return this;
        }

        public IndexBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public IndexBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public IndexBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public IndexBuilder WithFileGroupName(System.Text.StringBuilder fileGroupName)
        {
            FileGroupName = fileGroupName;
            return this;
        }

        public IndexBuilder WithFileGroupName(System.Func<System.Text.StringBuilder> fileGroupNameDelegate)
        {
            _fileGroupNameDelegate = new (fileGroupNameDelegate);
            return this;
        }

        public IndexBuilder WithFileGroupName(string value)
        {
            if (FileGroupName == null)
                FileGroupName = new System.Text.StringBuilder();
            FileGroupName.Clear().Append(value);
            return this;
        }

        public IndexBuilder AppendToFileGroupName(string value)
        {
            if (FileGroupName == null)
                FileGroupName = new System.Text.StringBuilder();
            FileGroupName.Append(value);
            return this;
        }

        public IndexBuilder AppendLineToFileGroupName(string value)
        {
            if (FileGroupName == null)
                FileGroupName = new System.Text.StringBuilder();
            FileGroupName.AppendLine(value);
            return this;
        }

        public IndexBuilder()
        {
            Fields = new System.Collections.Generic.List<ModelFramework.Database.Builders.IndexFieldBuilder>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            #pragma warning disable CS8603 // Possible null reference return.
            _uniqueDelegate = new (() => default);
            _nameDelegate = new (() => new System.Text.StringBuilder());
            _fileGroupNameDelegate = new (() => new System.Text.StringBuilder());
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public IndexBuilder(ModelFramework.Database.Contracts.IIndex source)
        {
            Fields = new System.Collections.Generic.List<ModelFramework.Database.Builders.IndexFieldBuilder>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            Fields.AddRange(source.Fields.Select(x => new ModelFramework.Database.Builders.IndexFieldBuilder(x)));
            _uniqueDelegate = new (() => source.Unique);
            _nameDelegate = new (() => new System.Text.StringBuilder(source.Name));
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
            _fileGroupNameDelegate = new (() => new System.Text.StringBuilder(source.FileGroupName));
        }

        protected System.Lazy<bool> _uniqueDelegate;

        protected System.Lazy<System.Text.StringBuilder> _nameDelegate;

        protected System.Lazy<System.Text.StringBuilder> _fileGroupNameDelegate;
    }
#nullable restore
}

