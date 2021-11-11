using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Database.Default
{
    public class ViewCondition : IViewCondition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewCondition"/> class.
        /// </summary>
        /// <param name="combination">The combination.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="fileGroupName">Name of the file group.</param>
        /// <param name="metadata">The metadata.</param>
        public ViewCondition(string combination
            , string expression
            , string fileGroupName = null
            , IEnumerable<IMetadata> metadata = null)
        {
            Combination = combination;
            Expression = expression;
            FileGroupName = fileGroupName;
            Metadata = new List<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        /// <summary>
        /// Gets the expression.
        /// </summary>
        /// <value>
        /// The expression.
        /// </value>
        public string Expression { get; }

        /// <summary>
        /// Gets the combination.
        /// </summary>
        /// <value>
        /// The combination.
        /// </value>
        public string Combination { get; }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        public IReadOnlyCollection<IMetadata> Metadata { get; }

        /// <summary>
        /// Gets the name of the file group.
        /// </summary>
        /// <value>
        /// The name of the file group.
        /// </value>
        public string FileGroupName { get; }
    }
}
