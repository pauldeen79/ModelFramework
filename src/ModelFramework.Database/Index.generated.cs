﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 7.0.4
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
    public partial record Index : ModelFramework.Database.Contracts.IIndex
    {
        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Database.Contracts.IIndexField> Fields
        {
            get;
        }

        public bool Unique
        {
            get;
        }

        public string Name
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Common.Contracts.IMetadata> Metadata
        {
            get;
        }

        public string FileGroupName
        {
            get;
        }

        public Index(System.Collections.Generic.IEnumerable<ModelFramework.Database.Contracts.IIndexField> fields, bool unique, string name, System.Collections.Generic.IEnumerable<ModelFramework.Common.Contracts.IMetadata> metadata, string fileGroupName)
        {
            this.Fields = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Database.Contracts.IIndexField>(fields);
            this.Unique = unique;
            this.Name = name;
            this.Metadata = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Common.Contracts.IMetadata>(metadata);
            this.FileGroupName = fileGroupName;
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }
#nullable restore
}

