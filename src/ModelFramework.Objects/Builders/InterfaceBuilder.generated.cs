﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 7.0.11
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
    public partial class InterfaceBuilder : TypeBaseBuilder<InterfaceBuilder, ModelFramework.Objects.Contracts.IInterface>
    {
        public override ModelFramework.Objects.Contracts.IInterface BuildTyped()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new ModelFramework.Objects.Interface(Namespace?.ToString(), Partial, new CrossCutting.Common.ValueCollection<System.String>(Interfaces), Properties.Select(x => x.Build()), Methods.Select(x => x.Build()), new CrossCutting.Common.ValueCollection<System.String>(GenericTypeArguments), new CrossCutting.Common.ValueCollection<System.String>(GenericTypeArgumentConstraints), Metadata.Select(x => x.Build()), Visibility, Name?.ToString(), Attributes.Select(x => x.Build()));
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public InterfaceBuilder() : base()
        {
            #pragma warning disable CS8603 // Possible null reference return.
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public InterfaceBuilder(ModelFramework.Objects.Contracts.IInterface source) : base(source)
        {
        }
    }
#nullable restore
}

