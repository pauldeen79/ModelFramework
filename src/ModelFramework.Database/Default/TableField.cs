using ModelFramework.Common.Contracts;
using ModelFramework.Database.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelFramework.Database.Default
{
    public class TableField : ITableField
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TableField"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="isRequired">if set to <c>true</c> [is required].</param>
        /// <param name="isIdentity">if set to <c>true</c> [is identity].</param>
        /// <param name="numericPrecision">The numeric precision.</param>
        /// <param name="numericScale">The numeric scale.</param>
        /// <param name="stringLength">Length of the string.</param>
        /// <param name="stringCollation">The string collation.</param>
        /// <param name="isStringMaxLength">Length of the is string max.</param>
        /// <param name="checkConstraint">Optional check constraint.</param>
        /// <param name="metadata">The metadata.</param>
        /// <exception cref="ArgumentOutOfRangeException">name;Name cannot be null or whitespace</exception>
        public TableField
            (
            string name,
            string type,
            bool isRequired = false,
            bool isIdentity = false,
            byte? numericPrecision = null,
            byte? numericScale = null,
            int? stringLength = null,
            string stringCollation = null,
            bool? isStringMaxLength = null,
            ITableFieldCheckConstraint checkConstraint = null,
            IEnumerable<IMetadata> metadata = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException(nameof(name), "Name cannot be null or whitespace");

            Name = name;
            Type = type;
            IsRequired = isRequired;
            IsIdentity = isIdentity;
            NumericPrecision = numericPrecision;
            NumericScale = numericScale;
            StringLength = stringLength;
            StringCollation = stringCollation;
            IsStringMaxLength = isStringMaxLength;
            CheckConstraint = checkConstraint;
            Metadata = new List<IMetadata>(metadata ?? Enumerable.Empty<IMetadata>());
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string /*TableFieldType*/ Type { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is identity.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is identity; otherwise, <c>false</c>.
        /// </value>
        public bool IsIdentity { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is required.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is required; otherwise, <c>false</c>.
        /// </value>
        public bool IsRequired { get; }

        /// <summary>
        /// Gets the numeric precision.
        /// </summary>
        /// <value>
        /// The numeric precision.
        /// </value>
        public byte? NumericPrecision { get; }

        /// <summary>
        /// Gets the numeric scale.
        /// </summary>
        /// <value>
        /// The numeric scale.
        /// </value>
        public byte? NumericScale { get; }

        /// <summary>
        /// Gets the length of the string.
        /// </summary>
        /// <value>
        /// The length of the string.
        /// </value>
        public int? StringLength { get; }

        /// <summary>
        /// Gets the string collation.
        /// </summary>
        /// <value>
        /// The string collation.
        /// </value>
        public string StringCollation { get; }

        /// <summary>
        /// Gets the length of the is string max.
        /// </summary>
        /// <value>
        /// The length of the is string max.
        /// </value>
        public bool? IsStringMaxLength { get; }

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

        /// <summary>
        /// Gets the check constraint.
        /// </summary>
        /// <value>The check constraint.</value>
        public ITableFieldCheckConstraint CheckConstraint { get; }
    }
}
