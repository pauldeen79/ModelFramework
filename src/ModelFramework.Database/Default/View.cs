using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Database.Default
{
    public class View : IView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="View" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="top">The top.</param>
        /// <param name="topPercent">if set to <c>true</c> [top percent].</param>
        /// <param name="distinct">if set to <c>true</c> [distinct].</param>
        /// <param name="definition">The definition.</param>
        /// <param name="selectFields">The select fields.</param>
        /// <param name="orderByFields">The order by fields.</param>
        /// <param name="groupByFields">The group by fields.</param>
        /// <param name="sources">The sources.</param>
        /// <param name="conditions">The conditions.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="ArgumentOutOfRangeException">name - Name cannot be null or whitespace</exception>
        public View(string name
            //, bool withCheckOption = false
            //, bool schemaBinding = false
            //, bool viewMetadata = false
            , int? top = null
            , bool topPercent = false
            , bool distinct = false
            , string definition = null
            , IEnumerable<IViewField> selectFields = null
            , IEnumerable<IViewOrderByField> orderByFields = null
            , IEnumerable<IViewField> groupByFields = null
            , IEnumerable<IViewSource> sources = null
            , IEnumerable<IViewCondition> conditions = null
            , IEnumerable<IMetadata> metadata = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");

            Name = name;
            //WithCheckOption = withCheckOption;
            //SchemaBinding = schemaBinding;
            //ViewMetadata = viewMetadata;
            Top = top;
            TopPercent = topPercent;
            Distinct = distinct;
            Definition = definition;
            SelectFields = new List<IViewField>(selectFields ?? Enumerable.Empty<IViewField>());
            OrderByFields = new List<IViewOrderByField>(orderByFields ?? Enumerable.Empty<IViewOrderByField>());
            GroupByFields = new List<IViewField>(groupByFields ?? Enumerable.Empty<IViewField>());
            Sources = new List<IViewSource>(sources ?? Enumerable.Empty<IViewSource>());
            Conditions = new List<IViewCondition>(conditions ?? Enumerable.Empty<IViewCondition>());
            Metadata = new List<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        /// <summary>
        /// Gets the select fields.
        /// </summary>
        /// <value>
        /// The select fields.
        /// </value>
        public IReadOnlyCollection<IViewField> SelectFields { get; }

        /// <summary>
        /// Gets the order by fields.
        /// </summary>
        /// <value>
        /// The order by fields.
        /// </value>
        public IReadOnlyCollection<IViewOrderByField> OrderByFields { get; }

        /// <summary>
        /// Gets the group by fields.
        /// </summary>
        /// <value>
        /// The group by fields.
        /// </value>
        public IReadOnlyCollection<IViewField> GroupByFields { get; }

        /// <summary>
        /// Gets the sources.
        /// </summary>
        /// <value>
        /// The sources.
        /// </value>
        public IReadOnlyCollection<IViewSource> Sources { get; }

        /// <summary>
        /// Gets the conditions.
        /// </summary>
        /// <value>
        /// The conditions.
        /// </value>
        public IReadOnlyCollection<IViewCondition> Conditions { get; }

        /*
        /// <summary>
        /// Gets a value indicating whether [with check option].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [with check option]; otherwise, <c>false</c>.
        /// </value>
        public bool WithCheckOption { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="View"/> is encryption.
        /// </summary>
        /// <value>
        ///   <c>true</c> if encryption; otherwise, <c>false</c>.
        /// </value>
        public bool Encryption { get; }

        /// <summary>
        /// Gets a value indicating whether [schema binding].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [schema binding]; otherwise, <c>false</c>.
        /// </value>
        public bool SchemaBinding { get; }

        /// <summary>
        /// Gets a value indicating whether [view metadata].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [view metadata]; otherwise, <c>false</c>.
        /// </value>
        public bool ViewMetadata { get; }
        */

        /// <summary>
        /// Gets the top.
        /// </summary>
        /// <value>
        /// The top.
        /// </value>
        public int? Top { get; }

        /// <summary>
        /// Gets a value indicating whether [top percent].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [top percent]; otherwise, <c>false</c>.
        /// </value>
        public bool TopPercent { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="View"/> is distinct.
        /// </summary>
        /// <value>
        ///   <c>true</c> if distinct; otherwise, <c>false</c>.
        /// </value>
        public bool Distinct { get; }

        /// <summary>
        /// Gets the definition.
        /// </summary>
        /// <value>
        /// The definition.
        /// </value>
        public string Definition { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        public IReadOnlyCollection<IMetadata> Metadata { get; }
    }
}
