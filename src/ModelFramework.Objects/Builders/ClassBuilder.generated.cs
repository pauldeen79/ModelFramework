﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.3
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
    public partial class ClassBuilder
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

        public string Namespace
        {
            get
            {
                return _namespaceDelegate.Value;
            }
            set
            {
                _namespaceDelegate = new (() => value);
            }
        }

        public bool Partial
        {
            get
            {
                return _partialDelegate.Value;
            }
            set
            {
                _partialDelegate = new (() => value);
            }
        }

        public System.Collections.Generic.List<string> Interfaces
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassPropertyBuilder> Properties
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassMethodBuilder> Methods
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

        public System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder> Metadata
        {
            get;
            set;
        }

        public ModelFramework.Objects.Contracts.Visibility Visibility
        {
            get
            {
                return _visibilityDelegate.Value;
            }
            set
            {
                _visibilityDelegate = new (() => value);
            }
        }

        public string Name
        {
            get
            {
                return _nameDelegate.Value;
            }
            set
            {
                _nameDelegate = new (() => value);
            }
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeBuilder> Attributes
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Builders.EnumBuilder> Enums
        {
            get;
            set;
        }

        public ClassBuilder AddAttributes(params ModelFramework.Objects.Builders.AttributeBuilder[] attributes)
        {
            Attributes.AddRange(attributes);
            return this;
        }

        public ClassBuilder AddAttributes(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Builders.AttributeBuilder> attributes)
        {
            return AddAttributes(attributes.ToArray());
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

        public ClassBuilder AddEnums(params ModelFramework.Objects.Builders.EnumBuilder[] enums)
        {
            Enums.AddRange(enums);
            return this;
        }

        public ClassBuilder AddEnums(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Builders.EnumBuilder> enums)
        {
            return AddEnums(enums.ToArray());
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

        public ClassBuilder AddGenericTypeArgumentConstraints(params string[] genericTypeArgumentConstraints)
        {
            GenericTypeArgumentConstraints.AddRange(genericTypeArgumentConstraints);
            return this;
        }

        public ClassBuilder AddGenericTypeArgumentConstraints(System.Collections.Generic.IEnumerable<string> genericTypeArgumentConstraints)
        {
            return AddGenericTypeArgumentConstraints(genericTypeArgumentConstraints.ToArray());
        }

        public ClassBuilder AddGenericTypeArguments(params string[] genericTypeArguments)
        {
            GenericTypeArguments.AddRange(genericTypeArguments);
            return this;
        }

        public ClassBuilder AddGenericTypeArguments(System.Collections.Generic.IEnumerable<string> genericTypeArguments)
        {
            return AddGenericTypeArguments(genericTypeArguments.ToArray());
        }

        public ClassBuilder AddInterfaces(params string[] interfaces)
        {
            Interfaces.AddRange(interfaces);
            return this;
        }

        public ClassBuilder AddInterfaces(System.Collections.Generic.IEnumerable<string> interfaces)
        {
            return AddInterfaces(interfaces.ToArray());
        }

        public ClassBuilder AddInterfaces(params System.Type[] types)
        {
            return AddInterfaces(types.Select(x => x.FullName));
        }

        public ClassBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public ClassBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public ClassBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public ClassBuilder AddMethods(params ModelFramework.Objects.Builders.ClassMethodBuilder[] methods)
        {
            Methods.AddRange(methods);
            return this;
        }

        public ClassBuilder AddMethods(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Builders.ClassMethodBuilder> methods)
        {
            return AddMethods(methods.ToArray());
        }

        public ClassBuilder AddProperties(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Builders.ClassPropertyBuilder> properties)
        {
            return AddProperties(properties.ToArray());
        }

        public ClassBuilder AddProperties(params ModelFramework.Objects.Builders.ClassPropertyBuilder[] properties)
        {
            Properties.AddRange(properties);
            return this;
        }

        public ClassBuilder AddSubClasses(params ModelFramework.Objects.Builders.ClassBuilder[] subClasses)
        {
            SubClasses.AddRange(subClasses);
            return this;
        }

        public ClassBuilder AddSubClasses(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Builders.ClassBuilder> subClasses)
        {
            return AddSubClasses(subClasses.ToArray());
        }

        public ModelFramework.Objects.Contracts.IClass Build()
        {
            return new ModelFramework.Objects.Class(Fields.Select(x => x.Build()), Static, Sealed, SubClasses.Select(x => x.Build()), Constructors.Select(x => x.Build()), BaseClass, Record, Namespace, Partial, new CrossCutting.Common.ValueCollection<System.String>(Interfaces), Properties.Select(x => x.Build()), Methods.Select(x => x.Build()), new CrossCutting.Common.ValueCollection<System.String>(GenericTypeArguments), new CrossCutting.Common.ValueCollection<System.String>(GenericTypeArgumentConstraints), Metadata.Select(x => x.Build()), Visibility, Name, Attributes.Select(x => x.Build()), Enums.Select(x => x.Build()));
        }

        public ClassBuilder WithBaseClass(System.Func<string> baseClassDelegate)
        {
            _baseClassDelegate = new (baseClassDelegate);
            return this;
        }

        public ClassBuilder WithBaseClass(string baseClass)
        {
            BaseClass = baseClass;
            return this;
        }

        public ClassBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public ClassBuilder WithName(System.Func<string> nameDelegate)
        {
            _nameDelegate = new (nameDelegate);
            return this;
        }

        public ClassBuilder WithNamespace(System.Func<string> namespaceDelegate)
        {
            _namespaceDelegate = new (@namespaceDelegate);
            return this;
        }

        public ClassBuilder WithNamespace(string @namespace)
        {
            Namespace = @namespace;
            return this;
        }

        public ClassBuilder WithPartial(System.Func<bool> partialDelegate)
        {
            _partialDelegate = new (partialDelegate);
            return this;
        }

        public ClassBuilder WithPartial(bool partial = true)
        {
            Partial = partial;
            return this;
        }

        public ClassBuilder WithRecord(System.Func<bool> recordDelegate)
        {
            _recordDelegate = new (recordDelegate);
            return this;
        }

        public ClassBuilder WithRecord(bool record = true)
        {
            Record = record;
            return this;
        }

        public ClassBuilder WithSealed(System.Func<bool> sealedDelegate)
        {
            _sealedDelegate = new (@sealedDelegate);
            return this;
        }

        public ClassBuilder WithSealed(bool @sealed = true)
        {
            Sealed = @sealed;
            return this;
        }

        public ClassBuilder WithStatic(System.Func<bool> staticDelegate)
        {
            _staticDelegate = new (@staticDelegate);
            return this;
        }

        public ClassBuilder WithStatic(bool @static = true)
        {
            Static = @static;
            return this;
        }

        public ClassBuilder WithVisibility(ModelFramework.Objects.Contracts.Visibility visibility)
        {
            Visibility = visibility;
            return this;
        }

        public ClassBuilder WithVisibility(System.Func<ModelFramework.Objects.Contracts.Visibility> visibilityDelegate)
        {
            _visibilityDelegate = new (visibilityDelegate);
            return this;
        }

        public ClassBuilder()
        {
            Fields = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassFieldBuilder>();
            SubClasses = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassBuilder>();
            Constructors = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassConstructorBuilder>();
            Interfaces = new System.Collections.Generic.List<string>();
            Properties = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassPropertyBuilder>();
            Methods = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassMethodBuilder>();
            GenericTypeArguments = new System.Collections.Generic.List<string>();
            GenericTypeArgumentConstraints = new System.Collections.Generic.List<string>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeBuilder>();
            Enums = new System.Collections.Generic.List<ModelFramework.Objects.Builders.EnumBuilder>();
            _staticDelegate = new (() => default);
            _sealedDelegate = new (() => default);
            _baseClassDelegate = new (() => string.Empty);
            _recordDelegate = new (() => default);
            _namespaceDelegate = new (() => string.Empty);
            _partialDelegate = new (() => default);
            _visibilityDelegate = new (() => ModelFramework.Objects.Contracts.Visibility.Public);
            _nameDelegate = new (() => string.Empty);
        }

        public ClassBuilder(ModelFramework.Objects.Contracts.IClass source)
        {
            Fields = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassFieldBuilder>();
            SubClasses = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassBuilder>();
            Constructors = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassConstructorBuilder>();
            Interfaces = new System.Collections.Generic.List<string>();
            Properties = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassPropertyBuilder>();
            Methods = new System.Collections.Generic.List<ModelFramework.Objects.Builders.ClassMethodBuilder>();
            GenericTypeArguments = new System.Collections.Generic.List<string>();
            GenericTypeArgumentConstraints = new System.Collections.Generic.List<string>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeBuilder>();
            Enums = new System.Collections.Generic.List<ModelFramework.Objects.Builders.EnumBuilder>();
            Fields.AddRange(source.Fields.Select(x => new ModelFramework.Objects.Builders.ClassFieldBuilder(x)));
            _staticDelegate = new (() => source.Static);
            _sealedDelegate = new (() => source.Sealed);
            SubClasses.AddRange(source.SubClasses.Select(x => new ModelFramework.Objects.Builders.ClassBuilder(x)));
            Constructors.AddRange(source.Constructors.Select(x => new ModelFramework.Objects.Builders.ClassConstructorBuilder(x)));
            _baseClassDelegate = new (() => source.BaseClass);
            _recordDelegate = new (() => source.Record);
            _namespaceDelegate = new (() => source.Namespace);
            _partialDelegate = new (() => source.Partial);
            Interfaces.AddRange(source.Interfaces);
            Properties.AddRange(source.Properties.Select(x => new ModelFramework.Objects.Builders.ClassPropertyBuilder(x)));
            Methods.AddRange(source.Methods.Select(x => new ModelFramework.Objects.Builders.ClassMethodBuilder(x)));
            GenericTypeArguments.AddRange(source.GenericTypeArguments);
            GenericTypeArgumentConstraints.AddRange(source.GenericTypeArgumentConstraints);
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
            _visibilityDelegate = new (() => source.Visibility);
            _nameDelegate = new (() => source.Name);
            Attributes.AddRange(source.Attributes.Select(x => new ModelFramework.Objects.Builders.AttributeBuilder(x)));
            Enums.AddRange(source.Enums.Select(x => new ModelFramework.Objects.Builders.EnumBuilder(x)));
        }

        private System.Lazy<bool> _staticDelegate;

        private System.Lazy<bool> _sealedDelegate;

        private System.Lazy<string> _baseClassDelegate;

        private System.Lazy<bool> _recordDelegate;

        private System.Lazy<string> _namespaceDelegate;

        private System.Lazy<bool> _partialDelegate;

        private System.Lazy<ModelFramework.Objects.Contracts.Visibility> _visibilityDelegate;

        private System.Lazy<string> _nameDelegate;
    }
#nullable restore
}

