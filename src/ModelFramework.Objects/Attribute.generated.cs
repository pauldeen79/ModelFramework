﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 7.0.10
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
    public partial record Attribute : ModelFramework.Objects.Contracts.IAttribute
    {
        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Objects.Contracts.IAttributeParameter> Parameters
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

        public Attribute(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.IAttributeParameter> parameters, System.Collections.Generic.IEnumerable<ModelFramework.Common.Contracts.IMetadata> metadata, string name)
        {
            this.Parameters = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Objects.Contracts.IAttributeParameter>(parameters);
            this.Metadata = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Common.Contracts.IMetadata>(metadata);
            this.Name = name;
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }
#nullable restore
}
