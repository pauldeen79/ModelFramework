﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.6
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
    public partial class UniqueConstraintBuilder
    {
        public System.Collections.Generic.List<ModelFramework.Database.Builders.UniqueConstraintFieldBuilder> Fields
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

        public string FileGroupName
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

        public UniqueConstraintBuilder AddFields(params ModelFramework.Database.Builders.UniqueConstraintFieldBuilder[] fields)
        {
            Fields.AddRange(fields);
            return this;
        }

        public UniqueConstraintBuilder AddFields(System.Collections.Generic.IEnumerable<ModelFramework.Database.Builders.UniqueConstraintFieldBuilder> fields)
        {
            return AddFields(fields.ToArray());
        }

        public UniqueConstraintBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public UniqueConstraintBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public UniqueConstraintBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public ModelFramework.Database.Contracts.IUniqueConstraint Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            return new ModelFramework.Database.UniqueConstraint(Fields.Select(x => x.Build()), Name, Metadata.Select(x => x.Build()), FileGroupName);
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public UniqueConstraintBuilder WithFileGroupName(System.Func<string> fileGroupNameDelegate)
        {
            _fileGroupNameDelegate = new (fileGroupNameDelegate);
            return this;
        }

        public UniqueConstraintBuilder WithFileGroupName(string fileGroupName)
        {
            FileGroupName = fileGroupName;
            return this;
        }

        public UniqueConstraintBuilder WithName(System.Func<string> nameDelegate)
        {
            _nameDelegate = new (nameDelegate);
            return this;
        }

        public UniqueConstraintBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public UniqueConstraintBuilder()
        {
            Fields = new System.Collections.Generic.List<ModelFramework.Database.Builders.UniqueConstraintFieldBuilder>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            #pragma warning disable CS8603 // Possible null reference return.
            _nameDelegate = new (() => string.Empty);
            _fileGroupNameDelegate = new (() => string.Empty);
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public UniqueConstraintBuilder(ModelFramework.Database.Contracts.IUniqueConstraint source)
        {
            Fields = new System.Collections.Generic.List<ModelFramework.Database.Builders.UniqueConstraintFieldBuilder>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            Fields.AddRange(source.Fields.Select(x => new ModelFramework.Database.Builders.UniqueConstraintFieldBuilder(x)));
            _nameDelegate = new (() => source.Name);
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
            _fileGroupNameDelegate = new (() => source.FileGroupName);
        }

        private System.Lazy<string> _nameDelegate;

        private System.Lazy<string> _fileGroupNameDelegate;
    }
#nullable restore
}

