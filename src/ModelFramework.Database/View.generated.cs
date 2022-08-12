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

namespace ModelFramework.Database
{
#nullable enable
    public partial record View : ModelFramework.Database.Contracts.IView
    {
        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Database.Contracts.IViewField> SelectFields
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Database.Contracts.IViewOrderByField> OrderByFields
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Database.Contracts.IViewField> GroupByFields
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Database.Contracts.IViewSource> Sources
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Database.Contracts.IViewCondition> Conditions
        {
            get;
        }

        public System.Nullable<int> Top
        {
            get;
        }

        public bool TopPercent
        {
            get;
        }

        public bool Distinct
        {
            get;
        }

        public string Definition
        {
            get;
        }

        public string Name
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<ModelFramework.Common.Contracts.IMetadata> Metadata
        {
            get;
        }

        public View(System.Collections.Generic.IEnumerable<ModelFramework.Database.Contracts.IViewField> selectFields, System.Collections.Generic.IEnumerable<ModelFramework.Database.Contracts.IViewOrderByField> orderByFields, System.Collections.Generic.IEnumerable<ModelFramework.Database.Contracts.IViewField> groupByFields, System.Collections.Generic.IEnumerable<ModelFramework.Database.Contracts.IViewSource> sources, System.Collections.Generic.IEnumerable<ModelFramework.Database.Contracts.IViewCondition> conditions, System.Nullable<int> top, bool topPercent, bool distinct, string definition, string name, System.Collections.Generic.IEnumerable<ModelFramework.Common.Contracts.IMetadata> metadata)
        {
            this.SelectFields = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Database.Contracts.IViewField>(selectFields);
            this.OrderByFields = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Database.Contracts.IViewOrderByField>(orderByFields);
            this.GroupByFields = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Database.Contracts.IViewField>(groupByFields);
            this.Sources = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Database.Contracts.IViewSource>(sources);
            this.Conditions = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Database.Contracts.IViewCondition>(conditions);
            this.Top = top;
            this.TopPercent = topPercent;
            this.Distinct = distinct;
            this.Definition = definition;
            this.Name = name;
            this.Metadata = new CrossCutting.Common.ReadOnlyValueCollection<ModelFramework.Common.Contracts.IMetadata>(metadata);
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }
#nullable restore
}

