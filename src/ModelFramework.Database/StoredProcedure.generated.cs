﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 7.0.12
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
    public partial record StoredProcedure : ModelFramework.Database.Contracts.IStoredProcedure
    {
        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Database.Contracts.ISqlStatement> Statements
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Database.Contracts.IStoredProcedureParameter> Parameters
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

        public StoredProcedure(System.Collections.Generic.IEnumerable<ModelFramework.Database.Contracts.ISqlStatement> statements, System.Collections.Generic.IEnumerable<ModelFramework.Database.Contracts.IStoredProcedureParameter> parameters, string name, System.Collections.Generic.IEnumerable<ModelFramework.Common.Contracts.IMetadata> metadata)
        {
            this.Statements = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Database.Contracts.ISqlStatement>(statements);
            this.Parameters = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Database.Contracts.IStoredProcedureParameter>(parameters);
            this.Name = name;
            this.Metadata = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Common.Contracts.IMetadata>(metadata);
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }
#nullable restore
}

