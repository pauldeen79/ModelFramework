using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Default;

namespace ModelFramework.Objects.Builders
{
    public class ClassPropertyBuilder
    {
        public bool Static { get; set; }
        public bool HasGetter { get; set; }
        public bool HasSetter { get; set; }
        public bool HasInit { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public Visibility Visibility { get; set; }
        public Visibility? GetterVisibility { get; set; }
        public Visibility? SetterVisibility { get; set; }
        public Visibility? InitVisibility { get; set; }
        public string GetterBody { get; set; }
        public string SetterBody { get; set; }
        public string InitBody { get; set; }
        public string Name { get; set; }
        public List<AttributeBuilder> Attributes { get; set; }
        public string TypeName { get; set; }
        public bool Virtual { get; set; }
        public bool Abstract { get; set; }
        public bool Protected { get; set; }
        public bool Override { get; set; }
        public bool IsNullable { get; set; }
        public string ExplicitInterfaceName { get; set; }
        public List<ICodeStatementBuilder> GetterCodeStatements { get; set; }
        public List<ICodeStatementBuilder> SetterCodeStatements { get; set; }
        public List<ICodeStatementBuilder> InitCodeStatements { get; set; }
        public IClassProperty Build()
        {
            return new ClassProperty(Name,
                                     TypeName,
                                     Static,
                                     Virtual,
                                     Abstract,
                                     Protected,
                                     Override,
                                     HasGetter,
                                     HasSetter, 
                                     HasInit,
                                     IsNullable,
                                     Visibility,
                                     GetterVisibility,
                                     SetterVisibility,
                                     InitVisibility,
                                     GetterBody,
                                     SetterBody,
                                     InitBody,
                                     ExplicitInterfaceName,
                                     Metadata.Select(x => x.Build()),
                                     Attributes.Select(x => x.Build()),
                                     GetterCodeStatements.Select(x => x.Build()),
                                     SetterCodeStatements.Select(x => x.Build()),
                                     InitCodeStatements.Select(x => x.Build()));
        }
        public ClassPropertyBuilder Clear()
        {
            Static = default;
            HasGetter = true;
            HasSetter = true;
            HasInit = default;
            Metadata.Clear();
            Visibility = default;
            GetterVisibility = default;
            SetterVisibility = default;
            InitVisibility = default;
            GetterBody = default;
            SetterBody = default;
            InitBody = default;
            Name = default;
            Attributes.Clear();
            TypeName = default;
            Virtual = default;
            Abstract = default;
            Protected = default;
            Override = default;
            IsNullable = default;
            ExplicitInterfaceName = default;
            GetterCodeStatements.Clear();
            SetterCodeStatements.Clear();
            InitCodeStatements.Clear();
            return this;
        }
        public ClassPropertyBuilder Update(ClassProperty source)
        {
            Metadata = new List<MetadataBuilder>();
            Attributes = new List<AttributeBuilder>();
            GetterCodeStatements = new List<ICodeStatementBuilder>();
            SetterCodeStatements = new List<ICodeStatementBuilder>();
            InitCodeStatements = new List<ICodeStatementBuilder>();

            Static = source.Static;
            HasGetter = source.HasGetter;
            HasSetter = source.HasSetter;
            HasInit = source.HasInit;
            Metadata.AddRange(source.Metadata?.Select(x => new MetadataBuilder(x)) ?? Enumerable.Empty<MetadataBuilder>());
            Visibility = source.Visibility;
            GetterVisibility = source.GetterVisibility;
            SetterVisibility = source.SetterVisibility;
            InitVisibility = source.InitVisibility;
            GetterBody = source.GetterBody;
            SetterBody = source.SetterBody;
            InitBody = source.InitBody;
            Name = source.Name;
            Attributes.AddRange(source.Attributes?.Select(x => new AttributeBuilder(x)) ?? Enumerable.Empty<AttributeBuilder>());
            TypeName = source.TypeName;
            Virtual = source.Virtual;
            Abstract = source.Abstract;
            Protected = source.Protected;
            Override = source.Override;
            IsNullable = source.IsNullable;
            ExplicitInterfaceName = source.ExplicitInterfaceName;
            GetterCodeStatements.AddRange(source.GetterCodeStatements?.Select(x => x.CreateBuilder()) ?? Enumerable.Empty<ICodeStatementBuilder>());
            SetterCodeStatements.AddRange(source.SetterCodeStatements?.Select(x => x.CreateBuilder()) ?? Enumerable.Empty<ICodeStatementBuilder>());
            InitCodeStatements.AddRange(source.InitCodeStatements?.Select(x => x.CreateBuilder()) ?? Enumerable.Empty<ICodeStatementBuilder>());

            return this;
        }
        public ClassPropertyBuilder WithStatic(bool @static = true)
        {
            Static = @static;
            return this;
        }
        public ClassPropertyBuilder WithHasGetter(bool hasGetter = true)
        {
            HasGetter = hasGetter;
            return this;
        }
        public ClassPropertyBuilder WithHasSetter(bool hasSetter = true)
        {
            HasSetter = hasSetter;
            if (hasSetter)
            {
                HasInit = false;
            }
            return this;
        }
        public ClassPropertyBuilder WithHasInit(bool hasInit = true)
        {
            HasInit = hasInit;
            if (hasInit)
            {
                HasSetter = false;
            }
            return this;
        }
        public ClassPropertyBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public ClassPropertyBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ClassPropertyBuilder AddMetadata(params MetadataBuilder[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata);
            }
            return this;
        }
        public ClassPropertyBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ClassPropertyBuilder AddMetadata(params IMetadata[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public ClassPropertyBuilder WithVisibility(Visibility visibility)
        {
            Visibility = visibility;
            return this;
        }
        public ClassPropertyBuilder WithGetterVisibility(Visibility? getterVisibility)
        {
            GetterVisibility = getterVisibility;
            return this;
        }
        public ClassPropertyBuilder WithSetterVisibility(Visibility? setterVisibility)
        {
            SetterVisibility = setterVisibility;
            return this;
        }
        public ClassPropertyBuilder WithInitVisibility(Visibility? initVisibility)
        {
            InitVisibility = initVisibility;
            return this;
        }
        public ClassPropertyBuilder WithGetterBody(string getterBody)
        {
            GetterBody = getterBody;
            return this;
        }
        public ClassPropertyBuilder WithSetterBody(string setterBody)
        {
            SetterBody = setterBody;
            return this;
        }
        public ClassPropertyBuilder WithInitBody(string initBody)
        {
            InitBody = initBody;
            return this;
        }
        public ClassPropertyBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public ClassPropertyBuilder ClearAttributes()
        {
            Attributes.Clear();
            return this;
        }
        public ClassPropertyBuilder AddAttributes(IEnumerable<AttributeBuilder> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }
        public ClassPropertyBuilder AddAttributes(params AttributeBuilder[] attributes)
        {
            Attributes.AddRange(attributes);
            return this;
        }
        public ClassPropertyBuilder AddAttributes(IEnumerable<IAttribute> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }
        public ClassPropertyBuilder AddAttributes(params IAttribute[] attributes)
        {
            if (attributes != null)
            {
                foreach (var itemToAdd in attributes)
                {
                    Attributes.Add(new AttributeBuilder(itemToAdd));
                }
            }
            return this;
        }
        public ClassPropertyBuilder WithTypeName(string typeName)
        {
            TypeName = typeName;
            return this;
        }
        public ClassPropertyBuilder WithVirtual(bool @virtual = true)
        {
            Virtual = @virtual;
            return this;
        }
        public ClassPropertyBuilder WithAbstract(bool @abstract = true)
        {
            Abstract = @abstract;
            return this;
        }
        public ClassPropertyBuilder WithProtected(bool @protected = true)
        {
            Protected = @protected;
            return this;
        }
        public ClassPropertyBuilder WithOverride(bool @override = true)
        {
            Override = @override;
            return this;
        }
        public ClassPropertyBuilder WithIsNullable(bool isNullable = true)
        {
            IsNullable = isNullable;
            return this;
        }
        public ClassPropertyBuilder WithExplicitInterfaceName(string explicitInterfaceName)
        {
            ExplicitInterfaceName = explicitInterfaceName;
            return this;
        }
        public ClassPropertyBuilder ClearGetterCodeStatements()
        {
            GetterCodeStatements.Clear();
            return this;
        }
        public ClassPropertyBuilder AddGetterCodeStatements(IEnumerable<ICodeStatementBuilder> getterCodeStatements)
        {
            return AddGetterCodeStatements(getterCodeStatements.ToArray());
        }
        public ClassPropertyBuilder AddGetterCodeStatements(params ICodeStatementBuilder[] getterCodeStatements)
        {
            if (getterCodeStatements != null)
            {
                GetterCodeStatements.AddRange(getterCodeStatements);
            }
            return this;
        }
        public ClassPropertyBuilder AddGetterCodeStatements(IEnumerable<ICodeStatement> getterCodeStatements)
        {
            return AddGetterCodeStatements(getterCodeStatements.ToArray());
        }
        public ClassPropertyBuilder AddGetterCodeStatements(params ICodeStatement[] getterCodeStatements)
        {
            if (getterCodeStatements != null)
            {
                GetterCodeStatements.AddRange(getterCodeStatements.Select(x => x.CreateBuilder()));
            }
            return this;
        }
        public ClassPropertyBuilder ClearSetterCodeStatements()
        {
            SetterCodeStatements.Clear();
            return this;
        }
        public ClassPropertyBuilder AddSetterCodeStatements(IEnumerable<ICodeStatementBuilder> setterCodeStatements)
        {
            return AddSetterCodeStatements(setterCodeStatements.ToArray());
        }
        public ClassPropertyBuilder AddSetterCodeStatements(params ICodeStatementBuilder[] setterCodeStatements)
        {
            if (setterCodeStatements != null)
            {
                SetterCodeStatements.AddRange(setterCodeStatements);
            }
            return this;
        }
        public ClassPropertyBuilder AddSetterCodeStatements(IEnumerable<ICodeStatement> setterCodeStatements)
        {
            return AddSetterCodeStatements(setterCodeStatements.ToArray());
        }
        public ClassPropertyBuilder AddSetterCodeStatements(params ICodeStatement[] setterCodeStatements)
        {
            if (setterCodeStatements != null)
            {
                SetterCodeStatements.AddRange(setterCodeStatements.Select(x => x.CreateBuilder()));
            }
            return this;
        }
        public ClassPropertyBuilder ClearInitCodeStatements()
        {
            InitCodeStatements.Clear();
            return this;
        }
        public ClassPropertyBuilder AddInitCodeStatements(IEnumerable<ICodeStatementBuilder> initCodeStatements)
        {
            return AddInitCodeStatements(initCodeStatements.ToArray());
        }
        public ClassPropertyBuilder AddInitCodeStatements(params ICodeStatementBuilder[] initCodeStatements)
        {
            if (initCodeStatements != null)
            {
                InitCodeStatements.AddRange(initCodeStatements);
            }
            return this;
        }
        public ClassPropertyBuilder AddInitCodeStatements(IEnumerable<ICodeStatement> initCodeStatements)
        {
            return AddInitCodeStatements(initCodeStatements.ToArray());
        }
        public ClassPropertyBuilder AddInitCodeStatements(params ICodeStatement[] initCodeStatements)
        {
            if (initCodeStatements != null)
            {
                InitCodeStatements.AddRange(initCodeStatements.Select(x => x.CreateBuilder()));
            }
            return this;
        }
        public ClassPropertyBuilder()
        {
            HasGetter = true;
            HasSetter = true;
            Metadata = new List<MetadataBuilder>();
            Attributes = new List<AttributeBuilder>();
            GetterCodeStatements = new List<ICodeStatementBuilder>();
            SetterCodeStatements = new List<ICodeStatementBuilder>();
            InitCodeStatements = new List<ICodeStatementBuilder>();
        }
        public ClassPropertyBuilder(IClassProperty source)
        {
            Static = source.Static;
            HasGetter = source.HasGetter;
            HasSetter = source.HasSetter;
            HasInit = source.HasInit;
            Metadata = new List<MetadataBuilder>(source.Metadata?.Select(x => new MetadataBuilder(x)) ?? Enumerable.Empty<MetadataBuilder>());
            Visibility = source.Visibility;
            GetterVisibility = source.GetterVisibility;
            SetterVisibility = source.SetterVisibility;
            InitVisibility = source.InitVisibility;
            GetterBody = source.GetterBody;
            SetterBody = source.SetterBody;
            InitBody = source.InitBody;
            Name = source.Name;
            Attributes = new List<AttributeBuilder>(source.Attributes?.Select(x => new AttributeBuilder(x)) ?? Enumerable.Empty<AttributeBuilder>());
            TypeName = source.TypeName;
            Virtual = source.Virtual;
            Abstract = source.Abstract;
            Protected = source.Protected;
            Override = source.Override;
            IsNullable = source.IsNullable;
            ExplicitInterfaceName = source.ExplicitInterfaceName;
            GetterCodeStatements = new List<ICodeStatementBuilder>(source.GetterCodeStatements?.Select(x => x.CreateBuilder()) ?? Enumerable.Empty<ICodeStatementBuilder>());
            SetterCodeStatements = new List<ICodeStatementBuilder>(source.SetterCodeStatements?.Select(x => x.CreateBuilder()) ?? Enumerable.Empty<ICodeStatementBuilder>());
            InitCodeStatements = new List<ICodeStatementBuilder>(source.InitCodeStatements?.Select(x => x.CreateBuilder()) ?? Enumerable.Empty<ICodeStatementBuilder>());
        }
    }
}
