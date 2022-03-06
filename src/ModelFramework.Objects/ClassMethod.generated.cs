﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.2
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

        public CrossCutting.Common.ValueCollection<string> GenericTypeArguments
        {
            get;
        }

        public CrossCutting.Common.ValueCollection<string> GenericTypeArgumentConstraints
        {
            get;
        }

        public CrossCutting.Common.ValueCollection<ModelFramework.Common.Contracts.IMetadata> Metadata
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

        public CrossCutting.Common.ValueCollection<ModelFramework.Objects.Contracts.IAttribute> Attributes
        {
            get;
        }

        public CrossCutting.Common.ValueCollection<ModelFramework.Objects.Contracts.ICodeStatement> CodeStatements
        {
            get;
        }

        public CrossCutting.Common.ValueCollection<ModelFramework.Objects.Contracts.IParameter> Parameters
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

        public string ExplicitInterfaceName
        {
            get;
        }

        public ClassMethod(bool partial, bool extensionMethod, bool @operator, CrossCutting.Common.ValueCollection<string> genericTypeArguments, CrossCutting.Common.ValueCollection<string> genericTypeArgumentConstraints, System.Collections.Generic.IEnumerable<ModelFramework.Common.Contracts.IMetadata> metadata, bool @static, bool @virtual, bool @abstract, bool @protected, bool @override, ModelFramework.Objects.Contracts.Visibility visibility, string name, System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.IAttribute> attributes, System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.ICodeStatement> codeStatements, System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.IParameter> parameters, string typeName, bool isNullable, string explicitInterfaceName)
        {
            this.Partial = partial;
            this.ExtensionMethod = extensionMethod;
            this.Operator = @operator;
            this.GenericTypeArguments = genericTypeArguments;
            this.GenericTypeArgumentConstraints = genericTypeArgumentConstraints;
            this.Metadata = new CrossCutting.Common.ValueCollection<ModelFramework.Common.Contracts.IMetadata>(metadata);
            this.Static = @static;
            this.Virtual = @virtual;
            this.Abstract = @abstract;
            this.Protected = @protected;
            this.Override = @override;
            this.Visibility = visibility;
            this.Name = name;
            this.Attributes = new CrossCutting.Common.ValueCollection<ModelFramework.Objects.Contracts.IAttribute>(attributes);
            this.CodeStatements = new CrossCutting.Common.ValueCollection<ModelFramework.Objects.Contracts.ICodeStatement>(codeStatements);
            this.Parameters = new CrossCutting.Common.ValueCollection<ModelFramework.Objects.Contracts.IParameter>(parameters);
            this.TypeName = typeName;
            this.IsNullable = isNullable;
            this.ExplicitInterfaceName = explicitInterfaceName;
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }
#nullable restore
}

