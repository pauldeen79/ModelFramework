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
    public partial class IndexFieldBuilder
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

        public IndexFieldBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public IndexFieldBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public IndexFieldBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public ModelFramework.Database.Contracts.IIndexField Build()
        {
            return new ModelFramework.Database.IndexField(IsDescending, Name, Metadata.Select(x => x.Build()));
        }

        public IndexFieldBuilder WithIsDescending(bool isDescending = true)
        {
            IsDescending = isDescending;
            return this;
        }

        public IndexFieldBuilder WithIsDescending(System.Func<bool> isDescendingDelegate)
        {
            _isDescendingDelegate = new (isDescendingDelegate);
            return this;
        }

        public IndexFieldBuilder WithName(System.Func<string> nameDelegate)
        {
            _nameDelegate = new (nameDelegate);
            return this;
        }

        public IndexFieldBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public IndexFieldBuilder()
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            _isDescendingDelegate = new (() => default);
            _nameDelegate = new (() => string.Empty);
        }

        public IndexFieldBuilder(ModelFramework.Database.Contracts.IIndexField source)
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            _isDescendingDelegate = new (() => source.IsDescending);
            _nameDelegate = new (() => source.Name);
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
        }

        private System.Lazy<bool> _isDescendingDelegate;

        private System.Lazy<string> _nameDelegate;
    }
#nullable restore
}

