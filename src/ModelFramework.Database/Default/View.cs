using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;

namespace ModelFramework.Database.Default
{
    public record View : IView
    {
#pragma warning disable S107 // Methods should not have too many parameters
        public View(string name,
                    int? top = null,
                    bool topPercent = false,
                    bool distinct = false,
                    string definition = "",
                    IEnumerable<IViewField>? selectFields = null,
                    IEnumerable<IViewOrderByField>? orderByFields = null,
                    IEnumerable<IViewField>? groupByFields = null,
                    IEnumerable<IViewSource>? sources = null,
                    IEnumerable<IViewCondition>? conditions = null,
                    IEnumerable<IMetadata>? metadata = null)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");
            }

            Name = name;
            Top = top;
            TopPercent = topPercent;
            Distinct = distinct;
            Definition = definition;
            SelectFields = new ValueCollection<IViewField>(selectFields ?? Enumerable.Empty<IViewField>());
            OrderByFields = new ValueCollection<IViewOrderByField>(orderByFields ?? Enumerable.Empty<IViewOrderByField>());
            GroupByFields = new ValueCollection<IViewField>(groupByFields ?? Enumerable.Empty<IViewField>());
            Sources = new ValueCollection<IViewSource>(sources ?? Enumerable.Empty<IViewSource>());
            Conditions = new ValueCollection<IViewCondition>(conditions ?? Enumerable.Empty<IViewCondition>());
            Metadata = new ValueCollection<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        public ValueCollection<IViewField> SelectFields { get; }
        public ValueCollection<IViewOrderByField> OrderByFields { get; }
        public ValueCollection<IViewField> GroupByFields { get; }
        public ValueCollection<IViewSource> Sources { get; }
        public ValueCollection<IViewCondition> Conditions { get; }
        public int? Top { get; }
        public bool TopPercent { get; }
        public bool Distinct { get; }
        public string Definition { get; }
        public string Name { get; }
        public ValueCollection<IMetadata> Metadata { get; }
    }
}
