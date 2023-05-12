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
    public partial class ViewModel
    {
        public System.Collections.Generic.List<ModelFramework.Database.Models.ViewFieldModel> SelectFields
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Database.Models.ViewOrderByFieldModel> OrderByFields
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Database.Models.ViewFieldModel> GroupByFields
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Database.Models.ViewSourceModel> Sources
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Database.Models.ViewConditionModel> Conditions
        {
            get;
            set;
        }

        public System.Nullable<int> Top
        {
            get;
            set;
        }

        public bool TopPercent
        {
            get;
            set;
        }

        public bool Distinct
        {
            get;
            set;
        }

        public string Definition
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel> Metadata
        {
            get;
            set;
        }

        public ModelFramework.Database.Contracts.IView ToEntity()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new ModelFramework.Database.View(SelectFields.Select(x => x.ToEntity()), OrderByFields.Select(x => x.ToEntity()), GroupByFields.Select(x => x.ToEntity()), Sources.Select(x => x.ToEntity()), Conditions.Select(x => x.ToEntity()), Top, TopPercent, Distinct, Definition, Name, Metadata.Select(x => x.ToEntity()));
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public ViewModel()
        {
            SelectFields = new System.Collections.Generic.List<ModelFramework.Database.Models.ViewFieldModel>();
            OrderByFields = new System.Collections.Generic.List<ModelFramework.Database.Models.ViewOrderByFieldModel>();
            GroupByFields = new System.Collections.Generic.List<ModelFramework.Database.Models.ViewFieldModel>();
            Sources = new System.Collections.Generic.List<ModelFramework.Database.Models.ViewSourceModel>();
            Conditions = new System.Collections.Generic.List<ModelFramework.Database.Models.ViewConditionModel>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel>();
            #pragma warning disable CS8603 // Possible null reference return.
            TopPercent = default(System.Boolean);
            Distinct = default(System.Boolean);
            Definition = string.Empty;
            Name = string.Empty;
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public ViewModel(ModelFramework.Database.Contracts.IView source)
        {
            SelectFields = new System.Collections.Generic.List<ModelFramework.Database.Models.ViewFieldModel>();
            OrderByFields = new System.Collections.Generic.List<ModelFramework.Database.Models.ViewOrderByFieldModel>();
            GroupByFields = new System.Collections.Generic.List<ModelFramework.Database.Models.ViewFieldModel>();
            Sources = new System.Collections.Generic.List<ModelFramework.Database.Models.ViewSourceModel>();
            Conditions = new System.Collections.Generic.List<ModelFramework.Database.Models.ViewConditionModel>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Models.MetadataModel>();
            SelectFields.AddRange(source.SelectFields.Select(x => new ModelFramework.Database.Models.ViewFieldModel(x)));
            OrderByFields.AddRange(source.OrderByFields.Select(x => new ModelFramework.Database.Models.ViewOrderByFieldModel(x)));
            GroupByFields.AddRange(source.GroupByFields.Select(x => new ModelFramework.Database.Models.ViewFieldModel(x)));
            Sources.AddRange(source.Sources.Select(x => new ModelFramework.Database.Models.ViewSourceModel(x)));
            Conditions.AddRange(source.Conditions.Select(x => new ModelFramework.Database.Models.ViewConditionModel(x)));
            Top = source.Top;
            TopPercent = source.TopPercent;
            Distinct = source.Distinct;
            Definition = source.Definition;
            Name = source.Name;
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Models.MetadataModel(x)));
        }
    }
#nullable restore
}

