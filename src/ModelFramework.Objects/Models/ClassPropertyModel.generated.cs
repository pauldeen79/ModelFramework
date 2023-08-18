﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 7.0.10
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
    public partial class ClassPropertyModel
    {
        public bool HasGetter
        {
            get;
            set;
        }

        public bool HasSetter
        {
            get;
            set;
        }

        public bool HasInitializer
        {
            get;
            set;
        }

        public System.Nullable<ModelFramework.Objects.Contracts.Visibility> GetterVisibility
        {
            get;
            set;
        }

        public System.Nullable<ModelFramework.Objects.Contracts.Visibility> SetterVisibility
        {
            get;
            set;
        }

        public System.Nullable<ModelFramework.Objects.Contracts.Visibility> InitializerVisibility
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementModel> GetterCodeStatements
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementModel> SetterCodeStatements
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementModel> InitializerCodeStatements
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

        public string TypeName
        {
            get;
            set;
        }

        public bool IsNullable
        {
            get;
            set;
        }

        public bool IsValueType
        {
            get;
            set;
        }

        public string ExplicitInterfaceName
        {
            get;
            set;
        }

        public string ParentTypeFullName
        {
            get;
            set;
        }

        public ModelFramework.Objects.Contracts.IClassProperty ToEntity()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new ModelFramework.Objects.ClassProperty(HasGetter, HasSetter, HasInitializer, GetterVisibility, SetterVisibility, InitializerVisibility, GetterCodeStatements.Select(x => x.ToEntity()), SetterCodeStatements.Select(x => x.ToEntity()), InitializerCodeStatements.Select(x => x.ToEntity()), Metadata.Select(x => x.ToEntity()), Static, Virtual, Abstract, Protected, Override, Visibility, Name, Attributes.Select(x => x.ToEntity()), TypeName, IsNullable, IsValueType, ExplicitInterfaceName, ParentTypeFullName);
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public ClassPropertyModel()
        {
            GetterCodeStatements = new System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementModel>();
            SetterCodeStatements = new System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementModel>();
            InitializerCodeStatements = new System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementModel>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel>();
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Models.AttributeModel>();
            #pragma warning disable CS8603 // Possible null reference return.
            HasGetter = true;
            HasSetter = true;
            HasInitializer = default(System.Boolean)!;
            Static = default(System.Boolean)!;
            Virtual = default(System.Boolean)!;
            Abstract = default(System.Boolean)!;
            Protected = default(System.Boolean)!;
            Override = default(System.Boolean)!;
            Visibility = default(ModelFramework.Objects.Contracts.Visibility)!;
            Name = string.Empty;
            TypeName = string.Empty;
            IsNullable = default(System.Boolean)!;
            IsValueType = default(System.Boolean)!;
            ExplicitInterfaceName = string.Empty;
            ParentTypeFullName = string.Empty;
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public ClassPropertyModel(ModelFramework.Objects.Contracts.IClassProperty source)
        {
            GetterCodeStatements = new System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementModel>();
            SetterCodeStatements = new System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementModel>();
            InitializerCodeStatements = new System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementModel>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel>();
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Models.AttributeModel>();
            HasGetter = source.HasGetter;
            HasSetter = source.HasSetter;
            HasInitializer = source.HasInitializer;
            GetterVisibility = source.GetterVisibility;
            SetterVisibility = source.SetterVisibility;
            InitializerVisibility = source.InitializerVisibility;
            GetterCodeStatements.AddRange(source.GetterCodeStatements.Select(x => x.CreateModel()));
            SetterCodeStatements.AddRange(source.SetterCodeStatements.Select(x => x.CreateModel()));
            InitializerCodeStatements.AddRange(source.InitializerCodeStatements.Select(x => x.CreateModel()));
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Models.MetadataModel(x)));
            Static = source.Static;
            Virtual = source.Virtual;
            Abstract = source.Abstract;
            Protected = source.Protected;
            Override = source.Override;
            Visibility = source.Visibility;
            Name = source.Name;
            Attributes.AddRange(source.Attributes.Select(x => new ModelFramework.Objects.Models.AttributeModel(x)));
            TypeName = source.TypeName;
            IsNullable = source.IsNullable;
            IsValueType = source.IsValueType;
            ExplicitInterfaceName = source.ExplicitInterfaceName;
            ParentTypeFullName = source.ParentTypeFullName;
        }
    }
#nullable restore
}
