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

namespace ModelFramework.Objects.Builders
{
#nullable enable
    public partial class ClassBuilder : TypeBaseBuilder<ClassBuilder, ModelFramework.Objects.Contracts.IClass>
    {
        public System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassFieldBuilder> Fields
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

        public System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassBuilder> SubClasses
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassConstructorBuilder> Constructors
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

        public System.Collections.Generic.List<ModelFramework.Objects.Builders.EnumBuilder> Enums
        {
            get;
            set;
        }

        public override ModelFramework.Objects.Contracts.IClass BuildTyped()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new ModelFramework.Objects.Class(Fields.Select(x => x.Build()), Static, Sealed, Abstract, SubClasses.Select(x => x.BuildTyped()), Constructors.Select(x => x.Build()), BaseClass, Record, Namespace, Partial, new CrossCutting.Common.ValueCollection<System.String>(Interfaces), Properties.Select(x => x.Build()), Methods.Select(x => x.Build()), new CrossCutting.Common.ValueCollection<System.String>(GenericTypeArguments), new CrossCutting.Common.ValueCollection<System.String>(GenericTypeArgumentConstraints), Metadata.Select(x => x.Build()), Visibility, Name, Attributes.Select(x => x.Build()), Enums.Select(x => x.Build()));
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public ClassBuilder AddFields(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Builders.ClassFieldBuilder> fields)
        {
            return AddFields(fields.ToArray());
        }

        public ClassBuilder AddFields(params ModelFramework.Objects.Builders.ClassFieldBuilder[] fields)
        {
            Fields.AddRange(fields);
            return this;
        }

        public ClassBuilder WithStatic(bool @static = true)
        {
            Static = @static;
            return this;
        }

        public ClassBuilder WithSealed(bool @sealed = true)
        {
            Sealed = @sealed;
            return this;
        }

        public ClassBuilder WithAbstract(bool @abstract = true)
        {
            Abstract = @abstract;
            return this;
        }

        public ClassBuilder AddSubClasses(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Builders.ClassBuilder> subClasses)
        {
            return AddSubClasses(subClasses.ToArray());
        }

        public ClassBuilder AddSubClasses(params ModelFramework.Objects.Builders.ClassBuilder[] subClasses)
        {
            SubClasses.AddRange(subClasses);
            return this;
        }

        public ClassBuilder AddConstructors(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Builders.ClassConstructorBuilder> constructors)
        {
            return AddConstructors(constructors.ToArray());
        }

        public ClassBuilder AddConstructors(params ModelFramework.Objects.Builders.ClassConstructorBuilder[] constructors)
        {
            Constructors.AddRange(constructors);
            return this;
        }

        public ClassBuilder WithBaseClass(string baseClass)
        {
            BaseClass = baseClass;
            return this;
        }

        public ClassBuilder WithRecord(bool record = true)
        {
            Record = record;
            return this;
        }

        public ClassBuilder AddEnums(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Builders.EnumBuilder> enums)
        {
            return AddEnums(enums.ToArray());
        }

        public ClassBuilder AddEnums(params ModelFramework.Objects.Builders.EnumBuilder[] enums)
        {
            Enums.AddRange(enums);
            return this;
        }

        public ClassBuilder() : base()
        {
            Fields = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassFieldBuilder>();
            SubClasses = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassBuilder>();
            Constructors = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassConstructorBuilder>();
            Enums = new System.Collections.Generic.List<ModelFramework.Objects.Builders.EnumBuilder>();
            BaseClass = string.Empty;
        }

        public ClassBuilder(ModelFramework.Objects.Contracts.IClass source) : base(source)
        {
            Fields = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassFieldBuilder>();
            SubClasses = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassBuilder>();
            Constructors = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassConstructorBuilder>();
            Enums = new System.Collections.Generic.List<ModelFramework.Objects.Builders.EnumBuilder>();
            Fields.AddRange(source.Fields.Select(x => new ModelFramework.Objects.Builders.ClassFieldBuilder(x)));
            Static = source.Static;
            Sealed = source.Sealed;
            Abstract = source.Abstract;
            SubClasses.AddRange(source.SubClasses.Select(x => new ModelFramework.Objects.Builders.ClassBuilder(x)));
            Constructors.AddRange(source.Constructors.Select(x => new ModelFramework.Objects.Builders.ClassConstructorBuilder(x)));
            BaseClass = source.BaseClass;
            Record = source.Record;
            Enums.AddRange(source.Enums.Select(x => new ModelFramework.Objects.Builders.EnumBuilder(x)));
        }
    }
#nullable restore
}

