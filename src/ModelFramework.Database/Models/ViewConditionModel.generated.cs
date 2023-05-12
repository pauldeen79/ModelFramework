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

namespace ModelFramework.Database.Models
{
#nullable enable
    public partial class ViewConditionModel
    {
        public string Expression
        {
            get;
            set;
        }

        public string Combination
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel> Metadata
        {
            get;
            set;
        }

        public string FileGroupName
        {
            get;
            set;
        }

        public ModelFramework.Database.Contracts.IViewCondition ToEntity()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new ModelFramework.Database.ViewCondition(Expression, Combination, Metadata.Select(x => x.ToEntity()), FileGroupName);
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public ViewConditionModel()
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel>();
            #pragma warning disable CS8603 // Possible null reference return.
            Expression = string.Empty;
            Combination = string.Empty;
            FileGroupName = string.Empty;
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public ViewConditionModel(ModelFramework.Database.Contracts.IViewCondition source)
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel>();
            Expression = source.Expression;
            Combination = source.Combination;
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Models.MetadataModel(x)));
            FileGroupName = source.FileGroupName;
        }
    }
#nullable restore
}

