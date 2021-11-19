using System;
using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Default;
using ModelFramework.Objects.Settings;

namespace ModelFramework.Objects.Builders
{
    public class ClassBuilder
    {
        public List<string> Interfaces { get; set; }
        public List<ClassFieldBuilder> Fields { get; set; }
        public List<ClassPropertyBuilder> Properties { get; set; }
        public bool Static { get; set; }
        public bool Sealed { get; set; }
        public bool Partial { get; set; }
        public bool AutoGenerateInterface { get; set; }
        public InterfaceSettingsBuilder AutoGenerateInterfaceSettings { get; set; }
        public bool Record { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public Visibility Visibility { get; set; }
        public string Name { get; set; }
        public List<AttributeBuilder> Attributes { get; set; }
        public List<ClassBuilder> SubClasses { get; set; }
        public List<EnumBuilder> Enums { get; set; }
        public string Namespace { get; set; }
        public List<ClassConstructorBuilder> Constructors { get; set; }
        public List<ClassMethodBuilder> Methods { get; set; }
        public string BaseClass { get; set; }
        public List<string> GenericTypeArguments { get; set; }
        public IClass Build()
        {
            return new Class(Name,
                             Namespace,
                             Visibility,
                             BaseClass,
                             Static,
                             Sealed,
                             Partial,
                             AutoGenerateInterface,
                             AutoGenerateInterfaceSettings?.Build(),
                             Record,
                             Interfaces,
                             Fields.Select(x => x.Build()),
                             Properties.Select(x => x.Build()),
                             Methods.Select(x => x.Build()),
                             Constructors.Select(x => x.Build()),
                             Metadata.Select(x => x.Build()),
                             Attributes.Select(x => x.Build()),
                             SubClasses.Select(x => x.Build()),
                             Enums.Select(x => x.Build()),
                             GenericTypeArguments);
        }
        public ClassBuilder Clear()
        {
            Interfaces.Clear();
            Fields.Clear();
            Properties.Clear();
            Static = default;
            Sealed = default;
            Partial = default;
            AutoGenerateInterface = default;
            AutoGenerateInterfaceSettings = new InterfaceSettingsBuilder();
            Record = default;
            Metadata.Clear();
            Visibility = default;
            Name = default;
            Attributes.Clear();
            SubClasses.Clear();
            Enums.Clear();
            Namespace = default;
            Constructors.Clear();
            Methods.Clear();
            BaseClass = default;
            GenericTypeArguments = default;
            return this;
        }
        public ClassBuilder ClearInterfaces()
        {
            Interfaces.Clear();
            return this;
        }
        public ClassBuilder AddInterfaces(IEnumerable<string> interfaces)
        {
            return AddInterfaces(interfaces.ToArray());
        }
        public ClassBuilder AddInterfaces(params string[] interfaces)
        {
            if (interfaces != null)
            {
                Interfaces.AddRange(interfaces);
            }
            return this;
        }
        public ClassBuilder ClearFields()
        {
            Fields.Clear();
            return this;
        }
        public ClassBuilder AddFields(IEnumerable<ClassFieldBuilder> fields)
        {
            return AddFields(fields.ToArray());
        }
        public ClassBuilder AddFields(params ClassFieldBuilder[] fields)
        {
            if (fields != null)
            {
                Fields.AddRange(fields);
            }
            return this;
        }
        public ClassBuilder AddFields(IEnumerable<IClassField> fields)
        {
            return AddFields(fields.ToArray());
        }
        public ClassBuilder AddFields(params IClassField[] fields)
        {
            if (fields != null)
            {
                Fields.AddRange(fields.Select(x => new ClassFieldBuilder(x)));
            }
            return this;
        }
        public ClassBuilder ClearProperties()
        {
            Properties.Clear();
            return this;
        }
        public ClassBuilder AddProperties(IEnumerable<ClassPropertyBuilder> properties)
        {
            return AddProperties(properties.ToArray());
        }
        public ClassBuilder AddProperties(params ClassPropertyBuilder[] properties)
        {
            if (properties != null)
            {
                Properties.AddRange(properties);
            }
            return this;
        }
        public ClassBuilder AddProperties(IEnumerable<IClassProperty> properties)
        {
            return AddProperties(properties.ToArray());
        }
        public ClassBuilder AddProperties(IClassProperty[] properties)
        {
            if (properties != null)
            {
                Properties.AddRange(properties.Select(x => new ClassPropertyBuilder(x)));
            }
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
        public ClassBuilder WithPartial(bool partial = true)
        {
            Partial = partial;
            return this;
        }
        public ClassBuilder WithAutoGenerateInterface(bool autoGenerateInterface = true)
        {
            AutoGenerateInterface = autoGenerateInterface;
            return this;
        }
        public ClassBuilder WithAutoGenerateInterfaceSettings(InterfaceSettings autoGenerateInterfaceSettings)
        {
            AutoGenerateInterfaceSettings = new InterfaceSettingsBuilder(autoGenerateInterfaceSettings);
            return this;
        }
        public ClassBuilder WithAutoGenerateInterfaceSettings(InterfaceSettingsBuilder autoGenerateInterfaceSettingsBuilder)
        {
            AutoGenerateInterfaceSettings = autoGenerateInterfaceSettingsBuilder;
            return this;
        }
        public ClassBuilder WithRecord(bool record = true)
        {
            Record = record;
            return this;
        }
        public ClassBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public ClassBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ClassBuilder AddMetadata(params MetadataBuilder[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata);
            }
            return this;
        }
        public ClassBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ClassBuilder AddMetadata(params IMetadata[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public ClassBuilder WithVisibility(Visibility visibility)
        {
            Visibility = visibility;
            return this;
        }
        public ClassBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public ClassBuilder ClearAttributes()
        {
            Attributes.Clear();
            return this;
        }
        public ClassBuilder AddAttributes(IEnumerable<AttributeBuilder> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }
        public ClassBuilder AddAttributes(params AttributeBuilder[] attributes)
        {
            if (attributes != null)
            {
                Attributes.AddRange(attributes);
            }
            return this;
        }
        public ClassBuilder AddAttributes(IEnumerable<IAttribute> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }
        public ClassBuilder AddAttributes(params IAttribute[] attributes)
        {
            if (attributes != null)
            {
                Attributes.AddRange(attributes.Select(x => new AttributeBuilder(x)));
            }
            return this;
        }
        public ClassBuilder ClearSubClasses()
        {
            SubClasses.Clear();
            return this;
        }
        public ClassBuilder AddSubClasses(IEnumerable<ClassBuilder> subClasses)
        {
            return AddSubClasses(subClasses.ToArray());
        }
        public ClassBuilder AddSubClasses(params ClassBuilder[] subClasses)
        {
            if (subClasses != null)
            {
                SubClasses.AddRange(subClasses);
            }
            return this;
        }
        public ClassBuilder AddSubClasses(IEnumerable<IClass> subClasses)
        {
            return AddSubClasses(subClasses.ToArray());
        }
        public ClassBuilder AddSubClasses(params IClass[] subClasses)
        {
            if (subClasses != null)
            {
                SubClasses.AddRange(subClasses.Select(x => new ClassBuilder(x)));
            }
            return this;
        }
        public ClassBuilder ClearEnums()
        {
            Enums.Clear();
            return this;
        }
        public ClassBuilder AddEnums(IEnumerable<EnumBuilder> enums)
        {
            return AddEnums(enums.ToArray());
        }
        public ClassBuilder AddEnums(params EnumBuilder[] enums)
        {
            Enums.AddRange(enums);
            return this;
        }
        public ClassBuilder AddEnums(IEnumerable<IEnum> enums)
        {
            return AddEnums(enums.ToArray());
        }
        public ClassBuilder AddEnums(params IEnum[] enums)
        {
            if (enums != null)
            {
                foreach (var itemToAdd in enums)
                {
                    Enums.Add(new EnumBuilder(itemToAdd));
                }
            }
            return this;
        }
        public ClassBuilder WithNamespace(string @namespace)
        {
            Namespace = @namespace;
            return this;
        }
        public ClassBuilder ClearConstructors()
        {
            Constructors.Clear();
            return this;
        }
        public ClassBuilder AddConstructors(IEnumerable<ClassConstructorBuilder> constructors)
        {
            return AddConstructors(constructors.ToArray());
        }
        public ClassBuilder AddConstructors(params ClassConstructorBuilder[] constructors)
        {
            if (constructors != null)
            {
                Constructors.AddRange(constructors);
            }
            return this;
        }
        public ClassBuilder AddConstructors(IEnumerable<IClassConstructor> constructors)
        {
            return AddConstructors(constructors.ToArray());
        }
        public ClassBuilder AddConstructors(params IClassConstructor[] constructors)
        {
            if (constructors != null)
            {
                Constructors.AddRange(constructors.Select(x => new ClassConstructorBuilder(x)));
            }
            return this;
        }
        public ClassBuilder ClearMethods()
        {
            Methods.Clear();
            return this;
        }
        public ClassBuilder AddMethods(IEnumerable<ClassMethodBuilder> methods)
        {
            return AddMethods(methods.ToArray());
        }
        public ClassBuilder AddMethods(params ClassMethodBuilder[] methods)
        {
            if (methods != null)
            {
                Methods.AddRange(methods);
            }
            return this;
        }
        public ClassBuilder AddMethods(IEnumerable<IClassMethod> methods)
        {
            return AddMethods(methods.ToArray());
        }
        public ClassBuilder AddMethods(params IClassMethod[] methods)
        {
            if (methods != null)
            {
                Methods.AddRange(methods.Select(x => new ClassMethodBuilder(x)));
            }
            return this;
        }
        public ClassBuilder AddGenericTypeArguments(IEnumerable<string> methods)
        {
            return AddGenericTypeArguments(methods.ToArray());
        }
        public ClassBuilder AddGenericTypeArguments(params string[] methods)
        {
            if (methods != null)
            {
                GenericTypeArguments.AddRange(methods);
            }
            return this;
        }
        public ClassBuilder WithBaseClass(string baseClass)
        {
            BaseClass = baseClass;
            return this;
        }
        public ClassBuilder()
        {
            Interfaces = new List<string>();
            Fields = new List<ClassFieldBuilder>();
            Properties = new List<ClassPropertyBuilder>();
            AutoGenerateInterfaceSettings = new InterfaceSettingsBuilder();
            Metadata = new List<MetadataBuilder>();
            Attributes = new List<AttributeBuilder>();
            SubClasses = new List<ClassBuilder>();
            Enums = new List<EnumBuilder>();
            Constructors = new List<ClassConstructorBuilder>();
            Methods = new List<ClassMethodBuilder>();
            GenericTypeArguments = new List<string>();
        }
        public ClassBuilder(IClass source)
        {
            Interfaces = new List<string>(source.Interfaces ?? Enumerable.Empty<string>());
            Fields = new List<ClassFieldBuilder>(source.Fields?.Select(x => new ClassFieldBuilder(x)) ?? Enumerable.Empty<ClassFieldBuilder>());
            Properties = new List<ClassPropertyBuilder>(source.Properties?.Select(x => new ClassPropertyBuilder(x)) ?? Enumerable.Empty<ClassPropertyBuilder>());
            Static = source.Static;
            Sealed = source.Sealed;
            Partial = source.Partial;
            AutoGenerateInterface = source.AutoGenerateInterface;
            AutoGenerateInterfaceSettings = source.AutoGenerateInterfaceSettings == null
                ? null
                : new InterfaceSettingsBuilder(source.AutoGenerateInterfaceSettings);
            Metadata = new List<MetadataBuilder>(source.Metadata.Select(x => new MetadataBuilder(x)));
            Visibility = source.Visibility;
            Name = source.Name;
            Attributes = new List<AttributeBuilder>(source.Attributes.Select(x => new AttributeBuilder(x)));
            SubClasses = new List<ClassBuilder>(source.SubClasses.Select(x => new ClassBuilder(x)));
            Enums = new List<EnumBuilder>(source.Enums.Select(x => new EnumBuilder(x)));
            Namespace = source.Namespace;
            Constructors = new List<ClassConstructorBuilder>(source.Constructors.Select(x => new ClassConstructorBuilder(x)));
            Methods = new List<ClassMethodBuilder>(source.Methods.Select(x => new ClassMethodBuilder(x)));
            BaseClass = source.BaseClass;
            GenericTypeArguments = new List<string>(source.GenericTypeArguments ?? Enumerable.Empty<string>());
        }
    }
}
