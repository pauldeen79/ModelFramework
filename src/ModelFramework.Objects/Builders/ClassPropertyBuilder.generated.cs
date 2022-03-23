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
    public partial class ClassPropertyBuilder
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

        public System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementBuilder> GetterCodeStatements
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementBuilder> SetterCodeStatements
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementBuilder> InitializerCodeStatements
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder> Metadata
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

        public System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeBuilder> Attributes
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

        public string ExplicitInterfaceName
        {
            get;
            set;
        }

        public ClassPropertyBuilder AddAttributes(params ModelFramework.Objects.Builders.AttributeBuilder[] attributes)
        {
            Attributes.AddRange(attributes);
            return this;
        }

        public ClassPropertyBuilder AddAttributes(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Builders.AttributeBuilder> attributes)
        {
            return AddAttributes(attributes.ToArray());
        }

        public ClassPropertyBuilder AddGetterCodeStatements(params ModelFramework.Objects.Contracts.ICodeStatementBuilder[] getterCodeStatements)
        {
            GetterCodeStatements.AddRange(getterCodeStatements);
            return this;
        }

        public ClassPropertyBuilder AddGetterCodeStatements(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.ICodeStatementBuilder> getterCodeStatements)
        {
            return AddGetterCodeStatements(getterCodeStatements.ToArray());
        }

        public ClassPropertyBuilder AddInitializerCodeStatements(params ModelFramework.Objects.Contracts.ICodeStatementBuilder[] initializerCodeStatements)
        {
            InitializerCodeStatements.AddRange(initializerCodeStatements);
            return this;
        }

        public ClassPropertyBuilder AddInitializerCodeStatements(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.ICodeStatementBuilder> initializerCodeStatements)
        {
            return AddInitializerCodeStatements(initializerCodeStatements.ToArray());
        }

        public ClassPropertyBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public ClassPropertyBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public ClassPropertyBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public ClassPropertyBuilder AddSetterCodeStatements(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.ICodeStatementBuilder> setterCodeStatements)
        {
            return AddSetterCodeStatements(setterCodeStatements.ToArray());
        }

        public ClassPropertyBuilder AddSetterCodeStatements(params ModelFramework.Objects.Contracts.ICodeStatementBuilder[] setterCodeStatements)
        {
            SetterCodeStatements.AddRange(setterCodeStatements);
            return this;
        }

        public ModelFramework.Objects.Contracts.IClassProperty Build()
        {
            return new ModelFramework.Objects.ClassProperty(HasGetter, HasSetter, HasInitializer, GetterVisibility, SetterVisibility, InitializerVisibility, GetterCodeStatements.Select(x => x.Build()), SetterCodeStatements.Select(x => x.Build()), InitializerCodeStatements.Select(x => x.Build()), Metadata.Select(x => x.Build()), Static, Virtual, Abstract, Protected, Override, Visibility, Name, Attributes.Select(x => x.Build()), TypeName, IsNullable, ExplicitInterfaceName);
        }

        public ClassPropertyBuilder WithAbstract(bool @abstract = true)
        {
            Abstract = @abstract;
            return this;
        }

        public ClassPropertyBuilder WithExplicitInterfaceName(string explicitInterfaceName)
        {
            ExplicitInterfaceName = explicitInterfaceName;
            return this;
        }

        public ClassPropertyBuilder WithGetterVisibility(System.Nullable<ModelFramework.Objects.Contracts.Visibility> getterVisibility)
        {
            GetterVisibility = getterVisibility;
            return this;
        }

        public ClassPropertyBuilder WithHasGetter(bool hasGetter = true)
        {
            HasGetter = hasGetter;
            return this;
        }

        public ClassPropertyBuilder WithHasInitializer(bool hasInitializer = true)
        {
            HasInitializer = hasInitializer;
            if (hasInitializer)
            {
                HasSetter = false;
            }
            return this;
        }

        public ClassPropertyBuilder WithHasSetter(bool hasSetter = true)
        {
            HasSetter = hasSetter;
            if (hasSetter)
            {
                HasInitializer = false;
            }
            return this;
        }

        public ClassPropertyBuilder WithInitializerVisibility(System.Nullable<ModelFramework.Objects.Contracts.Visibility> initializerVisibility)
        {
            InitializerVisibility = initializerVisibility;
            return this;
        }

        public ClassPropertyBuilder WithIsNullable(bool isNullable = true)
        {
            IsNullable = isNullable;
            return this;
        }

        public ClassPropertyBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public ClassPropertyBuilder WithOverride(bool @override = true)
        {
            Override = @override;
            return this;
        }

        public ClassPropertyBuilder WithProtected(bool @protected = true)
        {
            Protected = @protected;
            return this;
        }

        public ClassPropertyBuilder WithSetterVisibility(System.Nullable<ModelFramework.Objects.Contracts.Visibility> setterVisibility)
        {
            SetterVisibility = setterVisibility;
            return this;
        }

        public ClassPropertyBuilder WithStatic(bool @static = true)
        {
            Static = @static;
            return this;
        }

        public ClassPropertyBuilder WithType(System.Type type)
        {
            TypeName = type.AssemblyQualifiedName;
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

        public ClassPropertyBuilder WithVisibility(ModelFramework.Objects.Contracts.Visibility visibility)
        {
            Visibility = visibility;
            return this;
        }

        public ClassPropertyBuilder()
        {
            GetterCodeStatements = new System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementBuilder>();
            SetterCodeStatements = new System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementBuilder>();
            InitializerCodeStatements = new System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementBuilder>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeBuilder>();
            HasGetter = true;
            HasSetter = true;
            HasInitializer = default;
            Static = default;
            Virtual = default;
            Abstract = default;
            Protected = default;
            Override = default;
            Visibility = ModelFramework.Objects.Contracts.Visibility.Public;
            Name = string.Empty;
            TypeName = string.Empty;
            IsNullable = default;
            ExplicitInterfaceName = string.Empty;
        }

        public ClassPropertyBuilder(ModelFramework.Objects.Contracts.IClassProperty source)
        {
            GetterCodeStatements = new System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementBuilder>();
            SetterCodeStatements = new System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementBuilder>();
            InitializerCodeStatements = new System.Collections.Generic.List<ModelFramework.Objects.Contracts.ICodeStatementBuilder>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            Attributes = new System.Collections.Generic.List<ModelFramework.Objects.Builders.AttributeBuilder>();
            HasGetter = source.HasGetter;
            HasSetter = source.HasSetter;
            HasInitializer = source.HasInitializer;
            GetterVisibility = source.GetterVisibility;
            SetterVisibility = source.SetterVisibility;
            InitializerVisibility = source.InitializerVisibility;
            GetterCodeStatements.AddRange(source.GetterCodeStatements.Select(x => x.CreateBuilder()));
            SetterCodeStatements.AddRange(source.SetterCodeStatements.Select(x => x.CreateBuilder()));
            InitializerCodeStatements.AddRange(source.InitializerCodeStatements.Select(x => x.CreateBuilder()));
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
            Static = source.Static;
            Virtual = source.Virtual;
            Abstract = source.Abstract;
            Protected = source.Protected;
            Override = source.Override;
            Visibility = source.Visibility;
            Name = source.Name;
            Attributes.AddRange(source.Attributes.Select(x => new ModelFramework.Objects.Builders.AttributeBuilder(x)));
            TypeName = source.TypeName;
            IsNullable = source.IsNullable;
            ExplicitInterfaceName = source.ExplicitInterfaceName;
        }
    }
#nullable restore
}

