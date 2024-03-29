﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 8.0.3
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelFramework.Objects
{
#nullable enable
    public partial record ClassProperty : ModelFramework.Objects.Contracts.IClassProperty
    {
        public bool HasGetter
        {
            get;
        }

        public bool HasSetter
        {
            get;
        }

        public bool HasInitializer
        {
            get;
        }

        public System.Nullable<ModelFramework.Objects.Contracts.Visibility> GetterVisibility
        {
            get;
        }

        public System.Nullable<ModelFramework.Objects.Contracts.Visibility> SetterVisibility
        {
            get;
        }

        public System.Nullable<ModelFramework.Objects.Contracts.Visibility> InitializerVisibility
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Objects.Contracts.ICodeStatement> GetterCodeStatements
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Objects.Contracts.ICodeStatement> SetterCodeStatements
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Objects.Contracts.ICodeStatement> InitializerCodeStatements
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Common.Contracts.IMetadata> Metadata
        {
            get;
        }

        public bool Static
        {
            get;
        }

        public bool Virtual
        {
            get;
        }

        public bool Abstract
        {
            get;
        }

        public bool Protected
        {
            get;
        }

        public bool Override
        {
            get;
        }

        public ModelFramework.Objects.Contracts.Visibility Visibility
        {
            get;
        }

        public string Name
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Objects.Contracts.IAttribute> Attributes
        {
            get;
        }

        public string TypeName
        {
            get;
        }

        public bool IsNullable
        {
            get;
        }

        public bool IsValueType
        {
            get;
        }

        public string ExplicitInterfaceName
        {
            get;
        }

        public string ParentTypeFullName
        {
            get;
        }

        public ClassProperty(bool hasGetter, bool hasSetter, bool hasInitializer, System.Nullable<ModelFramework.Objects.Contracts.Visibility> getterVisibility, System.Nullable<ModelFramework.Objects.Contracts.Visibility> setterVisibility, System.Nullable<ModelFramework.Objects.Contracts.Visibility> initializerVisibility, System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.ICodeStatement> getterCodeStatements, System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.ICodeStatement> setterCodeStatements, System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.ICodeStatement> initializerCodeStatements, System.Collections.Generic.IEnumerable<ModelFramework.Common.Contracts.IMetadata> metadata, bool @static, bool @virtual, bool @abstract, bool @protected, bool @override, ModelFramework.Objects.Contracts.Visibility visibility, string name, System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.IAttribute> attributes, string typeName, bool isNullable, bool isValueType, string explicitInterfaceName, string parentTypeFullName)
        {
            this.HasGetter = hasGetter;
            this.HasSetter = hasSetter;
            this.HasInitializer = hasInitializer;
            this.GetterVisibility = getterVisibility;
            this.SetterVisibility = setterVisibility;
            this.InitializerVisibility = initializerVisibility;
            this.GetterCodeStatements = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Objects.Contracts.ICodeStatement>(getterCodeStatements);
            this.SetterCodeStatements = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Objects.Contracts.ICodeStatement>(setterCodeStatements);
            this.InitializerCodeStatements = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Objects.Contracts.ICodeStatement>(initializerCodeStatements);
            this.Metadata = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Common.Contracts.IMetadata>(metadata);
            this.Static = @static;
            this.Virtual = @virtual;
            this.Abstract = @abstract;
            this.Protected = @protected;
            this.Override = @override;
            this.Visibility = visibility;
            this.Name = name;
            this.Attributes = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Objects.Contracts.IAttribute>(attributes);
            this.TypeName = typeName;
            this.IsNullable = isNullable;
            this.IsValueType = isValueType;
            this.ExplicitInterfaceName = explicitInterfaceName;
            this.ParentTypeFullName = parentTypeFullName;
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }
#nullable restore
}

