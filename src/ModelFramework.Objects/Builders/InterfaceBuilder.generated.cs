﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.7
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
        public override ModelFramework.Objects.Contracts.IInterface Build()
        {
#pragma warning disable CS8604 // Possible null reference argument.
            return new ModelFramework.Objects.Interface(Namespace, Partial, new CrossCutting.Common.ValueCollection<System.String>(Interfaces), Properties.Select(x => x.Build()), Methods.Select(x => x.Build()), new CrossCutting.Common.ValueCollection<System.String>(GenericTypeArguments), new CrossCutting.Common.ValueCollection<System.String>(GenericTypeArgumentConstraints), Metadata.Select(x => x.Build()), Visibility, Name, Attributes.Select(x => x.Build()));
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

