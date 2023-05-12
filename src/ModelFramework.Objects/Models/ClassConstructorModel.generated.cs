﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 7.0.5
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
    public partial class ClassConstructorModel
    {
        public string ChainCall
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel> Metadata
        {
            get;
            set;
        }

        public bool Static
        {
            get;
            set;
        }

        public bool Virtual
        {
            get;
            set;
        }

        public bool Abstract
        {
            get;
            set;
        }

        public bool Protected
        {
            get;
            set;
        }

        public bool Override
        {
            get;
            set;
        }

        public ModelFramework.Objects.Contracts.Visibility Visibility
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Models.AttributeModel> Attributes
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementModel> CodeStatements
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Models.ParameterModel> Parameters
        {
            get;
            set;
        }

        public ModelFramework.Objects.Contracts.IClassConstructor ToEntity()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new ModelFramework.Objects.ClassConstructor(ChainCall, Metadata.Select(x => x.ToEntity()), Static, Virtual, Abstract, Protected, Override, Visibility, Attributes.Select(x => x.ToEntity()), CodeStatements.Select(x => x.ToEntity()), Parameters.Select(x => x.ToEntity()));
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public ClassConstructorModel()
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel>();
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Models.AttributeModel>();
            CodeStatements = new System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementModel>();
            Parameters = new System.Collections.Generic.List<ModelFramework.Objects.Models.ParameterModel>();
            #pragma warning disable CS8603 // Possible null reference return.
            ChainCall = string.Empty;
            Static = default(System.Boolean);
            Virtual = default(System.Boolean);
            Abstract = default(System.Boolean);
            Protected = default(System.Boolean);
            Override = default(System.Boolean);
            Visibility = default(ModelFramework.Objects.Contracts.Visibility);
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public ClassConstructorModel(ModelFramework.Objects.Contracts.IClassConstructor source)
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel>();
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Models.AttributeModel>();
            CodeStatements = new System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementModel>();
            Parameters = new System.Collections.Generic.List<ModelFramework.Objects.Models.ParameterModel>();
            ChainCall = source.ChainCall;
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Models.MetadataModel(x)));
            Static = source.Static;
            Virtual = source.Virtual;
            Abstract = source.Abstract;
            Protected = source.Protected;
            Override = source.Override;
            Visibility = source.Visibility;
            Attributes.AddRange(source.Attributes.Select(x => new ModelFramework.Objects.Models.AttributeModel(x)));
            CodeStatements.AddRange(source.CodeStatements.Select(x => x.CreateModel()));
            Parameters.AddRange(source.Parameters.Select(x => new ModelFramework.Objects.Models.ParameterModel(x)));
        }
    }
#nullable restore
}

