﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 7.0.12
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
    public abstract partial class TypeBaseBuilder<TBuilder, TEntity> : TypeBaseBuilder
        where TEntity : ModelFramework.Objects.Contracts.ITypeBase
        where TBuilder : TypeBaseBuilder<TBuilder, TEntity>
    {
        public abstract TEntity BuildTyped();

        public override ModelFramework.Objects.Contracts.ITypeBase Build()
        {
            return BuildTyped();
        }

        public TBuilder WithNamespace(string @namespace)
        {
            Namespace = @namespace;
            return (TBuilder)this;
        }

        public TBuilder WithPartial(bool partial = true)
        {
            Partial = partial;
            return (TBuilder)this;
        }

        public TBuilder AddInterfaces(System.Collections.Generic.IEnumerable<string> interfaces)
        {
            return AddInterfaces(interfaces.ToArray());
        }

        public TBuilder AddInterfaces(params string[] interfaces)
        {
            Interfaces.AddRange(interfaces);
            return (TBuilder)this;
        }

        public TBuilder AddProperties(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Builders.ClassPropertyBuilder> properties)
        {
            return AddProperties(properties.ToArray());
        }

        public TBuilder AddProperties(params ModelFramework.Objects.Builders.ClassPropertyBuilder[] properties)
        {
            Properties.AddRange(properties);
            return (TBuilder)this;
        }

        public TBuilder AddMethods(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Builders.ClassMethodBuilder> methods)
        {
            return AddMethods(methods.ToArray());
        }

        public TBuilder AddMethods(params ModelFramework.Objects.Builders.ClassMethodBuilder[] methods)
        {
            Methods.AddRange(methods);
            return (TBuilder)this;
        }

        public TBuilder AddGenericTypeArguments(System.Collections.Generic.IEnumerable<string> genericTypeArguments)
        {
            return AddGenericTypeArguments(genericTypeArguments.ToArray());
        }

        public TBuilder AddGenericTypeArguments(params string[] genericTypeArguments)
        {
            GenericTypeArguments.AddRange(genericTypeArguments);
            return (TBuilder)this;
        }

        public TBuilder AddGenericTypeArgumentConstraints(System.Collections.Generic.IEnumerable<string> genericTypeArgumentConstraints)
        {
            return AddGenericTypeArgumentConstraints(genericTypeArgumentConstraints.ToArray());
        }

        public TBuilder AddGenericTypeArgumentConstraints(params string[] genericTypeArgumentConstraints)
        {
            GenericTypeArgumentConstraints.AddRange(genericTypeArgumentConstraints);
            return (TBuilder)this;
        }

        public TBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public TBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return (TBuilder)this;
        }

        public TBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return (TBuilder)this;
        }

        public TBuilder WithVisibility(ModelFramework.Objects.Contracts.Visibility visibility)
        {
            Visibility = visibility;
            return (TBuilder)this;
        }

        public TBuilder WithName(string name)
        {
            Name = name;
            return (TBuilder)this;
        }

        public TBuilder AddAttributes(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Builders.AttributeBuilder> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }

        public TBuilder AddAttributes(params ModelFramework.Objects.Builders.AttributeBuilder[] attributes)
        {
            Attributes.AddRange(attributes);
            return (TBuilder)this;
        }

        protected TypeBaseBuilder() : base()
        {
        }

        protected TypeBaseBuilder(ModelFramework.Objects.Contracts.ITypeBase source) : base(source)
        {
        }
    }
#nullable restore
}

