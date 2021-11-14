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
            return new Class(Name, Namespace, Visibility, BaseClass, Static, Sealed, Partial, AutoGenerateInterface, AutoGenerateInterfaceSettings.Build(), Record, Interfaces, Fields.Select(x => x.Build()), Properties.Select(x => x.Build()), Methods.Select(x => x.Build()), Constructors.Select(x => x.Build()), Metadata.Select(x => x.Build()), Attributes.Select(x => x.Build()), SubClasses.Select(x => x.Build()), Enums.Select(x => x.Build()), GenericTypeArguments);
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
#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high
        public ClassBuilder Update(IClass source)
#pragma warning restore S3776 // Cognitive Complexity of methods should not be too high
        {
            Interfaces = new List<string>();
            Fields = new List<ClassFieldBuilder>();
            Properties = new List<ClassPropertyBuilder>();
            Static = default;
            Sealed = default;
            Partial = default;
            AutoGenerateInterface = default;
            AutoGenerateInterfaceSettings = new InterfaceSettingsBuilder();
            Record = default;
            Metadata = new List<MetadataBuilder>();
            Visibility = default;
            Name = default;
            Attributes = new List<AttributeBuilder>();
            SubClasses = new List<ClassBuilder>();
            Enums = new List<EnumBuilder>();
            Namespace = default;
            Constructors = new List<ClassConstructorBuilder>();
            Methods = new List<ClassMethodBuilder>();
            BaseClass = default;
            GenericTypeArguments = new List<string>();
            if (source != null)
            {
                if (source.Interfaces != null) foreach (var x in source.Interfaces) Interfaces.Add(x);
                if (source.Fields != null) Fields.AddRange(source.Fields.Select(x => new ClassFieldBuilder(x)));
                Properties.AddRange(source.Properties.Select(x => new ClassPropertyBuilder(x)));
                Static = source.Static;
                Sealed = source.Sealed;
                Partial = source.Partial;
                AutoGenerateInterface = source.AutoGenerateInterface;
                AutoGenerateInterfaceSettings.Update(source.AutoGenerateInterfaceSettings);
                Record = source.Record;
                if (source.Metadata != null) Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));
                Visibility = source.Visibility;
                Name = source.Name;
                if (source.Attributes != null) Attributes.AddRange(source.Attributes.Select(x => new AttributeBuilder(x)));
                if (source.SubClasses != null) SubClasses.AddRange(source.SubClasses.Select(x => new ClassBuilder(x)));
                if (source.Enums != null) Enums.AddRange(source.Enums.Select(x => new EnumBuilder(x)));
                Namespace = source.Namespace;
                if (source.Constructors != null) Constructors.AddRange(source.Constructors.Select(x => new ClassConstructorBuilder(x)));
                if (source.Metadata != null) Methods.AddRange(source.Methods.Select(x => new ClassMethodBuilder(x)));
                BaseClass = source.BaseClass;
                if (source.GenericTypeArguments != null) GenericTypeArguments.AddRange(source.GenericTypeArguments);
            }
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
        public ClassBuilder WithStatic(bool @static)
        {
            Static = @static;
            return this;
        }
        public ClassBuilder WithSealed(bool @sealed)
        {
            Sealed = @sealed;
            return this;
        }
        public ClassBuilder WithPartial(bool partial)
        {
            Partial = partial;
            return this;
        }
        public ClassBuilder WithAutoGenerateInterface(bool autoGenerateInterface)
        {
            AutoGenerateInterface = autoGenerateInterface;
            return this;
        }
        public ClassBuilder WithAutoGenerateInterfaceSettings(InterfaceSettings autoGenerateInterfaceSettings)
        {
            AutoGenerateInterfaceSettings.Update(autoGenerateInterfaceSettings);
            return this;
        }
        public ClassBuilder WithAutoGenerateInterfaceSettings(InterfaceSettingsBuilder autoGenerateInterfaceSettingsBuilder)
        {
            AutoGenerateInterfaceSettings = autoGenerateInterfaceSettingsBuilder;
            return this;
        }
        public ClassBuilder WithRecord(bool record)
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
        public ClassBuilder(IClass source = null)
        {
            if (source != null)
            {
                Interfaces = new List<string>(source.Interfaces ?? Enumerable.Empty<string>());
                Fields = new List<ClassFieldBuilder>(source.Fields?.Select(x => new ClassFieldBuilder(x)) ?? Enumerable.Empty<ClassFieldBuilder>());
                Properties = new List<ClassPropertyBuilder>(source.Properties?.Select(x => new ClassPropertyBuilder(x)) ?? Enumerable.Empty<ClassPropertyBuilder>());
                Static = source.Static;
                Sealed = source.Sealed;
                Partial = source.Partial;
                AutoGenerateInterface = source.AutoGenerateInterface;
                AutoGenerateInterfaceSettings = new InterfaceSettingsBuilder(source.AutoGenerateInterfaceSettings);
                Metadata = new List<MetadataBuilder>(source.Metadata.Select(x => new MetadataBuilder(x)));
                Visibility = source.Visibility;
                Name = source.Name;
                Attributes = new List<AttributeBuilder>(source.Attributes.Select(x => new AttributeBuilder(x)));
                SubClasses = new List<ClassBuilder>(source.SubClasses.Select(x => new ClassBuilder(x)));
                Enums = new List<EnumBuilder>(source.Enums.Select(x => new EnumBuilder(x)));
                Namespace = source.Namespace;
                Constructors = new List<ClassConstructorBuilder>(source.Constructors.Select(x=> new ClassConstructorBuilder(x)));
                Methods = new List<ClassMethodBuilder>(source.Methods.Select(x => new ClassMethodBuilder(x)));
                BaseClass = source.BaseClass;
                GenericTypeArguments = new List<string>(source.GenericTypeArguments ?? Array.Empty<string>());
            }
            else
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
        }
#pragma warning disable S107 // Methods should not have too many parameters
        public ClassBuilder(string name,
                            string @namespace,
                            Visibility visibility = Visibility.Public,
                            string baseClass = null,
                            bool @static = false,
                            bool @sealed = false,
                            bool partial = false,
                            bool autoGenerateInterface = false,
                            InterfaceSettings autoGenerateInterfaceSettings = null,
                            bool record = false,
                            IEnumerable<string> interfaces = null,
                            IEnumerable<IClassField> fields = null,
                            IEnumerable<IClassProperty> properties = null,
                            IEnumerable<IClassMethod> methods = null,
                            IEnumerable<IClassConstructor> constructors = null,
                            IEnumerable<IMetadata> metadata = null,
                            IEnumerable<IAttribute> attributes = null,
                            IEnumerable<IClass> subClasses = null,
                            IEnumerable<IEnum> enums = null,
                            IEnumerable<string> genericTypeArguments = null)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            Interfaces = new List<string>();
            Fields = new List<ClassFieldBuilder>();
            Properties = new List<ClassPropertyBuilder>();
            Metadata = new List<MetadataBuilder>();
            Attributes = new List<AttributeBuilder>();
            SubClasses = new List<ClassBuilder>();
            Enums = new List<EnumBuilder>();
            Constructors = new List<ClassConstructorBuilder>();
            Methods = new List<ClassMethodBuilder>();
            GenericTypeArguments = new List<string>();
            Name = name;
            Namespace = @namespace;
            Visibility = visibility;
            BaseClass = baseClass;
            Static = @static;
            Sealed = @sealed;
            Partial = partial;
            AutoGenerateInterfaceSettings = new InterfaceSettingsBuilder(autoGenerateInterfaceSettings);
            AutoGenerateInterface = autoGenerateInterface;
            Record = record;
            if (interfaces != null) Interfaces.AddRange(interfaces);
            if (fields != null) Fields.AddRange(fields.Select(x => new ClassFieldBuilder(x)));
            if (properties != null) Properties.AddRange(properties.Select(x => new ClassPropertyBuilder(x)));
            if (metadata != null) Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            if (attributes != null) Attributes.AddRange(attributes.Select(x => new AttributeBuilder(x)));
            if (subClasses != null) SubClasses.AddRange(subClasses.Select(x => new ClassBuilder(x)));
            if (enums != null) Enums.AddRange(enums.Select(x => new EnumBuilder(x)));
            if (constructors != null) Constructors.AddRange(constructors.Select(x => new ClassConstructorBuilder(x)));
            if (methods != null) Methods.AddRange(methods.Select(x => new ClassMethodBuilder(x)));
            if (genericTypeArguments != null) GenericTypeArguments.AddRange(genericTypeArguments);
        }
    }
}
