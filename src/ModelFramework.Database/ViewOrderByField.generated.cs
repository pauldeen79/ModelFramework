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
    public partial record ViewOrderByField : ModelFramework.Database.Contracts.IViewOrderByField
    {
        public bool IsDescending
        {
            get;
        }

        public string SourceSchemaName
        {
            get;
        }

        public string SourceObjectName
        {
            get;
        }

        public string Expression
        {
            get;
        }

        public string Alias
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

        public ViewOrderByField(bool isDescending, string sourceSchemaName, string sourceObjectName, string expression, string alias, string name, System.Collections.Generic.IEnumerable<ModelFramework.Common.Contracts.IMetadata> metadata)
        {
            this.IsDescending = isDescending;
            this.SourceSchemaName = sourceSchemaName;
            this.SourceObjectName = sourceObjectName;
            this.Expression = expression;
            this.Alias = alias;
            this.Name = name;
            this.Metadata = new CrossCutting.Common.ValueCollection<ModelFramework.Common.Contracts.IMetadata>(metadata);
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }
#nullable restore
}

