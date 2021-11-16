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
            Definition = default;
            Name = default;
            Metadata.Clear();
            return this;
        }
        public ViewBuilder Update(IView source)
        {
            SelectFields = new List<ViewFieldBuilder>();
            OrderByFields = new List<ViewOrderByFieldBuilder>();
            GroupByFields = new List<ViewFieldBuilder>();
            Sources = new List<ViewSourceBuilder>();
            Conditions = new List<ViewConditionBuilder>();
            Metadata = new List<MetadataBuilder>();

            if (source.SelectFields != null) SelectFields.AddRange(source.SelectFields.Select(x => new ViewFieldBuilder(x)));
            if (source.OrderByFields != null) OrderByFields.AddRange(source.OrderByFields.Select(x => new ViewOrderByFieldBuilder(x)));
            if (source.GroupByFields != null) GroupByFields.AddRange(source.GroupByFields.Select(x => new ViewFieldBuilder(x)));
            if (source.Sources != null) Sources.AddRange(source.Sources.Select(x => new ViewSourceBuilder(x)));
            if (source.Conditions != null) Conditions.AddRange(source.Conditions.Select(x => new ViewConditionBuilder(x)));
            Top = source.Top;
            TopPercent = source.TopPercent;
            Distinct = source.Distinct;
            Definition = source.Definition;
            Name = source.Name;
            if (source.Metadata != null) Metadata.AddRange(source.Metadata.Select(x => new MetadataBuilder(x)));

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
            if (selectFields != null)
            {
                foreach (var itemToAdd in selectFields)
                {
                    SelectFields.Add(itemToAdd);
                }
            }
            return this;
        }
        public ViewBuilder AddSelectFields(IEnumerable<IViewField> selectFields)
        {
            return AddSelectFields(selectFields.ToArray());
        }
        public ViewBuilder AddSelectFields(params IViewField[] selectFields)
        {
            if (selectFields != null)
            {
                foreach (var itemToAdd in selectFields)
                {
                    SelectFields.Add(new ViewFieldBuilder(itemToAdd));
                }
            }
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
            if (orderByFields != null)
            {
                foreach (var itemToAdd in orderByFields)
                {
                    OrderByFields.Add(itemToAdd);
                }
            }
            return this;
        }
        public ViewBuilder AddOrderByFields(IEnumerable<IViewOrderByField> orderByFields)
        {
            return AddOrderByFields(orderByFields.ToArray());
        }
        public ViewBuilder AddOrderByFields(params IViewOrderByField[] orderByFields)
        {
            if (orderByFields != null)
            {
                foreach (var itemToAdd in orderByFields)
                {
                    OrderByFields.Add(new ViewOrderByFieldBuilder(itemToAdd));
                }
            }
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
            if (groupByFields != null)
            {
                foreach (var itemToAdd in groupByFields)
                {
                    GroupByFields.Add(itemToAdd);
                }
            }
            return this;
        }
        public ViewBuilder AddGroupByFields(IEnumerable<IViewField> groupByFields)
        {
            return AddGroupByFields(groupByFields.ToArray());
        }
        public ViewBuilder AddGroupByFields(params IViewField[] groupByFields)
        {
            if (groupByFields != null)
            {
                foreach (var itemToAdd in groupByFields)
                {
                    GroupByFields.Add(new ViewFieldBuilder(itemToAdd));
                }
            }
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
            if (sources != null)
            {
                foreach (var itemToAdd in sources)
                {
                    Sources.Add(itemToAdd);
                }
            }
            return this;
        }
        public ViewBuilder AddSources(IEnumerable<IViewSource> sources)
        {
            return AddSources(sources.ToArray());
        }
        public ViewBuilder AddSources(params IViewSource[] sources)
        {
            if (sources != null)
            {
                foreach (var itemToAdd in sources)
                {
                    Sources.Add(new ViewSourceBuilder(itemToAdd));
                }
            }
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
            if (conditions != null)
            {
                foreach (var itemToAdd in conditions)
                {
                    Conditions.Add(itemToAdd);
                }
            }
            return this;
        }
        public ViewBuilder AddConditions(IEnumerable<IViewCondition> conditions)
        {
            return AddConditions(conditions.ToArray());
        }
        public ViewBuilder AddConditions(params IViewCondition[] conditions)
        {
            if (conditions != null)
            {
                foreach (var itemToAdd in conditions)
                {
                    Conditions.Add(new ViewConditionBuilder(itemToAdd));
                }
            }
            return this;
        }
        public ViewBuilder WithTop(int? top)
        {
            Top = top;
            return this;
        }
        public ViewBuilder WithTopPercent(bool topPercent)
        {
            TopPercent = topPercent;
            return this;
        }
        public ViewBuilder WithDistinct(bool distinct)
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
            if (metadata != null)
            {
                foreach (var itemToAdd in metadata)
                {
                    Metadata.Add(itemToAdd);
                }
            }
            return this;
        }
        public ViewBuilder AddMetadata(IEnumerable<IMetadata> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }
        public ViewBuilder AddMetadata(params IMetadata[] metadata)
        {
            if (metadata != null)
            {
                Metadata.AddRange(metadata.Select(x => new MetadataBuilder(x)));
            }
            return this;
        }
        public ViewBuilder()
        {
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

            foreach (var x in source.SelectFields ?? Enumerable.Empty<IViewField>()) SelectFields.Add(new ViewFieldBuilder(x));
            foreach (var x in source.OrderByFields ?? Enumerable.Empty<IViewOrderByField>()) OrderByFields.Add(new ViewOrderByFieldBuilder(x));
            foreach (var x in source.GroupByFields ?? Enumerable.Empty<IViewField>()) GroupByFields.Add(new ViewFieldBuilder(x));
            foreach (var x in source.Sources ?? Enumerable.Empty<IViewSource>()) Sources.Add(new ViewSourceBuilder(x));
            foreach (var x in source.Conditions ?? Enumerable.Empty<IViewCondition>()) Conditions.Add(new ViewConditionBuilder(x));
            Top = source.Top;
            TopPercent = source.TopPercent;
            Distinct = source.Distinct;
            Definition = source.Definition;
            Name = source.Name;
            foreach (var x in source.Metadata ?? Enumerable.Empty<IMetadata>()) Metadata.Add(new MetadataBuilder(x));
        }
    }
}
