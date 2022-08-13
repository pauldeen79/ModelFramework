﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.8
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelFramework.Objects.Builders
{
#nullable enable
    public partial class EnumBuilder
    {
        public System.Collections.Generic.List<ModelFramework.Objects.Builders.EnumMemberBuilder> Members
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeBuilder> Attributes
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder> Metadata
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

        public ModelFramework.Objects.Contracts.Visibility Visibility
        {
            get
            {
                return _visibilityDelegate.Value;
            }
            set
            {
                _visibilityDelegate = new (() => value);
            }
        }

        public EnumBuilder AddAttributes(params ModelFramework.Objects.Builders.AttributeBuilder[] attributes)
        {
            Attributes.AddRange(attributes);
            return this;
        }

        public EnumBuilder AddAttributes(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Builders.AttributeBuilder> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }

        public EnumBuilder AddMembers(params ModelFramework.Objects.Builders.EnumMemberBuilder[] members)
        {
            Members.AddRange(members);
            return this;
        }

        public EnumBuilder AddMembers(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Builders.EnumMemberBuilder> members)
        {
            return AddMembers(members.ToArray());
        }

        public EnumBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public EnumBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public EnumBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public ModelFramework.Objects.Contracts.IEnum Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            return new ModelFramework.Objects.Enum(Members.Select(x => x.Build()), Attributes.Select(x => x.Build()), Metadata.Select(x => x.Build()), Name, Visibility);
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public EnumBuilder WithName(System.Func<string> nameDelegate)
        {
            _nameDelegate = new (nameDelegate);
            return this;
        }

        public EnumBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public EnumBuilder WithVisibility(ModelFramework.Objects.Contracts.Visibility visibility)
        {
            Visibility = visibility;
            return this;
        }

        public EnumBuilder WithVisibility(System.Func<ModelFramework.Objects.Contracts.Visibility> visibilityDelegate)
        {
            _visibilityDelegate = new (visibilityDelegate);
            return this;
        }

        public EnumBuilder()
        {
            Members = new System.Collections.Generic.List<ModelFramework.Objects.Builders.EnumMemberBuilder>();
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeBuilder>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            #pragma warning disable CS8603 // Possible null reference return.
            _nameDelegate = new (() => string.Empty);
            _visibilityDelegate = new (() => ModelFramework.Objects.Contracts.Visibility.Public);
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public EnumBuilder(ModelFramework.Objects.Contracts.IEnum source)
        {
            Members = new System.Collections.Generic.List<ModelFramework.Objects.Builders.EnumMemberBuilder>();
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeBuilder>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            Members.AddRange(source.Members.Select(x => new ModelFramework.Objects.Builders.EnumMemberBuilder(x)));
            Attributes.AddRange(source.Attributes.Select(x => new ModelFramework.Objects.Builders.AttributeBuilder(x)));
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
            _nameDelegate = new (() => source.Name);
            _visibilityDelegate = new (() => source.Visibility);
        }

        protected System.Lazy<string> _nameDelegate;

        protected System.Lazy<ModelFramework.Objects.Contracts.Visibility> _visibilityDelegate;
    }
#nullable restore
}

