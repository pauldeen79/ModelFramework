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

namespace ModelFramework.Objects
{
#nullable enable
    public partial record Enum : ModelFramework.Objects.Contracts.IEnum
    {
        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Objects.Contracts.IEnumMember> Members
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Objects.Contracts.IAttribute> Attributes
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Common.Contracts.IMetadata> Metadata
        {
            get;
        }

        public string Name
        {
            get;
        }

        public ModelFramework.Objects.Contracts.Visibility Visibility
        {
            get;
        }

        public Enum(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.IEnumMember> members, System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.IAttribute> attributes, System.Collections.Generic.IEnumerable<ModelFramework.Common.Contracts.IMetadata> metadata, string name, ModelFramework.Objects.Contracts.Visibility visibility)
        {
            this.Members = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Objects.Contracts.IEnumMember>(members);
            this.Attributes = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Objects.Contracts.IAttribute>(attributes);
            this.Metadata = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Common.Contracts.IMetadata>(metadata);
            this.Name = name;
            this.Visibility = visibility;
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }
#nullable restore
}

