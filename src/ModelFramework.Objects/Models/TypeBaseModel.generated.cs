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
    public abstract partial class TypeBaseModel<TBuilder, TEntity> : TypeBaseModel
        where TEntity : ModelFramework.Objects.Contracts.ITypeBase
        where TBuilder : TypeBaseModel<TBuilder, TEntity>
    {
        public abstract TEntity ToTypedEntity();

        public override ModelFramework.Objects.Contracts.ITypeBase ToEntity()
        {
            return ToTypedEntity();
        }

        protected TypeBaseModel() : base()
        {
        }

        protected TypeBaseModel(ModelFramework.Objects.Contracts.ITypeBase source) : base(source)
        {
        }
    }
#nullable restore
}
