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
    public partial record Schema : ModelFramework.Database.Contracts.ISchema
    {
        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Database.Contracts.ITable> Tables
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Database.Contracts.IStoredProcedure> StoredProcedures
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Database.Contracts.IView> Views
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

        public Schema(System.Collections.Generic.IEnumerable<ModelFramework.Database.Contracts.ITable> tables, System.Collections.Generic.IEnumerable<ModelFramework.Database.Contracts.IStoredProcedure> storedProcedures, System.Collections.Generic.IEnumerable<ModelFramework.Database.Contracts.IView> views, string name, System.Collections.Generic.IEnumerable<ModelFramework.Common.Contracts.IMetadata> metadata)
        {
            this.Tables = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Database.Contracts.ITable>(tables);
            this.StoredProcedures = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Database.Contracts.IStoredProcedure>(storedProcedures);
            this.Views = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Database.Contracts.IView>(views);
            this.Name = name;
            this.Metadata = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Common.Contracts.IMetadata>(metadata);
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }
#nullable restore
}

