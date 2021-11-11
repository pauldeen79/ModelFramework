using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Default;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Objects.Builders
{
    public class InterfaceBuilder
    {
        public string Namespace { get; set; }
        public List<string> Interfaces { get; set; }
        public List<ClassPropertyBuilder> Properties { get; set; }
        public List<ClassMethodBuilder> Methods { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public Visibility Visibility { get; set; }
        public bool Partial { get; set; }
        public string Name { get; set; }
        public List<AttributeBuilder> Attributes { get; set; }
        public List<string> GenericTypeArguments { get; set; }
        public IInterface Build()
        {
            return new Interface(Name, Namespace, Visibility, Partial, Interfaces, Properties.Select(x => x.Build()), Methods.Select(x => x.Build()), Metadata.Select(x => x.Build()), Attributes.Select(x => x.Build()), GenericTypeArguments);
        }
        public InterfaceBuilder Clear()
        {
            Namespace = default;
            Interfaces.Clear();
            Properties.Clear();
            Methods.Clear();
            Metadata.Clear();
            Visibility = default;
            Name = default;
            Attributes.Clear();
            GenericTypeArguments = default;
            return this;
        }
        public InterfaceBuilder Update(IInterface source)
        {
            Namespace = default;
            Interfaces = new List<string>();
            Properties = new List<ClassPropertyBuilder>();
            Methods = new List<ClassMethodBuilder>();
            Metadata = new List<MetadataBuilder>();
            Visibility = default;
            Name = default;
            Attributes = new List<AttributeBuilder>();
            GenericTypeArguments = default;
            if (source != null)
            {
                Namespace = source.Namespace;
                foreach (var x in source.Interfaces) Interfaces.Add(x);
                Properties.AddRange(source.Properties.Select(x => new ClassPropertyBuilder(x)));
                Methods.AddRange(source.Methods.Select(x => new ClassMethodBuilder(x)));
                Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));
                Visibility = source.Visibility;
                Name = source.Name;
                Attributes.AddRange(source.Attributes.Select(x => new AttributeBuilder(x)));
                GenericTypeArguments = new List<string>(source.GenericTypeArguments ?? Array.Empty<string>());
            }
            return this;
        }
        public InterfaceBuilder WithNamespace(string @namespace)
        {
            Namespace = @namespace;
            return this;
        }
        public InterfaceBuilder WithPartial(bool partial)
        {
            Partial = partial;
            return this;
        }
        public InterfaceBuilder ClearInterfaces()
        {
            Interfaces.Clear();
            return this;
        }
        public InterfaceBuilder AddInterfaces(IEnumerable<string> interfaces)
        {
            return AddInterfaces(interfaces.ToArray());
        }
        public InterfaceBuilder AddInterfaces(params string[] interfaces)
        {
            if (interfaces != null)
            {
                Interfaces.AddRange(interfaces);
            }
            return this;
        }
        public InterfaceBuilder ClearProperties()
        {
            Properties.Clear();
            return this;
        }
        public InterfaceBuilder AddProperties(IEnumerable<ClassPropertyBuilder> properties)
        {
            return AddProperties(properties.ToArray());
        }
        public InterfaceBuilder AddProperties(params ClassPropertyBuilder[] properties)
        {
            if (properties != null)
            {
                Properties.AddRange(properties);
            }
            return this;
        }
        public InterfaceBuilder AddProperties(IEnumerable<IClassProperty> properties)
        {
            return AddProperties(properties.ToArray());
        }
        public InterfaceBuilder AddProperties(params IClassProperty[] properties)
        {
            if (properties != null)
            {
                Properties.AddRange(properties.Select(x => new ClassPropertyBuilder(x)));
            }
            return this;
        }
        public InterfaceBuilder ClearMethods()
        {
            Methods.Clear();
            return this;
        }
        public InterfaceBuilder AddMethods(IEnumerable<ClassMethodBuilder> methods)
        {
            return AddMethods(methods.ToArray());
        }
        public InterfaceBuilder AddMethods(params ClassMethodBuilder[] methods)
        {
            if (methods != null)
            {
                Methods.AddRange(methods);
            }
            return this;
        }
        public InterfaceBuilder AddMethods(IEnumerable<IClassMethod> methods)
        {
            return AddMethods(methods.ToArray());
        }
        public InterfaceBuilder AddMethods(params IClassMethod[] methods)
        {
            if (methods != null)
            {
                Methods.AddRange(methods.Select(x => new ClassMethodBuilder(x)));
            }
            return this;
        }
        public InterfaceBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public InterfaceBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public InterfaceBuilder AddMetadata(params MetadataBuilder[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata);
            }
            return this;
        }
        public InterfaceBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public InterfaceBuilder AddMetadata(params IMetadata[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public InterfaceBuilder WithVisibility(Visibility visibility)
        {
            Visibility = visibility;
            return this;
        }
        public InterfaceBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public InterfaceBuilder ClearAttributes()
        {
            Attributes.Clear();
            return this;
        }
        public InterfaceBuilder AddAttributes(IEnumerable<AttributeBuilder> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }
        public InterfaceBuilder AddAttributes(params AttributeBuilder[] attributes)
        {
            if (attributes != null)
            {
                Attributes.AddRange(attributes);
            }
            return this;
        }
        public InterfaceBuilder AddAttributes(IEnumerable<IAttribute> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }
        public InterfaceBuilder AddAttributes(params IAttribute[] attributes)
        {
            if (attributes != null)
            {
                Attributes.AddRange(attributes.Select(x => new AttributeBuilder(x)));
            }
            return this;
        }
        public InterfaceBuilder ClearGenericTypeArguments()
        {
            GenericTypeArguments.Clear();
            return this;
        }
        public InterfaceBuilder AddGenericTypeArguments(IEnumerable<string> methods)
        {
            return AddGenericTypeArguments(methods.ToArray());
        }
        public InterfaceBuilder AddGenericTypeArguments(params string[] methods)
        {
            if (methods != null)
            {
                GenericTypeArguments.AddRange(methods);
            }
            return this;
        }
        public InterfaceBuilder(IInterface source = null)
        {
            if (source != null)
            {
                Namespace = source.Namespace;
                Interfaces = new List<string>(source.Interfaces);
                Properties = new List<ClassPropertyBuilder>(source.Properties.Select(x => new ClassPropertyBuilder(x)));
                Methods = new List<ClassMethodBuilder>(source.Methods.Select(x => new ClassMethodBuilder(x)));
                Metadata = new List<MetadataBuilder>(source.Metadata.Select(x => new MetadataBuilder(x)));
                Visibility = source.Visibility;
                Name = source.Name;
                Attributes = new List<AttributeBuilder>(source.Attributes.Select(x => new AttributeBuilder(x)));
                GenericTypeArguments = new List<string>(source.GenericTypeArguments ?? Array.Empty<string>());
            }
            else
            {
                Interfaces = new List<string>();
                Properties = new List<ClassPropertyBuilder>();
                Methods = new List<ClassMethodBuilder>();
                Metadata = new List<MetadataBuilder>();
                Attributes = new List<AttributeBuilder>();
                GenericTypeArguments = new List<string>();
            }
        }
        public InterfaceBuilder(string name,
                                string @namespace,
                                Visibility visibility = Visibility.Public,
                                bool partial = false,
                                IEnumerable<string> interfaces = null,
                                IEnumerable<IClassProperty> properties = null,
                                IEnumerable<IClassMethod> methods = null,
                                IEnumerable<IMetadata> metadata = null,
                                IEnumerable<IAttribute> attributes = null,
                                IEnumerable<string> genericTypeArguments = null)
        {
            Interfaces = new List<string>();
            Properties = new List<ClassPropertyBuilder>();
            Methods = new List<ClassMethodBuilder>();
            Metadata = new List<MetadataBuilder>();
            Attributes = new List<AttributeBuilder>();
            GenericTypeArguments = new List<string>();
            Name = name;
            Namespace = @namespace;
            Partial = partial;
            Visibility = visibility;
            if (interfaces != null) Interfaces.AddRange(interfaces);
            if (properties != null) Properties.AddRange(properties.Select(x => new ClassPropertyBuilder(x)));
            if (metadata != null) Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            if (attributes != null) Attributes.AddRange(attributes.Select(x => new AttributeBuilder(x)));
            if (methods != null) Methods.AddRange(methods.Select(x => new ClassMethodBuilder(x)));
            if (genericTypeArguments != null) GenericTypeArguments.AddRange(genericTypeArguments);
        }
    }
}
