﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 7.0.14
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
    public partial record Class : ModelFramework.Objects.TypeBase, ModelFramework.Objects.Contracts.IClass
    {
        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Objects.Contracts.IClassField> Fields
        {
            get;
        }

        public bool Static
        {
            get;
        }

        public bool Sealed
        {
            get;
        }

        public bool Abstract
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Objects.Contracts.IClass> SubClasses
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Objects.Contracts.IClassConstructor> Constructors
        {
            get;
        }

        public string BaseClass
        {
            get;
        }

        public bool Record
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Objects.Contracts.IEnum> Enums
        {
            get;
        }

        public Class(System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.IClassField> fields, bool @static, bool @sealed, bool @abstract, System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.IClass> subClasses, System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.IClassConstructor> constructors, string baseClass, bool record, string @namespace, bool partial, System.Collections.Generic.IEnumerable<string> interfaces, System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.IClassProperty> properties, System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.IClassMethod> methods, System.Collections.Generic.IEnumerable<string> genericTypeArguments, System.Collections.Generic.IEnumerable<string> genericTypeArgumentConstraints, System.Collections.Generic.IEnumerable<ModelFramework.Common.Contracts.IMetadata> metadata, ModelFramework.Objects.Contracts.Visibility visibility, string name, System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.IAttribute> attributes, System.Collections.Generic.IEnumerable<ModelFramework.Objects.Contracts.IEnum> enums) : base(@namespace, partial, interfaces, properties, methods, genericTypeArguments, genericTypeArgumentConstraints, metadata, visibility, name, attributes)
        {
            this.Fields = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Objects.Contracts.IClassField>(fields);
            this.Static = @static;
            this.Sealed = @sealed;
            this.Abstract = @abstract;
            this.SubClasses = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Objects.Contracts.IClass>(subClasses);
            this.Constructors = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Objects.Contracts.IClassConstructor>(constructors);
            this.BaseClass = baseClass;
            this.Record = record;
            this.Enums = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Objects.Contracts.IEnum>(enums);
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }
#nullable restore
}

