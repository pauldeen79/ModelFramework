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
    public partial class PrimaryKeyConstraintFieldBuilder
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

        public ModelFramework.Database.Contracts.IPrimaryKeyConstraintField Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            return new ModelFramework.Database.PrimaryKeyConstraintField(IsDescending, Name?.ToString(), Metadata.Select(x => x.Build()));
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public PrimaryKeyConstraintFieldBuilder WithIsDescending(bool isDescending = true)
        {
            IsDescending = isDescending;
            return this;
        }

        public PrimaryKeyConstraintFieldBuilder WithIsDescending(System.Func<bool> isDescendingDelegate)
        {
            _isDescendingDelegate = new (isDescendingDelegate);
            return this;
        }

        public PrimaryKeyConstraintFieldBuilder WithName(System.Text.StringBuilder name)
        {
            Name = name;
            return this;
        }

        public PrimaryKeyConstraintFieldBuilder WithName(System.Func<System.Text.StringBuilder> nameDelegate)
        {
            _nameDelegate = new (nameDelegate);
            return this;
        }

        public PrimaryKeyConstraintFieldBuilder WithName(string value)
        {
            Name.Clear().Append(value);
            return this;
        }

        public PrimaryKeyConstraintFieldBuilder AppendToName(string value)
        {
            Name.Append(value);
            return this;
        }

        public PrimaryKeyConstraintFieldBuilder AppendLineToName(string value)
        {
            Name.AppendLine(value);
            return this;
        }

        public PrimaryKeyConstraintFieldBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public PrimaryKeyConstraintFieldBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public PrimaryKeyConstraintFieldBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public PrimaryKeyConstraintFieldBuilder()
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            #pragma warning disable CS8603 // Possible null reference return.
            _isDescendingDelegate = new (() => default);
            _nameDelegate = new (() => new System.Text.StringBuilder());
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public PrimaryKeyConstraintFieldBuilder(ModelFramework.Database.Contracts.IPrimaryKeyConstraintField source)
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            _isDescendingDelegate = new (() => source.IsDescending);
            _nameDelegate = new (() => new System.Text.StringBuilder(source.Name));
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
        }

        protected System.Lazy<bool> _isDescendingDelegate;

        protected System.Lazy<System.Text.StringBuilder> _nameDelegate;
    }
#nullable restore
}

