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
            get
            {
                return _staticDelegate.Value;
            }
            set
            {
                _staticDelegate = new (() => value);
            }
        }

        public bool Sealed
        {
            get
            {
                return _sealedDelegate.Value;
            }
            set
            {
                _sealedDelegate = new (() => value);
            }
        }

        public bool Abstract
        {
            get
            {
                return _abstractDelegate.Value;
            }
            set
            {
                _abstractDelegate = new (() => value);
            }
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

        public System.Text.StringBuilder BaseClass
        {
            get
            {
                return _baseClassDelegate.Value;
            }
            set
            {
                _baseClassDelegate = new (() => value);
            }
        }

        public bool Record
        {
            get
            {
                return _recordDelegate.Value;
            }
            set
            {
                _recordDelegate = new (() => value);
            }
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Builders.EnumBuilder> Enums
        {
            get;
            set;
        }

        public override ModelFramework.Objects.Contracts.IClass BuildTyped()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            return new ModelFramework.Objects.Class(Fields.Select(x => x.Build()), Static, Sealed, Abstract, SubClasses.Select(x => x.BuildTyped()), Constructors.Select(x => x.Build()), BaseClass?.ToString(), Record, Namespace?.ToString(), Partial, new CrossCutting.Common.ValueCollection<System.String>(Interfaces), Properties.Select(x => x.Build()), Methods.Select(x => x.Build()), new CrossCutting.Common.ValueCollection<System.String>(GenericTypeArguments), new CrossCutting.Common.ValueCollection<System.String>(GenericTypeArgumentConstraints), Metadata.Select(x => x.Build()), Visibility, Name?.ToString(), Attributes.Select(x => x.Build()), Enums.Select(x => x.Build()));
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public override ModelFramework.Objects.Contracts.ITypeBase Build()
        {
            return BuildTyped();
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

        public ClassBuilder WithStatic(System.Func<bool> staticDelegate)
        {
            _staticDelegate = new (@staticDelegate);
            return this;
        }

        public ClassBuilder WithSealed(bool @sealed = true)
        {
            Sealed = @sealed;
            return this;
        }

        public ClassBuilder WithSealed(System.Func<bool> sealedDelegate)
        {
            _sealedDelegate = new (@sealedDelegate);
            return this;
        }

        public ClassBuilder WithAbstract(bool @abstract = true)
        {
            Abstract = @abstract;
            return this;
        }

        public ClassBuilder WithAbstract(System.Func<bool> abstractDelegate)
        {
            _abstractDelegate = new (@abstractDelegate);
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

        public ClassBuilder WithBaseClass(System.Text.StringBuilder baseClass)
        {
            BaseClass = baseClass;
            return this;
        }

        public ClassBuilder WithBaseClass(System.Func<System.Text.StringBuilder> baseClassDelegate)
        {
            _baseClassDelegate = new (baseClassDelegate);
            return this;
        }

        public ClassBuilder WithBaseClass(string value)
        {
            if (BaseClass == null)
                BaseClass = new System.Text.StringBuilder();
            BaseClass.Clear().Append(value);
            return this;
        }

        public ClassBuilder AppendToBaseClass(string value)
        {
            if (BaseClass == null)
                BaseClass = new System.Text.StringBuilder();
            BaseClass.Append(value);
            return this;
        }

        public ClassBuilder AppendLineToBaseClass(string value)
        {
            if (BaseClass == null)
                BaseClass = new System.Text.StringBuilder();
            BaseClass.AppendLine(value);
            return this;
        }

        public ClassBuilder WithRecord(bool record = true)
        {
            Record = record;
            return this;
        }

        public ClassBuilder WithRecord(System.Func<bool> recordDelegate)
        {
            _recordDelegate = new (recordDelegate);
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
            #pragma warning disable CS8603 // Possible null reference return.
            _staticDelegate = new (() => default);
            _sealedDelegate = new (() => default);
            _abstractDelegate = new (() => default);
            _baseClassDelegate = new (() => new System.Text.StringBuilder());
            _recordDelegate = new (() => default);
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public ClassBuilder(ModelFramework.Objects.Contracts.IClass source) : base(source)
        {
            Fields = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassFieldBuilder>();
            SubClasses = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassBuilder>();
            Constructors = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassConstructorBuilder>();
            Enums = new System.Collections.Generic.List<ModelFramework.Objects.Builders.EnumBuilder>();
            Fields.AddRange(source.Fields.Select(x => new ModelFramework.Objects.Builders.ClassFieldBuilder(x)));
            _staticDelegate = new (() => source.Static);
            _sealedDelegate = new (() => source.Sealed);
            _abstractDelegate = new (() => source.Abstract);
            SubClasses.AddRange(source.SubClasses.Select(x => new ModelFramework.Objects.Builders.ClassBuilder(x)));
            Constructors.AddRange(source.Constructors.Select(x => new ModelFramework.Objects.Builders.ClassConstructorBuilder(x)));
            _baseClassDelegate = new (() => new System.Text.StringBuilder(source.BaseClass));
            _recordDelegate = new (() => source.Record);
            Enums.AddRange(source.Enums.Select(x => new ModelFramework.Objects.Builders.EnumBuilder(x)));
        }

        protected System.Lazy<bool> _staticDelegate;

        protected System.Lazy<bool> _sealedDelegate;

        protected System.Lazy<bool> _abstractDelegate;

        protected System.Lazy<System.Text.StringBuilder> _baseClassDelegate;

        protected System.Lazy<bool> _recordDelegate;
    }
#nullable restore
}

