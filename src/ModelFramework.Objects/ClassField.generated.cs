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

namespace ModelFramework.Objects
{
#nullable enable
    public partial record ClassField : ModelFramework.Objects.Contracts.IClassField
    {
        public bool ReadOnly
        {
            get;
        }

        public bool Constant
        {
            get;
        }

        public bool Event
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Common.Contracts.IMetadata> Metadata
        {
            get;
        }

        public bool Static
        {
            get;
        }

        public bool Virtual
        {
            get;
        }

        public bool Abstract
        {
            get;
        }

        public bool Protected
        {
            get;
        }

        public bool Override
        {
            get;
        }

        public ModelFramework.Objects.Contracts.Visibility Visibility
        {
            get;
        }

        public string Name
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Objects.Contracts.IAttribute> Attributes
        {
            get;
        }

        public string TypeName
        {
            get;
        }

        public bool IsNullable
        {
            get;
        }

        public bool IsValueType
        {
            get;
        }

        public object? DefaultValue
        {
            get;
        }

        public string ParentTypeFullName
        {
            get;
        }

        public ClassField(bool readOnly, bool constant, bool @event, System.Collections.Generic.IEnumerable<ModelFramework.Common.Contracts.IMetadata> metadata, bool @static, bool @virtual, bool @abstract, bool @protected, bool @override, ModelFramework.Objects.Contracts.Visibility visibility, string name, System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.IAttribute> attributes, string typeName, bool isNullable, bool isValueType, object? defaultValue, string parentTypeFullName)
        {
            this.ReadOnly = readOnly;
            this.Constant = constant;
            this.Event = @event;
            this.Metadata = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Common.Contracts.IMetadata>(metadata);
            this.Static = @static;
            this.Virtual = @virtual;
            this.Abstract = @abstract;
            this.Protected = @protected;
            this.Override = @override;
            this.Visibility = visibility;
            this.Name = name;
            this.Attributes = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Objects.Contracts.IAttribute>(attributes);
            this.TypeName = typeName;
            this.IsNullable = isNullable;
            this.IsValueType = isValueType;
            this.DefaultValue = defaultValue;
            this.ParentTypeFullName = parentTypeFullName;
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }
#nullable restore
}

