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

namespace ModelFramework.Database.Builders
{
#nullable enable
    public partial class ViewBuilder
    {
        public System.Collections.Generic.List<ModelFramework.Database.Builders.ViewFieldBuilder> SelectFields
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Database.Builders.ViewOrderByFieldBuilder> OrderByFields
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Database.Builders.ViewFieldBuilder> GroupByFields
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Database.Builders.ViewSourceBuilder> Sources
        {
            get;
            set;
        }

        public System.Collections.Generic.List<ModelFramework.Database.Builders.ViewConditionBuilder> Conditions
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

        public System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder> Metadata
        {
            get;
            set;
        }

        public ModelFramework.Database.Contracts.IView Build()
        {
            return new ModelFramework.Database.View(SelectFields.Select(x => x.Build()), OrderByFields.Select(x => x.Build()), GroupByFields.Select(x => x.Build()), Sources.Select(x => x.Build()), Conditions.Select(x => x.Build()), Top, TopPercent, Distinct, Definition, Name, Metadata.Select(x => x.Build()));
        }

        public ViewBuilder AddSelectFields(System.Collections.Generic.IEnumerable<ModelFramework.Database.Builders.ViewFieldBuilder> selectFields)
        {
            return AddSelectFields(selectFields.ToArray());
        }

        public ViewBuilder AddSelectFields(params ModelFramework.Database.Builders.ViewFieldBuilder[] selectFields)
        {
            SelectFields.AddRange(selectFields);
            return this;
        }

        public ViewBuilder AddOrderByFields(System.Collections.Generic.IEnumerable<ModelFramework.Database.Builders.ViewOrderByFieldBuilder> orderByFields)
        {
            return AddOrderByFields(orderByFields.ToArray());
        }

        public ViewBuilder AddOrderByFields(params ModelFramework.Database.Builders.ViewOrderByFieldBuilder[] orderByFields)
        {
            OrderByFields.AddRange(orderByFields);
            return this;
        }

        public ViewBuilder AddGroupByFields(System.Collections.Generic.IEnumerable<ModelFramework.Database.Builders.ViewFieldBuilder> groupByFields)
        {
            return AddGroupByFields(groupByFields.ToArray());
        }

        public ViewBuilder AddGroupByFields(params ModelFramework.Database.Builders.ViewFieldBuilder[] groupByFields)
        {
            GroupByFields.AddRange(groupByFields);
            return this;
        }

        public ViewBuilder AddSources(System.Collections.Generic.IEnumerable<ModelFramework.Database.Builders.ViewSourceBuilder> sources)
        {
            return AddSources(sources.ToArray());
        }

        public ViewBuilder AddSources(params ModelFramework.Database.Builders.ViewSourceBuilder[] sources)
        {
            Sources.AddRange(sources);
            return this;
        }

        public ViewBuilder AddConditions(System.Collections.Generic.IEnumerable<ModelFramework.Database.Builders.ViewConditionBuilder> conditions)
        {
            return AddConditions(conditions.ToArray());
        }

        public ViewBuilder AddConditions(params ModelFramework.Database.Builders.ViewConditionBuilder[] conditions)
        {
            Conditions.AddRange(conditions);
            return this;
        }

        public ViewBuilder WithTop(System.Nullable<int> top)
        {
            Top = top;
            return this;
        }

        public ViewBuilder WithTopPercent(bool topPercent = true)
        {
            TopPercent = topPercent;
            return this;
        }

        public ViewBuilder WithDistinct(bool distinct = true)
        {
            Distinct = distinct;
            return this;
        }

        public ViewBuilder WithDefinition(string definition)
        {
            Definition = definition;
            return this;
        }

        public ViewBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public ViewBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public ViewBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public ViewBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public ViewBuilder()
        {
            SelectFields = new System.Collections.Generic.List<ModelFramework.Database.Builders.ViewFieldBuilder>();
            OrderByFields = new System.Collections.Generic.List<ModelFramework.Database.Builders.ViewOrderByFieldBuilder>();
            GroupByFields = new System.Collections.Generic.List<ModelFramework.Database.Builders.ViewFieldBuilder>();
            Sources = new System.Collections.Generic.List<ModelFramework.Database.Builders.ViewSourceBuilder>();
            Conditions = new System.Collections.Generic.List<ModelFramework.Database.Builders.ViewConditionBuilder>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            TopPercent = default;
            Distinct = default;
            Definition = string.Empty;
            Name = string.Empty;
        }

        public ViewBuilder(ModelFramework.Database.Contracts.IView source)
        {
            SelectFields = new System.Collections.Generic.List<ModelFramework.Database.Builders.ViewFieldBuilder>();
            OrderByFields = new System.Collections.Generic.List<ModelFramework.Database.Builders.ViewOrderByFieldBuilder>();
            GroupByFields = new System.Collections.Generic.List<ModelFramework.Database.Builders.ViewFieldBuilder>();
            Sources = new System.Collections.Generic.List<ModelFramework.Database.Builders.ViewSourceBuilder>();
            Conditions = new System.Collections.Generic.List<ModelFramework.Database.Builders.ViewConditionBuilder>();
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            SelectFields.AddRange(source.SelectFields.Select(x => new ModelFramework.Database.Builders.ViewFieldBuilder(x)));
            OrderByFields.AddRange(source.OrderByFields.Select(x => new ModelFramework.Database.Builders.ViewOrderByFieldBuilder(x)));
            GroupByFields.AddRange(source.GroupByFields.Select(x => new ModelFramework.Database.Builders.ViewFieldBuilder(x)));
            Sources.AddRange(source.Sources.Select(x => new ModelFramework.Database.Builders.ViewSourceBuilder(x)));
            Conditions.AddRange(source.Conditions.Select(x => new ModelFramework.Database.Builders.ViewConditionBuilder(x)));
            Top = source.Top;
            TopPercent = source.TopPercent;
            Distinct = source.Distinct;
            Definition = source.Definition;
            Name = source.Name;
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
        }
    }
#nullable restore
}

