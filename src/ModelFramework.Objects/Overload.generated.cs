﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.9
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
    public partial record Overload : ModelFramework.Objects.Contracts.IOverload
    {
        public string MethodName
        {
            get;
        }

        public string InitializeExpression
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Objects.Contracts.IParameter> Parameters
        {
            get;
        }

        public Overload(string methodName, string initializeExpression, System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.IParameter> parameters)
        {
            this.MethodName = methodName;
            this.InitializeExpression = initializeExpression;
            this.Parameters = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Objects.Contracts.IParameter>(parameters);
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }
#nullable restore
}
