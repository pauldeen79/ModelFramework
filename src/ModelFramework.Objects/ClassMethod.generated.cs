﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.8
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
    public partial record ClassMethod : ModelFramework.Objects.Contracts.IClassMethod
    {
        public bool Partial
        {
            get;
        }

        public bool ExtensionMethod
        {
            get;
        }

        public bool Operator
        {
            get;
        }

        public bool Async
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<string> GenericTypeArguments
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<string> GenericTypeArgumentConstraints
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

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Objects.Contracts.ICodeStatement> CodeStatements
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Objects.Contracts.IParameter> Parameters
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

        public ClassMethod(bool partial, bool extensionMethod, bool @operator, bool async, System.Collections.Generic.IReadOnlyCollection<string> genericTypeArguments, System.Collections.Generic.IReadOnlyCollection<string> genericTypeArgumentConstraints, System.Collections.Generic.IEnumerable<ModelFramework.Common.Contracts.IMetadata> metadata, bool @static, bool @virtual, bool @abstract, bool @protected, bool @override, ModelFramework.Objects.Contracts.Visibility visibility, string name, System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.IAttribute> attributes, System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.ICodeStatement> codeStatements, System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.IParameter> parameters, string typeName, bool isNullable, bool isValueType, string explicitInterfaceName, string parentTypeFullName)
        {
            this.Partial = partial;
            this.ExtensionMethod = extensionMethod;
            this.Operator = @operator;
            this.Async = async;
            this.GenericTypeArguments = genericTypeArguments;
            this.GenericTypeArgumentConstraints = genericTypeArgumentConstraints;
            this.Metadata = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Common.Contracts.IMetadata>(metadata);
            this.Static = @static;
            this.Virtual = @virtual;
            this.Abstract = @abstract;
            this.Protected = @protected;
            this.Override = @override;
            this.Visibility = visibility;
            this.Name = name;
            this.Attributes = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Objects.Contracts.IAttribute>(attributes);
            this.CodeStatements = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Objects.Contracts.ICodeStatement>(codeStatements);
            this.Parameters = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Objects.Contracts.IParameter>(parameters);
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

