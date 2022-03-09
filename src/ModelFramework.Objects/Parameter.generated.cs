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

namespace ModelFramework.Objects
{
#nullable enable
    public partial record Parameter : ModelFramework.Objects.Contracts.IParameter
    {
        public bool IsParamArray
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

        public CrossCutting.Common.ValueCollection<ModelFramework.Objects.Contracts.IAttribute> Attributes
        {
            get;
        }

        public CrossCutting.Common.ValueCollection<ModelFramework.Common.Contracts.IMetadata> Metadata
        {
            get;
        }

        public string Name
        {
            get;
        }

        public object? DefaultValue
        {
            get;
        }

        public Parameter(bool isParamArray, string typeName, bool isNullable, System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.IAttribute> attributes, System.Collections.Generic.IEnumerable<ModelFramework.Common.Contracts.IMetadata> metadata, string name, object? defaultValue)
        {
            this.IsParamArray = isParamArray;
            this.TypeName = typeName;
            this.IsNullable = isNullable;
            this.Attributes = new CrossCutting.Common.ValueCollection<ModelFramework.Objects.Contracts.IAttribute>(attributes);
            this.Metadata = new CrossCutting.Common.ValueCollection<ModelFramework.Common.Contracts.IMetadata>(metadata);
            this.Name = name;
            this.DefaultValue = defaultValue;
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }
#nullable restore
}

