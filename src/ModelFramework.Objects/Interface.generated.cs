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

namespace ModelFramework.Objects
{
#nullable enable
    public partial record Interface : ModelFramework.Objects.Contracts.IInterface
    {
        public string Namespace
        {
            get;
        }

        public bool Partial
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<string> Interfaces
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Objects.Contracts.IClassProperty> Properties
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Objects.Contracts.IClassMethod> Methods
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<string> GenericTypeArguments
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<string> GenericTypeArgumentConstraints
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Common.Contracts.IMetadata> Metadata
        {
            get;
        }

        public ModelFramework.Objects.Contracts.Visibility Visibility
        {
            get;
        }

        public string Name
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Objects.Contracts.IAttribute> Attributes
        {
            get;
        }

        public Interface(string @namespace, bool partial, System.Collections.Generic.IReadOnlyCollection<string> interfaces, System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.IClassProperty> properties, System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.IClassMethod> methods, System.Collections.Generic.IReadOnlyCollection<string> genericTypeArguments, System.Collections.Generic.IReadOnlyCollection<string> genericTypeArgumentConstraints, System.Collections.Generic.IEnumerable<ModelFramework.Common.Contracts.IMetadata> metadata, ModelFramework.Objects.Contracts.Visibility visibility, string name, System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.IAttribute> attributes)
        {
            this.Namespace = @namespace;
            this.Partial = partial;
            this.Interfaces = interfaces;
            this.Properties = new CrossCutting.Common.ValueCollection<ModelFramework.Objects.Contracts.IClassProperty>(properties);
            this.Methods = new CrossCutting.Common.ValueCollection<ModelFramework.Objects.Contracts.IClassMethod>(methods);
            this.GenericTypeArguments = genericTypeArguments;
            this.GenericTypeArgumentConstraints = genericTypeArgumentConstraints;
            this.Metadata = new CrossCutting.Common.ValueCollection<ModelFramework.Common.Contracts.IMetadata>(metadata);
            this.Visibility = visibility;
            this.Name = name;
            this.Attributes = new CrossCutting.Common.ValueCollection<ModelFramework.Objects.Contracts.IAttribute>(attributes);
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }
#nullable restore
}

