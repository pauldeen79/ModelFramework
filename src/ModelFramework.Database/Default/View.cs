using System;
using System.Collections.Generic;
using CrossCutting.Common;
using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;

namespace ModelFramework.Database.Default
{
    public record View : IView
    {
#pragma warning disable S107 // Methods should not have too many parameters
        public View(string name,
                    int? top,
                    bool topPercent,
                    bool distinct,
                    string definition,
                    IEnumerable<IViewField> selectFields,
                    IEnumerable<IViewOrderByField> orderByFields,
                    IEnumerable<IViewField> groupByFields,
                    IEnumerable<IViewSource> sources,
                    IEnumerable<IViewCondition> conditions,
                    IEnumerable<IMetadata> metadata)
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
            SelectFields = new ValueCollection<IViewField>(selectFields);
            OrderByFields = new ValueCollection<IViewOrderByField>(orderByFields);
            GroupByFields = new ValueCollection<IViewField>(groupByFields);
            Sources = new ValueCollection<IViewSource>(sources);
            Conditions = new ValueCollection<IViewCondition>(conditions);
            Metadata = new ValueCollection<IMetadata>(metadata);
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
