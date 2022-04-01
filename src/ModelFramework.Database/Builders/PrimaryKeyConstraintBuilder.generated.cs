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
    public partial class PrimaryKeyConstraintBuilder
    {
        public System.Collections.Generic.List<ModelFramework.Database.Builders.PrimaryKeyConstraintFieldBuilder> Fields
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

        public PrimaryKeyConstraintBuilder AddFields(params ModelFramework.Database.Builders.PrimaryKeyConstraintFieldBuilder[] fields)
        {
            Fields.AddRange(fields);
            return this;
        }

        public PrimaryKeyConstraintBuilder AddFields(System.Collections.Generic.IEnumerable<ModelFramework.Database.Builders.PrimaryKeyConstraintFieldBuilder> fields)
        {
            return AddFields(fields.ToArray());
        }

        public PrimaryKeyConstraintBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public PrimaryKeyConstraintBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public PrimaryKeyConstraintBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public ModelFramework.Database.Contracts.IPrimaryKeyConstraint Build()
        {
            return new ModelFramework.Database.PrimaryKeyConstraint(Fields.Select(x => x.Build()), Name, Metadata.Select(x => x.Build()), FileGroupName);
        }

        public PrimaryKeyConstraintBuilder WithFileGroupName(System.Func<string> fileGroupNameDelegate)
        {
            _fileGroupNameDelegate = new (fileGroupNameDelegate);
            return this;
        }

        public PrimaryKeyConstraintBuilder WithFileGroupName(string fileGroupName)
        {
            FileGroupName = fileGroupName;
            return this;
        }

        public PrimaryKeyConstraintBuilder WithName(System.Func<string> nameDelegate)
        {
            _nameDelegate = new (nameDelegate);
            return this;
        }

        public PrimaryKeyConstraintBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public PrimaryKeyConstraintBuilder()
        {
            Fields = new System.Collections.Generic.List<ModelFramework.Database.Builders.PrimaryKeyConstraintFieldBuilder>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            _nameDelegate = new (() => string.Empty);
            _fileGroupNameDelegate = new (() => string.Empty);
        }

        public PrimaryKeyConstraintBuilder(ModelFramework.Database.Contracts.IPrimaryKeyConstraint source)
        {
            Fields = new System.Collections.Generic.List<ModelFramework.Database.Builders.PrimaryKeyConstraintFieldBuilder>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            Fields.AddRange(source.Fields.Select(x => new ModelFramework.Database.Builders.PrimaryKeyConstraintFieldBuilder(x)));
            _nameDelegate = new (() => source.Name);
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
            _fileGroupNameDelegate = new (() => source.FileGroupName);
        }

        private System.Lazy<string> _nameDelegate;

        private System.Lazy<string> _fileGroupNameDelegate;
    }
#nullable restore
}

