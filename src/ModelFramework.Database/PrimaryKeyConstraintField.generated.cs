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

namespace ModelFramework.Database
{
#nullable enable
    public partial record PrimaryKeyConstraintField : ModelFramework.Database.Contracts.IPrimaryKeyConstraintField
    {
        public bool IsDescending
        {
            get;
        }

        public string Name
        {
            get;
        }

        public CrossCutting.Common.ValueCollection<ModelFramework.Common.Contracts.IMetadata> Metadata
        {
            get;
        }

        public PrimaryKeyConstraintField(bool isDescending, string name, System.Collections.Generic.IEnumerable<ModelFramework.Common.Contracts.IMetadata> metadata)
        {
            this.IsDescending = isDescending;
            this.Name = name;
            this.Metadata = new CrossCutting.Common.ValueCollection<ModelFramework.Common.Contracts.IMetadata>(metadata);
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }
#nullable restore
}

