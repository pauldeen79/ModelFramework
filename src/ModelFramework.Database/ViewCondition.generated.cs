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
    public partial record ViewCondition : ModelFramework.Database.Contracts.IViewCondition
    {
        public string Expression
        {
            get;
        }

        public string Combination
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

        public ViewCondition(string expression, string combination, System.Collections.Generic.IEnumerable<ModelFramework.Common.Contracts.IMetadata> metadata, string fileGroupName)
        {
            this.Expression = expression;
            this.Combination = combination;
            this.Metadata = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Common.Contracts.IMetadata>(metadata);
            this.FileGroupName = fileGroupName;
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }
#nullable restore
}

