﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 8.0.0
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
    public partial record Interface : ModelFramework.Objects.TypeBase, ModelFramework.Objects.Contracts.IInterface
    {
        public Interface(string @namespace, bool partial, System.Collections.Generic.IEnumerable<string> interfaces, System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.IClassProperty> properties, System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.IClassMethod> methods, System.Collections.Generic.IEnumerable<string> genericTypeArguments, System.Collections.Generic.IEnumerable<string> genericTypeArgumentConstraints, System.Collections.Generic.IEnumerable<ModelFramework.Common.Contracts.IMetadata> metadata, ModelFramework.Objects.Contracts.Visibility visibility, string name, System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.IAttribute> attributes) : base(@namespace, partial, interfaces, properties, methods, genericTypeArguments, genericTypeArgumentConstraints, metadata, visibility, name, attributes)
        {
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }
#nullable restore
}

