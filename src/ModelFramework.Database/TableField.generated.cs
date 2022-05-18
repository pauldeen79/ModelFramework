﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.5
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
    public partial record TableField : ModelFramework.Database.Contracts.ITableField
    {
        public string Type
        {
            get;
        }

        public bool IsIdentity
        {
            get;
        }

        public bool IsRequired
        {
            get;
        }

        public System.Nullable<byte> NumericPrecision
        {
            get;
        }

        public System.Nullable<byte> NumericScale
        {
            get;
        }

        public System.Nullable<int> StringLength
        {
            get;
        }

        public string StringCollation
        {
            get;
        }

        public bool IsStringMaxLength
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

        public CrossCutting.Common.ValueCollection<ModelFramework.Database.Contracts.ICheckConstraint> CheckConstraints
        {
            get;
        }

        public TableField(string type, bool isIdentity, bool isRequired, System.Nullable<byte> numericPrecision, System.Nullable<byte> numericScale, System.Nullable<int> stringLength, string stringCollation, bool isStringMaxLength, string name, System.Collections.Generic.IEnumerable<ModelFramework.Common.Contracts.IMetadata> metadata, System.Collections.Generic.IEnumerable<ModelFramework.Database.Contracts.ICheckConstraint> checkConstraints)
        {
            this.Type = type;
            this.IsIdentity = isIdentity;
            this.IsRequired = isRequired;
            this.NumericPrecision = numericPrecision;
            this.NumericScale = numericScale;
            this.StringLength = stringLength;
            this.StringCollation = stringCollation;
            this.IsStringMaxLength = isStringMaxLength;
            this.Name = name;
            this.Metadata = new CrossCutting.Common.ValueCollection<ModelFramework.Common.Contracts.IMetadata>(metadata);
            this.CheckConstraints = new CrossCutting.Common.ValueCollection<ModelFramework.Database.Contracts.ICheckConstraint>(checkConstraints);
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }
#nullable restore
}

