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

namespace ModelFramework.Objects.Builders
{
#nullable enable
    public partial class ClassFieldBuilder
    {
        public bool ReadOnly
        {
            get;
            set;
        }

        public bool Constant
        {
            get;
            set;
        }

        public bool Event
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder> Metadata
        {
            get;
            set;
        }

        public bool Static
        {
            get;
            set;
        }

        public bool Virtual
        {
            get;
            set;
        }

        public bool Abstract
        {
            get;
            set;
        }

        public bool Protected
        {
            get;
            set;
        }

        public bool Override
        {
            get;
            set;
        }

        public ModelFramework.Objects.Contracts.Visibility Visibility
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeBuilder> Attributes
        {
            get;
            set;
        }

        public string TypeName
        {
            get;
            set;
        }

        public bool IsNullable
        {
            get;
            set;
        }

        public object? DefaultValue
        {
            get;
            set;
        }

        public ClassFieldBuilder AddAttributes(params ModelFramework.Objects.Builders.AttributeBuilder[] attributes)
        {
            Attributes.AddRange(attributes);
            return this;
        }

        public ClassFieldBuilder AddAttributes(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Builders.AttributeBuilder> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }

        public ClassFieldBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public ClassFieldBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public ClassFieldBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public ModelFramework.Objects.Contracts.IClassField Build()
        {
            return new ModelFramework.Objects.ClassField(ReadOnly, Constant, Event, Metadata.Select(x => x.Build()), Static, Virtual, Abstract, Protected, Override, Visibility, Name, Attributes.Select(x => x.Build()), TypeName, IsNullable, DefaultValue);
        }

        public ClassFieldBuilder WithAbstract(bool @abstract = true)
        {
            Abstract = @abstract;
            return this;
        }

        public ClassFieldBuilder WithConstant(bool constant = true)
        {
            Constant = constant;
            return this;
        }

        public ClassFieldBuilder WithDefaultValue(object? defaultValue)
        {
            DefaultValue = defaultValue;
            return this;
        }

        public ClassFieldBuilder WithEvent(bool @event = true)
        {
            Event = @event;
            return this;
        }

        public ClassFieldBuilder WithIsNullable(bool isNullable = true)
        {
            IsNullable = isNullable;
            return this;
        }

        public ClassFieldBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public ClassFieldBuilder WithOverride(bool @override = true)
        {
            Override = @override;
            return this;
        }

        public ClassFieldBuilder WithProtected(bool @protected = true)
        {
            Protected = @protected;
            return this;
        }

        public ClassFieldBuilder WithReadOnly(bool readOnly = true)
        {
            ReadOnly = readOnly;
            return this;
        }

        public ClassFieldBuilder WithStatic(bool @static = true)
        {
            Static = @static;
            return this;
        }

        public ClassFieldBuilder WithType(System.Type type)
        {
            TypeName = type.AssemblyQualifiedName;
            return this;
        }

        public ClassFieldBuilder WithTypeName(string typeName)
        {
            TypeName = typeName;
            return this;
        }

        public ClassFieldBuilder WithVirtual(bool @virtual = true)
        {
            Virtual = @virtual;
            return this;
        }

        public ClassFieldBuilder WithVisibility(ModelFramework.Objects.Contracts.Visibility visibility)
        {
            Visibility = visibility;
            return this;
        }

        public ClassFieldBuilder()
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeBuilder>();
            ReadOnly = default;
            Constant = default;
            Event = default;
            Static = default;
            Virtual = default;
            Abstract = default;
            Protected = default;
            Override = default;
            Visibility = ModelFramework.Objects.Contracts.Visibility.Private;
            Name = string.Empty;
            TypeName = string.Empty;
            IsNullable = default;
        }

        public ClassFieldBuilder(ModelFramework.Objects.Contracts.IClassField source)
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeBuilder>();
            ReadOnly = source.ReadOnly;
            Constant = source.Constant;
            Event = source.Event;
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
            Static = source.Static;
            Virtual = source.Virtual;
            Abstract = source.Abstract;
            Protected = source.Protected;
            Override = source.Override;
            Visibility = source.Visibility;
            Name = source.Name;
            Attributes.AddRange(source.Attributes.Select(x => new ModelFramework.Objects.Builders.AttributeBuilder(x)));
            TypeName = source.TypeName;
            IsNullable = source.IsNullable;
            DefaultValue = source.DefaultValue;
        }
    }
#nullable restore
}
