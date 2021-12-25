using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using ModelFramework.Database.Default;

namespace ModelFramework.Database.Builders
{
    public class ViewBuilder
    {
        public List<ViewFieldBuilder> SelectFields { get; set; }
        public List<ViewOrderByFieldBuilder> OrderByFields { get; set; }
        public List<ViewFieldBuilder> GroupByFields { get; set; }
        public List<ViewSourceBuilder> Sources { get; set; }
        public List<ViewConditionBuilder> Conditions { get; set; }
        public int? Top { get; set; }
        public bool TopPercent { get; set; }
        public bool Distinct { get; set; }
        public string Definition { get; set; }
        public string Name { get; set; }
        public List<MetadataBuilder> Metadata { get; set; }
        public IView Build()
        {
            return new View(Name,
                            Top,
                            TopPercent,
                            Distinct,
                            Definition,
                            SelectFields.Select(x => x.Build()),
                            OrderByFields.Select(x => x.Build()),
                            GroupByFields.Select(x => x.Build()),
                            Sources.Select(x => x.Build()),
                            Conditions.Select(x => x.Build()),
                            Metadata.Select(x => x.Build()));
        }
        public ViewBuilder Clear()
        {
            SelectFields.Clear();
            OrderByFields.Clear();
            GroupByFields.Clear();
            Sources.Clear();
            Conditions.Clear();
            Top = default;
            TopPercent = default;
            Distinct = default;
            Definition = string.Empty;
            Name = string.Empty;
            Metadata.Clear();
            return this;
        }
        public ViewBuilder ClearSelectFields()
        {
            SelectFields.Clear();
            return this;
        }
        public ViewBuilder AddSelectFields(IEnumerable<ViewFieldBuilder> selectFields)
        {
            return AddSelectFields(selectFields.ToArray());
        }
        public ViewBuilder AddSelectFields(params ViewFieldBuilder[] selectFields)
        {
            SelectFields.AddRange(selectFields);
            return this;
        }
        public ViewBuilder AddSelectFields(IEnumerable<IViewField> selectFields)
        {
            return AddSelectFields(selectFields.ToArray());
        }
        public ViewBuilder AddSelectFields(params IViewField[] selectFields)
        {
            SelectFields.AddRange(selectFields.Select(itemToAdd => new ViewFieldBuilder(itemToAdd)));
            return this;
        }
        public ViewBuilder ClearOrderByFields()
        {
            OrderByFields.Clear();
            return this;
        }
        public ViewBuilder AddOrderByFields(IEnumerable<ViewOrderByFieldBuilder> orderByFields)
        {
            return AddOrderByFields(orderByFields.ToArray());
        }
        public ViewBuilder AddOrderByFields(params ViewOrderByFieldBuilder[] orderByFields)
        {
            OrderByFields.AddRange(orderByFields);
            return this;
        }
        public ViewBuilder AddOrderByFields(IEnumerable<IViewOrderByField> orderByFields)
        {
            return AddOrderByFields(orderByFields.ToArray());
        }
        public ViewBuilder AddOrderByFields(params IViewOrderByField[] orderByFields)
        {
            OrderByFields.AddRange(orderByFields.Select(itemToAdd => new ViewOrderByFieldBuilder(itemToAdd)));
            return this;
        }
        public ViewBuilder ClearGroupByFields()
        {
            GroupByFields.Clear();
            return this;
        }
        public ViewBuilder AddGroupByFields(IEnumerable<ViewFieldBuilder> groupByFields)
        {
            return AddGroupByFields(groupByFields.ToArray());
        }
        public ViewBuilder AddGroupByFields(params ViewFieldBuilder[] groupByFields)
        {
            GroupByFields.AddRange(groupByFields);
            return this;
        }
        public ViewBuilder AddGroupByFields(IEnumerable<IViewField> groupByFields)
        {
            return AddGroupByFields(groupByFields.ToArray());
        }
        public ViewBuilder AddGroupByFields(params IViewField[] groupByFields)
        {
            GroupByFields.AddRange(groupByFields.Select(itemToAdd => new ViewFieldBuilder(itemToAdd)));
            return this;
        }
        public ViewBuilder ClearSources()
        {
            Sources.Clear();
            return this;
        }
        public ViewBuilder AddSources(IEnumerable<ViewSourceBuilder> sources)
        {
            return AddSources(sources.ToArray());
        }
        public ViewBuilder AddSources(params ViewSourceBuilder[] sources)
        {
            Sources.AddRange(sources);
            return this;
        }
        public ViewBuilder AddSources(IEnumerable<IViewSource> sources)
        {
            return AddSources(sources.ToArray());
        }
        public ViewBuilder AddSources(params IViewSource[] sources)
        {
            Sources.AddRange(sources.Select(itemToAdd => new ViewSourceBuilder(itemToAdd)));
            return this;
        }
        public ViewBuilder ClearConditions()
        {
            Conditions.Clear();
            return this;
        }
        public ViewBuilder AddConditions(IEnumerable<ViewConditionBuilder> conditions)
        {
            return AddConditions(conditions.ToArray());
        }
        public ViewBuilder AddConditions(params ViewConditionBuilder[] conditions)
        {
            Conditions.AddRange(conditions);
            return this;
        }
        public ViewBuilder AddConditions(IEnumerable<IViewCondition> conditions)
        {
            return AddConditions(conditions.ToArray());
        }
        public ViewBuilder AddConditions(params IViewCondition[] conditions)
        {
            Conditions.AddRange(conditions.Select(itemToAdd => new ViewConditionBuilder(itemToAdd)));
            return this;
        }
        public ViewBuilder WithTop(int? top)
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
        public ViewBuilder ClearMetadata()
        {
            Metadata.Clear();
            return this;
        }
        public ViewBuilder AddMetadata(IEnumerable<MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ViewBuilder AddMetadata(params MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }
        public ViewBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ViewBuilder AddMetadata(params IMetadata[] metadata)
        {
            Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            return this;
        }
        public ViewBuilder()
        {
            Name = string.Empty;
            Definition = string.Empty;
            SelectFields = new List<ViewFieldBuilder>();
            OrderByFields = new List<ViewOrderByFieldBuilder>();
            GroupByFields = new List<ViewFieldBuilder>();
            Sources = new List<ViewSourceBuilder>();
            Conditions = new List<ViewConditionBuilder>();
            Metadata = new List<MetadataBuilder>();
        }
        public ViewBuilder(IView source)
        {
            SelectFields = new List<ViewFieldBuilder>();
            OrderByFields = new List<ViewOrderByFieldBuilder>();
            GroupByFields = new List<ViewFieldBuilder>();
            Sources = new List<ViewSourceBuilder>();
            Conditions = new List<ViewConditionBuilder>();
            Metadata = new List<MetadataBuilder>();
            SelectFields.AddRange(source.SelectFields.Select(x => new ViewFieldBuilder(x)));
            OrderByFields.AddRange(source.OrderByFields.Select(x => new ViewOrderByFieldBuilder(x)));
            GroupByFields.AddRange(source.GroupByFields.Select(x => new ViewFieldBuilder(x)));
            Sources.AddRange(source.Sources.Select(x => new ViewSourceBuilder(x)));
            Conditions.AddRange(source.Conditions.Select(x => new ViewConditionBuilder(x)));
            Top = source.Top;
            TopPercent = source.TopPercent;
            Distinct = source.Distinct;
            Definition = source.Definition;
            Name = source.Name;
            Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));
        }
    }
}
