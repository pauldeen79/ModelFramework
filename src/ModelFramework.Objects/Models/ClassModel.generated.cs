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
    public partial class ClassModel : TypeBaseModel<ClassModel, ModelFramework.Objects.Contracts.IClass>
    {
        public System.Collections.Generic.List<ModelFramework.Objects.Models.ClassFieldModel> Fields
        {
            get;
            set;
        }

        public bool Static
        {
            get;
            set;
        }

        public bool Sealed
        {
            get;
            set;
        }

        public bool Abstract
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Models.ClassModel> SubClasses
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Models.ClassConstructorModel> Constructors
        {
            get;
            set;
        }

        public string BaseClass
        {
            get;
            set;
        }

        public bool Record
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Models.EnumModel> Enums
        {
            get;
            set;
        }

        public override ModelFramework.Objects.Contracts.IClass ToTypedEntity()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new ModelFramework.Objects.Class(Fields.Select(x => x.ToEntity()), Static, Sealed, Abstract, SubClasses.Select(x => x.ToTypedEntity()), Constructors.Select(x => x.ToEntity()), BaseClass, Record, Namespace, Partial, new System.Collections.Generic.List<System.String>(Interfaces), Properties.Select(x => x.ToEntity()), Methods.Select(x => x.ToEntity()), new System.Collections.Generic.List<System.String>(GenericTypeArguments), new System.Collections.Generic.List<System.String>(GenericTypeArgumentConstraints), Metadata.Select(x => x.ToEntity()), Visibility, Name, Attributes.Select(x => x.ToEntity()), Enums.Select(x => x.ToEntity()));
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public ClassModel() : base()
        {
            Fields = new System.Collections.Generic.List<ModelFramework.Objects.Models.ClassFieldModel>();
            SubClasses = new System.Collections.Generic.List<ModelFramework.Objects.Models.ClassModel>();
            Constructors = new System.Collections.Generic.List<ModelFramework.Objects.Models.ClassConstructorModel>();
            Enums = new System.Collections.Generic.List<ModelFramework.Objects.Models.EnumModel>();
            BaseClass = string.Empty;
        }

        public ClassModel(ModelFramework.Objects.Contracts.IClass source) : base(source)
        {
            Fields = new System.Collections.Generic.List<ModelFramework.Objects.Models.ClassFieldModel>();
            SubClasses = new System.Collections.Generic.List<ModelFramework.Objects.Models.ClassModel>();
            Constructors = new System.Collections.Generic.List<ModelFramework.Objects.Models.ClassConstructorModel>();
            Enums = new System.Collections.Generic.List<ModelFramework.Objects.Models.EnumModel>();
            Fields.AddRange(source.Fields.Select(x => new ModelFramework.Objects.Models.ClassFieldModel(x)));
            Static = source.Static;
            Sealed = source.Sealed;
            Abstract = source.Abstract;
            SubClasses.AddRange(source.SubClasses.Select(x => new ModelFramework.Objects.Models.ClassModel(x)));
            Constructors.AddRange(source.Constructors.Select(x => new ModelFramework.Objects.Models.ClassConstructorModel(x)));
            BaseClass = source.BaseClass;
            Record = source.Record;
            Enums.AddRange(source.Enums.Select(x => new ModelFramework.Objects.Models.EnumModel(x)));
        }
    }
#nullable restore
}

