﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 7.0.5
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelFramework.Objects.Models
{
#nullable enable
    public partial class InterfaceModel : TypeBaseModel<InterfaceModel, ModelFramework.Objects.Contracts.IInterface>
    {
        public override ModelFramework.Objects.Contracts.IInterface ToTypedEntity()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new ModelFramework.Objects.Interface(Namespace, Partial, new System.Collections.Generic.List<System.String>(Interfaces), Properties.Select(x => x.ToEntity()), Methods.Select(x => x.ToEntity()), new System.Collections.Generic.List<System.String>(GenericTypeArguments), new System.Collections.Generic.List<System.String>(GenericTypeArgumentConstraints), Metadata.Select(x => x.ToEntity()), Visibility, Name, Attributes.Select(x => x.ToEntity()));
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public InterfaceModel() : base()
        {
            #pragma warning disable CS8603 // Possible null reference return.
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public InterfaceModel(ModelFramework.Objects.Contracts.IInterface source) : base(source)
        {
        }
    }
#nullable restore
}

