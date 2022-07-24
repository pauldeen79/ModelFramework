﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 6.0.7
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
    public partial class CheckConstraintBuilder
    {
        public string Expression
        {
            get
            {
                return _expressionDelegate.Value;
            }
            set
            {
                _expressionDelegate = new (() => value);
            }
        }

        public string Name
        {
            get
            {
                return _nameDelegate.Value;
            }
            set
            {
                _nameDelegate = new (() => value);
            }
        }

        public System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder> Metadata
        {
            get;
            set;
        }

        public CheckConstraintBuilder AddMetadata(params ModelFramework.Common.Builders.MetadataBuilder[] metadata)
        {
            Metadata.AddRange(metadata);
            return this;
        }

        public CheckConstraintBuilder AddMetadata(System.Collections.Generic.IEnumerable<ModelFramework.Common.Builders.MetadataBuilder> metadata)
        {
            return AddMetadata(metadata.ToArray());
        }

        public CheckConstraintBuilder AddMetadata(string name, object? value)
        {
            AddMetadata(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));
            return this;
        }

        public ModelFramework.Database.Contracts.ICheckConstraint Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            return new ModelFramework.Database.CheckConstraint(Expression, Name, Metadata.Select(x => x.Build()));
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public CheckConstraintBuilder WithExpression(System.Func<string> expressionDelegate)
        {
            _expressionDelegate = new (expressionDelegate);
            return this;
        }

        public CheckConstraintBuilder WithExpression(string expression)
        {
            Expression = expression;
            return this;
        }

        public CheckConstraintBuilder WithName(System.Func<string> nameDelegate)
        {
            _nameDelegate = new (nameDelegate);
            return this;
        }

        public CheckConstraintBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public CheckConstraintBuilder()
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            #pragma warning disable CS8603 // Possible null reference return.
            _expressionDelegate = new (() => string.Empty);
            _nameDelegate = new (() => string.Empty);
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public CheckConstraintBuilder(ModelFramework.Database.Contracts.ICheckConstraint source)
        {
            Metadata = new System.Collections.Generic.List<ModelFramework.Common.Builders.MetadataBuilder>();
            _expressionDelegate = new (() => source.Expression);
            _nameDelegate = new (() => source.Name);
            Metadata.AddRange(source.Metadata.Select(x => new ModelFramework.Common.Builders.MetadataBuilder(x)));
        }

        private System.Lazy<string> _expressionDelegate;

        private System.Lazy<string> _nameDelegate;
    }
#nullable restore
}

