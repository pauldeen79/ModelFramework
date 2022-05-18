﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.5
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
    public partial record ClassConstructor : ModelFramework.Objects.Contracts.IClassConstructor
    {
        public string ChainCall
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

        public ClassConstructor(string chainCall, System.Collections.Generic.IEnumerable<ModelFramework.Common.Contracts.IMetadata> metadata, bool @static, bool @virtual, bool @abstract, bool @protected, bool @override, ModelFramework.Objects.Contracts.Visibility visibility, System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.IAttribute> attributes, System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.ICodeStatement> codeStatements, System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.IParameter> parameters)
        {
            this.ChainCall = chainCall;
            this.Metadata = new CrossCutting.Common.ValueCollection<ModelFramework.Common.Contracts.IMetadata>(metadata);
            this.Static = @static;
            this.Virtual = @virtual;
            this.Abstract = @abstract;
            this.Protected = @protected;
            this.Override = @override;
            this.Visibility = visibility;
            this.Attributes = new CrossCutting.Common.ValueCollection<ModelFramework.Objects.Contracts.IAttribute>(attributes);
            this.CodeStatements = new CrossCutting.Common.ValueCollection<ModelFramework.Objects.Contracts.ICodeStatement>(codeStatements);
            this.Parameters = new CrossCutting.Common.ValueCollection<ModelFramework.Objects.Contracts.IParameter>(parameters);
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }
#nullable restore
}

