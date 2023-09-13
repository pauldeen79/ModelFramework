﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 7.0.11
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelFramework.Objects.Models
{
#nullable enable
    public abstract partial class TypeBaseModel
    {
        public string Namespace
        {
            get;
            set;
        }

        public bool Partial
        {
            get;
            set;
        }

        public System.Collections.Generic.List<string> Interfaces
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Models.ClassPropertyModel> Properties
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Models.ClassMethodModel> Methods
        {
            get;
            set;
        }

        public System.Collections.Generic.List<string> GenericTypeArguments
        {
            get;
            set;
        }

        public System.Collections.Generic.List<string> GenericTypeArgumentConstraints
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel> Metadata
        {
            get;
            set;
        }

        public ModelFramework.Objects.Contracts.Visibility Visibility
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Models.AttributeModel> Attributes
        {
            get;
            set;
        }

        public abstract ModelFramework.Objects.Contracts.ITypeBase ToEntity();

        protected TypeBaseModel()
        {
            Interfaces = new System.Collections.Generic.List<string>();
            Properties = new System.Collections.Generic.List<ModelFramework.Objects.Models.ClassPropertyModel>();
            Methods = new System.Collections.Generic.List<ModelFramework.Objects.Models.ClassMethodModel>();
            GenericTypeArguments = new System.Collections.Generic.List<string>();
            GenericTypeArgumentConstraints = new System.Collections.Generic.List<string>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel>();
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Models.AttributeModel>();
            #pragma warning disable CS8603 // Possible null reference return.
            Namespace = string.Empty;
            Partial = default(System.Boolean);
            Visibility = default(ModelFramework.Objects.Contracts.Visibility)!;
            Name = string.Empty;
            #pragma warning restore CS8603 // Possible null reference return.
        }

        protected TypeBaseModel(ModelFramework.Objects.Contracts.ITypeBase source)
        {
            Interfaces = new System.Collections.Generic.List<string>();
            Properties = new System.Collections.Generic.List<ModelFramework.Objects.Models.ClassPropertyModel>();
            Methods = new System.Collections.Generic.List<ModelFramework.Objects.Models.ClassMethodModel>();
            GenericTypeArguments = new System.Collections.Generic.List<string>();
            GenericTypeArgumentConstraints = new System.Collections.Generic.List<string>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel>();
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Models.AttributeModel>();
            Namespace = source.Namespace;
            Partial = source.Partial;
            Interfaces.AddRange(source.Interfaces);
            Properties.AddRange(source.Properties.Select(x => new ModelFramework.Objects.Models.ClassPropertyModel(x)));
            Methods.AddRange(source.Methods.Select(x => new ModelFramework.Objects.Models.ClassMethodModel(x)));
            GenericTypeArguments.AddRange(source.GenericTypeArguments);
            GenericTypeArgumentConstraints.AddRange(source.GenericTypeArgumentConstraints);
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Models.MetadataModel(x)));
            Visibility = source.Visibility;
            Name = source.Name;
            Attributes.AddRange(source.Attributes.Select(x => new ModelFramework.Objects.Models.AttributeModel(x)));
        }
    }
#nullable restore
}

